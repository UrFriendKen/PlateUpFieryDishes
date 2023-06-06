using Kitchen;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenDishesOnFire
{
    public class PatchController : RestaurantSystem, IModSystem
    {
        private static PatchController _instance;
        protected override void Initialise()
        {
            base.Initialise();
            _instance = this;
        }

        protected override void OnUpdate()
        {
        }

        internal static bool OrderMatchCandidateFireState(Entity request, Entity candidate)
        {
            if (_instance == null)
            {
                return true;
            }
            return _instance.Has<CItemOnFire>(request) == _instance.Has<CItemOnFire>(candidate);
        }
    }
}
