using Kitchen;
using KitchenData;
using KitchenInferno.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateInGroup(typeof(DestructionGroup))]
    public class DestroyItemOnFireAfterDuration : DaySystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct CHasBeenDestroyed : IComponentData, IModComponent { }

        EntityQuery DestroyableItems;
        EntityQuery DestroyedItemsToBeReset;
        Dictionary<int, Item> SpecialDestroyedItems;

        protected override void Initialise()
        {
            base.Initialise();
            DestroyableItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CItem), typeof(CItemOnFire), typeof(CDestroyItemOnFireDuration)));

            DestroyedItemsToBeReset = GetEntityQuery(new QueryHelper()
                .All(typeof(CHasBeenDestroyed))
                .None(typeof(CChangeItemType)));

            SpecialDestroyedItems = new Dictionary<int, Item>()
            {
                { ItemReferences.PlateDirty, GDOUtils.GetCastedGDO<Item, DirtyPlateWithBurnedFood>() },
                { ItemReferences.PlateDirtywithBone, GDOUtils.GetCastedGDO<Item, DirtyPlateWithBurnedFood>() }
            };
        }

        protected override void OnUpdate()
        {
            EntityManager.RemoveComponent<CHasBeenDestroyed>(DestroyedItemsToBeReset);

            using NativeArray<Entity> entities = DestroyableItems.ToEntityArray(Allocator.Temp);
            using NativeArray<CItem> items = DestroyableItems.ToComponentDataArray<CItem>(Allocator.Temp);
            using NativeArray<CItemOnFire> onFires = DestroyableItems.ToComponentDataArray<CItemOnFire>(Allocator.Temp);
            using NativeArray<CDestroyItemOnFireDuration> destroyDurations = DestroyableItems.ToComponentDataArray<CDestroyItemOnFireDuration>(Allocator.Temp);

            float dt = Time.DeltaTime;

            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                CItem item = items[i];
                CItemOnFire onFire = onFires[i];
                CDestroyItemOnFireDuration destroyDuration = destroyDurations[i];
                if (onFire.BurningDuration > destroyDuration.TotalTime)
                {
                    EntityManager.RemoveComponent<CItemOnFire>(entity);
                    EntityManager.RemoveComponent<CDestroyItemOnFireDuration>(entity);
                    Set<CHasBeenDestroyed>(entity);
                    Set(entity, new CChangeItemType()
                    {
                        NewID = DetermineDestroyedItemID(item)
                    });
                    continue;
                }
                onFire.BurningDuration += dt * onFire.BurnSpeed;
                Set(entity, onFire);
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
