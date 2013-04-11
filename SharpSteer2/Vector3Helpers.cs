// Copyright (c) 2002-2003, Sony Computer Entertainment America
// Copyright (c) 2002-2003, Craig Reynolds <craig_reynolds@playstation.sony.com>
// Copyright (C) 2007 Bjoern Graf <bjoern.graf@gmx.net>
// Copyright (C) 2007 Michael Coles <michael@digini.com>
// All rights reserved.
//
// This software is licensed as described in the file license.txt, which
// you should have received as part of this distribution. The terms
// are also available at http://www.codeplex.com/SharpSteer/Project/License.aspx.

using System;
using Microsoft.Xna.Framework;

namespace SharpSteer2
{
    public static class Vector3Helpers
    {
        /// <summary>
        /// return component of vector parallel to a unit basis vector
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="unitBasis">A unit length basis vector</param>
        /// <returns></returns>
        public static Vector3 ParallelComponent(Vector3 vector, Vector3 unitBasis)
        {
            float projection = Vector3.Dot(vector, unitBasis);
            return unitBasis * projection;
        }

        /// <summary>
        /// return component of vector perpendicular to a unit basis vector
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="unitBasis">A unit length basis vector</param>
        /// <returns></returns>
        public static Vector3 PerpendicularComponent(Vector3 vector, Vector3 unitBasis)
        {
            return (vector - ParallelComponent(vector, unitBasis));
        }

        /// <summary>
        /// clamps the length of a given vector to maxLength.  If the vector is
        /// shorter its value is returned unaltered, if the vector is longer
        /// the value returned has length of maxLength and is paralle to the
        /// original input.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static Vector3 TruncateLength(Vector3 vector, float maxLength)
        {
            float maxLengthSquared = maxLength * maxLength;
            float vecLengthSquared = vector.LengthSquared();
            if (vecLengthSquared <= maxLengthSquared)
                return vector;

            return (vector * (maxLength / (float)Math.Sqrt(vecLengthSquared)));
        }

        // forces a 3d position onto the XZ (aka y=0) plane
        public static Vector3 SetYtoZero(Vector3 vector)
        {
            return new Vector3(vector.X, 0, vector.Z);
        }

        // rotate this vector about the global Y (up) axis by the given angle
        public static Vector3 RotateAboutGlobalY(Vector3 vector, float angle)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);
            return new Vector3((vector.X * c) + (vector.Z * s), (vector.Y), (vector.Z * c) - (vector.X * s));
        }

        // version for caching sin/cos computation
        public static Vector3 RotateAboutGlobalY(Vector3 vector, float angle, ref float sin, ref float cos)
        {
            // is both are zero, they have not be initialized yet
// ReSharper disable CompareOfFloatsByEqualityOperator
            if (sin == 0 && cos == 0)
// ReSharper restore CompareOfFloatsByEqualityOperator
            {
                sin = (float)Math.Sin(angle);
                cos = (float)Math.Cos(angle);
            }
            return new Vector3((vector.X * cos) + (vector.Z * sin), vector.Y, (vector.Z * cos) - (vector.X * sin));
        }

        // if this position is outside sphere, push it back in by one diameter
        public static Vector3 SphericalWrapAround(Vector3 vector, Vector3 center, float radius)
        {
            Vector3 offset = vector - center;
            float r = offset.Length();
            if (r > radius)
                return vector + ((offset / r) * radius * -2);
            return vector;
        }

        /// <summary>
        /// Returns a position randomly distributed on a disk of unit radius
        /// on the XZ (Y=0) plane, centered at the origin.  Orientation will be
        /// random and length will range between 0 and 1
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomVectorOnUnitRadiusXZDisk()
        {
            Vector3 v;
            do
            {
                v.X = (Utilities.Random() * 2) - 1;
                v.Y = 0;
                v.Z = (Utilities.Random() * 2) - 1;
            }
            while (v.Length() >= 1);

            return v;
        }

        /// <summary>
        /// Returns a position randomly distributed inside a sphere of unit radius
        /// centered at the origin.  Orientation will be random and length will range
        /// between 0 and 1
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomVectorInUnitRadiusSphere()
        {
            Vector3 v = new Vector3();
            do
            {
                v.X = (Utilities.Random() * 2) - 1;
                v.Y = (Utilities.Random() * 2) - 1;
                v.Z = (Utilities.Random() * 2) - 1;
            }
            while (v.Length() >= 1);

            return v;
        }

        /// <summary>
        /// Returns a position randomly distributed on the surface of a sphere
        /// of unit radius centered at the origin.  Orientation will be random
        /// and length will be 1
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomUnitVector()
        {
            Vector3 temp = RandomVectorInUnitRadiusSphere();
            temp.Normalize();

            return temp;
        }

        /// <summary>
        /// Returns a position randomly distributed on a circle of unit radius
        /// on the XZ (Y=0) plane, centered at the origin.  Orientation will be
        /// random and length will be 1
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomUnitVectorOnXZPlane()
        {
            Vector3 temp = RandomVectorInUnitRadiusSphere();

            temp.Y = 0;
            temp.Normalize();

            return temp;
        }

        // ----------------------------------------------------------------------------
        // used by limitMaxDeviationAngle / limitMinDeviationAngle below
        private static Vector3 LimitDeviationAngleUtility(bool insideOrOutside, Vector3 source, float cosineOfConeAngle, Vector3 basis)
        {
            // immediately return zero length input vectors
            float sourceLength = source.Length();

            if (Math.Abs(sourceLength - 0) < float.Epsilon)
                return source;

            // measure the angular diviation of "source" from "basis"
            Vector3 direction = source / sourceLength;

            float cosineOfSourceAngle = Vector3.Dot(direction, basis);

            // Simply return "source" if it already meets the angle criteria.
            // (note: we hope this top "if" gets compiled out since the flag
            // is a constant when the function is inlined into its caller)
            if (insideOrOutside)
            {
                // source vector is already inside the cone, just return it
                if (cosineOfSourceAngle >= cosineOfConeAngle)
                    return source;
            }
            else if (cosineOfSourceAngle <= cosineOfConeAngle)
                    return source;

            // find the portion of "source" that is perpendicular to "basis"
            Vector3 perp = PerpendicularComponent(source, basis);

            // normalize that perpendicular
            Vector3 unitPerp = perp;
            unitPerp.Normalize();

            // construct a new vector whose length equals the source vector,
            // and lies on the intersection of a plane (formed the source and
            // basis vectors) and a cone (whose axis is "basis" and whose
            // angle corresponds to cosineOfConeAngle)
            float perpDist = (float)Math.Sqrt(1 - (cosineOfConeAngle * cosineOfConeAngle));
            Vector3 c0 = basis * cosineOfConeAngle;
            Vector3 c1 = unitPerp * perpDist;
            return (c0 + c1) * sourceLength;
        }

        // ----------------------------------------------------------------------------
        // Enforce an upper bound on the angle by which a given arbitrary vector
        // diviates from a given reference direction (specified by a unit basis
        // vector).  The effect is to clip the "source" vector to be inside a cone
        // defined by the basis and an angle.
        public static Vector3 LimitMaxDeviationAngle(Vector3 source, float cosineOfConeAngle, Vector3 basis)
        {
            return LimitDeviationAngleUtility(true, // force source INSIDE cone
                source, cosineOfConeAngle, basis);
        }

        // ----------------------------------------------------------------------------
        // Enforce a lower bound on the angle by which a given arbitrary vector
        // diviates from a given reference direction (specified by a unit basis
        // vector).  The effect is to clip the "source" vector to be outside a cone
        // defined by the basis and an angle.
        public static Vector3 LimitMinDeviationAngle(Vector3 source, float cosineOfConeAngle, Vector3 basis)
        {
            return LimitDeviationAngleUtility(false, // force source OUTSIDE cone
                source, cosineOfConeAngle, basis);
        }

        // ----------------------------------------------------------------------------
        // Returns the distance between a point and a line.  The line is defined in
        // terms of a point on the line ("lineOrigin") and a UNIT vector parallel to
        // the line ("lineUnitTangent")
        public static float DistanceFromLine(Vector3 point, Vector3 lineOrigin, Vector3 lineUnitTangent)
        {
            Vector3 offset = point - lineOrigin;
            Vector3 perp = PerpendicularComponent(offset, lineUnitTangent);
            return perp.Length();
        }

        // ----------------------------------------------------------------------------
        // given a vector, return a vector perpendicular to it (note that this
        // arbitrarily selects one of the infinitude of perpendicular vectors)
        public static Vector3 FindPerpendicularIn3d(Vector3 direction)
        {
            // to be filled in:
            Vector3 quasiPerp;  // a direction which is "almost perpendicular"
            Vector3 result;     // the computed perpendicular to be returned

            // three mutually perpendicular basis vectors
            Vector3 i = Vector3.Right;
            Vector3 j = Vector3.Up;
            Vector3 k = Vector3.Backward;

            // measure the projection of "direction" onto each of the axes
            float id = Vector3.Dot(i, direction);
            float jd = Vector3.Dot(j, direction);
            float kd = Vector3.Dot(k, direction);

            // set quasiPerp to the basis which is least parallel to "direction"
            if ((id <= jd) && (id <= kd))
            {
                quasiPerp = i;               // projection onto i was the smallest
            }
            else
            {
                if ((jd <= id) && (jd <= kd))
                    quasiPerp = j;           // projection onto j was the smallest
                else
                    quasiPerp = k;           // projection onto k was the smallest
            }

            // return the cross product (direction x quasiPerp)
            // which is guaranteed to be perpendicular to both of them
            Vector3.Cross(ref direction, ref quasiPerp, out result);

            return result;
        }
    }
}
