using Kitchen;
using KitchenLib.References;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenInferno
{
    public struct SWildfiresTimeTracker : IComponentData, IModComponent
    {
        public float LastTime;

        public float Delay;
    }

    public class ConstantWildfires : DaySystem, IModSystem
    {
        private const float TIME_INTERVAL = 25f;
        private const float PERCENT_AFFECTED = 0.6f;
        private readonly HashSet<int> AffectedAppliances = new HashSet<int>()
        {
                ApplianceReferences.HobStarting,
                ApplianceReferences.Hob,
                ApplianceReferences.HobSafe,
                ApplianceReferences.HobDanger,
                ApplianceReferences.Oven,
                ApplianceReferences.Microwave
        };

        private EntityQuery FlammableAppliances;

        protected override void Initialise()
        {
            base.Initialise();
            FlammableAppliances = GetEntityQuery(new QueryHelper().All(typeof(CAppliance), typeof(CIsInteractive)).None(typeof(CFireImmune), typeof(CApplianceTable), typeof(CApplianceChair), typeof(CIsOnFire)));
        }

        protected override void OnUpdate()
        {
            if (!HasStatus(Main.WILDFIRES_EFFECT_STATUS))
            {
                return;
            }

            bool isInit = Require(out SWildfiresTimeTracker timeTracker);
            float totalTime = base.Time.TotalTime;

            if (isInit && totalTime - timeTracker.LastTime < timeTracker.Delay)
            {
                return;
            }
            timeTracker.LastTime = totalTime;
            timeTracker.Delay = Random.Range(0.75f, 1.5f) * TIME_INTERVAL;

            if (isInit)
            {
                List<int> indexes = new List<int>();
                using NativeArray<CAppliance> appliances = FlammableAppliances.ToComponentDataArray<CAppliance>(Allocator.Temp);
                using NativeArray<Entity> entities = FlammableAppliances.ToEntityArray(Allocator.Temp);
                for (int i = 0; i < appliances.Length; i++)
                {
                    CAppliance appliance = appliances[i];
                    if (AffectedAppliances.Contains(appliance.ID))
                    {
                        indexes.Add(i);
                    }
                }

                indexes.ShuffleInPlace();
                int affectedCount = Mathf.CeilToInt(PERCENT_AFFECTED * indexes.Count);
                for (int i = 0; i < Mathf.Min(affectedCount, indexes.Count); i++)
                {
                    base.EntityManager.AddComponent<CIsOnFire>(entities[indexes[i]]);
                }
            }
            Set(timeTracker);
        }
    }
}
