using Kitchen;
using KitchenData;
using KitchenMods;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using static KitchenInferno.DestroyItemOnFireAfterDuration;

namespace KitchenInferno
{
    [UpdateInGroup(typeof(HighPriorityInteractionGroup))]
    public class SetItemOnFire : DaySystem, IModSystem
    {
        EntityQuery AppliancesOnFire;
        EntityQuery FlammableItemModifiers;
        EntityQuery FireOrderChances;

        protected override void Initialise()
        {
            base.Initialise();
            AppliancesOnFire = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliance), typeof(CItemHolder), typeof(CIsOnFire)));
            FlammableItemModifiers = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliesEffect), typeof(CFlammableItemsModifier)));
            FireOrderChances = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliesEffect), typeof(CFireOrderChance)));
        }

        protected override void OnUpdate()
        {
            if (AppliancesOnFire.IsEmpty || FireOrderChances.IsEmpty)
                return;

            float burnSpeed = 1f;
            using NativeArray<CAppliesEffect> appliesEffects = FlammableItemModifiers.ToComponentDataArray<CAppliesEffect>(Allocator.Temp);
            using NativeArray<CFlammableItemsModifier> flammableItemModifiers = FlammableItemModifiers.ToComponentDataArray<CFlammableItemsModifier>(Allocator.Temp);
            for (int i = 0; i < appliesEffects.Length; i++)
            {
                CAppliesEffect apply = appliesEffects[i];
                CFlammableItemsModifier modifier = flammableItemModifiers[i];
                if (!apply.IsActive)
                    continue;
                burnSpeed *= 1f + modifier.BurnSpeedChange;
            }
            if (burnSpeed < 0f)
                burnSpeed = 0f;

            using NativeArray<CItemHolder> holders = AppliancesOnFire.ToComponentDataArray<CItemHolder>(Allocator.Temp);
            for (int i = 0; i < holders.Length; i++)
            {
                CItemHolder holder = holders[i];
                if (holder.HeldItem == default || Has<CItemOnFire>(holder.HeldItem) || Has<CFireImmuneMenuItem>(holder.HeldItem) ||
                    Has<CHasBeenDestroyed>(holder.HeldItem) || !Require(holder.HeldItem, out CItem item) || !CanBeSetOnFire(item))
                    continue;

                Set(holder.HeldItem, new CItemOnFire()
                {
                    BurnSpeed = burnSpeed
                });

                Set(holder.HeldItem, new CDestroyItemOnFireDuration()
                {
                    TotalTime = Main.BASE_FOOD_DESTROY_TIME
                });
            }
        }

        private bool CanBeSetOnFire(int itemID)
        {
            return GameData.Main.Get<Dish>().SelectMany(x => x.UnlocksMenuItems).Where(x => x.Phase != MenuPhase.Side).Select(x => x.Item.ID).Distinct().Where(x => x != 0).Contains(itemID);
        }
    }
}
