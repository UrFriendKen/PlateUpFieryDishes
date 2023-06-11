using Kitchen;
using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateInGroup(typeof(EndOfDayProgressionGroup))]
    [UpdateAfter(typeof(CheckSellingRequiredAppliance))]
    public class CheckSellingIgniteItemAppliances : RestaurantSystem, IModSystem
    {
        EntityQuery FireOrders;
        EntityQuery CurrentAppliances;

        protected override void Initialise()
        {
            base.Initialise();
            FireOrders = GetEntityQuery(typeof(CFireOrderChance));
            CurrentAppliances = GetEntityQuery(new QueryHelper().All(typeof(CAppliance)).None(typeof(CDestroyApplianceAtDay)));
        }

        protected override void OnUpdate()
        {
            int igniteItemProcessID = Main.IgniteItemProcess?.GameDataObject?.ID ?? 0;
            if (FireOrders.IsEmpty || igniteItemProcessID == 0)
                return;

            using NativeArray<CAppliance> appliances = CurrentAppliances.ToComponentDataArray<CAppliance>(Allocator.Temp);
            foreach (CAppliance cAppliance in appliances)
            {
                if (!Data.TryGet<Appliance>(cAppliance, out var appliance, warn_if_fail: true))
                {
                    continue;
                }
                foreach (Appliance.ApplianceProcesses process in appliance.Processes)
                {
                    if (process.Validity != ProcessValidity.DoesNotRegister && process.Process.ID == igniteItemProcessID)
                    {
                        return;
                    }
                }
            }
            Set<CheckSellingRequiredAppliance.SWarning>();
        }
    }
}
