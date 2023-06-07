using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using System.Collections.Generic;

namespace KitchenInferno.Customs
{
    public class FlamingMeals : CustomUnlockCard
    {
        public override string UniqueNameID => "flamingMeals1";
        public override bool IsUnlockable => true;
        protected virtual float FireMealChance { get; set; } = 0.5f;
        public override List<UnlockEffect> Effects => new List<UnlockEffect>()
        {
            new GlobalEffect()
            {
                EffectCondition = new CEffectAlways(),
                EffectType = new CFireOrderChance()
                {
                    OrderChance = FireMealChance
                }
            }
        };
        public override UnlockGroup UnlockGroup => UnlockGroup.Generic;
        public override CardType CardType => CardType.Default;
        public override List<(Locale, UnlockInfo)> InfoList => new List<(Locale, UnlockInfo)>()
        {
            (Locale.English, new UnlockInfo()
            {
                Locale = Locale.English,
                Name = "Flaming Meals",
                Description = $"{FireMealChance * 100f:0}",
                FlavourText = ""
            })
        };
    }
}
