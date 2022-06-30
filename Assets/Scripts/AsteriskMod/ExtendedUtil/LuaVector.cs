using UnityEngine;

namespace AsteriskMod.ExtendedUtil
{
    public class LuaVector
    {
        public float x;
        public float y;

        public LuaVector(float x, float y)
        {
            this.x = x;
            this.y = y;
            //MoveAction = (a, b) => { };
        }

        public float? this[int index]
        {
            get
            {
                if (index == 1) return x;
                if (index == 2) return y;
                return null;
            }
            set
            {
                if (!value.HasValue) return;
                if (index == 1) x = value.Value;
                if (index == 2) y = value.Value;
            }
        }

        public float Magnitude() { return Mathf.Sqrt(x * x + y * y); }

        public void Normalize()
        {
            float magnitude = Magnitude();
            if (magnitude < 1.192092896e-07f)
            {
                x = y = 0f;
                return;
            }
            x /= magnitude;
            y /= magnitude;
        }

        public float Distance(LuaVector other) { return (this - other).Magnitude(); }


        public static LuaVector operator +(LuaVector a, LuaVector b) { return new LuaVector(a.x + b.x, a.y + b.y); }
        public static LuaVector operator -(LuaVector a, LuaVector b) { return new LuaVector(a.x - b.x, a.y - b.y); }
        public static LuaVector operator *(LuaVector a, float b) { return new LuaVector(a.x * b, a.y * b); }
        public static LuaVector operator /(LuaVector a, float b) { return new LuaVector(a.x / b, a.y / b); }

        public static LuaVector operator -(LuaVector a) { return new LuaVector(-a.x, -a.y); }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType()) return false;
            LuaVector other = (LuaVector)obj;
            return (this.x == other.x) && (this.y == other.y);
        }
        public static bool operator ==(LuaVector a, LuaVector b) { return a.Equals(b); }
        public static bool operator !=(LuaVector a, LuaVector b) { return !(a == b); }
        public override int GetHashCode() { return this.x.GetHashCode() ^ this.y.GetHashCode(); }

        public static LuaVector Zero { get { return new LuaVector(0, 0); } }


        public LuaVector Copy() { return new LuaVector(this.x, this.y); }
        public LuaVector Clone() { return new LuaVector(this.x, this.y); }

        /*
        public System.Action<float, float> MoveAction;

        public void Move(float x, float y)
        {
            MoveAction(x, y);
            this.x += x;
            this.y += y;
        }

        public void MoveTo(float newX, float newY)
        {
            MoveAction(x, y);
            this.x = newX;
            this.y = newY;
        }
        */
    }
}
