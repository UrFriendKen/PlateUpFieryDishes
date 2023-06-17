using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenInferno
{
    public class PatchController : RestaurantSystem, IModSystem
    {
        private static PatchController _instance;
        private EntityQuery Fires;
        private EntityQuery FireSpreadModifiers;

        protected override void Initialise()
        {
            base.Initialise();
            _instance = this;
            Fires = GetEntityQuery(typeof(CAppliance), typeof(CIsOnFire));
            FireSpreadModifiers = GetEntityQuery(typeof(CFireSpreadModifier));
        }

        protected override void OnUpdate()
        {
        }

        internal static bool StaticHas<T>(Entity entity) where T : struct, IComponentData
        {
            return _instance?.Has<T>(entity) ?? false;
        }

        internal static bool OrderMatchCandidateFireState(Entity request, Entity candidate)
        {
            if (_instance == null)
            {
                return true;
            }
            return _instance.Has<CItemOnFire>(request) == _instance.Has<CItemOnFire>(candidate) && !_instance.Has<DestroyItemOnFireAfterDuration.CHasBeenDestroyed>(candidate);
        }

        internal static float GetFireSpreadModifier()
        {
            if (_instance == null)
            {
                return 1f;
            }

            float multiplier = 1f;
            using NativeArray<CFireSpreadModifier> modifiers = _instance.FireSpreadModifiers.ToComponentDataArray<CFireSpreadModifier>(Allocator.Temp);
            for (int i = 0; i < modifiers.Length; i++)
            {
                float modifier = modifiers[i].SpreadChanceModifier;
                multiplier *= modifier < 0f ? 0f : modifier;
            }
            return multiplier < 0f ? 0f : multiplier;
        }

        internal static bool GetFireOrderBonus(CWaitingForItem satisfiedOrder, out int amount)
        {
            if (_instance?.Has<CItemOnFire>(satisfiedOrder.Item) ?? false)
            {
                amount = Mathf.CeilToInt(0.4f * satisfiedOrder.Reward);
                return true;
            }
            amount = 0;
            return false;
        }

        internal static bool GetActiveFireBonus(CWaitingForItem satisfiedOrder, out int amount)
        {
            if (_instance?.HasStatus(Main.PYROMANIA_EFFECT_STATUS) ?? false)
            {
                amount = Mathf.CeilToInt(_instance.Fires.CalculateEntityCount() / (_instance.Bounds.size.x + 1) / (_instance.Bounds.size.z + 1) * satisfiedOrder.Reward);
                return true;
            }
            amount = 0;
            return false;
        }
    }
}
