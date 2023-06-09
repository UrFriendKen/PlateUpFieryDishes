using Kitchen;
using KitchenMods;
using Unity.Entities;
using UnityEngine;

namespace KitchenInferno
{
    public class CreateMassFireSchedulePyromania : RestaurantSystem, IModSystem
    {
        public struct SMassFireCacheHash : IComponentData, IModComponent
        {
            public int Hash;
        }

        public struct MassFireScheduleParameters
        {
            public bool IsActive;

            public bool IsPracticeMode;

            public int Day;

            //public int Players;

            //public KitchenParameters Parameters;

            //public float CustomersPerHour;

            //public float CustomersPerHourIncrease;

            //public DataObjectList Status;

            //public FixedListInt128 TypeValues;

            //public FixedListFloat128 TypeProbabilities;

            public override int GetHashCode()
            {
                if (!IsActive)
                    return 0;
                int day = Day;
                //day = (day * 397) ^ IsPracticeMode.GetHashCode();
                //day = (day * 397) ^ Players;
                //day = (day * 397) ^ Parameters.GetHashCode();
                //day = (day * 397) ^ CustomersPerHour.GetHashCode();
                //day = (day * 397) ^ CustomersPerHourIncrease.GetHashCode();
                //day = (day * 397) ^ Status.GetHashCode();
                //day = (day * 397) ^ TypeValues.GetHashCode();
                //day = (day * 397) ^ TypeProbabilities.GetHashCode();
                return day;
            }
        }

        private EntityQuery Schedule;

        protected override void Initialise()
        {
            base.Initialise();
            Schedule = GetEntityQuery(typeof(CScheduledMassFire));
        }

        protected override void OnUpdate()
        {
            if (Has<SPracticeMode>())
            {
                return;
            }
            int day = GetOrDefault<SDay>().Day + (Has<SIsNightTime>() ? 1 : 0);

            MassFireScheduleParameters scheduleParameters = default;
            scheduleParameters.IsActive = HasStatus(Main.PYROMANIA_EFFECT_STATUS);
            scheduleParameters.IsPracticeMode = Has<SPracticeMode>();
            scheduleParameters.Day = day;
            int hashCode = scheduleParameters.GetHashCode();
            int hash = GetOrCreate<SMassFireCacheHash>().Hash;
            if (hashCode != hash)
            {
                base.EntityManager.DestroyEntity(Schedule);
                GetMassFiresForDay(scheduleParameters);
                Set(new SMassFireCacheHash
                {
                    Hash = hashCode
                });
            }
        }

        public int DetermineTotalMassFires(MassFireScheduleParameters schedule_parameters)
        {
            //float dayLength = ProgressionHelpers.GetDayLength(schedule_parameters.Day);
            //float num = DifficultyHelpers.CustomerPlayersRateModifier(schedule_parameters.Players);
            //float num2 = schedule_parameters.Parameters.CurrentCourses switch
            //{
            //    1 => 1f,
            //    2 => 1.25f,
            //    3 => 1.5f,
            //    _ => 1f,
            //};
            //float num3 = schedule_parameters.Parameters.CustomersPerHour * DifficultyHelpers.CustomerModifierRateModifier(schedule_parameters.Parameters.CustomersPerHourReduction);
            //float num4 = schedule_parameters.Day switch
            //{
            //    0 => 1f,
            //    1 => 1.25f,
            //    2 => 1.5f,
            //    _ => schedule_parameters.CustomersPerHour + schedule_parameters.CustomersPerHourIncrease * (float)(schedule_parameters.Day - 3),
            //};
            //if (schedule_parameters.Day > 15)
            //{
            //    num4 += schedule_parameters.CustomersPerHourIncrease * Mathf.Pow(schedule_parameters.Day - 15, 1.5f);
            //}
            //return dayLength / 25f * num3 * num4 * num / num2;

            if (!schedule_parameters.IsActive)
                return 0;
            int day = schedule_parameters.Day;
            return Mathf.Clamp(((day - 1) / 5) + 1, 1, 4);
        }

        private void GetMassFiresForDay(MassFireScheduleParameters schedule_parameters)
        {
            float maxTime = 1f;
            int totalMassFires = DetermineTotalMassFires(schedule_parameters);
            float timeVariance = 0.1f * maxTime / totalMassFires;

            for (int i = 0; i < totalMassFires; i++)
            {
                float expectedTime = maxTime * (i + 1.0f) / (totalMassFires + 1.0f);
                expectedTime = Random.Range(expectedTime - timeVariance, expectedTime + timeVariance);
                if (expectedTime > 1f)
                {
                    expectedTime = 1.1f + (expectedTime - 1f) * 2f;
                }
                AddMassFire(Mathf.Clamp(expectedTime, 0f, maxTime));
            }
        }

        private void AddMassFire(float time)
        {
            if (!Has<SIsDayTime>() || !Require<STime>(out var comp) || !(time < comp.TimeOfDayUnbounded))
            {
                Entity e = base.EntityManager.CreateEntity(typeof(CScheduledMassFire));
                Set(e, new CScheduledMassFire
                {
                    TimeOfDay = time
                });
            }
        }
    }
}
