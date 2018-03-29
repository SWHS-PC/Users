using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Spaceship
{
    class Computationals
    {
        public const float PI = (float)Math.PI;

        public static Vector2 ComputeGravity(float seconds, float g, Vector2 from, Vector2 to)
        {
            Vector2 v = to - from;
            
            float lengthSquared = Math.Max(0.001f, v.LengthSquared());
            float length = (float)Math.Sqrt(lengthSquared);
            float acceleration = seconds * g / lengthSquared;
            return v * (acceleration / length);
        }

        public static float ComputeOrbitalRadiansPerSecond(float gravity, float radius)
        {
            return (float)(Math.Sqrt(gravity / radius) / radius);
        }

        public static Vector2 ComputeOrbitalVelocity(float orbitalAngle, float orbitalRadius, float angularVelocity)
        {
            float angle = orbitalAngle + (PI * 0.5f);
            float speed = orbitalRadius * angularVelocity;

            return new Vector2((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);
        }

        public static Vector2 ComputeOrbitalPosition(float orbitalAngle, float orbitalRadius)
        {
            return new Vector2((float)Math.Cos(orbitalAngle) * orbitalRadius, (float)Math.Sin(orbitalAngle) * orbitalRadius);
        }
    }
}
