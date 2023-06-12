using KitchenLib.Utils;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class LavaRockLarge : CustomSettingAppliance
    {
        public override string UniqueNameID => "lavaRockLarge";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Lava Rock Large");
        public override void SetupPrefab(GameObject prefab)
        {
            Material[] matArr = new Material[] { MaterialUtils.GetExistingMaterial("Rock") };
            MaterialUtils.ApplyMaterial(prefab, "Rock", matArr);
        }
    }
}
