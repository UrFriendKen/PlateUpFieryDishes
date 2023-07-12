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

        [HarmonyPatch(typeof(GroupReceiveItem), "Satisfy")]
        [HarmonyPostfix]
        static void Satisfy_Postfix(CWaitingForItem satisfied_order)
        {
            if (PatchController.GetFireOrderBonus(satisfied_order, out int fireOrderBonus))
                GroupReceiveItem_Patch2.FireOrderBonus += fireOrderBonus;
            if (PatchController.GetActiveFireBonus(satisfied_order, out int activeFireBonus))
                GroupReceiveItem_Patch2.ActiveFireBonus += activeFireBonus;
        }
    }


    [HarmonyPatch]
    internal static class GroupReceiveItem_Patch2
    {
        public static int FireOrderBonus;
        public static int ActiveFireBonus;

        static MethodBase TargetMethod()
        {
            Type type = AccessTools.FirstInner(typeof(GroupReceiveItem), t => t.Name.Contains("c__DisplayClass_OnUpdate_LambdaJob0"));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        static bool Prefix(Entity e)
        {
            FireOrderBonus = 0;
            ActiveFireBonus = 0;
            return !PatchController.StaticHas<COrderFramesDelay>(e);
        }

        static void Postfix(ref EntityContext ___ctx, Entity e, ref CGroupReward reward)
        {
            if (FireOrderBonus > 0)
            {
                CommitCompletedGroups.AddEvent(___ctx, e, Main.CustomDummyFireOrderAppliance?.GameDataObject?.ID ?? 0, FireOrderBonus);
                reward += FireOrderBonus;
            }
            if (ActiveFireBonus > 0)
            {
                CommitCompletedGroups.AddEvent(___ctx, e, Main.CustomDummyActiveFireAppliance?.GameDataObject?.ID ?? 0, ActiveFireBonus);
                reward += ActiveFireBonus;
            }
        }
    }
}
