using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>PlayerのHP用とLua用の<see cref="LifeBarController"/></summary>
    public class PlayerLifeBar : MonoBehaviour
    {
        public Color fillColor;
        public Color backgroundColor;
        public Image fill;
        public Image background;

        private float currentFill = 1.0f;
        private float oldFill = 1.0f;
        private float desiredFill = 1.0f;
        private const float fillLinearTime = 1.0f; // 現在のバーの位置(長さ)が目的の位置(長さ)に移動するまでの時間
        private float fillTimer;
        private float totalwidth;
        //* private const bool player = false;
        //* private const bool whenDamage = false;
        //* private float whenDamageValue = 0.0f;

        // --------------------------------------------------------------------------------
        //                            Asterisk Mod Addition
        // --------------------------------------------------------------------------------
        private bool _limited = true;
        // --------------------------------------------------------------------------------

        private void Start()
        {
            totalwidth = fill.rectTransform.rect.width;
            background.color = backgroundColor;
            fill.color = fillColor;
            // プレハブ(Prefab)用 順番を正しく入れ替える。
            background.transform.SetAsLastSibling();
            fill.transform.SetAsLastSibling();
        }

        /// <summary>Fillバー(黄色と赤色なら黄色の方) の長さを代入した値に応じて 即座に 変える。</summary>
        /// <param name="fillvalue">0.0 ~ 1.0 の範囲。</param>
        [MoonSharpHidden]
        private void setInstant(float fillvalue)
        {
            currentFill = fillvalue;
            desiredFill = fillvalue;
            float realMaxLength = PlayerCharacter.instance.MaxHP;
            if (_limited) realMaxLength = Mathf.Min(realMaxLength, 100);
            //fill.fillAmount = fillvalue;
            //fill.rectTransform.sizeDelta = new Vector2(1, fillvalue);
            //* if (player) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * 90, fill.rectTransform.offsetMin.y);
            //* else if (whenDamage) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * whenDamageValue, fill.rectTransform.offsetMin.y);
            //* else
            //* {
                if (fillvalue > 1)
                    fillvalue = 1;
                //fill.rectTransform.offsetMax = new Vector2(-(Mathf.Min(PlayerCharacter.instance.MaxHP, 100) * (1 - fillvalue)) * 1.2f, fill.rectTransform.offsetMin.y);
                fill.rectTransform.offsetMax = new Vector2(-(realMaxLength * (1 - fillvalue)) * 1.2f, fill.rectTransform.offsetMin.y);
            //* }
            //fill.rectTransform.offsetMax = new Vector2(-(1 - fillvalue) * PlayerCharacter.instance.MaxHP * 1.2f, fill.rectTransform.offsetMin.y);
        }

        /// <summary>現在のFillバーの位置(長さ)から 代入した位置(長さ)への linear-time(線形時間遷移(等速直線運動かと思われ)) を開始する。</summary>
        /// <param name="fillvalue">0.0 ~ 1.0 の範囲。</param>
        [MoonSharpHidden]
        private void setLerp(float fillvalue)
        {
            if (fillvalue > 1)
                fillvalue = 1;
            oldFill = currentFill;
            desiredFill = fillvalue;
            fillTimer = 0.0f;
        }

        /// <summary>第一引数の位置(長さ)から 第二引数の位置(長さ)への linear-time(線形時間遷移(等速直線運動かと思われ)) を開始する。</summary>
        /// <param name="originalValue">開始値, 0.0 ~ 1.0 の範囲。</param>
        /// <param name="fillValue">終了値, 0.0 ~ 1.0 の範囲。</param>
        [MoonSharpHidden]
        private void setLerp(float originalValue, float fillValue)
        {
            setInstant(originalValue);
            setLerp(fillValue);
        }

        /// <summary>Fillバー(デフォルトでは黄色)の色を変える。</summary>
        [MoonSharpHidden]
        private void setFillColor(Color c)
        {
            fillColor = c;
            fill.color = c;
        }

        /// <summary>Backgroundバー(デフォルトでは赤色)の色を変える。</summary>
        [MoonSharpHidden]
        private void setBackgroundColor(Color c)
        {
            backgroundColor = c;
            background.color = c;
        }

        /// <summary>表示/非表示の切り替え</summary>
        /// <param name="visible">true で表示, falseで非表示</param>
        [MoonSharpHidden]
        private void setVisible(bool visible)
        {
            foreach (Image img in GetComponentsInChildren<Image>())
                img.enabled = visible;
        }

        /// <summary>SetLerp用。</summary>
        private void Update()
        {
            if (currentFill == desiredFill || UIController.instance.frozenState != UIController.UIState.PAUSE) return;

            currentFill = Mathf.Lerp(oldFill, desiredFill, fillTimer / fillLinearTime);
            //fill.fillAmount = currentFill;
            //fill.rectTransform.sizeDelta = new Vector2(0, -(1-currentFill) * totalwidth);
            //* if (player) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * PlayerCharacter.instance.HP * 1.2f, fill.rectTransform.offsetMin.y);
            //* else if (whenDamage) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * whenDamageValue, fill.rectTransform.offsetMin.y);
            /** else*/ fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * totalwidth, fill.rectTransform.offsetMin.y);
            fillTimer += Time.deltaTime;
        }


        // --------------------------------------------------------------------------------
        //                            Asterisk Mod Addition
        // --------------------------------------------------------------------------------
        private RectTransform self;

        private void Awake()
        {
            self = GetComponent<RectTransform>();
        }

        [MoonSharpHidden]
        internal void setHP(float hpCurrent)
        {
            if (_controlOverride) return;
            float hpMax = PlayerCharacter.instance.MaxHP,
                  hpFrac = hpCurrent / hpMax;
            setInstant(hpFrac);
            _hp = PlayerCharacter.instance.HP;
            _maxhp = PlayerCharacter.instance.MaxHP;
        }

        [MoonSharpHidden]
        internal void setMaxHP(bool maxhpOnly = false)
        {
            if (_controlOverride) return;
            //* self.sizeDelta = new Vector2(Mathf.Min(120, PlayerCharacter.instance.MaxHP * 1.2f), self.sizeDelta.y);
            //* self.sizeDelta = new Vector2(Mathf.Min(100, PlayerCharacter.instance.MaxHP) * 1.2f, self.sizeDelta.y);
            float realMaxLength = PlayerCharacter.instance.MaxHP;
            if (_limited) realMaxLength = Mathf.Min(100, realMaxLength);
            self.sizeDelta = new Vector2(realMaxLength * 1.2f, self.sizeDelta.y);
            if (maxhpOnly) return;
            setHP(PlayerCharacter.instance.HP);
        }

        /// <summary>プレイヤーのHP表示用のLifeBarかどうか</summary>
        [MoonSharpHidden] internal bool isOriginal;
        private bool _controlOverride;
        private float _hp;
        private float _maxhp;

        [MoonSharpHidden]
        internal void Initialize(bool isOriginalUI)
        {
            isOriginal = isOriginalUI;
            _controlOverride = !isOriginal;
            if (isOriginal)
            {
                _hp = PlayerCharacter.instance.HP;
                _maxhp = PlayerCharacter.instance.MaxHP;
                return;
            }
            _hp = 20;
            _maxhp = 20;
            SetHP(_hp, _maxhp);
        }

        public bool controlOverride
        {
            get { return _controlOverride; }
            set { SetControlOverride(value); }
        }

        public void SetControlOverride(bool active)
        {
            if (!isOriginal) return;
            _controlOverride = active;
            if (!_controlOverride) setMaxHP();
        }

        public bool limited
        {
            get { return _limited; }
            set
            {
                _limited = value;
                if (_controlOverride)
                {
                    SetHP(_hp, _maxhp);
                }
                else
                {
                    setMaxHP();
                }
            }
        }

        public float hp
        {
            get { return _hp; }
            set { SetHP(value, _maxhp); }
        }

        public float maxhp
        {
            get { return _maxhp; }
            set { SetHP(_hp, value); }
        }

        public void SetHP(float newHP, float newMaxHP)
        {
            float realMaxHP = _maxhp;
            if (_limited) realMaxHP = Mathf.Min(_maxhp, 100);

            // Set Max HP
            if (_maxhp != newMaxHP)
            {
                _maxhp = newMaxHP;
                //self.sizeDelta = new Vector2(Mathf.Min(120, PlayerCharacter.instance.MaxHP * 1.2f), self.sizeDelta.y);
                self.sizeDelta = new Vector2(realMaxHP * 1.2f, self.sizeDelta.y);
            }

            // Set Current HP
            _hp = newHP;
            float hpMax = _maxhp,
                  hpFrac = newHP / hpMax;
            currentFill = hpFrac;
            desiredFill = hpFrac;
            if (hpFrac > 1)
                hpFrac = 1;
            fill.rectTransform.offsetMax = new Vector2(-(realMaxHP * (1 - hpFrac)) * 1.2f, fill.rectTransform.offsetMin.y);
        }

        public float[] fillcolor
        {
            get { return new[] { fillColor.r, fillColor.g, fillColor.b }; }
            set
            {
                if (value == null)
                    throw new CYFException("lifeBar.fillcolor can not be set to a nil value.");
                Color c;
                switch (value.Length)
                {
                    case 3: c = new Color(value[0], value[1], value[2], fillalpha); break;
                    case 4: c = new Color(value[0], value[1], value[2], value[3]); break;
                    default:
                        throw new CYFException("You need 3 or 4 numeric values when setting color of a lifebar's fillbar.");
                }
                setFillColor(c);
            }
        }
        public float[] fillcolor32
        {
            get { return new float[] { ((Color32)fillColor).r, ((Color32)fillColor).g, ((Color32)fillColor).b }; }
            set
            {
                if (value == null)
                    throw new CYFException("lifeBar.fillcolor32 can not be set to a nil value.");
                switch (value.Length)
                {
                    case 3: fillcolor = new[] { value[0] / 255, value[1] / 255, value[2] / 255, fillalpha }; break;
                    case 4: fillcolor = new[] { value[0] / 255, value[1] / 255, value[2] / 255, value[3] / 255 }; break;
                    default:
                        throw new CYFException("You need 3 or 4 numeric values when setting color of a lifebar's fillbar.");
                }
            }
        }
        public float fillalpha
        {
            get { return fillColor.a; }
            set { fillcolor = new[] { fillColor.r, fillColor.g, fillColor.b, Mathf.Clamp01(value) }; }
        }
        public float fillalpha32
        {
            get { return ((Color32)fillColor).a; }
            set { fillalpha = value / 255; }
        }

        public float[] bgcolor
        {
            get { return new[] { backgroundColor.r, backgroundColor.g, backgroundColor.b }; }
            set
            {
                if (value == null)
                    throw new CYFException("lifeBar.bgcolor can not be set to a nil value.");
                Color c;
                switch (value.Length)
                {
                    case 3: c = new Color(value[0], value[1], value[2], bgalpha); break;
                    case 4: c = new Color(value[0], value[1], value[2], value[3]); break;
                    default:
                        throw new CYFException("You need 3 or 4 numeric values when setting color of a lifebar's backgroundbar.");
                }
                setBackgroundColor(c);
            }
        }
        public float[] bgcolor32
        {
            get { return new float[] { ((Color32)backgroundColor).r, ((Color32)backgroundColor).g, ((Color32)backgroundColor).b }; }
            set
            {
                if (value == null)
                    throw new CYFException("lifeBar.bgcolor32 can not be set to a nil value.");
                switch (value.Length)
                {
                    case 3: bgcolor = new[] { value[0] / 255, value[1] / 255, value[2] / 255, bgalpha }; break;
                    case 4: bgcolor = new[] { value[0] / 255, value[1] / 255, value[2] / 255, value[3] / 255 }; break;
                    default:
                        throw new CYFException("You need 3 or 4 numeric values when setting color of a lifebar's backgroundbar.");
                }
            }
        }
        public float bgalpha
        {
            get { return backgroundColor.a; }
            set { bgcolor = new[] { backgroundColor.r, backgroundColor.g, backgroundColor.b, Mathf.Clamp01(value) }; }
        }
        public float bgalpha32
        {
            get { return ((Color32)backgroundColor).a; }
            set { bgalpha = value / 255; }
        }
        // --------------------------------------------------------------------------------
    }
}
