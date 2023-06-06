using Kitchen;
using KitchenData;
using KitchenDishesOnFire.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace KitchenDishesOnFire
{
    public class DestroyItemOnFireAfterDuration : DaySystem, IModSystem
    {
        EntityQuery DestroyableItems;
        Dictionary<int, Item> SpecialDestroyedItems;

        protected override void Initialise()
        {
            base.Initialise();
            DestroyableItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CItem), typeof(CDestroyItemOnFireDuration)));

            SpecialDestroyedItems = new Dictionary<int, Item>()
            {
                { ItemReferences.PlateDirty, GDOUtils.GetCastedGDO<Item, DirtyPlateWithBurnedFood>() },
                { ItemReferences.PlateDirtywithBone, GDOUtils.GetCastedGDO<Item, DirtyPlateWithBurnedFood>() }
            };
        }

        protected override void OnUpdate()
        {
            using NativeArray<Entity> entities = DestroyableItems.ToEntityArray(Allocator.Temp);
            using NativeArray<CItem> items = DestroyableItems.ToComponentDataArray<CItem>(Allocator.Temp);
            using NativeArray<CDestroyItemOnFireDuration> destroyDurations = DestroyableItems.ToComponentDataArray<CDestroyItemOnFireDuration>(Allocator.Temp);

            float dt = Time.DeltaTime;

            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                CItem item = items[i];
                CDestroyItemOnFireDuration destroyDuration = destroyDurations[i];
                if (destroyDuration.RemainingTime < 0f)
                {
                    if (Has<CItemOnFire>(entity))
                        EntityManager.RemoveComponent<CItemOnFire>(entity);
                    if (Has<CDestroyItemOnFireDuration>(entity))
                        EntityManager.RemoveComponent<CDestroyItemOnFireDuration>(entity);

                    Set(entity, new CChangeItemType()
                    {
                        NewID = DetermineDestroyedItemID(item)
                    });
                    continue;
                }
                destroyDuration.RemainingTime -= dt;
                Set(entity, destroyDuration);
            }
        }

        private int DetermineDestroyedItemID(CItem cItem)
        {
            int result = 0;
            if (GameData.Main.TryGet(cItem, out Item item) && item.DirtiesTo != default)
            {
                if (SpecialDestroyedItems.TryGetValue(item.DirtiesTo.ID, out Item specialDestroyedItem))
                {
                    result = specialDestroyedItem?.ID ?? 0;
                }
                else
                {
                    result = item.DirtiesTo.ID;
                }
            }
            if (result == 0)
            {
                result = ItemReferences.BurnedFood;
            }
            return result;
        }
    }
}
