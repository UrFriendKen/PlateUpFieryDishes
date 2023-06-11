using Kitchen.ShopBuilder;
using Kitchen.ShopBuilder.Filters;
using KitchenData;
using KitchenInferno.Customs;
using KitchenLib.Utils;
using KitchenMods;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace KitchenInferno
{
    [UpdateAfter(typeof(CreateShopOptions))]
    public class FilterByFireOrders : ShopBuilderFilter, IModSystem
    {
        private EntityQuery FireOrdersQuery;

        private bool HasFireOrders;

        protected override void Initialise()
        {
            base.Initialise();
            FireOrdersQuery = GetEntityQuery(typeof(CFireOrderChance));
        }

        protected override void BeforeRun()
        {
            base.BeforeRun();
            HasFireOrders = !FireOrdersQuery.IsEmpty;
        }
        protected override void Filter(ref CShopBuilderOption option)
        {
            if (!option.IsRemoved && option.Staple != ShopStapleType.BonusStaple && option.Staple != ShopStapleType.WhenMissing &&
                GameData.Main.TryGet<Appliance>(option.Appliance, out var appliance) && IsRequired(appliance) && !HasFireOrders)
            {
                option.IsRemoved = true;
                option.FilteredBy = this;
            }
        }

        private bool IsRequired(Appliance appliance)
        {
            if (appliance.Properties.Select(x => x.GetType()).Contains(typeof(CSellRequiresFireOrder)))
                return true;
            return false;
        }
    }
}
