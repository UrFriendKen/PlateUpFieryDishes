using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using Unity.Collections;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class InfernoSpecialCard : CustomModularUnlockPack
    {
        public override string UniqueNameID => "infernoSpecialCardModularUnlockPack";

        public override List<IUnlockSet> Sets
        {
            get
            {
                UnlockSetFixed fixedUnlockSet = new UnlockSetFixed();
                fixedUnlockSet.Unlocks.Add(GDOUtils.GetCastedGDO<Unlock, FlamingMealsForced>());

                return new List<IUnlockSet>()
                {
                    fixedUnlockSet
                };
            }
        }

        public override List<IUnlockFilter> Filter => new List<IUnlockFilter>()
        {
            new FilterBasic()
            {
                IgnoreUnlockability = true,
                IgnoreFranchiseTier = true,
                IgnoreDuplicateFilter = false,
                IgnoreRequirements = true,
                AllowBaseDishes = false
            }
        };

        public override List<IUnlockSorter> Sorters => new List<IUnlockSorter>();

        public override List<ConditionalOptions> ConditionalOptions => new List<ConditionalOptions>()
        {
            new ConditionalOptions()
            {
                Selector = new UnlockSelectorForcedCard(),
                Condition = new UnlockConditionOnce()
                {
                    Day = 0
                }
            }
        };
    }
}
