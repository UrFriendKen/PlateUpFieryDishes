using KitchenLib.Utils;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class LavaRock : CustomSettingAppliance
    {
        public override string UniqueNameID => "lavaRock";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Lava Rock");
        public override void SetupPrefab(GameObject prefab)
        {
            Material[] matArr = new Material[] { MaterialUtils.GetExistingMaterial("Rock") };
            MaterialUtils.ApplyMaterial(prefab, "Rock", matArr);
        }
    }
}
