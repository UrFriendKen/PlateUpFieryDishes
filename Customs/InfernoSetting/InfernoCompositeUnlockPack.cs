using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class InfernoCompositeUnlockPack : CustomCompositeUnlockPack
    {
        public override string UniqueNameID => "infernoCompositeUnlockPack";
        public override List<UnlockPack> Packs => new List<UnlockPack>()
        {
            GDOUtils.GetExistingGDO(ModularUnlockPackReferences.FranchiseCardsPack) as ModularUnlockPack,
            GDOUtils.GetExistingGDO(ModularUnlockPackReferences.NormalCardsPack) as ModularUnlockPack,
            GDOUtils.GetExistingGDO(ModularUnlockPackReferences.ThemeCardsPack) as ModularUnlockPack,
            GDOUtils.GetCastedGDO<ModularUnlockPack, InfernoSpecialCard>()
        };
    }
}
