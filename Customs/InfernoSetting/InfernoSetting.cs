using KitchenData;
using KitchenInferno.Customs.InfernoSetting;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenInferno.Customs.Inferno
{
    public class InfernoSetting : CustomRestaurantSetting
    {
        public override int BaseGameDataObjectID => 2002876295;
        public override string UniqueNameID => "inferno";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Inferno Snowglobe");//(GDOUtils.GetExistingGDO(AssetReference.HalloweenSetting) as RestaurantSetting).Prefab;
        public override List<(Locale, BasicInfo)> InfoList => new List<(Locale, BasicInfo)>()
        {
            (Locale.English, new BasicInfo()
            {
                Locale = Locale.English,
                Name = "Inferno"
            })
        };
        public override WeatherMode WeatherMode => WeatherMode.None;
        public override List<IDecorationConfiguration> Decorators => new List<IDecorationConfiguration>()
        {
            new InfernoDecorator.DecorationsConfiguration()
            {
                Scatters = new List<InfernoDecorator.DecorationsConfiguration.Scatter>()
                {
                    new InfernoDecorator.DecorationsConfiguration.Scatter()
                    {
                        Probability = 0.15f,
                        Appliance = GDOUtils.GetCastedGDO<Appliance, LavaRock>()
                    },
                    new InfernoDecorator.DecorationsConfiguration.Scatter()
                    {
                        Probability = 0.15f,
                        Appliance = GDOUtils.GetCastedGDO<Appliance, LavaRockNarrow>()
                    },
                    new InfernoDecorator.DecorationsConfiguration.Scatter()
                    {
                        Probability = 0.15f,
                        Appliance = GDOUtils.GetCastedGDO<Appliance, LavaRockLarge>()
                    }
                },
                FrontTile = GDOUtils.GetCastedGDO<Appliance, InfernoFloor>(),
                Bridge = GDOUtils.GetCastedGDO<Appliance, InfernoBridge>(),
                FrontWall = GDOUtils.GetCastedGDO<Appliance, InfernoWall>(),
                FrontPillar = GDOUtils.GetCastedGDO<Appliance, InfernoPillar>(),
                Ground = GDOUtils.GetCastedGDO<Appliance, LavaGround>(),
                BorderSpacing = 1f
            }
        };
        public override Unlock StartingUnlock => GDOUtils.GetCastedGDO<Unlock, PyromaniaUnlock>();
        public override UnlockPack UnlockPack => GDOUtils.GetCastedGDO<CompositeUnlockPack, InfernoCompositeUnlockPack>();

        public override void OnRegister(RestaurantSetting gameDataObject)
        {
            MaterialUtils.ApplyMaterial(Prefab, "Volcano/Surface", new Material[] { MaterialUtils.GetExistingMaterial("Rock") });
            MaterialUtils.ApplyMaterial(Prefab, "Volcano/Lava", new Material[] { MaterialUtils.GetCustomMaterial("Inferno Lava") });
            MaterialUtils.ApplyMaterial(Prefab, "Volcano/Base", new Material[] { MaterialUtils.GetCustomMaterial("Dark Rock") });
        }
    }
}
