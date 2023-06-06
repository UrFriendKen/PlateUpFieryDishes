using HarmonyLib;
using Kitchen;
using KitchenData;
using UnityEngine;

namespace KitchenDishesOnFire.Patches
{
    [HarmonyPatch]
    static class ItemCollectionView_Patch
    {
        [HarmonyPatch(typeof(ItemCollectionView), "UpdateDrawnItem")]
        [HarmonyPostfix]
        static void UpdateDrawnItem_Postfix(ref ItemCollectionView.DrawnItem[] ___DrawnItems, int index, ItemCollectionView.ItemData item_info)
        {
            ItemList.ItemComponentEnumerator enumerator = item_info.Components.GetEnumerator();

            int fireItemID = Main.CustomFireItem.GameDataObject?.ID ?? 0;
            GameObject fireItemPrefab = Main.CustomFireItem.GameDataObject?.Prefab?.transform.Find("Fire Item Template")?.gameObject;

            if (fireItemID == 0 || fireItemPrefab == null)
            {
                return;
            }
            
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == fireItemID)
                {
                    GameObject fireInstance = GameObject.Instantiate(fireItemPrefab);
                    fireInstance.transform.SetParent(___DrawnItems[index].Object.transform, false);
                    return;
                }
            }
        }
    }
}
