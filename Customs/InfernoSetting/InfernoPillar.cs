using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class InfernoPillar : CustomSettingAppliance
    {
        public override string UniqueNameID => "infernoPillar";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Inferno Pillar");
        public override void SetupPrefab(GameObject prefab)
        {
            Material[] matArr = new Material[] { MaterialUtils.GetCustomMaterial("Inferno Rock") };
            MaterialUtils.ApplyMaterial(prefab, "Inferno Pillar", matArr);
        }
    }
}
