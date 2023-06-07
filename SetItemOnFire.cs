using Kitchen;
using KitchenData;
using KitchenMods;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace KitchenDishesOnFire
{
    [UpdateBefore(typeof(InteractionGroup))]
    [UpdateAfter(typeof(GroupReceiveItem))]
    public class SetItemOnFire : DaySystem, IModSystem
    {
        EntityQuery AppliancesOnFire;

        protected override void Initialise()
        {
            base.Initialise();
            AppliancesOnFire = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliance), typeof(CItemHolder), typeof(CIsOnFire)));
        }

        protected override void OnUpdate()
        {
            using NativeArray<CItemHolder> holders = AppliancesOnFire.ToComponentDataArray<CItemHolder>(Allocator.Temp);
            for (int i = 0; i < holders.Length; i++)
            {
                CItemHolder holder = holders[i];
                if (holder.HeldItem == default || Has<CItemOnFire>(holder.HeldItem) || !Require(holder.HeldItem, out CItem item) || !CanBeSetOnFire(item))
                    continue;
                Set<CItemOnFire>(holder.HeldItem);

                float startTime = Main.GetDestroyItemDelay();
                Set(holder.HeldItem, new CDestroyItemOnFireDuration()
                {
                    StartTime = startTime,
                    RemainingTime = startTime
                });
            }
        }

        private bool CanBeSetOnFire(int itemID)
        {
            return GameData.Main.Get<Dish>().SelectMany(x => x.UnlocksMenuItems).Select(x => x.Item.ID).Distinct().Where(x => x != 0).Contains(itemID);
        }
    }
}
