using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenInferno.Customs;
using KitchenLib.Utils;
using System.Reflection;
using UnityEngine;
using UnityEngine.VFX;

namespace KitchenInferno.Patches
{
    [HarmonyPatch]
    static class LocalViewRouter_Patch
    {
        static MethodInfo m_GetPrefab = typeof(LocalViewRouter).GetMethod("GetPrefab", BindingFlags.NonPublic | BindingFlags.Instance);

        static GameObject _firePrefab;

        static bool fireItemInit = false;

        [HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
        [HarmonyPostfix]
        static void GetPrefab_Postfix(ref LocalViewRouter __instance, ViewType view_type, ref GameObject __result)
        {
            if ((view_type != ViewType.Item && view_type != ViewType.ItemCollectionView) || __result.transform.Find("Fire") != null)
            {
                return;
            }

            if (_firePrefab == null)
            {
                object applianceViewPrefabObj = m_GetPrefab?.Invoke(__instance, new object[] { ViewType.Appliance });
                if (applianceViewPrefabObj == null)
                {
                    return;
                }
                GameObject applianceViewPrefab = (GameObject)applianceViewPrefabObj;
                _firePrefab = applianceViewPrefab.transform.Find("Fire")?.gameObject;
                if (_firePrefab == null)
                    return;
            }

            if (!fireItemInit)
            {
                fireItemInit = true;
                GameDataObject fireItemGDO = GDOUtils.GetCustomGameDataObject<FireItem>()?.GameDataObject;
                if (fireItemGDO != null)
                {
                    GameObject container = new GameObject("Fire Item Prefab Container");
                    container.SetActive(false);
                    GameObject fireItemTemplate = CreateFireItemInstance(container);
                    fireItemTemplate.name = "Fire Item Template";
                    ((Item)fireItemGDO).Prefab = container;
                    VisualEffect fireItemVfx = fireItemTemplate.GetComponent<VisualEffect>();
                    fireItemVfx?.SetFloat("Active", Main.GetFireDisplayIntensity());
                }
            }

            GameObject fire = CreateFireItemInstance(__result);
            VisualEffect fireVfx = fire.GetComponent<VisualEffect>();

            ItemOnFireView itemOnFireView = __result.AddComponent<ItemOnFireView>();
            itemOnFireView.FireVfx = fireVfx;

            GameObject CreateFireItemInstance(GameObject parentTo)
            {
                GameObject fire = GameObject.Instantiate(_firePrefab);
                fire.name = "Fire";
                if (parentTo != null)
                {
                    fire.transform.SetParent(parentTo.transform, false);
                }
                fire.transform.localPosition = Vector3.zero;
                fire.transform.localScale = Vector3.one * 0.4f;
                fire.transform.localRotation = Quaternion.identity;

                return fire;
            }
        }
    }
}
