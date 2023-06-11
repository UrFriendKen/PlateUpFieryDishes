using Kitchen;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenInferno
{
    [UpdateAfter(typeof(AssignMenuRequests))]
    public class MarkOrderedItemsOnFire : DaySystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct CPerformed : IComponentData, IModComponent { }

        EntityQuery FireOrderChances;
        EntityQuery Orders;

        protected override void Initialise()
        {
            base.Initialise();

            Orders = GetEntityQuery(new QueryHelper()
                .All(typeof(CWaitingForItem))
                .None(typeof(CPerformed)));
            FireOrderChances = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliesEffect), typeof(CFireOrderChance)));
        }

        protected override void OnUpdate()
        {
            if (FireOrderChances.IsEmpty || Orders.IsEmpty)
                return;

            float chance = 0f;
            using NativeArray<CAppliesEffect> appliesEffects = FireOrderChances.ToComponentDataArray<CAppliesEffect>(Allocator.Temp);
            using NativeArray<CFireOrderChance> fireOrderChances = FireOrderChances.ToComponentDataArray<CFireOrderChance>(Allocator.Temp);
            for (int i = 0; i < appliesEffects.Length; i++)
            {
                CAppliesEffect apply = appliesEffects[i];
                CFireOrderChance modifier = fireOrderChances[i];
                if (!apply.IsActive)
                    continue;
                chance += (1f - chance) * Mathf.Clamp01(modifier.OrderChance);
            }

            using NativeArray<Entity> entities = Orders.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                DynamicBuffer<CWaitingForItem> orderItems = GetBuffer<CWaitingForItem>(entities[i]);

                for (int j = 0; j < orderItems.Length; j++)
                {
                    Main.LogInfo(orderItems[j].ItemID);
                    if (!orderItems[j].IsSide && (Random.value < chance || chance == 1f))
                    {
                        Main.LogError("Set fire");
                        Set(orderItems[j].Item, default(CItemOnFire));
                    }   
                }
                Set<CPerformed>(entities[i]);
            }
        }
    }
}
