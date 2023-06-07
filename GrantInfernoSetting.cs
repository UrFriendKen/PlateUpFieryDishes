using Kitchen;
using KitchenInferno.Customs.Inferno;
using KitchenLib.Utils;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateBefore(typeof(GrantUpgrades))]
    public class GrantInfernoSetting : FranchiseFirstFrameSystem, IModSystem
    {
        public static List<int> SettingOptionsToAdd = new List<int> { GDOUtils.GetCustomGameDataObject<InfernoSetting>().ID };

        private EntityQuery SettingUpgrades;

        protected override void Initialise()
        {
            base.Initialise();
            SettingUpgrades = GetEntityQuery(typeof(CSettingUpgrade));
        }

        protected override void OnUpdate()
        {
            foreach (int settingOptionToAdd in SettingOptionsToAdd)
            {
                bool shouldAdd = true;
                using NativeArray<Entity> settingUpgrades = SettingUpgrades.ToEntityArray(Allocator.Temp);
                foreach (Entity settingUpgrade in settingUpgrades)
                {
                    if (base.EntityManager.GetComponentData<CSettingUpgrade>(settingUpgrade).SettingID == settingOptionToAdd)
                    {
                        shouldAdd = false;
                        break;
                    }
                }

                if (shouldAdd)
                {
                    Entity entity = base.EntityManager.CreateEntity(typeof(CSettingUpgrade), typeof(CPersistThroughSceneChanges));
                    base.EntityManager.SetComponentData(entity, new CSettingUpgrade
                    {
                        SettingID = settingOptionToAdd
                    });
                }
            }
        }
    }

}
