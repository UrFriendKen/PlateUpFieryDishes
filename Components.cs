using KitchenMods;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace KitchenDishesOnFire
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CItemOnFire : IComponentData, IModComponent
    {
    }

    public struct CDestroyItemOnFireDuration : IComponentData, IModComponent
    {
        public float StartTime;
        public float RemainingTime;
    }
}
