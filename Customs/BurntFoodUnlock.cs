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
            { Locale.English, "Set any other starter, main, or dessert menu item on fire. Wait for it to burn. Collect burnt food and serve." },
            { Locale.French, "Mettez le feu à toute autre entrée, plat principal ou dessert. Attendez qu'il brûle. Ramassez les aliments brûlés et servez." },
            { Locale.German, "Setzen Sie alle anderen Vorspeisen-, Haupt- oder Nachspeisegerichte in Brand. Warten Sie, bis es brennt. Angebranntes Essen aufsammeln und servieren." },
            { Locale.Polish, "Rozświetl każdą inną przystawkę, danie główne lub deser. Poczekaj, aż się spalą. Podnieś przypalone jedzenie i podawaj." },
            { Locale.PortugueseBrazil, "Coloque fogo em qualquer outro item do menu inicial, principal ou sobremesa. Espere que queime. Recolha os alimentos queimados e sirva." },
            { Locale.Japanese, "他のスターター、メイン、またはデザートのメニュー項目に火をつけます。燃え上がるのを待ちます。焦げた食べ物を集めて提供します。" },
            { Locale.ChineseSimplified, "点燃任何其他开胃菜、主菜或甜点菜单项。等待它燃烧。收集烧焦的食物并上菜。" },
            { Locale.ChineseTraditional, "點燃任何其他開胃菜、主菜或甜點菜單項。等待它燃燒。收集燒焦的食物並上菜。" },
            { Locale.Korean, "다른 스타터, 메인 또는 디저트 메뉴 항목을 불에 태우십시오. 화상을 입을 때까지 기다리십시오. 탄 음식을 모아 서빙하십시오." },
            { Locale.Turkish, "Diğer başlangıç, ana veya tatlı menü öğelerini ateşe verin. Yanmasını bekleyin. Yanmış yiyecekleri toplayın ve servis yapın." }

        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Locale = Locale.English,
                Name = "Burnt Food",
                Description = "Adds burnt food as a dessert",
                FlavourText = "Someone will order this, right?"
            }),
            (Locale.French, new UnlockInfo()
            {
                Locale = Locale.French,
                Name = "Nourriture brûlée",
                Description = "Ajoute des aliments brûlés comme dessert"
            }),
            (Locale.German, new UnlockInfo()
            {
                Locale = Locale.German,
                Name = "Verbranntes Essen",
                Description = "Fügen Sie angebranntes Essen als Nachtisch hinzu"
            }),
            (Locale.Spanish, new UnlockInfo()
            {
                Locale = Locale.Spanish,
                Name = "Comida quemada",
                Description = "Agrega comida quemada como postre"
            }),
            (Locale.Polish, new UnlockInfo()
            {
                Locale = Locale.Polish,
                Name = "Spalone jedzenie",
                Description = "Dodaje spalone jedzenie jako deser"
            }),
            (Locale.PortugueseBrazil, new UnlockInfo()
            {
                Locale = Locale.PortugueseBrazil,
                Name = "Comida queimada",
                Description = "Adiciona comida queimada como sobremesa"
            }),
            (Locale.Japanese, new UnlockInfo()
            {
                Locale = Locale.Japanese,
                Name = "焦げた食べ物",
                Description = "デザートとして焦げた食べ物を追加する"
            }),
            (Locale.ChineseSimplified, new UnlockInfo()
            {
                Locale = Locale.ChineseSimplified,
                Name = "烧焦的食物",
                Description = "添加烧焦的食物作为甜点"
            }),
            (Locale.ChineseTraditional, new UnlockInfo()
            {
                Locale = Locale.ChineseTraditional,
                Name = "燒焦的食物",
                Description = "添加燒焦的食物作為甜點"
            }),
            (Locale.Korean, new UnlockInfo()
            {
                Locale = Locale.Korean,
                Name = "탄 음식",
                Description = "탄 음식을 디저트로 추가"
            }),
            (Locale.Turkish, new UnlockInfo()
            {
                Locale = Locale.Turkish,
                Name = "Yanmış yemek",
                Description = "Yanmış yiyecekleri tatlı olarak ekler"
            })
        };
    }
}
