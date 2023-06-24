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

    public class ConstantWildfires : RestaurantSystem, IModSystem
    {
        private const float MINIMUM_INTERVAL = 25f;
        private const float INTERVAL_RANGE = 18.75f;
        private const float DAY_DECAY_RATE = 0.01f;
        private const float DAY_FACTOR_LIMIT = 0.7f;
        private const float PLAYER_FACTOR_EFFECT = 0.4f;

        private const float PERCENT_AFFECTED = 0.4f;
        private readonly HashSet<int> AffectedAppliances = new HashSet<int>()
        {
                ApplianceReferences.HobStarting,
                ApplianceReferences.Hob,
                ApplianceReferences.HobSafe,
                ApplianceReferences.HobDanger,
                ApplianceReferences.Oven,
                ApplianceReferences.Microwave,

                ApplianceReferences.Mixer,
                ApplianceReferences.MixerPusher,
                ApplianceReferences.MixerRapid,
                ApplianceReferences.MixerHeated,

                ApplianceReferences.SinkStarting,
                ApplianceReferences.SinkNormal,
                ApplianceReferences.SinkSoak,
                ApplianceReferences.SinkPower,
                ApplianceReferences.SinkLarge,
                ApplianceReferences.DishWasher
        };

        EntityQuery Players;
        EntityQuery FlammableAppliances;
        EntityQuery WildfiresTimeTracker;

        protected override void Initialise()
        {
            base.Initialise();
            Players = GetEntityQuery(typeof(CPlayer));
            FlammableAppliances = GetEntityQuery(new QueryHelper().All(typeof(CAppliance), typeof(CIsInteractive)).None(typeof(CFireImmune), typeof(CApplianceTable), typeof(CApplianceChair), typeof(CIsOnFire)));
            WildfiresTimeTracker = GetEntityQuery(typeof(SWildfiresTimeTracker));
        }

        protected override void OnUpdate()
        {
            if (!HasStatus(Main.WILDFIRES_EFFECT_STATUS) || Has<SIsNightTime>())
            {
                EntityManager.DestroyEntity(WildfiresTimeTracker);
                return;
            }

            float totalTime = base.Time.TotalTime;

            bool isInit = Require(out SWildfiresTimeTracker timeTracker);
            if (isInit && totalTime - timeTracker.LastTime < timeTracker.Delay)
            {
                return;
            }

            int players = Players.CalculateEntityCount();
            int day = GetOrDefault<SDay>().Day - (Has<SIsDayTime>() ? 1 : 0);
            float minDayDelay = ((1 - DAY_FACTOR_LIMIT) * Mathf.Exp(-DAY_DECAY_RATE * day) + DAY_FACTOR_LIMIT) * MINIMUM_INTERVAL;
            float additionalDelay = Mathf.Pow(Random.value, PLAYER_FACTOR_EFFECT * (players - 1) + 1f) * INTERVAL_RANGE;

            timeTracker.LastTime = totalTime;
            timeTracker.Delay = minDayDelay + additionalDelay;

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
