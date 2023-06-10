using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenInferno.Customs
{
    public class IgniteItemProcess : CustomProcess
    {
        public override string UniqueNameID => "igniteItemProcess";
        public override GameDataObject BasicEnablingAppliance => GDOUtils.GetCastedGDO<Appliance, TorchProvider>();
        public override List<(Locale, ProcessInfo)> InfoList => new List<(Locale, ProcessInfo)>()
        {
            (Locale.English, new ProcessInfo()
            {
                Name = "Cook",
                Icon = "$cook$",
                Locale = Locale.English
            })
        };
    }
}
