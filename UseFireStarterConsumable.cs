using Kitchen;
using KitchenData;
using KitchenMods;
using Unity.Entities;

namespace KitchenInferno
{
    public class UseFireStarter : ItemInteractionSystem, IModSystem
    {
        Entity FireStarterEntity;
        CFireStarter FireStarter;

        protected override bool IsPossible(ref InteractionData data)
        {
            if (!Require(data.Interactor, out CItemHolder holder) || holder.HeldItem == default)
                return false;
            if (!Require(holder.HeldItem, out FireStarter))
                return false;
            FireStarterEntity = holder.HeldItem;
            if (!Require(data.Target, out CAppliance appliance) || appliance.Layer != OccupancyLayer.Default)
                return false;
            if (Has<CFireImmune>(data.Target) || Has<CApplianceChair>(data.Target) || Has<CIsOnFire>(data.Target))
                return false;
            return true;
        }

        protected override void Perform(ref InteractionData data)
        {
            if (FireStarter.Charges > 0)
            {
                Set<CIsOnFire>(data.Target);
            }
            if (--FireStarter.Charges <= 0)
            {
                EntityManager.DestroyEntity(FireStarterEntity);
                return;
            }
            Set(FireStarterEntity, FireStarter);
        }
    }
}
