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
                Description = "Customer can request for food on fire (Increases order chance in Inferno)",
                FlavourText = "Mmm... Burns the tongue! Just how I like it."
            }),
            (Locale.French, new UnlockInfo()
            {
                Locale = Locale.French,
                Name = "Repas en feu",
                Description = "Le client peut demander de la nourriture en feu"
            }),
            (Locale.German, new UnlockInfo()
            {
                Locale = Locale.German,
                Name = "Flammende Mahlzeiten",
                Description = "Der Kunde kann Essen in Flammen anfordern"
            }),
            (Locale.Spanish, new UnlockInfo()
            {
                Locale = Locale.Spanish,
                Name = "Comidas en llamas",
                Description = "El cliente puede solicitar comida en llamas"
            }),
            (Locale.Polish, new UnlockInfo()
            {
                Locale = Locale.Polish,
                Name = "Płonące posiłki",
                Description = "Klient może poprosić o jedzenie w ogniu"
            }),
            (Locale.PortugueseBrazil, new UnlockInfo()
            {
                Locale = Locale.PortugueseBrazil,
                Name = "Refeições flamejantes",
                Description = "O cliente pode solicitar comida no fogo"
            }),
            (Locale.Japanese, new UnlockInfo()
            {
                Locale = Locale.Japanese,
                Name = "燃えるような食事",
                Description = "顧客は火のある食べ物を注文できます"
            }),
            (Locale.ChineseSimplified, new UnlockInfo()
            {
                Locale = Locale.ChineseSimplified,
                Name = "燃烧的饭菜",
                Description = "顾客可以要求食物着火"
            }),
            (Locale.ChineseTraditional, new UnlockInfo()
            {
                Locale = Locale.ChineseTraditional,
                Name = "燃燒的飯菜",
                Description = "顧客可以要求食物著火"
            }),
            (Locale.Korean, new UnlockInfo()
            {
                Locale = Locale.Korean,
                Name = "불타는 식사",
                Description = "고객은 불이 붙은 음식을 주문할 수 있습니다"
            }),
            (Locale.Turkish, new UnlockInfo()
            {
                Locale = Locale.Turkish,
                Name = "Alevli yemekler",
                Description = "Müşteri ateşle yemek sipariş edebilir"
            })
        };
    }
}
