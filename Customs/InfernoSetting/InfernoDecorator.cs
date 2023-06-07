using Kitchen;
using Kitchen.Layouts;
using KitchenData;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenInferno.Customs.Inferno
{
    public class InfernoDecorator : Decorator
    {
        public class DecorationsConfiguration : IDecorationConfiguration
        {
            public struct Scatter
            {
                public float Probability;

                public Appliance Appliance;
            }

            public List<Scatter> Scatters;

            public Appliance FrontTile;

            public Appliance Bridge;

            public Appliance Fog;

            public Appliance FrontWall;

            public Appliance FrontPillar;

            public float BorderSpacing;

            public IDecorator Decorator => new InfernoDecorator();
        }

        public override bool Decorate(Room room)
        {
            if (Configuration is DecorationsConfiguration decorationsConfiguration)
            {
                Bounds bounds = Blueprint.GetBounds();
                Vector3 frontDoor = Blueprint.GetFrontDoor();
                for (float num = bounds.min.x - 4f; num <= bounds.max.x + 4f; num += 1f)
                {
                    foreach (DecorationsConfiguration.Scatter scatter in decorationsConfiguration.Scatters)
                    {
                        if (Random.value < scatter.Probability)
                        {
                            NewPiece(scatter.Appliance, num, bounds.min.y - 6f);
                        }
                        if (Random.value < scatter.Probability)
                        {
                            NewPiece(scatter.Appliance, num, bounds.max.y + 3f);
                        }
                    }
                }
                for (float num2 = bounds.min.y - 2f; num2 <= bounds.max.y + 2f; num2 += 1f)
                {
                    foreach (DecorationsConfiguration.Scatter scatter2 in decorationsConfiguration.Scatters)
                    {
                        if (num2 > bounds.min.y && Random.value < scatter2.Probability)
                        {
                            NewPiece(scatter2.Appliance, bounds.min.x - 3f, num2);
                        }
                        if (Random.value < scatter2.Probability)
                        {
                            NewPiece(scatter2.Appliance, bounds.max.x + 4f, num2);
                        }
                    }
                }
                for (float num3 = bounds.min.y - 2f; num3 <= bounds.max.y + 12f; num3 += 1f)
                {
                    NewPiece(decorationsConfiguration.FrontTile, num3, bounds.min.y - 1f);
                    NewPiece(decorationsConfiguration.FrontTile, num3, bounds.min.y - 2f);
                }
                if (decorationsConfiguration.FrontWall != null && decorationsConfiguration.BorderSpacing != 0f)
                {
                    for (float num4 = bounds.min.x; num4 <= bounds.max.x + 1f; num4 += decorationsConfiguration.BorderSpacing)
                    {
                        NewPiece(decorationsConfiguration.FrontPillar, num4, bounds.min.y - 0.7f);
                        if (!(Mathf.Abs(num4 - frontDoor.x) < 0.7f) && !(num4 >= bounds.max.x + decorationsConfiguration.BorderSpacing / 2f))
                        {
                            NewPiece(decorationsConfiguration.FrontWall, num4, bounds.min.y - 0.7f);
                        }
                    }
                }
                NewPiece(decorationsConfiguration.Bridge, bounds.min.x, bounds.min.y - 1f);
                for (float num5 = bounds.min.x; num5 <= bounds.max.x + 10f; num5 += 1f)
                {
                    NewPiece(decorationsConfiguration.FrontTile, num5, bounds.min.y - 1f);
                    NewPiece(decorationsConfiguration.FrontTile, num5, bounds.min.y - 2f);
                }
                for (float num6 = bounds.min.x - 1f; num6 <= bounds.max.x + 1f; num6 += 1f)
                {
                    NewPiece(AssetReference.OutdoorMovementBlocker, num6, bounds.min.y - 3f);
                }
                float x = ((frontDoor.x < 3f) ? (frontDoor.x + 1f) : (frontDoor.x - 1f));
                NewPiece(AssetReference.Nameplate, x, bounds.min.y - 1f);
                NewPiece(AssetReference.OutdoorMovementBlocker, bounds.min.x - 1f, bounds.min.y - 1f);
                NewPiece(AssetReference.OutdoorMovementBlocker, bounds.min.x - 1f, bounds.min.y - 2f);
                NewPiece(AssetReference.OutdoorMovementBlocker, bounds.max.x + 1f, bounds.min.y - 1f);
                NewPiece(AssetReference.OutdoorMovementBlocker, bounds.max.x + 1f, bounds.min.y - 2f);
                NewPiece(Profile.ExternalBin, frontDoor.x, frontDoor.z - 3f);
                NewPiece(decorationsConfiguration.FrontTile, frontDoor.x, frontDoor.z - 3f);
                NewPiece(decorationsConfiguration.Fog, 0f, 0f);
                return true;
            }
            return false;
        }
    }
}
