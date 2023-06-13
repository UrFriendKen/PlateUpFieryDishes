using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateInGroup(typeof(ApplyStateChangeEffectsGroup))]
    public class ApplyOrderFramesDelay : DaySystem, IModSystem
    {
        EntityQuery Groups;
        protected override void Initialise()
        {
            base.Initialise();
            Groups = GetEntityQuery(new QueryHelper()
                .Any(typeof(CGroupStateChanged), typeof(COrderFramesDelay)));
        }

        protected override void OnUpdate()
        {
            using NativeArray<Entity> entities = Groups.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];

                if (!Require(entity, out COrderFramesDelay framesDelay) || Has<CGroupStateChanged>(entity))
                {
                    if (Has<CGroupAwaitingOrder>(entity))
                    {
                        Set(entity, new COrderFramesDelay()
                        {
                            Remaining = 2
                        });
                    }
                    continue;
                }
                framesDelay.Remaining -= 1;
                if (framesDelay.Remaining <= 0)
                {
                    EntityManager.RemoveComponent<COrderFramesDelay>(entity);
                    continue;
                }

                Set(entity, framesDelay);
            }
        }
    }
}
