using KitchenData;
using KitchenInferno.Utils;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;

namespace KitchenInferno.Customs
{
    public class PyroPatronsCopy : CustomUnlockCard
    {
        public override string UniqueNameID => "pyroPatronsCopy";
        public override List<UnlockEffect> Effects => new List<UnlockEffect>()
        {
            new StatusEffect()
            {
                Status = RestaurantStatus.HalloweenTrickCustomersStartFiresWhenLeaving
            }
        };
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Large;
        public override bool IsUnlockable => true;
        public override UnlockGroup UnlockGroup => UnlockGroup.Generic;
        public override CardType CardType => CardType.Default;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override List<(Locale, UnlockInfo)> InfoList => UnlockHelpers.CopyInfo(UnlockReferences.TrickCustomersLeavingStartFires);
    }
}
