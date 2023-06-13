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
                Name = "Wild Fires",
                Description = "Randomly sets cooking appliances on fire",
                FlavourText = "Who forgot to turn the gas off?!"
            }),
            (Locale.French, new UnlockInfo()
            {
                Locale = Locale.French,
                Name = "Incendies sauvages",
                Description = "Met le feu aux appareils de cuisson au hasard"
            }),
            (Locale.German, new UnlockInfo()
            {
                Locale = Locale.German,
                Name = "Waldbrände",
                Description = "Setzt Kochgeräte willkürlich in Brand"
            }),
            (Locale.Spanish, new UnlockInfo()
            {
                Locale = Locale.Spanish,
                Name = "Incendios forestales",
                Description = "Al azar prende fuego a los aparatos de cocina"
            }),
            (Locale.Polish, new UnlockInfo()
            {
                Locale = Locale.Polish,
                Name = "Dzikie pożary",
                Description = "Losowo podpala urządzenia kuchenne"
            }),
            (Locale.PortugueseBrazil, new UnlockInfo()
            {
                Locale = Locale.PortugueseBrazil,
                Name = "Incêndios selvagens",
                Description = "Aleatoriamente incendeia utensílios de cozinha"
            }),
            (Locale.Japanese, new UnlockInfo()
            {
                Locale = Locale.Japanese,
                Name = "野火",
                Description = "調理器具にランダムで火をつける"
            }),
            (Locale.ChineseSimplified, new UnlockInfo()
            {
                Locale = Locale.ChineseSimplified,
                Name = "野火",
                Description = "随意点燃炊具"
            }),
            (Locale.ChineseTraditional, new UnlockInfo()
            {
                Locale = Locale.ChineseTraditional,
                Name = "野火",
                Description = "隨意點燃炊具"
            }),
            (Locale.Korean, new UnlockInfo()
            {
                Locale = Locale.Korean,
                Name = "들불",
                Description = "무작위로 일부 조리기구에 불을 붙입니다."
            }),
            (Locale.Turkish, new UnlockInfo()
            {
                Locale = Locale.Turkish,
                Name = "Kontrol dışı ateş",
                Description = "Pişirme cihazlarını rastgele ateşe verir"
            })
        };
    }
}
