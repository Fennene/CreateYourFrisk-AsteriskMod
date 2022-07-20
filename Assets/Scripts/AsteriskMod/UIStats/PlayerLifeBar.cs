using System.Collections.Generic;
using MoonSharp.Interpreter;
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
            // --------------------------------------------------------------------------------
            //                            Asterisk Mod Addition
            // --------------------------------------------------------------------------------
            if (isOriginal) return;
            // --------------------------------------------------------------------------------
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
            //*if (fillvalue > 1)
            //*    fillvalue = 1;
            if (_limited && fillvalue > 1) fillvalue = 1;
            //fill.rectTransform.offsetMax = new Vector2(-(Mathf.Min(PlayerCharacter.instance.MaxHP, 100) * (1 - fillvalue)) * 1.2f, fill.rectTransform.offsetMin.y);
            fill.rectTransform.offsetMax = new Vector2(-(realMaxLength * (1 - fillvalue)) * _hpscale, fill.rectTransform.offsetMin.y);
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
            /** else*/
            fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * totalwidth, fill.rectTransform.offsetMin.y);
            fillTimer += Time.deltaTime;
        }


        // --------------------------------------------------------------------------------
        //                            Asterisk Mod Addition
        // --------------------------------------------------------------------------------
        [MoonSharpHidden] internal RectTransform self;

        //public LuaSpriteShader bgshader;
        //public LuaSpriteShader fillshader;
        [ShouldAddToDocument]
        private LuaSpriteShader _bgshader;
        private LuaSpriteShader _fillshader;
        public LuaSpriteShader bgshader
        {
            get
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("PlayerLifeBar", "bgshader");
                return _bgshader;
            }
            set
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("PlayerLifeBar", "bgshader");
                _bgshader = value;
            }
        }
        public LuaSpriteShader fillshader
        {
            get
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("PlayerLifeBar", "fillshader");
                return _fillshader;
            }
            set
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("PlayerLifeBar", "fillshader");
                _fillshader = value;
            }
        }

        //private Dictionary<string, Image> subbars;

        private void Awake()
        {
            self = GetComponent<RectTransform>();
            _bgshader = new LuaSpriteShader("sprite", background.gameObject);
            _fillshader = new LuaSpriteShader("sprite", fill.gameObject);
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
        internal void setMaxHP(bool maxhpOnly = false, bool resetTextPosition = false)
        {
            if (_controlOverride) return;
            //* self.sizeDelta = new Vector2(Mathf.Min(120, PlayerCharacter.instance.MaxHP * 1.2f), self.sizeDelta.y);
            //* self.sizeDelta = new Vector2(Mathf.Min(100, PlayerCharacter.instance.MaxHP) * 1.2f, self.sizeDelta.y);
            float realMaxLength = PlayerCharacter.instance.MaxHP;
            if (_limited) realMaxLength = Mathf.Min(100, realMaxLength);
            //* self.sizeDelta = new Vector2(realMaxLength * 1.2f, self.sizeDelta.y);
            self.sizeDelta = new Vector2(realMaxLength * _hpscale, self.sizeDelta.y);
            if (resetTextPosition) PlayerLifeUI.instance.SetTextPosition();
            if (maxhpOnly) return;
            setHP(PlayerCharacter.instance.HP);
        }

        /// <summary>プレイヤーのHP表示用のLifeBarかどうか</summary>
        [MoonSharpHidden] internal bool isOriginal;
        private bool _controlOverride;
        private float _hp;
        private float _maxhp;
        private Vector2 relativePosition;
        private float _hpscale;

        [MoonSharpHidden]
        internal void Initialize(bool isOriginalUI)
        {
            isOriginal = isOriginalUI;
            _controlOverride = !isOriginal;
            relativePosition = Vector2.zero;
            _hpscale = 1.2f;
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
            get
            {
                CheckExists(true);
                return _limited;
            }
            set
            {
                CheckExists(false);
                _limited = value;
                if (_controlOverride)
                {
                    ForceSetHP(_hp, _maxhp);
                }
                else
                {
                    setMaxHP(resetTextPosition: true);
                }
            }
        }

        public float hp
        {
            get
            {
                CheckExists(true);
                return _hp;
            }
            set
            {
                SetHP(value, _maxhp);
            }
        }

        public float maxhp
        {
            get
            {
                CheckExists(true);
                return _maxhp;
            }
            set
            {
                SetHP(_hp, value);
            }
        }

        public float hpscale
        {
            get { return _hpscale; }
            set
            {
                CheckExists(false);
                _hpscale = value;
                if (_controlOverride)
                {
                    ForceSetHP(_hp, _maxhp);
                }
                else
                {
                    setMaxHP(resetTextPosition: true);
                }
            }
        }

        private void ForceSetHP(float newHP, float newMaxHP)
        {
            CheckExists(false);
            float realMaxHP = newMaxHP;
            if (_limited) realMaxHP = Mathf.Min(newMaxHP, 100);
            // Set Max HP
            _maxhp = newMaxHP;
            self.sizeDelta = new Vector2(realMaxHP * _hpscale, self.sizeDelta.y);
            if (isOriginal) PlayerLifeUI.instance.SetTextPosition();
            // Set Current HP
            _hp = newHP;
            float hpMax = _maxhp,
                  hpFrac = newHP / hpMax;
            currentFill = hpFrac;
            desiredFill = hpFrac;
            if (_limited && hpFrac > 1) hpFrac = 1;
            fill.rectTransform.offsetMax = new Vector2(-(realMaxHP * (1 - hpFrac)) * _hpscale, fill.rectTransform.offsetMin.y);
        }

        public void SetHP(float newHP, float newMaxHP)
        {
            CheckExists(false);

            float realMaxHP = newMaxHP;
            if (_limited) realMaxHP = Mathf.Min(newMaxHP, 100);

            // Set Max HP
            if (_maxhp != newMaxHP)
            {
                _maxhp = newMaxHP;
                //self.sizeDelta = new Vector2(Mathf.Min(120, PlayerCharacter.instance.MaxHP * 1.2f), self.sizeDelta.y);
                self.sizeDelta = new Vector2(realMaxHP * _hpscale, self.sizeDelta.y);
                if (isOriginal) PlayerLifeUI.instance.SetTextPosition();
            }

            // Set Current HP
            _hp = newHP;
            float hpMax = _maxhp,
                  hpFrac = newHP / hpMax;
            currentFill = hpFrac;
            desiredFill = hpFrac;
            if (_limited && hpFrac > 1) hpFrac = 1;
            fill.rectTransform.offsetMax = new Vector2(-(realMaxHP * (1 - hpFrac)) * _hpscale, fill.rectTransform.offsetMin.y);
        }

        [MoonSharpHidden]
        internal void LegacySetHP(float newHP, float newMaxHP)
        {
            float maxHP = Mathf.Min(newMaxHP, 100);

            // Set Current HP
            _hp = newHP;
            float hpMax = maxHP,
                  hpFrac = newHP / hpMax;
            currentFill = hpFrac;
            desiredFill = hpFrac;
            if (hpFrac > 1) hpFrac = 1;
            fill.rectTransform.offsetMax = new Vector2(-(maxHP * (1 - hpFrac)) * 1.2f, fill.rectTransform.offsetMin.y);
        }

        public float[] fillcolor
        {
            get
            {
                CheckExists(true);
                return new[] { fillColor.r, fillColor.g, fillColor.b };
            }
            set
            {
                CheckExists(false);
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
            get
            {
                CheckExists(true);
                return new float[] { ((Color32)fillColor).r, ((Color32)fillColor).g, ((Color32)fillColor).b };
            }
            set
            {
                CheckExists(false);
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
            get
            {
                CheckExists(true);
                return fillColor.a;
            }
            set
            {
                fillcolor = new[] { fillColor.r, fillColor.g, fillColor.b, Mathf.Clamp01(value) };
            }
        }
        public float fillalpha32
        {
            get
            {
                CheckExists(true);
                return ((Color32)fillColor).a;
            }
            set
            {
                fillalpha = value / 255;
            }
        }

        public float[] bgcolor
        {
            get
            {
                CheckExists(true);
                return new[] { backgroundColor.r, backgroundColor.g, backgroundColor.b };
            }
            set
            {
                CheckExists(false);
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
            get
            {
                CheckExists(true);
                return new float[] { ((Color32)backgroundColor).r, ((Color32)backgroundColor).g, ((Color32)backgroundColor).b };
            }
            set
            {
                CheckExists(false);
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
            get
            {
                CheckExists(true);
                return backgroundColor.a;
            }
            set
            {
                bgcolor = new[] { backgroundColor.r, backgroundColor.g, backgroundColor.b, Mathf.Clamp01(value) };
            }
        }
        public float bgalpha32
        {
            get
            {
                CheckExists(true);
                return ((Color32)backgroundColor).a;
            }
            set
            {
                bgalpha = value / 255;
            }
        }

        private void CheckExists(bool getting = false, bool move = false)
        {
            if (isactive) return;
            if (move) throw new CYFException("Attempt to move a removed LifeBar object.");
            throw new CYFException(getting ? "Attempt to get a parameter from a removed LifeBar object." : "Attempt to set a parameter to a removed LifeBar object.");
        }

        public bool isactive { get { return self != null; } }

        public void Remove()
        {
            if (self == null) return;
            if (isOriginal) throw new CYFException("Attempt to remove UI object.");
            Object.Destroy(self.gameObject);
            self = null;
        }

        public void MoveBelow(PlayerLifeBar otherBar)
        {
            CheckExists(false, true);
            if (isOriginal) throw new CYFException("The life bar of UI can't be moved below any life bars!");
            if (otherBar == null || !otherBar.isactive) throw new CYFException("The life bar object passed as an argument is nil or inactive.");
            if (transform.parent.parent != otherBar.transform.parent.parent) UnitaleUtil.Warn("You can't change the order of two life bar objects without the same parent.");
            else
            {
                try { transform.parent.SetSiblingIndex(otherBar.transform.parent.GetSiblingIndex()); }
                catch { throw new CYFException("Error while calling lifeBar.MoveBelow."); }
            }
        }

        public void MoveAbove(PlayerLifeBar otherBar)
        {
            CheckExists(false, true);
            if (isOriginal) throw new CYFException("The life bar of UI can't be moved above any life bars!");
            if (otherBar == null || !otherBar.isactive) throw new CYFException("The life bar object passed as an argument is nil or inactive.");
            if (transform.parent.parent != otherBar.transform.parent.parent) UnitaleUtil.Warn("You can't change the order of two life bar objects without the same parent.");
            else
            {
                try { transform.parent.SetSiblingIndex(otherBar.transform.parent.GetSiblingIndex() + 1); }
                catch { throw new CYFException("Error while calling lifeBar.MoveAbove."); }
            }
        }

        /**
        private void MoveLayer(bool below)
        {
            string findName = below ? "*BelowHPBar" : "*AboveHPBar";
            GameObject parent = GameObject.Find(findName);
            if (parent == null)
            {
                Debug.LogWarning("GameObject\"" + findName + "\" is not found.");
                throw new CYFException("Error while calling lifeBar.Move" + (below ? "Below" : "Above") +"UI.");
            }
            transform.parent = parent.transform;
        }

        public void MoveBelowUI()
        {
            CheckExists(false, true);
            if (isOriginal) throw new CYFException("The life bar of UI can't be moved below UI!");
            MoveLayer(true);
        }

        public void MoveAboveUI()
        {
            CheckExists(false, true);
            if (isOriginal) throw new CYFException("The life bar of UI can't be moved above UI!");
            MoveLayer(false);
        }
        */

        public void SendToTop()
        {
            CheckExists(false, true);
            if (isOriginal) throw new CYFException("The life bar of UI can't be sended to top!");
            self.SetAsLastSibling();
        }

        public void SendToBottom()
        {
            CheckExists(false, true);
            if (isOriginal) throw new CYFException("The life bar of UI can't be sended to bottom!");
            self.SetAsFirstSibling();
        }

        public float x
        {
            get
            {
                CheckExists(true);
                return relativePosition.x;
            }
            set
            {
                MoveTo(value, y);
            }
        }

        public float y
        {
            get
            {
                CheckExists(true);
                return relativePosition.y;
            }
            set
            {
                MoveTo(x, value);
            }
        }

        public int height
        {
            get
            {
                CheckExists(true);
                return Mathf.RoundToInt(self.sizeDelta.y);
            }
            set
            {
                self.sizeDelta = new Vector2(self.sizeDelta.x, value);
            }
        }

        public void Move(float x, float y)
        {
            CheckExists(false, true);
            relativePosition += new Vector2(x, y);
            self.anchoredPosition += new Vector2(x, y);
        }

        public void MoveTo(float newX, float newY)
        {
            CheckExists(false, true);
            Vector2 initPos = self.anchoredPosition - relativePosition;
            relativePosition = new Vector2(newX, newY);
            self.anchoredPosition = initPos + relativePosition;
        }

        [MoonSharpHidden, ToDo("Add")]
        public void MoveToAbs(float newX, float newY)
        {
            CheckExists(false, true);
            Vector2 initPos = self.anchoredPosition - relativePosition;
            self.gameObject.transform.position = new Vector3(newX, newY, self.gameObject.transform.position.z);
            relativePosition = self.anchoredPosition - initPos;
        }

        public string layer
        {
            get
            {
                if (isOriginal) return "*HPRect";
                if (transform.parent.name.StartsWith("*")) return transform.parent.name.Substring(1);
                return transform.parent.name.Substring(0, transform.parent.name.Length - 5);
            }
            set
            {
                if (isOriginal) throw new CYFException("The life bar of UI can't be moved above any life bars!");
                Transform parent = transform.parent;
                try
                {
                    if (value == "BelowHPBar" || value == "AboveHPBar") transform.SetParent(GameObject.Find("*" + value)    .transform);
                    else                                                transform.SetParent(GameObject.Find(value + "Layer").transform);
                }
                catch { transform.SetParent(parent); }
            }
        }

        [MoonSharpHidden, ToDo]
        public void AddSprite(LuaSpriteController sprite)
        {
            //sprite.GetTarget()
        }

        [MoonSharpHidden, ToDo("AddSubBar(), subvalue[], subcolor[], RemoveSubBar(), RemoveAllSubBars()")]
        public void AddSubBar(string key) { }
        /*
        private void PrepareSubBar(int capacity = -1)
        {
            if (subbars != null) return;
            if (capacity > 0)
            {
                subbars = new Dictionary<string, Image>(capacity);
            }
            else
            {
                subbars = new Dictionary<string, Image>();
            }
        }

        private void CheckSubExists(string key, bool getting = false)
        {
            CheckExists(getting, false);
            if (subbars == null || !subbars.ContainsKey(key)) throw new CYFException("SubLifeBar \"" + key + "\" is not found.");
            if (subbars[key] == null)
            {
                throw new CYFException(getting ? "Attempt to get a parameter from a removed SubLifeBar." : "Attempt to set a parameter to a removed SubLifeBar.");
            }
        }

        public void AddSubBar(string key)
        {
            CheckExists(false, false);
            PrepareSubBar();
            subbars[key] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/HPBarSub"), self.transform).GetComponent<Image>();
            subbars[key].transform.SetSiblingIndex(fill.transform.GetSiblingIndex());
        }


        public float subvalue[string key]
        {
            get
            {
                CheckExists(true);
                return _hp;
            }
            set
            {
                SetHP(value, _maxhp);
            }
        }
        */
        // --------------------------------------------------------------------------------
    }
}
