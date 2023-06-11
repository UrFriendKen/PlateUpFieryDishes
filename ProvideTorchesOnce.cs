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

        protected override void Initialise()
        {
            base.Initialise();
            FireOrders = GetEntityQuery(typeof(CFireOrderChance));
        }

        protected override void OnUpdate()
        {
            if (Has<SPerformed>() || FireOrders.IsEmpty)
                return;
            int torchProviderID = GDOUtils.GetCastedGDO<Appliance, TorchProvider>()?.ID ?? 0;
            if (torchProviderID != 0)
                PostHelpers.CreateApplianceParcel(EntityManager, GetFallbackTile(), torchProviderID);
            Set<SPerformed>();
        }
    }
}
