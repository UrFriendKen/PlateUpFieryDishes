using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{

    [UpdateInGroup(typeof(UpdateCustomerStatesGroup))]
    public class CustomersSetTablesOnFireWhenLeaving : GameSystemBase, IModSystem
    {
        EntityQuery Groups;

        protected override void Initialise()
        {
            base.Initialise();
            Groups = GetEntityQuery(new QueryHelper()
                .All(typeof(CGroupStartLeaving), typeof(CGroupMember), typeof(CCustomerSettings), typeof(CAssignedTable)));
        }

        protected override void OnUpdate()
        {
            if (!HasStatus(Main.PYRO_PATRONS_EFFECT_STATUS))
            {
                return;
            }
            using NativeArray<CAssignedTable> tables = Groups.ToComponentDataArray<CAssignedTable>(Allocator.Temp);

            for (int i = 0; i < tables.Length; i++)
            {
                if (!RequireBuffer(tables[i].Table, out DynamicBuffer<CTableSetParts> tablePartsBuffer))
                {
                    return;
                }
                foreach (CTableSetParts tablePart in tablePartsBuffer)
                {
                    if (Has<CAppliance>(tablePart))
                    {
                        Main.LogInfo("Set Fire");
                        Set<CIsOnFire>(tablePart);
                        break;
                    }
                }
            }
        }
    }
}
