using Kitchen;
using KitchenData;
using KitchenInferno.Customs.InfernoSetting;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenInferno.Customs.Inferno
{
    public class InfernoSetting : CustomRestaurantSetting
    {
        public override int BaseGameDataObjectID => 2002876295;
        public override string UniqueNameID => "inferno";
        public override GameObject Prefab => (GDOUtils.GetExistingGDO(AssetReference.HalloweenSetting) as RestaurantSetting).Prefab;
        public override List<(Locale, BasicInfo)> InfoList => new List<(Locale, BasicInfo)>()
        {
            (Locale.English, new BasicInfo()
            {
                Locale = Locale.English,
                Name = "Inferno"
            })
        };
        public override WeatherMode WeatherMode => WeatherMode.Wind;
        public override List<IDecorationConfiguration> Decorators => new List<IDecorationConfiguration>()
        {
            new InfernoDecorator.DecorationsConfiguration()
            {
                Scatters = new List<InfernoDecorator.DecorationsConfiguration.Scatter>()
                {
                    new InfernoDecorator.DecorationsConfiguration.Scatter()
                    {
                        Probability = 1,
                        Appliance = GDOUtils.GetExistingGDO(ApplianceReferences.HalloweenTrees) as Appliance
                    }
                },
                FrontTile = GDOUtils.GetExistingGDO(ApplianceReferences.HalloweenFloor) as Appliance,
                Bridge = GDOUtils.GetExistingGDO(ApplianceReferences.HalloweenBridge) as Appliance,
                Fog = GDOUtils.GetExistingGDO(ApplianceReferences.HalloweenFog) as Appliance,
                FrontWall = GDOUtils.GetExistingGDO(ApplianceReferences.HalloweenWall) as Appliance,
                FrontPillar = GDOUtils.GetExistingGDO(ApplianceReferences.HalloweenPillar) as Appliance,
                BorderSpacing = 1f
            }
        };
        public override Unlock StartingUnlock => GDOUtils.GetCastedGDO<Unlock, PyromaniaUnlock>();
        public override UnlockPack UnlockPack => GDOUtils.GetCastedGDO<CompositeUnlockPack, InfernoCompositeUnlockPack>();
    }
}
