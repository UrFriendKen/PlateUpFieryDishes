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
    public class FilterByTorchesProvided : ShopBuilderFilter, IModSystem
    {
        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void Filter(ref CShopBuilderOption option)
        {
            if (!Has<ProvideTorchesOnce.SPerformed>() && !option.IsRemoved && option.Staple != ShopStapleType.BonusStaple && option.Staple != ShopStapleType.WhenMissing &&
                GameData.Main.TryGet<Appliance>(option.Appliance, out var appliance) && IsRequired(appliance))
            {
                option.IsRemoved = true;
                option.FilteredBy = this;
            }
        }

        private bool IsRequired(Appliance appliance)
        {
            if (appliance.Properties.Select(x => x.GetType()).Contains(typeof(CFireStarterProvider)))
                return true;
            return false;
        }
    }
}
