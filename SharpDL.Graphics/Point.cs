using SDL2;

namespace SharpDL.Graphics
{
    public struct Point
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Point(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point value1, Point value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        public static bool operator !=(Point value1, Point value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                var o = (Point)obj;
                if (X == o.X && Y == o.Y)
                    return true;
                return false;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public SDL.SDL_Point ToSDLPoint()
        {
            var pt = new SDL.SDL_Point();
            pt.x = X;
            pt.y = Y;
            return pt;
        }
    }
}