using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenInferno.Customs
{
    public class DummyFireOrderAppliance : CustomAppliance
    {
        public override string UniqueNameID => "dummyFireOrderAppliance";
        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo("Food On Fire", "", new List<Appliance.Section>(), new List<string>()))
        };
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => false;
    }
}
