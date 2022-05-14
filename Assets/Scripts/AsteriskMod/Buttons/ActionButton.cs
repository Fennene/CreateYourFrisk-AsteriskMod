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
        private Vector2 _relativePosition;
        private Vector2 _relativePlayerPosition;
        private bool _playerPositionOverride;

        /// <summary>Initialize parameters for customize button</summary>
        private void Initialize()
        {
            isactive = true;
            _relativePosition = Vector2.zero;
            _relativePlayerPosition = Vector2.zero;
            _playerPositionOverride = false;
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

        internal Vector2 RelativePosition
        {
            get { return _relativePosition; }
            set
            {
                _relativePosition = value;
                UIController.instance.UpdatePlayerPositionOnAction();
            }
        }

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

        private float GetAbsX() // I don't like this method, but I could not found better others.
        {
            if (_buttonID <= 1) // FIGHT & ACT
            {
                return _gameObject.transform.position.x + _button.GetComponent<RectTransform>().sizeDelta.x / 2;
            }
            // ITEM & MERCY
            return _gameObject.transform.position.x - _button.GetComponent<RectTransform>().sizeDelta.x / 2;
        }

        private float GetAbsY()
        {
            return _gameObject.transform.position.y + _button.GetComponent<RectTransform>().sizeDelta.y / 2;
        }

        public int absx
        {
            get{ return Mathf.FloorToInt(GetAbsX()); }
            set { MoveToAbs(value, absy); }
        }

        public int absy
        {
            get { return Mathf.FloorToInt(GetAbsY()); }
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

        private float ConvertToAbsX(float x) // I don't like this method, but I could not found better others.
        {
            if (_buttonID <= 1) // FIGHT & ACT
            {
                return x - _button.GetComponent<RectTransform>().sizeDelta.x / 2;
            }
            // ITEM & MERCY
            return x + _button.GetComponent<RectTransform>().sizeDelta.x / 2;
        }

        private float ConvertToAbsY(float y) // I don't like this method, but I could not found better others.
        {
            return y - _button.GetComponent<RectTransform>().sizeDelta.y / 2;
        }

        public void MoveToAbs(int newX, int newY)
        {
            Vector2 initPos = _button.GetComponent<RectTransform>().anchoredPosition - RelativePosition;
            _gameObject.transform.position = new Vector3(ConvertToAbsX(newX), ConvertToAbsY(newY), _gameObject.transform.position.z);
            RelativePosition = _button.GetComponent<RectTransform>().anchoredPosition - initPos;
        }

        internal Vector2 RelativePlayerPosition
        {
            get { return _relativePlayerPosition; }
            set
            {
                _relativePlayerPosition = value;
                UIController.instance.UpdatePlayerPositionOnAction();
            }
        }

        public int playerx
        {
            get { return Mathf.FloorToInt(RelativePlayerPosition.x); }
            set { RelativePlayerPosition = new Vector2(value, RelativePlayerPosition.y); }
        }

        public int playery
        {
            get { return Mathf.FloorToInt(RelativePlayerPosition.y); }
            set { RelativePlayerPosition = new Vector2(RelativePlayerPosition.x, value); }
        }

        public bool playerabs
        {
            get { return _playerPositionOverride; }
            set
            {
                _playerPositionOverride = value;
                UIController.instance.UpdatePlayerPositionOnAction();
            }
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
                RelativePlayerPosition = Vector2.zero;
                playerabs = false;
            }
        }

        [MoonSharpHidden]
        internal void SetSize(int width, int height)
        {
            _button.rectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}
