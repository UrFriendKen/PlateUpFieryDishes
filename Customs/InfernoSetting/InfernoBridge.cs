using KitchenLib.Utils;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class InfernoBridge : CustomSettingAppliance
    {
        public override string UniqueNameID => "infernoBridge";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Inferno Bridge");
        public override void SetupPrefab(GameObject prefab)
        {
            Material[] matArr = new Material[] { MaterialUtils.GetCustomMaterial("Inferno Rock") };
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge/Arches", matArr);
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge/Bridge", matArr);
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge/Cube", matArr);
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge/Plane", matArr);
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge (1)/Arches", matArr);
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge (1)/Bridge", matArr);
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge (1)/Cube", matArr);
            MaterialUtils.ApplyMaterial(prefab, "Inferno Bridge (1)/Plane", matArr);
        }
    }
}
