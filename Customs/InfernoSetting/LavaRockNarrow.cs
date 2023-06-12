using KitchenLib.Utils;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class LavaRockNarrow : CustomSettingAppliance
    {
        public override string UniqueNameID => "lavaRockNarrow";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Lava Rock Narrow");
        public override void SetupPrefab(GameObject prefab)
        {
            Material[] matArr = new Material[] { MaterialUtils.GetExistingMaterial("Rock") };
            MaterialUtils.ApplyMaterial(prefab, "Rock", matArr);
        }
    }
}
