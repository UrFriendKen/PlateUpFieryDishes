using Kitchen;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenInferno
{
    public static class PrefabGenerator
    {
        static GameObject HiderContainer;
        static GameObject ConsumableStorePrefab;

        static void ParentToHider(GameObject child)
        {
            if (HiderContainer == null)
            {
                HiderContainer = new GameObject("Inferno Prefabs");
                HiderContainer.SetActive(false);
            }
            child.transform.SetParent(HiderContainer.transform, false);
        }

        private const float FOH_ITEM_MIN_X = -0.35f;
        private const float FOH_ITEM_MAX_X = 0.35f;
        private const float FOH_ITEM_MIN_Z = -0.1f;
        private const float FOH_ITEM_MAX_Z = 0.1f;
        private static FieldInfo f_LimitedItemSourceView_Items = typeof(LimitedItemSourceView).GetField("Items", BindingFlags.NonPublic | BindingFlags.Instance);
        public static GameObject CreateConsumableProviderPrefab(string name, int displayCount, float scale = 1f)
        {
            if (ConsumableStorePrefab == null)
            {
                ConsumableStorePrefab = GameObject.Instantiate(Main.Bundle.LoadAsset<GameObject>("Consumable Store"));
                ConsumableStorePrefab.name = "Consumable Store";
                Material[] wood_default = new Material[] { MaterialUtils.GetExistingMaterial("Wood - Default") };
                Material[] wood4_painted = new Material[] { MaterialUtils.GetExistingMaterial("Wood 4 - Painted") };
                Material[] metal_brass = new Material[] { MaterialUtils.GetExistingMaterial("Metal - Brass") };
                MaterialUtils.ApplyMaterial(ConsumableStorePrefab, "FoH Stand/FoHStand/Body", wood4_painted);
                MaterialUtils.ApplyMaterial(ConsumableStorePrefab, "FoH Stand/FoHStand/Doors", wood4_painted);
                MaterialUtils.ApplyMaterial(ConsumableStorePrefab, "FoH Stand/FoHStand/Handles", metal_brass);
                MaterialUtils.ApplyMaterial(ConsumableStorePrefab, "FoH Stand/FoHStand/Sides", wood_default);
                MaterialUtils.ApplyMaterial(ConsumableStorePrefab, "FoH Stand/FoHStand/Top", wood_default);
                LimitedItemSourceView limitedItemSourceView = ConsumableStorePrefab.AddComponent<LimitedItemSourceView>();
                ParentToHider(ConsumableStorePrefab);
            }
            GameObject instance = GameObject.Instantiate(ConsumableStorePrefab);
            ParentToHider(instance);
            if (!name.IsNullOrEmpty())
                instance.name = name;

            Transform itemsAnchor = instance.transform.Find("FoH Stand")?.Find("Items Anchor");
            if (itemsAnchor == null)
            {
                Main.LogError("PrefabGenerator failed to initialize LimitedItemSourceView.");
                return instance;
            }

            List<GameObject> itemsList = new List<GameObject>();
            if (itemsAnchor != null)
            {
                displayCount = Mathf.Clamp(displayCount, 1, 12);
                if (displayCount == 1)
                {
                    CreateItem(0, (FOH_ITEM_MAX_X + FOH_ITEM_MIN_X) / 2f, (FOH_ITEM_MAX_Z + FOH_ITEM_MIN_Z) / 2f);
                }
                if (displayCount <= 4)
                {
                    float xSpacing = (FOH_ITEM_MAX_X - FOH_ITEM_MIN_X) / displayCount;
                    float zPos = (FOH_ITEM_MIN_Z + FOH_ITEM_MAX_Z) / 2f;

                    for (int i = 0; i < displayCount; i++)
                    {
                        CreateItem(i, FOH_ITEM_MIN_X + xSpacing * (i + 0.5f), zPos);
                    }
                }
                else
                {
                    float zSpacing = (FOH_ITEM_MAX_Z - FOH_ITEM_MIN_Z) / 2f;

                    int backRowCount = Mathf.CeilToInt(displayCount / 2f);
                    int frontRowCount = displayCount - backRowCount;

                    float xSpacing = (FOH_ITEM_MAX_X - FOH_ITEM_MIN_X) / backRowCount;
                    float zPosBackRow = 0.5f * zSpacing;
                    float zPosFrontRow = -0.5f * zSpacing;

                    bool isEven = displayCount % 2 == 0;

                    for (int i = 0; i < backRowCount; i++)
                    {
                        CreateItem(i, FOH_ITEM_MIN_X + xSpacing * (i + 0.5f), zPosBackRow);
                    }
                    for (int i = 0; i < frontRowCount; i++)
                    {
                        CreateItem(i + backRowCount, FOH_ITEM_MIN_X + xSpacing * (i + (isEven ? 0.5f : 1f)), zPosFrontRow);
                    }
                }

                void CreateItem(int i, float x, float z)
                {
                    GameObject itemContainer = new GameObject($"Item{i + 1}");
                    itemContainer.transform.SetParent(itemsAnchor, false);
                    itemContainer.transform.localPosition = new Vector3(x, 0f, z);
                    itemContainer.transform.localRotation = Quaternion.identity;
                    itemContainer.transform.localScale = Vector3.one * scale;

                    GameObject itemPlaceholder = new GameObject($"Placeholder");
                    itemPlaceholder.transform.SetParent(itemContainer.transform, false);
                    itemsList.Add(itemPlaceholder);
                }
            }
            f_LimitedItemSourceView_Items.SetValue(instance.GetComponent<LimitedItemSourceView>(), itemsList);
            return instance;
        }
    }
}
