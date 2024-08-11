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
using static Kitchen.ItemGroupView;

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
        public const string MOD_VERSION = "0.4.12";
        public const string MOD_AUTHOR = "IcedMilo";
        public const string MOD_GAMEVERSION = ">=1.1.5";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until

        public static AssetBundle Bundle;

        public Main() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        internal static FireItem CustomFireItem;
        internal static DummyFireOrderAppliance CustomDummyFireOrderAppliance;
        internal static DummyActiveFireAppliance CustomDummyActiveFireAppliance;
        internal static InfernoSetting CustomInfernoSetting;
        internal static IgniteItemProcess IgniteItemProcess;

        internal static PreferenceSystemManager PrefManager;
        public const string FIRE_DISPLAY_INTENSITY_ID = "fireDisplayIntensity";
        public const string DIFFICULTY_FIRE_EXTINGUISHER_START_ID = "difficultyFireExtinguisherStart";
        public const string DIFFICULTY_FIRE_EXTINGUISHER_HOLD_ID = "difficultyFireExtinguisherHold";

        public static readonly RestaurantStatus PYROMANIA_EFFECT_STATUS = (RestaurantStatus)VariousUtils.GetID("pyromaniaEffect");
        public static readonly RestaurantStatus WILDFIRES_EFFECT_STATUS = (RestaurantStatus)VariousUtils.GetID("wildfiresEffect");
        public static readonly RestaurantStatus PYRO_PATRONS_EFFECT_STATUS = (RestaurantStatus)VariousUtils.GetID("newPyroPatronsEffect");

        internal const float BASE_FOOD_DESTROY_TIME = 5f;

        private static readonly FieldInfo fSubviewContainer = ReflectionUtils.GetField<ItemGroupView>("SubviewContainer");
        private static readonly FieldInfo fSubviewPrefab = ReflectionUtils.GetField<ItemGroupView>("SubviewPrefab");
        private static readonly FieldInfo fComponentGroups = ReflectionUtils.GetField<ItemGroupView>("ComponentGroups");

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            CustomFireItem = AddGameDataObject<FireItem>();
            CustomDummyFireOrderAppliance = AddGameDataObject<DummyFireOrderAppliance>();
            CustomDummyActiveFireAppliance = AddGameDataObject<DummyActiveFireAppliance>();

            AddGameDataObject<DirtyPlateWithBurnedFood>();

            AddGameDataObject<FlamingMeals>();
            AddGameDataObject<FlamingMealsForced>();
            AddGameDataObject<PyromaniaUnlock>();
            AddGameDataObject<InfernoSpecialCard>();
            AddGameDataObject<InfernoCompositeUnlockPack>();
            CustomInfernoSetting = AddGameDataObject<InfernoSetting>();
            AddGameDataObject<InfernoBridge>();
            AddGameDataObject<InfernoWall>();
            AddGameDataObject<InfernoPillar>();
            AddGameDataObject<InfernoFloor>();
            AddGameDataObject<LavaGround>();
            AddGameDataObject<LavaRock>();
            AddGameDataObject<LavaRockNarrow>();
            AddGameDataObject<LavaRockLarge>();

            AddGameDataObject<PyroPatronsCopy>();
            AddGameDataObject<Wildfires>();

            IgniteItemProcess = AddGameDataObject<IgniteItemProcess>();
            AddGameDataObject<Torch>();
            AddGameDataObject<TorchProvider>();

            AddGameDataObject<BurntFoodUnlock>();

            LogInfo("Done loading game data.");
        }

        private void AddMaterials()
        {
            AddCustomMaterial<InfernoSurfaceMaterial>();
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
            AddMaterials();
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

                .AddSubmenu("Difficulty Settings", "difficultySettings")
                    .AddLabel("Start with Fire Extinguisher")
                    .AddOption<bool>(
                        DIFFICULTY_FIRE_EXTINGUISHER_START_ID,
                        true,
                        new bool[] { false, true },
                        new string[] { "Disabled", "Enabled" })
                    .AddLabel("Hold items with Fire Extinguisher")
                    .AddInfo("Requires game restart")
                    .AddOption<bool>(
                        DIFFICULTY_FIRE_EXTINGUISHER_HOLD_ID,
                        true,
                        new bool[] { false, true },
                        new string[] { "Disabled", "Enabled" })
                    .AddSpacer()
                    .AddSpacer()
                .SubmenuDone()
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

            HashSet<int> addIgniteItemProcessAppliances = new HashSet<int>()
            {
                ApplianceReferences.HobStarting,
                ApplianceReferences.Hob,
                ApplianceReferences.HobDanger,
                ApplianceReferences.Oven,
                ApplianceReferences.Microwave
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

                if (PrefManager.Get<bool>(DIFFICULTY_FIRE_EXTINGUISHER_HOLD_ID) && args.gamedata.TryGet(ItemReferences.FireExtinguisher, out Item fireExtinguisher, warn_if_fail: true))
                {
                    for (int i = 0; i < fireExtinguisher.Properties.Count; i++)
                    {
                        if (!(fireExtinguisher.Properties[i] is CEquippableTool equippableTool))
                            continue;
                        equippableTool.CanHoldItems = true;
                        fireExtinguisher.Properties[i] = equippableTool;
                        break;
                    }
                }

                Process igniteItemProcessGDO = IgniteItemProcess?.GameDataObject;
                if (igniteItemProcessGDO != null)
                {
                    int igniteItemProcessID = igniteItemProcessGDO.ID;
                    foreach (int applianceID in addIgniteItemProcessAppliances)
                    {
                        if (!args.gamedata.TryGet(applianceID, out Appliance appliance, warn_if_fail: true))
                            continue;
                        if (appliance.Processes.Select(x => x.Process.ID).Contains(igniteItemProcessID))
                            continue;
                        appliance.Processes.Add(new Appliance.ApplianceProcesses()
                        {
                            Process = igniteItemProcessGDO,
                            IsAutomatic = false,
                            Speed = 1f,
                            Validity = ProcessValidity.Generic
                        });
                    }
                }

                // Used with base game burned food
                Item burnedFood = (Item)GDOUtils.GetExistingGDO(ItemReferences.BurnedFood);
                if (burnedFood != null)
                {
                    //burnedFood.IsMergeableSide = true;
                    //AddBaseGameItemPossibleSide(burnedFood);
                    burnedFood.ItemStorageFlags |= ItemStorage.StackableFood;
                    if (!burnedFood.Properties.Select(x => x.GetType()).Contains(typeof(CFireImmuneMenuItem)))
                        burnedFood.Properties.Add(new CFireImmuneMenuItem());
                }

                


                void AddBaseGameItemPossibleSide(Item item)
                {
                    ItemGroup itemGroup = args.gamedata.Get<ItemGroup>(ItemReferences.BurgerPlated);
                    ItemGroupView itemGroupView = itemGroup.Prefab.GetComponent<ItemGroupView>();
                    GameObject subviewPrefab = (GameObject)fSubviewPrefab.GetValue(itemGroupView);
                    ItemGroupView subviewItemGroupView = subviewPrefab.GetComponent<ItemGroupView>();
                    List<ComponentGroup> componentGroups = (List<ComponentGroup>)fComponentGroups.GetValue(subviewItemGroupView);
                    if (!componentGroups.Select(x => x.Item.ID).Contains(item.ID))
                    {
                        GameObject itemPrefabCopy = UnityEngine.Object.Instantiate(item.Prefab);
                        Transform itemPrefabTransform = itemPrefabCopy.transform;
                        itemPrefabTransform.parent = subviewPrefab.transform;
                        itemPrefabTransform.localPosition = Vector3.zero;
                        ItemGroupView.ComponentGroup componentGroup = new ItemGroupView.ComponentGroup()
                        {
                            GameObject = itemPrefabCopy,
                            Item = item
                        };
                        componentGroups.Add(componentGroup);
                        Main.LogInfo($"Added item prefab to side registry for item {item.ID} ({((UnityEngine.Object)(object)item).name}).");
                    }
                }

                if (CustomInfernoSetting?.GameDataObject != null)
                {
                    CustomSettingsAndLayouts.Registry.GrantCustomSetting(CustomInfernoSetting?.GameDataObject);
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
