using Kitchen;
using KitchenData;
using KitchenInferno.Customs;
using KitchenLib.Utils;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateBefore(typeof(GroupOrderIndicator))]
    [UpdateAfter(typeof(MarkOrderedItemsOnFire))]
    public class AddFireItemComponent : DaySystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct CPerformed : IComponentData, IModComponent { }

        EntityQuery Items;

        protected override void Initialise()
        {
            base.Initialise();
            Items = GetEntityQuery(new QueryHelper()
                .All(typeof(CItem), typeof(CItemOnFire))
                .None(typeof(CPerformed)));
        }

        protected override void OnUpdate()
        {
            using NativeArray<Entity> entities = Items.ToEntityArray(Allocator.Temp);
            using NativeArray<CItem> items = Items.ToComponentDataArray<CItem>(Allocator.Temp);

            int fireItemID = GDOUtils.GetCustomGameDataObject<FireItem>()?.GameDataObject?.ID ?? 0;

            if (fireItemID == 0)
                return;

            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                CItem item = items[i];
                ItemList.ItemComponentEnumerator enumerator = item.Items.GetEnumerator();

                bool shouldAdd = true;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == fireItemID)
                    {
                        shouldAdd = false;
                        break;
                    }
                }

                if (!shouldAdd)
                    continue;

                item.Items.Add(fireItemID);
                Set(entity, item);
            }
        }
    }
}
