using Kitchen;
using KitchenData;
using KitchenInferno.Customs;
using KitchenInferno.Customs.Inferno;
using KitchenInferno.Customs.InfernoSetting;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using PreferenceSystem;
using PreferenceSystem.Generators;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenInferno
{
    public class Main : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "IcedMilo.PlateUp.Inferno";
        public const string MOD_NAME = "Inferno";
        public const string MOD_VERSION = "0.2.2";
        public const string MOD_AUTHOR = "IcedMilo";
        public const string MOD_GAMEVERSION = ">=1.1.5";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until

        public static AssetBundle Bundle;

        public Main() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        internal static FireItem CustomFireItem;
        internal static InfernoSetting CustomInfernoSetting;

        internal static PreferenceSystemManager PrefManager;
        public const string FIRE_DISPLAY_INTENSITY_ID = "fireDisplayIntensity";

        public static readonly RestaurantStatus PYROMANIA_EFFECT_STATUS = (RestaurantStatus)VariousUtils.GetID("pyromaniaEffect");

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            CustomFireItem = AddGameDataObject<FireItem>();

            AddGameDataObject<DirtyPlateWithBurnedFood>();

            AddGameDataObject<FlamingMeals>();
            AddGameDataObject<FlamingMealsForced>();
            AddGameDataObject<PyromaniaUnlock>();
            AddGameDataObject<InfernoSpecialCard>();
            AddGameDataObject<InfernoCompositeUnlockPack>();
            CustomInfernoSetting = AddGameDataObject<InfernoSetting>();

            AddGameDataObject<PyroPatronsCopy>();
            AddGameDataObject<FreakFiresCopy>();

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {
        }

        internal static float GetFireDisplayIntensity()
        {
            return (PrefManager?.Get<int>(FIRE_DISPLAY_INTENSITY_ID) ?? 30f) / 100f;
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // TODO: Uncomment the following if you have an asset bundle.
            // TODO: Also, make sure to set EnableAssetBundleDeploy to 'true' in your ModName.csproj

            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();

            PrefManager = new PreferenceSystemManager(MOD_GUID, MOD_NAME);

            IntArrayGenerator intArrayGenerator = new IntArrayGenerator();
            intArrayGenerator.AddRange(0, 100, 10, null, delegate (string prefKey, int value)
            {
                return $"{value}%";
            });
            int[] zeroToHundredPercentValues = intArrayGenerator.GetArray();
            string[] zeroToHundredPercentStrings = intArrayGenerator.GetStrings();
            intArrayGenerator.Clear();

            intArrayGenerator.AddRange(10, 100, 10, null, delegate (string prefKey, int value)
            {
                return $"{value}%";
            });
            int[] tenToHundredPercentValues = intArrayGenerator.GetArray();
            string[] tenToHundredPercentStrings = intArrayGenerator.GetStrings();
            intArrayGenerator.Clear();

            intArrayGenerator.Add(-1, "Never");
            intArrayGenerator.AddRange(5, 20, 5, null, delegate (string prefKey, int value)
            {
                return $"{value} seconds";
            });
            int[] destroyItemDelayValues = intArrayGenerator.GetArray();
            string[] destroyItemDelayStrings = intArrayGenerator.GetStrings();
            intArrayGenerator.Clear();

            PrefManager
                .AddLabel("Fire Display Intensity")
                .AddOption<int>(
                    FIRE_DISPLAY_INTENSITY_ID,
                    30,
                    tenToHundredPercentValues,
                    tenToHundredPercentStrings, delegate (int value)
                    {
                        CustomFireItem.UpdateFireIntensity(value / 100f);
                    })
                .AddSpacer()
                .AddSpacer();

            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.PauseMenu);

            HashSet<int> addCatchFireOnFailurePyromaniaAppliances = new HashSet<int>()
            {
                ApplianceReferences.HobStarting,
                ApplianceReferences.Hob,
                //ApplianceReferences.HobSafe,
                ApplianceReferences.Oven
            };

            HashSet<int> addFireImmuneAppliances = new HashSet<int>()
            {
                ApplianceReferences.Nameplate,
                ApplianceReferences.WheelieBin
            };

            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                foreach (int applianceID in addCatchFireOnFailurePyromaniaAppliances)
                {
                    if (!args.gamedata.TryGet(applianceID, out Appliance appliance, warn_if_fail: true))
                        continue;
                    if (appliance.Properties.Select(x => x.GetType()).Contains(typeof(CCatchFireOnFailurePyromania)))
                        continue;
                    appliance.Properties.Add(new CCatchFireOnFailurePyromania());
                }

                foreach (int applianceID in addFireImmuneAppliances)
                {
                    if (!args.gamedata.TryGet(applianceID, out Appliance appliance, warn_if_fail: true))
                        continue;
                    if (appliance.Properties.Select(x => x.GetType()).Contains(typeof(CFireImmune)))
                        continue;
                    appliance.Properties.Add(new CFireImmune());
                }

                if (args.gamedata.TryGet(UnlockReferences.QuickerBurning, out UnlockCard highStandardsUnlock))
                {
                    if (!highStandardsUnlock.Effects.Where(x => x.GetType() == typeof(GlobalEffect)).Cast<GlobalEffect>().Select(x => x.EffectType.GetType()).Contains(typeof(CFlammableItemsModifier)))
                    {
                        highStandardsUnlock.Effects.Add(new GlobalEffect()
                        {
                            EffectCondition = new CEffectAlways(),
                            EffectType = new CFlammableItemsModifier()
                            {
                                BurnSpeedChange = 1f
                            }
                        });
                    }
                }
            };
        }
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
