using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenInferno.Customs
{
    public class DirtyPlateWithBurnedFood : CustomItem
    {
        private Item DirtyPlateGDO => GDOUtils.GetExistingGDO(ItemReferences.PlateDirty) as Item;
        private Item BurnedFoodGDO => (GDOUtils.GetExistingGDO(ItemReferences.BurnedFood) as Item);


        public override string UniqueNameID => "dirtyPlateWithBurnedFood";
        public override GameObject Prefab => Main.Bundle.LoadAsset<GameObject>("Plate - Dirty with burned food");
        public override Item SplitSubItem => BurnedFoodGDO;
        public override int SplitCount => 1;
        public override float SplitSpeed => 0.5f;
        public override List<Item> SplitDepletedItems => new List<Item>()
        {
            DirtyPlateGDO
        };
        public override Item DisposesTo => DirtyPlateGDO;

        public override void SetupPrefab(GameObject prefab)
        {
            MaterialUtils.ApplyMaterial(prefab, "Plate/Cylinder", new Material[] { MaterialUtils.GetExistingMaterial("Plate"), MaterialUtils.GetExistingMaterial("Plate - Ring") });
            MaterialUtils.ApplyMaterial(prefab, "Plate/Plane", new Material[] { MaterialUtils.GetExistingMaterial("Plate - Dirty Food") });
            MaterialUtils.ApplyMaterial(prefab, "Plate/Plane.001", new Material[] { MaterialUtils.GetExistingMaterial("Plate - Dirty Food 2") });

            Material appleBurnt = MaterialUtils.GetExistingMaterial("AppleBurnt");
            MaterialUtils.ApplyMaterial(prefab, "BurnedFood/Cookie", new Material[] { appleBurnt, appleBurnt });
        }
    }
}
