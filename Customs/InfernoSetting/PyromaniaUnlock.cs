using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using System.Collections.Generic;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class PyromaniaUnlock : CustomUnlockCard
    {
        public override string UniqueNameID => "pyromaniaUnlock";

        public const float PRICE_MODIFIER_PERCENT = 0.5f;

        public override List<UnlockEffect> Effects => new List<UnlockEffect>()
        {
            new GlobalEffect()
            {
                EffectCondition = new CEffectAlways(),
                EffectType = new CApplianceSpeedModifier()
                {
                    Process = ProcessReferences.Cook,
                    Speed = 0f,
                    BadSpeed = 0.5f
                }
            },
            new GlobalEffect()
            {
                EffectCondition = new CEffectAlways(),
                EffectType = new CFlammableItemsModifier()
                {
                    BurnSpeedChange = 0.667f
                }
            },

            new GlobalEffect()
            {
                EffectCondition = new CEffectAtNight(),
                EffectType = new CApplianceSpeedModifier()
                {
                    Process = ProcessReferences.Cook,
                    Speed = 0f,
                    BadSpeed = 1f
                }
            },
            new GlobalEffect()
            {
                EffectCondition = new CEffectAtNight(),
                EffectType = new CFlammableItemsModifier()
                {
                    BurnSpeedChange = 0.5f
                }
            },

            //new GlobalEffect()
            //{
            //    EffectCondition = new CEffectAlways(),
            //    EffectType = new CFireSpreadModifier()
            //    {
            //        SpreadChanceModifier = 2f
            //    }
            //},

            new GlobalEffect()
            {
                EffectCondition = new CEffectAlways(),
                EffectType = new CTableModifier()
                {
                    OrderingModifiers = new OrderingValues()
                    {
                        PriceModifier = -PRICE_MODIFIER_PERCENT
                    }
                }
            },

            new StatusEffect()
            {
                Status = Main.PYROMANIA_EFFECT_STATUS
            },

            new StatusEffect()
            {
                Status = Main.WILDFIRES_EFFECT_STATUS
            }
        };

        public override bool IsUnlockable => false;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Large;
        public override UnlockGroup UnlockGroup => UnlockGroup.Special;
        public override CardType CardType => CardType.Setting;

        public override List<(Locale, UnlockInfo)> InfoList => new List<(Locale, UnlockInfo)>()
        {
            (Locale.English, new UnlockInfo()
            {
                Locale = Locale.English,
                Name = "Pyromania",
                Description = "Fire spreads further, and through walls. Some appliance randomly catch fire. Food burns faster, and costs less. More money based on percent of restaurant on fire.",
                FlavourText = "Remember, bring your fire extinguishers to the party!"
            }),
            (Locale.French, new UnlockInfo()
            {
                Locale = Locale.French,
                Name = "Pyromanie",
                Description = "Le feu se propage plus loin, plus vite et à travers les murs. Les plaques de cuisson et les fours sont plus sujets au feu. Les aliments brûlent plus vite."
            }),
            (Locale.German, new UnlockInfo()
            {
                Locale = Locale.German,
                Name = "Pyromanie",
                Description = "Feuer breitet sich weiter, schneller und durch Wände aus. Kochfelder und Öfen sind anfälliger für Brände. Lebensmittel verbrennen schneller."
            }),
            (Locale.Spanish, new UnlockInfo()
            {
                Locale = Locale.Spanish,
                Name = "Piromanía",
                Description = "El fuego se propaga más lejos, más rápido y a través de las paredes. Las placas de cocción y los hornos son más propensos a incendiarse. La comida se quema más rápido."
            }),
            (Locale.Polish, new UnlockInfo()
            {
                Locale = Locale.Polish,
                Name = "Piromania",
                Description = "Ogień rozprzestrzenia się dalej, szybciej i przez ściany. Płyty grzejne i piekarniki są bardziej podatne na ogień. Jedzenie pali się szybciej."
            }),
            (Locale.PortugueseBrazil, new UnlockInfo()
            {
                Locale = Locale.PortugueseBrazil,
                Name = "Piromania",
                Description = "O fogo se espalha mais longe, mais rápido e através das paredes. Fogões e fornos são mais propensos ao fogo. A comida queima mais rápido."
            }),
            (Locale.Japanese, new UnlockInfo()
            {
                Locale = Locale.Japanese,
                Name = "放火マニア",
                Description = "火はより遠くに、より速く、壁を通って広がります。コンロやオーブンは火災が発生しやすくなります。食べ物が早く燃えます。"
            }),
            (Locale.ChineseSimplified, new UnlockInfo()
            {
                Locale = Locale.ChineseSimplified,
                Name = "纵火癖",
                Description = "火势蔓延得更远、更快，并穿过墙壁。炉灶和烤箱更容易着火。食物燃烧得更快。"
            }),
            (Locale.ChineseTraditional, new UnlockInfo()
            {
                Locale = Locale.ChineseTraditional,
                Name = "縱火癖",
                Description = "火勢蔓延得更遠、更快，並穿過牆壁。爐灶和烤箱更容易著火。食物燃燒得更快。"
            }),
            (Locale.Korean, new UnlockInfo()
            {
                Locale = Locale.Korean,
                Name = "방화광",
                Description = "불은 벽을 통해 더 멀리, 더 빠르게 퍼집니다. 호브와 오븐은 화재에 더 취약합니다. 음식이 더 빨리 연소됩니다."
            }),
            (Locale.Turkish, new UnlockInfo()
            {
                Locale = Locale.Turkish,
                Name = "Piromani",
                Description = "Yangın daha uzağa, daha hızlı ve duvarlardan yayılır. Ocaklar ve fırınlar yangına daha yatkındır. Yiyecekler daha hızlı yanar."
            })
        };
    }
}
