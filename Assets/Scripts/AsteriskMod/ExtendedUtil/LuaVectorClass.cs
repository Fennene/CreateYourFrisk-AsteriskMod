using UnityEngine;

namespace AsteriskMod.ExtendedUtil
{
    public class LuaVectorClass
    {
        public float x;
        public float y;

        public LuaVectorClass(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float Magnitude() { return Mathf.Sqrt(x * x + y * y); }

        public void Normalize()
        {
            float magnitude = Magnitude();
            if (magnitude < 1.192092896e-07f)
            {
                x = 0f;
                y = 0f;
                return;
            }
            x /= magnitude;
            y /= magnitude;
        }

        public static LuaVectorClass operator +(LuaVectorClass a, LuaVectorClass b) { return new LuaVectorClass(a.x + b.x, a.y + b.y); }
        public static LuaVectorClass operator -(LuaVectorClass a, LuaVectorClass b) { return new LuaVectorClass(a.x - b.x, a.y - b.y); }
        public static LuaVectorClass operator *(LuaVectorClass a, float b) { return new LuaVectorClass(a.x * b, a.y * b); }
        public static LuaVectorClass operator /(LuaVectorClass a, float b) { return new LuaVectorClass(a.x / b, a.y / b); }

        public static LuaVectorClass operator -(LuaVectorClass a) { return new LuaVectorClass(-a.x, -a.y); }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType()) return false;
            LuaVectorClass other = (LuaVectorClass)obj;
            return (this.x == other.x) && (this.y == other.y);
        }

        public static bool operator ==(LuaVectorClass a, LuaVectorClass b) { return a.Equals(b); }
        public static bool operator !=(LuaVectorClass a, LuaVectorClass b) { return !(a == b); }

        public override int GetHashCode() { return this.x.GetHashCode() ^ this.y.GetHashCode(); }
    }
}
