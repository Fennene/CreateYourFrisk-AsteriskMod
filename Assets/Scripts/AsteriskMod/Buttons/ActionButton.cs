using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>Manages the button in Battle Scene</summary>
    public class ActionButton
    {
        private GameObject _gameObject;
        /// <summary>Action Button's Core.</summary>
        private Image _button;
        /// <summary>Sprite for image.overrideSprite<br/>If the button is active, it's changed to this sprite.</summary>
        private Sprite _sprite;

        private ButtonManager _buttonManager;
        private int _buttonID;

        [MoonSharpHidden]
        public ActionButton(ButtonManager buttonManager, int buttonID)
        {
            _buttonManager = buttonManager;
            _buttonID = buttonID;
        }

        [MoonSharpHidden]
        /// <summary>Get Sprite for image.overrideSprite from sprite's path</summary>
        internal void Awake(string overrideSpritePath)
        {
            _sprite = SpriteRegistry.Get(overrideSpritePath);
            Initialize();
            _overrideSpritePath = overrideSpritePath;
        }

        [MoonSharpHidden]
        /// <summary>Get GameObject and Image Component from GameObjcet's name<br/>If CrateYourFrisk, changes sprite</summary>
        internal void Start(string gameObjetName, string crateSpritePath)
        {
            _gameObject = GameObject.Find(gameObjetName);
            _button = _gameObject.GetComponent<Image>();
            if (GlobalControls.crate)
            {
                _button.sprite = SpriteRegistry.Get(crateSpritePath);
                _button.GetComponent<AutoloadResourcesFromRegistry>().SpritePath = crateSpritePath;
            }
            _normalSpritePath = _button.GetComponent<AutoloadResourcesFromRegistry>().SpritePath;
        }

        [MoonSharpHidden]
        internal void ShowOverrideSprite()
        {
            _button.overrideSprite = _sprite;
        }

        [MoonSharpHidden]
        internal void HideOverrideSprite()
        {
            _button.overrideSprite = null;
        }

        // --------------------------------------------------------------------------------
        //                            Asterisk Mod Features
        // --------------------------------------------------------------------------------
        private string _normalSpritePath, _overrideSpritePath;

        [MoonSharpHidden]
        /// <summary>Initialize parameters for customize button</summary>
        internal void Initialize()
        {
            isactive = true;
            RelativePosition = Vector2.zero;
            xScale = 1;
            yScale = 1;
        }

        public bool isactive { get; private set; }

        public void SetActive(bool active)
        {
            if (!active) _buttonManager.CheckInactivate(_buttonID);
            isactive = active;
            _button.enabled = active;
        }

        public void SetSprite(string normalSpritePath, string overrideSpritePath, string prefix = "UI/Buttons", bool autoResize = false)
        {
            if (GlobalControls.crate) return;
            if (!string.IsNullOrEmpty(prefix)) prefix += "/";
            normalSpritePath   = prefix + normalSpritePath;
            overrideSpritePath = prefix + overrideSpritePath;
            if (!autoResize)
            {
                _button.sprite = SpriteRegistry.Get(normalSpritePath);
            }
            else
            {
                SpriteUtil.SwapSpriteFromFile(_button, normalSpritePath);
            }
            _sprite = SpriteRegistry.Get(overrideSpritePath);
            if (_button.overrideSprite != null)
            {
                _button.overrideSprite = _sprite;
            }
        }

        internal Vector2 RelativePosition { get; private set; }

        public int x
        {
            get { return Mathf.FloorToInt(RelativePosition.x); }
            set { MoveTo(value, y); }
        }

        public int y
        {
            get { return Mathf.FloorToInt(RelativePosition.y); }
            set { MoveTo(x, value); }
        }

        public int absx
        {
            get { return Mathf.FloorToInt(_gameObject.transform.position.x); }
            set { MoveToAbs(value, absy); }
        }

        public int absy
        {
            get { return Mathf.FloorToInt(_gameObject.transform.position.y); }
            set { MoveToAbs(absx, value); }
        }

        public void Move(int x, int y)
        {
            MoveTo(x + this.x, y + this.y);
        }

        public void MoveTo(int newX, int newY)
        {
            Vector2 initPos = _button.GetComponent<RectTransform>().anchoredPosition - RelativePosition;
            RelativePosition = new Vector2(newX, newY);
            _button.GetComponent<RectTransform>().anchoredPosition = initPos + RelativePosition;
        }

        public void MoveToAbs(int newX, int newY)
        {
            Vector2 initPos = _button.GetComponent<RectTransform>().anchoredPosition - RelativePosition;
            _gameObject.transform.position = new Vector3(newX, newY, _gameObject.transform.position.z);
            RelativePosition = _button.GetComponent<RectTransform>().anchoredPosition - initPos;
        }

        public float[] color
        {
            get
            {
                return new[] { _button.color.r, _button.color.g, _button.color.b };
            }
            set
            {
                if (value == null) throw new CYFException("button.color can not be set to a nil value.");
                switch (value.Length)
                {
                    case 3: _button.color = new Color(value[0], value[1], value[2], alpha);    break;
                    case 4: _button.color = new Color(value[0], value[1], value[2], value[3]); break;
                    default: throw new CYFException("You need 3 or 4 numeric values when setting a button's color.");
                }
            }
        }

        public byte[] color32
        {
            get
            {
                return new[] { ((Color32)_button.color).r, ((Color32)_button.color).g, ((Color32)_button.color).b };
            }
            set
            {
                if (value == null) throw new CYFException("button.color can not be set to a nil value.");
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] < 0) value[i] = 0;
                    else if (value[i] > 255) value[i] = 255;
                }
                switch (value.Length)
                {
                    case 3: _button.color = new Color32(value[0], value[1], value[2], alpha32);  break;
                    case 4: _button.color = new Color32(value[0], value[1], value[2], value[3]); break;
                    default: throw new CYFException("You need 3 or 4 numeric values when setting a button's color.");
                }
            }
        }

        public float alpha
        {
            get { return _button.color.a; }
            set { _button.color = new Color(_button.color.r, _button.color.g, _button.color.b, Mathf.Clamp01(value)); }
        }

        public byte alpha32
        {
            get { return ((Color32)_button.color).a; }
            set { _button.color = new Color32(((Color32)_button.color).r, ((Color32)_button.color).g, ((Color32)_button.color).b, (byte)value); }
        }

        private float xScale;
        private float yScale;

        public float xscale
        {
            get
            {
                return xScale;
            }
            set
            {
                xScale = value;
                Scale(xScale, yScale);
            }
        }

        public float yscale
        {
            get
            {
                return yScale;
            }
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
            Vector2 nativeSizeDelta = new Vector2(_button.sprite.texture.width, _button.sprite.texture.height);
            _gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(nativeSizeDelta.x * Mathf.Abs(xScale), nativeSizeDelta.y * Mathf.Abs(yScale));
        }

        public void Revert(bool revertPosition = true)
        {
            // Revert Sprite
            SpriteUtil.SwapSpriteFromFile(_button, _normalSpritePath);
            _sprite = SpriteRegistry.Get(_overrideSpritePath);
            if (_button.overrideSprite != null) _button.overrideSprite = _sprite;
            // Revert Color & Alpha
            _button.color = new Color(1, 1, 1, 1);
            // Revert Scale
            Scale(1, 1);
            // Revert Position
            if (revertPosition)
            {
                _button.GetComponent<RectTransform>().anchoredPosition -= RelativePosition;
                RelativePosition = Vector2.zero;
            }
        }
    }
}
