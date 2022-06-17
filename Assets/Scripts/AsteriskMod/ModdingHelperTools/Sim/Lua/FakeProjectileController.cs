using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeProjectileController
    {
        private FakeProjectile p;
        private readonly FakeSpriteController spr;
        //* private readonly Dictionary<string, DynValue> vars = new Dictionary<string, DynValue>();
        private float lastX;
        private float lastY;
        private float lastAbsX;
        private float lastAbsY;
        public static bool globalPixelPerfectCollision;

        public FakeProjectileController(FakeProjectile p)
        {
            this.p = p;
            spr = new FakeSpriteController(p.GetComponent<Image>());
        }

        // The x position of the sprite, relative to the arena position and its anchor.
        public float x
        {
            get { return p == null ? lastX : p.self.anchoredPosition.x - FakeArenaManager.arenaCenter.x; }
            set
            {
                if (p != null)
                    p.self.anchoredPosition = new Vector2(value + FakeArenaManager.arenaCenter.x, p.self.anchoredPosition.y);
            }
        }

        // The y position of the sprite, relative to the arena position and its anchor.
        public float y
        {
            get { return p == null ? lastY : p.self.anchoredPosition.y - FakeArenaManager.arenaCenter.y; }
            set
            {
                if (p != null)
                    p.self.anchoredPosition = new Vector2(p.self.anchoredPosition.x, value + FakeArenaManager.arenaCenter.y);
            }
        }

        // The x position of the sprite, relative to the bottom left corner of the screen.
        public float absx
        {
            get { return p == null ? lastAbsX : p.self.position.x; }
            set
            {
                if (p != null)
                    p.self.position = new Vector2(value, p.self.position.y);
            }
        }

        // The y position of the sprite, relative to the bottom left corner of the screen.
        public float absy
        {
            get { return p == null ? lastAbsY : p.self.position.y; }
            set
            {
                if (p != null)
                    p.self.position = new Vector2(p.self.position.x, value);
            }
        }

        /** ppcollision/ppchanged
        public bool ppcollision
        {
            get
            {
                if (p == null)
                    throw new CYFException("Attempted to get the collision mode of a removed bullet.");
                return p.isPP();
            }
            set
            {
                if (p == null)
                    throw new CYFException("Attempted to set the collision mode of a removed bullet.");
                if (!p.isPP() && value)
                    p.texture = ((Texture2D)p.GetComponent<Image>().mainTexture).GetPixels32();
                p.ppcollision = value;
                p.ppchanged = true;
            }
        }

        public bool ppchanged
        {
            get
            {
                if (p == null)
                    throw new CYFException("Attempted to get the value of bullet.ppchanged from a removed bullet.");
                return p.ppchanged;
            }
        }
        */

        public bool isactive
        {
            get { return p != null; }
        }

        public bool isPersistent = false;

        public string layer
        {
            get { return spr.gameObject.transform.parent.name == "BulletPool" ? "" : spr.gameObject.transform.parent.name.Substring(0, spr.gameObject.transform.parent.name.Length - 6); }
            set
            {
                Transform parent = spr.gameObject.transform.parent;
                try
                {
                    spr.gameObject.transform.SetParent(GameObject.Find(value == "" ? "BulletPool" : value + "Bullet").transform);
                }
                catch { spr.gameObject.transform.SetParent(parent); }
            }
        }

        public FakeSpriteController sprite
        {
            get { return spr; }
        }

        /** ResetCollisionSystem()
        public void ResetCollisionSystem()
        {
            if (p == null)
                throw new CYFException("Attempted to reset the personal collision system of a removed bullet.");
            p.ppchanged = false;
            p.ppcollision = globalPixelPerfectCollision;
        }
        */

        public void Remove()
        {
            if (!isactive) return;
            Transform[] pcs = UnitaleUtil.GetFirstChildren(p.transform);
            for (int i = 1; i < pcs.Length; i++)
                try { pcs[i].GetComponent<FakeProjectile>().ctrl.Remove(); }
                catch { new FakeSpriteController(pcs[i].GetComponent<Image>()).Remove(); }
            lastX = x;
            lastY = y;
            lastAbsX = absx;
            lastAbsY = absy;
            if (p.gameObject.GetComponent<KeyframeCollection>() != null)
                Object.Destroy(p.gameObject.GetComponent<KeyframeCollection>());
            p.gameObject.GetComponent<Mask>().enabled = false;
            p.gameObject.GetComponent<RectMask2D>().enabled = false;
            //*spr.StopAnimation();
            //*BulletPool.instance.Requeue(p);
            GameObject.Destroy(p.gameObject);
            p = null;
        }

        public void Move(float newX, float newY) { MoveToAbs(absx + newX, absy + newY); }

        public void MoveTo(float newX, float newY) { MoveToAbs(FakeArenaManager.arenaCenter.x + newX, FakeArenaManager.arenaCenter.y + newY); }

        public void MoveToAbs(float newX, float newY)
        {
            if (p == null)
            {
                if (GlobalControls.retroMode)
                    return;
                throw new CYFException("Attempted to move a removed bullet. You can use a bullet's isactive property to check if it has been removed.");
            }

            if (GlobalControls.retroMode) p.self.anchoredPosition = new Vector2(newX, newY);
            else p.self.position = new Vector2(newX, newY);
        }

        public void SendToTop() { p.self.SetAsLastSibling(); }

        public void SendToBottom() { p.self.SetAsFirstSibling(); }

        /** Get/SetVar()
        public void SetVar(string name, DynValue value)
        {
            if (name == null)
                throw new CYFException("bullet.SetVar: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
            vars[name] = value;
        }

        public DynValue GetVar(string name)
        {
            if (name == null)
                throw new CYFException("bullet.GetVar: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
            DynValue retval;
            return vars.TryGetValue(name, out retval) ? retval : null;
        }

        public DynValue this[string key]
        {
            get { return GetVar(key); }
            set { SetVar(key, value); }
        }
        */

        /** isColliding()
        public bool isColliding()
        {
            if (p == null)
                return false;
            return p.isPP() ? p.HitTestPP() : p.HitTest();
        }
        */
    }
}
