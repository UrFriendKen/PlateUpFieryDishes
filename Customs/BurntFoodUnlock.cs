using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenInferno.Customs
{
    public class BurntFoodUnlock : CustomDish
    {
        public override string UniqueNameID => "burntFoodDessert";
        public override DishType Type => DishType.Dessert;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override CardType CardType => CardType.Default;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override List<Dish.MenuItem> ResultingMenuItems => new List<Dish.MenuItem>
        {
            new Dish.MenuItem
            {
                //Item = GDOUtils.GetCastedGDO<Item, BurntCube>(),
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.BurnedFood),
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumIngredients => new HashSet<Item>
        {
        };

        public override HashSet<Process> RequiredProcesses => new HashSet<Process>
        {
            GDOUtils.GetCastedGDO<Process, IgniteItemProcess>()
        };

        public override Dictionary<Locale, string> Recipe => new Dictionary<Locale, string>
        {
            { Locale.English, "Set any other starter, main, or dessert menu item on fire. Wait for it to burn. Collect burnt food and serve." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            ( Locale.English, LocalisationUtils.CreateUnlockInfo("Burnt Food", "Adds burnt food as a dessert.", "Someone will order this, right?"))
        };
    }
}
