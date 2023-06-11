using HarmonyLib;
using Kitchen;
using KitchenData;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenInferno.Patches
{
    [HarmonyPatch]
    static class ItemGroupView_Patch
    {
        [HarmonyPatch(typeof(ItemGroupView), "AddComponent")]
        [HarmonyPrefix]
        static void AddComponent_Prefix(ref ItemGroupView __instance, ref Dictionary<int, ItemGroupView.ComponentGroup> ___DrawComponents, int id)
        {
            int fireItemID = Main.CustomFireItem?.GameDataObject?.ID ?? 0;
            if (___DrawComponents.ContainsKey(id) ||
                id != fireItemID || __instance.gameObject.name.StartsWith("Side Prefab"))
                return;
            GameObject fire = GameObject.Instantiate(Main.CustomFireItem.GameDataObject.Prefab);
            fire.transform.SetParent(__instance.gameObject.transform, false);
            ___DrawComponents.Add(fireItemID, new ItemGroupView.ComponentGroup()
            {
                GameObject = fire
            });
        }
    }

    //[HarmonyPatch]
    //static class ItemView_Patch
    //{
    //    [HarmonyPatch(typeof(ItemView), "UpdateData")]
    //    [HarmonyPostfix]
    //    static void UpdateData_Postfix(ref ItemView __instance, ItemView.ViewData view_data)
    //    {
    //        if (__instance.gameObject.transform.Find("Fire") != null)
    //            return;

    //        ItemList.ItemComponentEnumerator enumerator = view_data.Components.GetEnumerator();

    //        int fireItemID = Main.CustomFireItem.GameDataObject?.ID ?? 0;
    //        GameObject fireItemPrefab = Main.CustomFireItem.GameDataObject?.Prefab?.transform.Find("Fire Item Template")?.gameObject;
    //        if (fireItemID == 0 || fireItemPrefab == null)
    //        {
    //            return;
    //        }

    //        while (enumerator.MoveNext())
    //        {
    //            Main.LogInfo($"{enumerator.Current} ---- {fireItemID}");
    //            if (enumerator.Current == fireItemID)
    //            {
    //                GameObject fireInstance = GameObject.Instantiate(fireItemPrefab);
    //                fireInstance.name = "Fire";
    //                fireInstance.transform.SetParent(__instance.gameObject.transform, false);
    //                return;
    //            }
    //        }
    //    }
    //}
}
