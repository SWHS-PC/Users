using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace SpaceShip
{
    class Helpers
    {
        public const float PI = (float)Math.PI;

        public static Vector2 ComputeGravity(float seconds, float g, Vector2 from, Vector2 to)
        {
            // Compute the vector from the object to the gravitational source.
            Vector2 v = to - from;

            // Compute the length squared and the length of the vector, making
            // sure neither is zero to avoid future divide-by-zero.
            float lengthSquared = Math.Max(0.001f, v.LengthSquared());
            float length = (float)Math.Sqrt(lengthSquared);

            // The amount of acceleration is inversely proportional to the
            // length squared.
            float acceleration = seconds * g / lengthSquared;

            // To compute the acceleration vector:
            //  1. Divide v by its length, yielding a unit vector in the
            //     correct direction.
            //  2. Multiply the result by the acceleration.
            return v * (acceleration / length);
        }

        public static float ComputeOrbitalRadiansPerSecond(float gravity, float radius)
        {
            // Let g be gravity, which represents the product of mass times the
            // gravitational constant. Orbital velocity for a circular orbit is:
            //
            //  v = sqrt(g / r)
            //
            // Dividing by the circumference of the orbit (2 pi r) yields the number
            // of orbits per second (i.e., inverse of the period). However, we want
            // radians per second, so we multiply that result by 2 pi.
            //
            //       sqrt(g / r)
            //  a = -------------  * 2 pi
            //       2 pi * r
            //
            // The 2 pi cancels out, yielding sqrt(g / r) / r.

            return (float)(Math.Sqrt(gravity / radius) / radius);
        }

        public static Vector2 ComputeOrbitalVelocity(float orbitalAngle, float orbitalRadius, float angularVelocity)
        {
            float angle = orbitalAngle + (PI * 0.5f);
            float speed = orbitalRadius * angularVelocity;

            return new Vector2(
                (float)Math.Cos(angle) * speed,
                (float)Math.Sin(angle) * speed
                );
        }
    }
}
