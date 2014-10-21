﻿using SDL2;

namespace SharpDL.Graphics
{
    public struct Vector
    {
        public float X { get; set; }

        public float Y { get; set; }

        public static Vector One { get { return new Vector() { X = 1, Y = 1 }; } }

        public static Vector Zero { get { return new Vector() { X = 0, Y = 0 }; } }

        public Vector(float x, float y)
            : this()
        {
            X = x;
            Y = y;
        }

        public Vector Transform(Matrix matrix)
        {
            return new Vector((X * matrix.Row1Col1) + (Y * matrix.Row2Col1), (X * matrix.Row2Col1) + (Y * matrix.Row2Col2));
        }

        public Vector Add(Vector vector)
        {
            float x = X + vector.X;
            float y = Y + vector.Y;
            return new Vector(x, y);
        }

        public Vector Subtract(Vector vector)
        {
            float x = X - vector.X;
            float y = Y - vector.Y;
            return new Vector(x, y);
        }

        public static Vector operator -(Vector value1, Vector value2)
        {
            return value1.Subtract(value2);
        }

        public static Vector operator +(Vector value1, Vector value2)
        {
            return value1.Add(value2);
        }

        public static bool operator ==(Vector value1, Vector value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        public static bool operator !=(Vector value1, Vector value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector)
            {
                var o = (Vector)obj;
                if (X == o.X && Y == o.Y)
                    return true;
                return false;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int)(Y * 5f + X * 31f % int.MaxValue);
        }

        public SDL.SDL_Point ToSDLPoint()
        {
            var pt = new SDL.SDL_Point();
            pt.x = (int)X;
            pt.y = (int)Y;
            return pt;
        }
    }
}