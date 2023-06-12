using KitchenData;
using System.Collections.Generic;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class FlamingMealsForced : FlamingMeals
    {
        public override string UniqueNameID => "flamingMealsForced";
        protected override float FireMealChance => 0.35f;
        public override UnlockGroup UnlockGroup => UnlockGroup.Special;
        public override List<(Locale, UnlockInfo)> InfoList => new List<(Locale, UnlockInfo)>()
        {
            (Locale.English, new UnlockInfo()
            {
                Locale = Locale.English,
                Name = "Flaming Meals",
                Description = "Customer can request for food on fire",
                FlavourText = "Mmm... Burns the tongue! Just how I like it."
            })
        };
    }
}
