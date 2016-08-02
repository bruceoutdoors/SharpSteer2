using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SharpSteer2.Pathway
{
    /// <summary>
    /// A path consisting of a series of gates which must be passed through
    /// </summary>
    public class GatewayPathway
        : IPathway
    {
        public PolylinePathway Centerline
        {
            get
            {
                return _trianglePathway.Centerline;
            }
        }

        private readonly TrianglePathway _trianglePathway;
        public TrianglePathway TrianglePathway
        {
            get
            {
                return _trianglePathway;
            }
        }

        public GatewayPathway(IEnumerable<Gateway> gateways, bool cyclic = false)
        {
            List<TrianglePathway.Triangle> triangles = new List<TrianglePathway.Triangle>();

            bool first = true;
            Gateway previous = default(Gateway);
            Vector3 previousNormalized = Vector3.Zero;
            foreach (var gateway in gateways)
            {
                var n = Vector3.Normalize(gateway.B - gateway.A);

                if (!first)
                {
                    if (Vector3.Dot(n, previousNormalized) < 0)
                    {
                        triangles.Add(new TrianglePathway.Triangle(previous.A, previous.B, gateway.A));
                        triangles.Add(new TrianglePathway.Triangle(previous.A, gateway.A, gateway.B));
                    }
                    else
                    {
                        triangles.Add(new TrianglePathway.Triangle(previous.A, previous.B, gateway.A));
                        triangles.Add(new TrianglePathway.Triangle(previous.B, gateway.A, gateway.B));
                    }
                }
                first = false;

                previousNormalized = n;
                previous = gateway;
            }

            _trianglePathway = new TrianglePathway(triangles, cyclic);

        }

        public struct Gateway
        {
            public readonly Vector3 A;
            public readonly Vector3 B;

            public Gateway(Vector3 a, Vector3 b)
                : this()
            {
                A = a;
                B = b;
            }
        }

        public Vector3 MapPointToPath(Vector3 point, out Vector3 tangent, out float outside)
        {
            return _trianglePathway.MapPointToPath(point, out tangent, out outside);
        }

        public Vector3 MapPathDistanceToPoint(float pathDistance)
        {
            return _trianglePathway.MapPathDistanceToPoint(pathDistance);
        }

        public float MapPointToPathDistance(Vector3 point)
        {
            return _trianglePathway.MapPointToPathDistance(point);
        }
    }
}
