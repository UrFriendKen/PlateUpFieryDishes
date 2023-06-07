using HarmonyLib;
using Kitchen;
using Unity.Entities;

namespace KitchenInferno.Patches
{
    [HarmonyPatch]
    static class GroupReceiveItem_Patch
    {
        [HarmonyPatch(typeof(GroupReceiveItem), "IsRequestSatisfied")]
        [HarmonyPrefix]
        static bool IsRequestSatisfied_Prefix(Entity request, Entity candidate, ref bool __result)
        {
            __result = PatchController.OrderMatchCandidateFireState(request, candidate);
            return __result;
        }
    }
}
