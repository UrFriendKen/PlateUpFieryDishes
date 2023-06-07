using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    public class PatchController : RestaurantSystem, IModSystem
    {
        private static PatchController _instance;
        private EntityQuery FireSpreadModifiers;

        protected override void Initialise()
        {
            base.Initialise();
            _instance = this;
            FireSpreadModifiers = GetEntityQuery(typeof(CFireSpreadModifier));
        }

        protected override void OnUpdate()
        {
        }

        internal static bool OrderMatchCandidateFireState(Entity request, Entity candidate)
        {
            if (_instance == null)
            {
                return true;
            }
            return _instance.Has<CItemOnFire>(request) == _instance.Has<CItemOnFire>(candidate);
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
    }
}
