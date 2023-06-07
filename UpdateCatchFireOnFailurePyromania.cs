using Kitchen;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace KitchenInferno
{
    public class UpdateCatchFireOnFailurePyromania : RestaurantSystem, IModSystem
    {
        EntityQuery AppliancesUnmarkedNoBadProcesses;
        EntityQuery AppliancesRestoreNoBadProcesses;
        EntityQuery AppliancesToAdd;
        EntityQuery AppliancesToRemove;

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct CNoBadProcessesPlaceholder : IComponentData, IModComponent { }

        protected override void Initialise()
        {
            base.Initialise();
            AppliancesUnmarkedNoBadProcesses = GetEntityQuery(new QueryHelper()
                .All(typeof(CCatchFireOnFailurePyromania), typeof(CNoBadProcesses))
                .None(typeof(CNoBadProcessesPlaceholder)));
            AppliancesRestoreNoBadProcesses = GetEntityQuery(new QueryHelper()
                .All(typeof(CNoBadProcessesPlaceholder))
                .None(typeof(CNoBadProcesses)));

            AppliancesToAdd = GetEntityQuery(new QueryHelper()
                .All(typeof(CCatchFireOnFailurePyromania))
                .None(typeof(CCatchFireOnFailure)));
            AppliancesToRemove = GetEntityQuery(new QueryHelper()
                .All(typeof(CCatchFireOnFailurePyromania), typeof(CCatchFireOnFailure)));
        }

        protected override void OnUpdate()
        {
            EntityManager.AddComponent<CNoBadProcessesPlaceholder>(AppliancesUnmarkedNoBadProcesses);

            if (HasStatus(Main.PYROMANIA_EFFECT_STATUS))
            {
                EntityManager.RemoveComponent<CNoBadProcesses>(AppliancesToAdd);
                EntityManager.AddComponent<CCatchFireOnFailure>(AppliancesToAdd);
            }
            else
            {
                EntityManager.AddComponent<CNoBadProcesses>(AppliancesRestoreNoBadProcesses);
                EntityManager.RemoveComponent<CCatchFireOnFailure>(AppliancesToRemove);
            }
        }
    }
}
