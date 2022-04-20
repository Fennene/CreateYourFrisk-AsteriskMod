using MoonSharp.Interpreter;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class LimitedLuaStaticTextManager : StaticTextManager
    {
        private GameObject container;

        private float xScale = 1;
        private float yScale = 1;

        public bool isactive { get { return true; } }

        protected override void Awake()
        {
            base.Awake();
            container = gameObject;
        }

        public int x
        {
            get { return Mathf.RoundToInt(container.transform.localPosition.x); }
            set { MoveTo(value, y); }
        }

        public int y
        {
            get { return Mathf.RoundToInt(container.transform.localPosition.y); }
            set { MoveTo(x, value); }
        }

        public int absx
        {
            get { return Mathf.RoundToInt(container.transform.position.x); }
            set { MoveToAbs(value, absy); }
        }

        public int absy
        {
            get { return Mathf.RoundToInt(container.transform.position.y); }
            set { MoveTo(absx, value); }
        }

        public int textMaxWidth { get { return _textMaxWidth; } }

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
            xScale = xs;
            yScale = ys;

            container.gameObject.GetComponent<RectTransform>().localScale = new Vector3(xs, ys, 1.0f);
        }

        [MoonSharpHidden] public Color _color = Color.white;
        [MoonSharpHidden] public bool hasColorBeenSet;
        [MoonSharpHidden] public bool hasAlphaBeenSet;

        public float[] color
        {
            get { return new[] { _color.r, _color.g, _color.b }; }
            set
            {
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
                        if (i.color == defaultColor) i.color = _color;
                        else break; // Only because we can't go back to the default color

                if (currentColor == defaultColor)
                    currentColor = _color;
                defaultColor = _color;
            }
        }

        public float[] color32
        {
            get { return new float[] { ((Color32)_color).r, ((Color32)_color).g, ((Color32)_color).b }; }
            set
            {
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

        public float alpha
        {
            get { return _color.a; }
            set
            {
                color = new[] { _color.r, _color.g, _color.b, Mathf.Clamp01(value) };
                hasAlphaBeenSet = true;
            }
        }

        public float alpha32
        {
            get { return ((Color32)_color).a; }
            set { alpha = value / 255; }
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

        public void Move(int x, int y)
        {
            container.transform.localPosition = new Vector3(x + this.x, y + this.y, container.transform.localPosition.z);
        }

        public void MoveTo(int newX, int newY)
        {
            container.transform.localPosition = new Vector3(newX, newY, container.transform.localPosition.z);
        }

        public void MoveToAbs(int newX, int newY)
        {
            container.transform.position = new Vector3(newX, newY, container.transform.position.z);
        }

        public void SetAnchor(float newX, float newY)
        {
            container.GetComponent<RectTransform>().anchorMin = new Vector2(newX, newY);
            container.GetComponent<RectTransform>().anchorMax = new Vector2(newX, newY);
        }

        public int GetTextWidth() { return (int)AsteriskUtil.CalcTextWidth(this); }

        public int GetTextHeight() { return (int)AsteriskUtil.CalcTextHeight(this); }

        [MoonSharpHidden] internal Action<string> _SetText;

        public void SetText(string text)
        {
            if (_SetText == null) throw new CYFException("(Limited)StaticText.SetText: This static text does not be allowed change text.");
            _SetText(text);
        }

        [MoonSharpHidden] internal bool _BanControlOverride;
        [MoonSharpHidden] internal bool _controlOverride;

        public void SetControlOverride(bool active)
        {
            if (active && _BanControlOverride) throw new CYFException("LimitedStaticText.SetControlOverride: This static text does not be allowed override control.");
            _controlOverride = active;
        }
    }
}
