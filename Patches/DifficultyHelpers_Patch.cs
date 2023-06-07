using HarmonyLib;
using Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenInferno.Patches
{
    [HarmonyPatch]
    static class DifficultyHelpers_Patch
    {
        [HarmonyPatch(typeof(DifficultyHelpers), nameof(DifficultyHelpers.FireSpreadModifier))]
        [HarmonyPostfix]
        static void FireSpreadModifier_Postfix(ref float __result)
        {
            __result *= PatchController.GetFireSpreadModifier();
        }
    }
}
