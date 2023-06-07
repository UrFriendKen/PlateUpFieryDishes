using Kitchen;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateInGroup(typeof(PostTransitionGroup))]
    public class MarkNewInfernoRestaurant : RestaurantInitialisationSystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct SInfernoRestaurant : IComponentData, IModComponent { }

        protected override void OnUpdate()
        {
            if (TryGetSingletonEntity<SSceneData>(out Entity sceneDataEntity) && Require(sceneDataEntity, out CSetting setting) &&
                Main.CustomInfernoSetting.GameDataObject != null && setting.RestaurantSetting == Main.CustomInfernoSetting.GameDataObject.ID)
            {
                Set<SInfernoRestaurant>();
            }
        }
    }

    public abstract class InfernoRestaurantSystem : RestaurantSystem
    {
        protected override void Initialise()
        {
            base.Initialise();
            RequireSingletonForUpdate<MarkNewInfernoRestaurant.SInfernoRestaurant>();
        }
    }
}
