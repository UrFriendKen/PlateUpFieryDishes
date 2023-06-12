using Kitchen;
using KitchenData;
using KitchenInferno.Customs;
using KitchenLib.Utils;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace KitchenInferno
{
    public class ProvideTorchesOnce : NightSystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct SPerformed : IComponentData, IModComponent { }

        EntityQuery FireOrders;
        EntityQuery FireStarterProviders;

        protected override void Initialise()
        {
            base.Initialise();
            FireOrders = GetEntityQuery(typeof(CFireOrderChance));
            FireStarterProviders = GetEntityQuery(typeof(CAppliance), typeof(CFireStarterProvider), typeof(CItemProvider));
        }

        protected override void OnUpdate()
        {
            if (Has<SPerformed>())
                return;

            bool isProvided = false;
            if (!FireOrders.IsEmpty)
            {
                int torchProviderID = GDOUtils.GetCastedGDO<Appliance, TorchProvider>()?.ID ?? 0;
                if (torchProviderID != 0)
                    PostHelpers.CreateApplianceParcel(EntityManager, GetFallbackTile(), torchProviderID);
                isProvided = true;
            }
            if (!FireStarterProviders.IsEmpty)
            {
                isProvided = true;
            }
            if (isProvided)
                Set<SPerformed>();
        }
    }
}
