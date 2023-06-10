using KitchenData;
using KitchenInferno.Customs.InfernoSetting;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenInferno.Customs
{
    public class Wildfires : CustomUnlockCard
    {
        public override string UniqueNameID => "wildfires";
        public override List<UnlockEffect> Effects => new List<UnlockEffect>()
        {
            new StatusEffect()
            {
                Status = Main.WILDFIRES_EFFECT_STATUS
            }
        };
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Large;
        public override bool IsUnlockable => true;
        public override UnlockGroup UnlockGroup => UnlockGroup.Generic;
        public override CardType CardType => CardType.Default;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;

        public override List<Unlock> HardcodedBlockers => new List<Unlock>()
        {
            GDOUtils.GetCastedGDO<Unlock, PyromaniaUnlock>()
        };

        public override List<(Locale, UnlockInfo)> InfoList => new List<(Locale, UnlockInfo)>()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Wildfires",
                Description = "Randomly sets cooking appliances on fire",
                FlavourText = "Who forgot to turn the gas off?!"
            })
        };
    }
}
