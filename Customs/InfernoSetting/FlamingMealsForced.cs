using KitchenData;
using System.Collections.Generic;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class FlamingMealsForced : FlamingMeals
    {
        public override string UniqueNameID => "flamingMealsForced";
        protected override float FireMealChance => 0.35f;
        public override UnlockGroup UnlockGroup => UnlockGroup.Special;
    }
}
