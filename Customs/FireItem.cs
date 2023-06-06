using KitchenLib.Customs;
using UnityEngine;
using UnityEngine.VFX;

namespace KitchenDishesOnFire.Customs
{
    public class FireItem : CustomItem
    {
        public override string UniqueNameID => "fireItem";

        public void UpdateFireIntensity(float intensity)
        {
            this.GameDataObject?.Prefab?.transform.Find("Fire Item Template")?.GetComponent<VisualEffect>()?.SetFloat("Active", Mathf.Clamp01(intensity));
        }
    }
}
