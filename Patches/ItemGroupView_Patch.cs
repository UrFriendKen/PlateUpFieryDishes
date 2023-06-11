using HarmonyLib;
using Kitchen;
using System.Collections.Generic;
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
}
