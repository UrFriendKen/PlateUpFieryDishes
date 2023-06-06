using Kitchen;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenDishesOnFire
{
    public class MarkOrderedItemsOnFire : DaySystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct CPerformed : IComponentData, IModComponent { }

        EntityQuery RequestedItems;

        protected override void Initialise()
        {
            base.Initialise();

            RequestedItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CItem), typeof(CRequestItemOf))
                .None(typeof(CPerformed)));
        }

        protected override void OnUpdate()
        {
            using NativeArray<Entity> entities = RequestedItems.ToEntityArray(Allocator.Temp);

            float chance = Main.GetFireOrderChance();
            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                if (Random.value < chance)
                {
                    Set(entity, default(CItemOnFire));
                }
                Set<CPerformed>(entity);
            }
        }
    }
}
