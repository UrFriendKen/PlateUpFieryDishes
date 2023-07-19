using Kitchen;
using KitchenLib.References;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace KitchenInferno
{
    public class ProvideFireExtinguisher : NightSystem, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct SPerformed : IComponentData, IModComponent { }

        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnUpdate()
        {
            if (!HasStatus(Main.PYROMANIA_EFFECT_STATUS) || Has<SPerformed>())
                return;
            if (Main.PrefManager.Get<bool>(Main.DIFFICULTY_FIRE_EXTINGUISHER_START_ID))
                PostHelpers.CreateApplianceParcel(EntityManager, GetFallbackTile(), ApplianceReferences.FireExtinguisherHolder);
            Set<SPerformed>();
        }
    }
}
