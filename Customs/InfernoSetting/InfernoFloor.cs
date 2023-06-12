using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class InfernoFloor : CustomSettingAppliance
    {
        public override string UniqueNameID => "infernoFloor";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Inferno Floor");
        public override void SetupPrefab(GameObject prefab)
        {
            Material[] matArr = new Material[] { MaterialUtils.GetCustomMaterial("Inferno Rock - Cliff") };
            MaterialUtils.ApplyMaterial(prefab, "Primary/Inferno Ground Cliff", matArr);
        }
    }
}
