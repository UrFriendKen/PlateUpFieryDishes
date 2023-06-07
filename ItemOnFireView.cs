using Kitchen;
using MessagePack;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.VFX;

namespace KitchenInferno
{
    public class ItemOnFireView : UpdatableObjectView<ItemOnFireView.ViewData>
    {
        public class UpdateView : IncrementalViewSystemBase<ViewData>
        {
            EntityQuery Views;

            protected override void Initialise()
            {
                base.Initialise();
                Views = GetEntityQuery(typeof(CItem), typeof(CLinkedView));
            }

            protected override void OnUpdate()
            {
                using NativeArray<Entity> entities = Views.ToEntityArray(Allocator.Temp);
                using NativeArray<CLinkedView> views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);

                for (int i = 0; i < entities.Length; i++)
                {
                    Entity entity = entities[i];
                    CLinkedView view = views[i];

                    SendUpdate(view, new ViewData()
                    {
                        FireActive = Has<CItemOnFire>(entity)
                    });
                }
            }
        }

        public VisualEffect FireVfx;

        [MessagePackObject(false)]
        public class ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)] public bool FireActive;

            public bool IsChangedFrom(ViewData check)
            {
                return FireActive != check.FireActive;
            }

            public IUpdatableObject GetRelevantSubview(IObjectView view)
            {
                return view.GetSubView<ItemOnFireView>();
            }
        }

        protected override void UpdateData(ViewData data)
        {
            FireVfx?.SetFloat("Active", data.FireActive ? Main.GetFireDisplayIntensity() : 0f);
        }
    }
}
