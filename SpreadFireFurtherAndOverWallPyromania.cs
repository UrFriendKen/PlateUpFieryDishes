using Kitchen;
using Kitchen.Layouts;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenInferno
{
    [UpdateAfter(typeof(SpreadFire))]
    public class SpreadFireFurtherAndOverWallPyromania : GameSystemBase, IModSystem
    {
        private EntityQuery Players;
        private EntityQuery AppliancesOnFire;

        protected override void Initialise()
        {
            base.Initialise();
            Players = GetEntityQuery(typeof(CPlayer));
            AppliancesOnFire = GetEntityQuery(typeof(CIsOnFire), typeof(CPosition));
        }

        protected override void OnUpdate()
        {
            if (!HasStatus(Main.PYROMANIA_EFFECT_STATUS) || AppliancesOnFire.IsEmpty)
                return;

            using NativeArray<Entity> appliancesOnFire = AppliancesOnFire.ToEntityArray(Allocator.Temp);
            using NativeArray<CPosition> appliancesOnFirePosition = AppliancesOnFire.ToComponentDataArray<CPosition>(Allocator.Temp);

            float dt = Time.DeltaTime;
            float player_factor = DifficultyHelpers.FireSpreadModifier(Players.CalculateEntityCount());

            for (int i = 0; i < appliancesOnFire.Length; i++)
            {
                Entity appliance = appliancesOnFire[i];
                CPosition pos = appliancesOnFirePosition[i];

                int room = GetRoom(pos);
                foreach (LayoutPosition item in LayoutHelpers.AllNearbyRange2)
                {
                    Vector3 position = (Vector3)item + (Vector3)pos;
                    Entity primaryOccupant = GetPrimaryOccupant(position);
                    bool sameRoom = GetRoom(position) == room;
                    bool isRangeOne = LayoutHelpers.AllNearby.Contains(item);
                    if (EntityManager.HasComponent<CAppliance>(primaryOccupant) && EntityManager.HasComponent<CIsInteractive>(primaryOccupant) &&
                        !EntityManager.HasComponent<CFireImmune>(primaryOccupant) && !EntityManager.HasComponent<CIsOnFire>(primaryOccupant) &&
                        isRangeOne == !sameRoom)
                    {
                        double num = (EntityManager.HasComponent<CHighlyFlammable>(primaryOccupant) ? 0.1 : 0.02);
                        if ((double)Random.value < num * (double)dt * (double)player_factor)
                        {
                            EntityManager.AddComponent<CIsOnFire>(primaryOccupant);
                        }
                    }
                }
            }
        }
    }
}
