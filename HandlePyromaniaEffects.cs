using Kitchen;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace KitchenInferno
{
    public class HandlePyromaniaEffects : RestaurantSystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct SExtraShopBlueprints : IComponentData, IModComponent { }

        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnUpdate()
        {
            if (!HasStatus(Main.PYROMANIA_EFFECT_STATUS))
            {
                if (TryGetSingletonEntity<SExtraShopBlueprints>(out Entity e))
                    EntityManager.DestroyEntity(e);
            }
            else
            {
                if (!Has<SExtraShopBlueprints>())
                {
                    Entity e = EntityManager.CreateEntity();
                    Set<SExtraShopBlueprints>(e);
                    Set(e, new CRemovesShopBlueprint()
                    {
                        Count = -3
                    });
                }
            }
        }
    }
}
