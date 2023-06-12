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

            public Appliance FrontWall;

            public Appliance FrontPillar;

            public Appliance Ground;

            public float BorderSpacing;

            public IDecorator Decorator => new InfernoDecorator();
        }

        public override bool Decorate(Room room)
        {
            if (Configuration is DecorationsConfiguration decorationsConfiguration)
            {
                Bounds bounds = Blueprint.GetBounds();
                Vector3 frontDoor = Blueprint.GetFrontDoor();
                NewPiece(decorationsConfiguration.Ground, 0f, 0f);
                for (float num = bounds.min.x - 4f; num <= bounds.max.x + 4f; num += 1f)
                {
                    bool drawnBottom = false;
                    bool drawnTop = false;
                    foreach (DecorationsConfiguration.Scatter scatter in decorationsConfiguration.Scatters)
                    {
                        if (!drawnBottom && Random.value < scatter.Probability)
                        {
                            drawnBottom = true;
                            NewRandomScatter(scatter.Appliance, num, bounds.min.y - 6f);
                        }
                        if (!drawnTop && Random.value < scatter.Probability)
                        {
                            drawnTop = true;
                            NewRandomScatter(scatter.Appliance, num, bounds.max.y + 3f);
                        }
                    }
                }
                for (float num2 = bounds.min.y - 2f; num2 <= bounds.max.y + 2f; num2 += 1f)
                {
                    bool drawnLeft = false;
                    bool drawnRight = false;
                    foreach (DecorationsConfiguration.Scatter scatter2 in decorationsConfiguration.Scatters)
                    {
                        if (!drawnLeft && num2 > bounds.min.y && Random.value < scatter2.Probability)
                        {
                            drawnLeft = true;
                            NewRandomScatter(scatter2.Appliance, bounds.min.x - 3f, num2);
                        }
                        if (!drawnRight && Random.value < scatter2.Probability)
                        {
                            drawnRight = true;
                            NewRandomScatter(scatter2.Appliance, bounds.max.x + 4f, num2);
                        }
                    }
                }

                void NewRandomScatter(Appliance appliance, float x, float y)
                {
                    Vector2 randomXZOffset = Random.insideUnitCircle * 0.3f;
                    //Vector3 randomScale = (Random.insideUnitSphere * 0.2f) + (Vector3.one * 0.1f);
                    Vector2 randomXZfacing = Random.insideUnitCircle.normalized;
                    NewPiece(appliance, x + randomXZOffset.x, y + randomXZOffset.y, Quaternion.LookRotation(new Vector3(randomXZfacing.x, 0f, randomXZfacing.y), Vector3.up));
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
                return true;
            }
            return false;
        }
    }
}
