using KitchenDishesOnFire.Customs;
using KitchenLib;
using KitchenLib.Event;
using KitchenMods;
using PreferenceSystem;
using PreferenceSystem.Generators;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenDishesOnFire
{
    public class Main : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "IcedMilo.PlateUp.FieryDishes";
        public const string MOD_NAME = "Fiery Dishes";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "IcedMilo";
        public const string MOD_GAMEVERSION = ">=1.1.5";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        public Main() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        internal static FireItem CustomFireItem;

        internal static PreferenceSystemManager PrefManager;
        public const string FIRE_ORDER_CHANCE_ID = "fireOrderChance";
        public const string DESTROY_ITEM_DELAY_ID = "destroyItemDelay";
        public const string FIRE_DISPLAY_INTENSITY_ID = "fireDisplayIntensity";

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            CustomFireItem = AddGameDataObject<FireItem>();

            AddGameDataObject<DirtyPlateWithBurnedFood>();

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {
        }

        internal static float GetDestroyItemDelay()
        {
            int prefValue = PrefManager?.Get<int>(DESTROY_ITEM_DELAY_ID) ?? 10;
            return prefValue == -1 ? float.MaxValue : (float)prefValue;
        }

        internal static float GetFireOrderChance()
        {
            int prefValue = PrefManager?.Get<int>(FIRE_ORDER_CHANCE_ID) ?? 50;
            return (prefValue == 100 ? float.MaxValue : prefValue / 100f);
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

            PrefManager.AddLabel("Fire Order Chance")
                .AddOption<int>(
                    FIRE_ORDER_CHANCE_ID,
                    50,
                    zeroToHundredPercentValues,
                    zeroToHundredPercentStrings)
                .AddLabel("Destroy Item After")
                .AddOption<int>(
                    DESTROY_ITEM_DELAY_ID,
                    10,
                    destroyItemDelayValues,
                    destroyItemDelayStrings)
                .AddSpacer()
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

            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
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
