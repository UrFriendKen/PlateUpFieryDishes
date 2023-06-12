using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class LavaGround : CustomSettingAppliance
    {
        public override string UniqueNameID => "lavaGround";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Lava Ground");
        public override void SetupPrefab(GameObject prefab)
        {
            Material[] matArr = new Material[] { MaterialUtils.GetCustomMaterial("Inferno Lava") };
            MaterialUtils.ApplyMaterial(prefab, "Lava Ground", matArr);
        }
    }
}
