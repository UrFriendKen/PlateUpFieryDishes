using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using System.Collections.Generic;

namespace KitchenInferno.Customs
{
    public abstract class CustomSettingAppliance : CustomAppliance
    {
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>()
        {
            new CImmovable(),
            new CStatic()
        };
        public override bool IsNonInteractive => true;
    }
}
