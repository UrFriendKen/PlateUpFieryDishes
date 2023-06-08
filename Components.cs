using KitchenData;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace KitchenInferno
{
    public struct CItemOnFire : IComponentData, IModComponent
    {
        public float BurningDuration;
        public float BurnSpeed;
    }

    public struct COrderFramesDelay : IComponentData, IModComponent
    {
        public int Remaining;
    }

    public struct CDestroyItemOnFireDuration : IComponentData, IModComponent
    {
        public float TotalTime;
    }

    public struct CFlammableItemsModifier : IEffectType, IAttachableProperty, IComponentData, IModComponent
    {
        // Percentage Change between -1 and inf
        public float BurnSpeedChange;
    }

    public struct CFireOrderChance : IEffectType, IAttachableProperty, IComponentData, IModComponent
    {
        // Percentage between 0 and 1, inclusive
        public float OrderChance;
    }

    public struct CFireSpreadModifier : IEffectType, IAttachableProperty, IComponentData, IModComponent
    {
        // Percentage between 0 and inf
        public float SpreadChanceModifier;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CCatchFireOnFailurePyromania : IApplianceProperty, IAttachableProperty, IComponentData, IModComponent { }
}
