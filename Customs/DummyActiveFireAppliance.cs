using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenInferno.Customs
{
    public class DummyActiveFireAppliance : CustomAppliance
    {
        public override string UniqueNameID => "dummyActiveFireAppliance";
        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo("Active Fire Bonus", "", new List<Appliance.Section>(), new List<string>()))
        };
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => false;
    }
}
