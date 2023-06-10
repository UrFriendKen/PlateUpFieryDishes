using Kitchen;
using KitchenLib.References;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateInGroup(typeof(ApplyEffectsGroup))]
    public class CreateMassFires : DaySystem, IModSystem
    {
        private EntityQuery ScheduledMassFires;
        private EntityQuery Appliances;

        private readonly HashSet<int> AffectedAppliances = new HashSet<int>()
        {
                ApplianceReferences.HobStarting,
                ApplianceReferences.Hob,
                ApplianceReferences.HobSafe,
                ApplianceReferences.HobDanger,
                ApplianceReferences.Oven,
                ApplianceReferences.Microwave
        };

        protected override void Initialise()
        {
            base.Initialise();
            ScheduledMassFires = GetEntityQuery(typeof(CScheduledMassFire));
            Appliances = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliance))
                .None(typeof(CFireImmune), typeof(CIsOnFire), typeof(CNoBadProcesses)));
        }

        protected override void OnUpdate()
        {
            STime singleton = GetOrDefault<STime>();
            using NativeArray<Entity> entities = ScheduledMassFires.ToEntityArray(Allocator.Temp);
            using NativeArray<CScheduledMassFire> scheduledMassFires = ScheduledMassFires.ToComponentDataArray<CScheduledMassFire>(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                CScheduledMassFire scheduledMassFire = scheduledMassFires[i];
                if (singleton.TimeOfDayUnbounded > scheduledMassFire.TimeOfDay)
                {
                    NewMassFire();
                    base.EntityManager.DestroyEntity(entity);
                }
            }
        }

        protected void NewMassFire()
        {
            using NativeArray<Entity> entities = Appliances.ToEntityArray(Allocator.Temp);
            using NativeArray<CAppliance> appliances = Appliances.ToComponentDataArray<CAppliance>(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                if (AffectedAppliances.Contains(appliances[i].ID))
                {
                    Set<CIsOnFire>(entities[i]);
                }
            }
        }
    }
}
