using HarmonyLib;
using Kitchen;
using System;
using System.Reflection;
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


    [HarmonyPatch]
    static class GroupReceiveItem_Patch2
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.FirstInner(typeof(GroupReceiveItem), t => t.Name.Contains("c__DisplayClass_OnUpdate_LambdaJob0"));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        static bool Prefix(Entity e)
        {
            return !PatchController.StaticHas<COrderFramesDelay>(e);
        }
    }
}
