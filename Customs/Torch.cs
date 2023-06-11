using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using static KitchenLib.Utils.KitchenPropertiesUtils;

namespace KitchenInferno.Customs
{
    public class TorchProvider : CustomAppliance
    {
        public override string UniqueNameID => "torchProvider";

        private const int SUPPLY_COUNT = 5;
        public override GameObject Prefab => PrefabGenerator.CreateConsumableProviderPrefab("Torch Provider", 5);
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>()
        {
            GetCItemProvider(GDOUtils.GetCastedGDO<Item, Torch>()?.ID ?? 0, SUPPLY_COUNT, SUPPLY_COUNT, false, false, false, false, false, false, true),
            new CSellRequiresFireOrder()
        };
        public override List<Appliance.ApplianceProcesses> Processes => new List<Appliance.ApplianceProcesses>()
        {
            new Appliance.ApplianceProcesses()
            {
                Process = GDOUtils.GetCastedGDO<Process, IgniteItemProcess>(),
                IsAutomatic = false,
                Speed = 1f,
                Validity = ProcessValidity.Generic
            }
        };
        public override PriceTier PriceTier => PriceTier.Medium;
        public override bool IsPurchasable => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.Cooking;
        public override RarityTier RarityTier => RarityTier.Common;
        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>()
        {
            (Locale.English, new ApplianceInfo()
            {
                Name = "Torches",
                Description = "Hot, hot, hot!",
                Sections = new List<Appliance.Section>()
                {
                    new Appliance.Section()
                    {
                        Title = "Arson",
                        Description = "Interact to set appliance on fire",
                        RangeDescription = $"Supply: {SUPPLY_COUNT}"
                    }
                },
                Locale = Locale.English,
            })
        };
    }

    public class Torch : CustomItem
    {
        public override string UniqueNameID => "torch";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Torch");
        public override List<IItemProperty> Properties => new List<IItemProperty>()
        {
            new CFireStarter()
            {
                Charges = 1
            }
        };
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, TorchProvider>();
        public override ToolAttachPoint HoldPose => ToolAttachPoint.Hand;
        public override void SetupPrefab(GameObject prefab)
        {
            MaterialUtils.ApplyMaterial(prefab, "Stick", new Material[] { MaterialUtils.GetExistingMaterial("Wood - Default") });
            MaterialUtils.ApplyMaterial(prefab, "Grip", new Material[] { MaterialUtils.GetExistingMaterial("Cloth - Blue") });
            MaterialUtils.ApplyMaterial(prefab, "Head", new Material[] { MaterialUtils.GetExistingMaterial("Cloth - Cheap") });
        }
    }
}
