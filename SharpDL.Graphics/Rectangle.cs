using System;
using SDL2;

namespace SharpDL.Graphics
{
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Bottom { get { return Y + Height; } }

        public int Top { get { return Y; } }

        public int Left { get { return X; } }

        public int Right { get { return X + Width; } }

        public static Rectangle Empty
        {
            get
            {
                return new Rectangle()
                {
                    Height = 0,
                    Width = 0,
                    X = 0,
                    Y = 0
                };
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Width == 0 && Height == 0 && X == 0 && Y == 0;
            }
        }

        public int Area { get { return Width * Height; } }

        public Point Location { get { return new Point(X, Y); } }

        public Point Center
        {
            get
            {
                return new Point(X + (Width / 2), Y + (Height / 2));
            }
        }

        public Rectangle(int x, int y, int width, int height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public SDL.SDL_Rect ToSDLRect()
        {
            var rect = new SDL.SDL_Rect();
            rect.w = Width;
            rect.h = Height;
            rect.x = X;
            rect.y = Y;
            return rect;
        }

        public bool Contains(Point point)
        {
            if (Left <= point.X && Right >= point.X && Top <= point.Y && Bottom >= point.Y)
                return true;
            return false;
        }

        public bool Contains(Rectangle rectangle)
        {
            if (Left <= rectangle.Left && Right >= rectangle.Right && Top <= rectangle.Top && Bottom >= rectangle.Bottom)
                return true;
            return false;
        }

        public bool Contains(Vector vector)
        {
            if (Left <= vector.X && Right >= vector.X && Top <= vector.Y && Bottom >= vector.Y)
                return true;
            return false;
        }

        /// <summary>
        /// Determines if two rectangles intersect.
        /// </summary>
        /// <param name="rectangle">Rectangle.</param>
        public bool Intersects(Rectangle rectangle)
        {
            return rectangle.Left <= Right && Left <= rectangle.Right && rectangle.Top <= Bottom && Top <= rectangle.Bottom;
        }

        public Vector GetIntersectionDepth(Rectangle rectangle)
        {
            // Calculate half sizes.
            float halfWidthA = Width / 2.0f;
            float halfHeightA = Height / 2.0f;
            float halfWidthB = rectangle.Width / 2.0f;
            float halfHeightB = rectangle.Height / 2.0f;

            // Calculate centers.
            var centerA = new Vector(Left + halfWidthA, Top + halfHeightA);
            var centerB = new Vector(rectangle.Left + halfWidthB, rectangle.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector(depthX, depthY);
        }

        public static Rectangle Union(Rectangle rect1, Rectangle rect2)
        {
            int x = Math.Min(rect1.X, rect2.X);
            int y = Math.Min(rect1.Y, rect2.Y);

            int w = Math.Max(rect1.Right, rect2.Right) - x;
            int h = Math.Max(rect1.Bottom, rect2.Bottom) - y;

            return new Rectangle(x, y, w, h);
        }

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            if (!a.Intersects(b))
                return Empty;

            int x = Math.Max(a.X, b.X);
            int y = Math.Max(a.Y, b.Y);

            int w = Math.Min(a.Right, b.Right) - x;
            int h = Math.Min(a.Bottom, b.Bottom) - y;

            return new Rectangle(x, y, w, h);
        }


        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1}, Width:{2}, Height:{3}}}", X, Y, Width, Height);
        }

        public bool Equals(Rectangle other)
        {
            return (X == other.X && Y == other.Y && Width == other.Width && Height == other.Height);
        }

        #region Operators

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !a.Equals(b);
        }

        #endregion
            
    }
}