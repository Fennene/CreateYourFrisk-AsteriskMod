using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeLuaStaticTextManager : FakeStaticTextManager
    {
        private GameObject container;
        private Color textColor;
        private float xScale = 1;
        private float yScale = 1;

        private bool autoDestroyed;
        public bool isactive { get { return container != null && !autoDestroyed; } }

        protected override void Awake()
        {
            base.Awake();
            if (!IsUI)
                transform.SetParent(GameObject.Find("TopLayer").transform);
            container = gameObject;
        }

        //private void CheckExists() { if (!isactive) throw new CYFException("Attempt to perform action on removed static text object."); }

        public virtual void DestroyText()
        {
            if (!isactive) throw new CYFException("Attempt to remove a removed static text object.");
            autoDestroyed = true;
            GameObject.Destroy(this.gameObject);
        }


        public int x
        {
            get
            {
                //CheckExists();
                return Mathf.RoundToInt(container.transform.localPosition.x);
            }
            set { MoveTo(value, y); }
        }

        public int y
        {
            get
            {
                //CheckExists();
                return Mathf.RoundToInt(container.transform.localPosition.y);
            }
            set { MoveTo(x, value); }
        }

        public int absx
        {
            get
            {
                //CheckExists();
                return Mathf.RoundToInt(container.transform.position.x);
            }
            set { MoveToAbs(value, absy); }
        }

        public int absy
        {
            get
            {
                //CheckExists();
                return Mathf.RoundToInt(container.transform.position.y);
            }
            set { MoveTo(absx, value); }
        }

        public int textMaxWidth
        {
            get
            {
                //CheckExists();
                return _textMaxWidth;
            }
            set
            {
                //CheckExists();
                _textMaxWidth = value < 16 ? 16 : value;
            }
        }

        public float xscale
        {
            get { return xScale; }
            set
            {
                xScale = value;
                Scale(xScale, yScale);
            }
        }

        public float yscale
        {
            get { return yScale; }
            set
            {
                yScale = value;
                Scale(xScale, yScale);
            }
        }

        public void Scale(float xs, float ys)
        {
            //CheckExists();
            xScale = xs;
            yScale = ys;

            container.gameObject.GetComponent<RectTransform>().localScale = new Vector3(xs, ys, 1.0f);
        }

        public string layer
        {
            get
            {
                //CheckExists();
                if (!container.transform.parent.name.Contains("Layer"))
                    return "spriteObject";
                return container.transform.parent.name.Substring(0, container.transform.parent.name.Length - 5);
            }
            set
            {
                //CheckExists();
                try
                {
                    container.transform.SetParent(GameObject.Find(value + "Layer").transform);
                    foreach (Transform child in container.transform)
                    {
                        MaskImage childmask = child.gameObject.GetComponent<MaskImage>();
                        if (childmask != null)
                            childmask.inverted = false;
                    }
                }
                catch { throw new CYFException("The layer \"" + value + "\" doesn't exist."); }
            }
        }

        public void MoveBelow(FakeLuaStaticTextManager otherText)
        {
            //CheckExists();
            if (otherText == null || !otherText.isactive) throw new CYFException("The static text object passed as an argument is nil or inactive.");
            if (transform.parent.parent != otherText.transform.parent.parent) UnitaleUtil.Warn("You can't change the order of two static text objects without the same parent.");
            else
            {
                try { transform.parent.SetSiblingIndex(otherText.transform.parent.GetSiblingIndex()); }
                catch { throw new CYFException("Error while calling staticText.MoveBelow."); }
            }
        }
        public void MoveBelow(LuaTextManager otherText)
        {
            //CheckExists();
            if (otherText == null || !otherText.isactive) throw new CYFException("The static text object passed as an argument is nil or inactive.");
            if (transform.parent.parent != otherText.transform.parent.parent) UnitaleUtil.Warn("You can't change the order of two static text objects without the same parent.");
            else
            {
                try { transform.parent.SetSiblingIndex(otherText.transform.parent.GetSiblingIndex()); }
                catch { throw new CYFException("Error while calling staticText.MoveBelow."); }
            }
        }

        public void MoveAbove(FakeLuaStaticTextManager otherText)
        {
            //CheckExists();
            if (otherText == null || !otherText.isactive) throw new CYFException("The static text object passed as an argument is nil or inactive.");
            if (transform.parent.parent != otherText.transform.parent.parent) UnitaleUtil.Warn("You can't change the order of two static text objects without the same parent.");
            else
            {
                try { transform.parent.SetSiblingIndex(otherText.transform.parent.GetSiblingIndex() + 1); }
                catch { throw new CYFException("Error while calling staticText.MoveAbove."); }
            }
        }
        public void MoveAbove(LuaTextManager otherText)
        {
            //CheckExists();
            if (otherText == null || !otherText.isactive) throw new CYFException("The static text object passed as an argument is nil or inactive.");
            if (transform.parent.parent != otherText.transform.parent.parent) UnitaleUtil.Warn("You can't change the order of two static text objects without the same parent.");
            else
            {
                try { transform.parent.SetSiblingIndex(otherText.transform.parent.GetSiblingIndex() + 1); }
                catch { throw new CYFException("Error while calling staticText.MoveAbove."); }
            }
        }

        [MoonSharpHidden] public Color _color = Color.white;
        [MoonSharpHidden] public bool hasColorBeenSet;
        [MoonSharpHidden] public bool hasAlphaBeenSet;

        public float[] color
        {
            get { return new[] { _color.r, _color.g, _color.b }; }
            set
            {
                //CheckExists();
                if (value == null)
                    throw new CYFException("staticText.color can not be set to a nil value.");
                switch (value.Length)
                {
                    // If we don't have three or four floats, we throw an error
                    case 3: _color = new Color(value[0], value[1], value[2], alpha); break;
                    case 4: _color = new Color(value[0], value[1], value[2], value[3]); break;
                    default:
                        throw new CYFException("You need 3 or 4 numeric values when setting a static text's color.");
                }

                hasColorBeenSet = true;
                hasAlphaBeenSet = value.Length == 4;

                foreach (Image i in letterReferences)
                    if (i != null)
                        i.color = _color;

                if (currentColor == defaultColor)
                    currentColor = _color;
                defaultColor = _color;
            }
        }

        // The color of the text on a 32 bits format. It uses an array of three or four floats between 0 and 255
        public float[] color32
        {
            // We need first to convert the Color into a Color32, and then get the values.
            get { return new float[] { ((Color32)_color).r, ((Color32)_color).g, ((Color32)_color).b }; }
            set
            {
                //CheckExists();
                if (value == null)
                    throw new CYFException("staticText.color32 can not be set to a nil value.");
                switch (value.Length)
                {
                    // If we don't have three or four floats, we throw an error
                    case 3: color = new[] { value[0] / 255, value[1] / 255, value[2] / 255, alpha }; break;
                    case 4: color = new[] { value[0] / 255, value[1] / 255, value[2] / 255, value[3] / 255 }; break;
                    default:
                        throw new CYFException("You need 3 or 4 numeric values when setting a static text's color.");
                }
            }
        }

        // The alpha of the text. It is clamped between 0 and 1
        public float alpha
        {
            get { return _color.a; }
            set
            {
                //CheckExists();
                color = new[] { _color.r, _color.g, _color.b, Mathf.Clamp01(value) };
                hasAlphaBeenSet = true;
            }
        }

        // The alpha of the text in a 32 bits format. It is clamped between 0 and 255
        public float alpha32
        {
            get { return ((Color32)_color).a; }
            set
            {
                //CheckExists();
                alpha = value / 255;
            }
        }

        public DynValue GetLetters()
        {
            Table table = new Table(null);
            int key = 0;
            foreach (Image i in letterReferences)
                if (i != null)
                {
                    key++;
                    LuaSpriteController letter = new LuaSpriteController(i) { tag = "letter" };
                    table.Set(key, UserData.Create(letter, LuaSpriteController.data));
                }
            return DynValue.NewTable(table);
        }

        public void SetParent(LuaSpriteController parent)
        {
            //CheckExists();
            if (parent != null && parent.img.transform != null && parent.img.transform.parent.name == "SpritePivot")
                throw new CYFException("staticText.SetParent(): Can not use SetParent with an Overworld Event's sprite.");
            try
            {
                if (parent == null) throw new CYFException("staticText.SetParent(): Can't set a sprite's parent as nil.");
                container.transform.SetParent(parent.img.transform);
                foreach (Transform child in container.transform)
                {
                    MaskImage childmask = child.gameObject.GetComponent<MaskImage>();
                    if (childmask != null)
                        childmask.inverted = parent._masked == LuaSpriteController.MaskMode.INVERTEDSPRITE || parent._masked == LuaSpriteController.MaskMode.INVERTEDSTENCIL;
                }
            }
            catch { throw new CYFException("You tried to set a removed sprite/nil sprite as this static text object's parent."); }
        }

        public virtual void SetText(string text)
        {
            if (text == null)
                throw new CYFException("StaticText.SetText: the text argument must be a simple string.");
            try { SetText(new InstantTextMessage(text)); }
            catch { /* ignored */ }
        }

        public void SetFont(string fontName, bool firstTime = false)
        {
            if (fontName == null)
                throw new CYFException("StaticText.SetFont: The first argument (the font name) is nil.\n\nSee the documentation for proper usage.");
            //CheckExists();
            UnderFont uf = SimInstance.FakeSpriteFontRegistry.Get(fontName);
            if (uf == null)
                throw new CYFException("The font \"" + fontName + "\" doesn't exist.\nYou should check if you made a typo, or if the font really is in your mod.");
            SetFont(uf, firstTime);
            font = fontName;
            if (!firstTime)
                default_charset = uf;
            SetText(instantText);
        }

        public void Move(int x, int y)
        {
            //CheckExists();
            container.transform.localPosition = new Vector3(x + this.x, y + this.y, container.transform.localPosition.z);
        }

        public void MoveTo(int newX, int newY)
        {
            //CheckExists();
            container.transform.localPosition = new Vector3(newX, newY, container.transform.localPosition.z);
        }

        public void MoveToAbs(int newX, int newY)
        {
            //CheckExists();
            container.transform.position = new Vector3(newX, newY, container.transform.position.z);
        }

        public void SetAnchor(float newX, float newY)
        {
            //CheckExists();
            container.GetComponent<RectTransform>().anchorMin = new Vector2(newX, newY);
            container.GetComponent<RectTransform>().anchorMax = new Vector2(newX, newY);
        }

        public int GetTextWidth()
        {
            //CheckExists();
            return (int)AsteriskUtil.CalcTextWidth(this);
        }

        public int GetTextHeight()
        {
            //CheckExists();
            return (int)AsteriskUtil.CalcTextHeight(this);
        }

        public int width { get { return GetTextWidth(); } }

        public int height { get { return GetTextHeight(); } }

        public string text { get { return instantText.Text; } }

        internal string font { get; set; }
    }
}
