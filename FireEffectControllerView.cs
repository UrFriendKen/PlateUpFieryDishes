using Kitchen;
using MessagePack;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.VFX;

namespace KitchenInferno
{
    public class UpdateHeldFireStarterEffect : IncrementalViewSystemBase<FireEffectControllerView.ViewData>
    {
        EntityQuery Views;

        protected override void Initialise()
        {
            base.Initialise();
            Views = GetEntityQuery(typeof(CItem), typeof(CFireStarter), typeof(CLinkedView));
        }

        protected override void OnUpdate()
        {
            NativeArray<Entity> entities = Views.ToEntityArray(Allocator.Temp);
            NativeArray<CLinkedView> views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                CLinkedView view = views[i];

                SendUpdate(view, new FireEffectControllerView.ViewData()
                {
                    Active = Has<CHeldBy>(entity)
                });
            }
        }
    }

    public class FireEffectControllerView : UpdatableObjectView<FireEffectControllerView.ViewData>
    {
        [MessagePackObject(false)]
        public class ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)] public bool Active;

            public IUpdatableObject GetRelevantSubview(IObjectView view)
            {
                return view.GetSubView<FireEffectControllerView>();
            }

            public bool IsChangedFrom(ViewData check)
            {
                return Active != check.Active;
            }
        }

        public VisualEffect Fire;
        public ViewData Data;

        protected override void UpdateData(ViewData data)
        {
            Data = data;
        }

        public void Update()
        {
            Fire?.SetFloat("Active", (Data?.Active ?? false) ? Main.GetFireDisplayIntensity() : 0);
        }
    }
}
