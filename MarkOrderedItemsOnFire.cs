using Kitchen;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenInferno
{
    public class MarkOrderedItemsOnFire : DaySystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct CPerformed : IComponentData, IModComponent { }

        EntityQuery RequestedItems;
        EntityQuery FireOrderChances;

        protected override void Initialise()
        {
            base.Initialise();

            RequestedItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CItem), typeof(CRequestItemOf))
                .None(typeof(CPerformed)));
            FireOrderChances = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliesEffect), typeof(CFireOrderChance)));
        }

        protected override void OnUpdate()
        {
            if (FireOrderChances.IsEmpty || RequestedItems.IsEmpty)
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

            Main.LogInfo(chance);
            using NativeArray<Entity> entities = RequestedItems.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                if (Random.value < chance || chance == 1f)
                {
                    Set(entity, default(CItemOnFire));
                }
                Set<CPerformed>(entity);
            }
        }
    }
}
