using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod
{
    /// <summary>
    /// Manager class of each buttons in Battle Scene<br/>
    /// Use in UIController
    /// </summary>
    public class ButtonManager
    {
        [MoonSharpHidden]
        internal ActionButton[] ActionButtons { get; private set; }

        [MoonSharpHidden]
        public ButtonManager()
        {
            ActionButtons = new ActionButton[4];
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i] = new ActionButton(this, i);
            }
        }

        private string GetTexturePath(string buttonTextureName, bool active)
        {
            return "UI/Buttons/" + buttonTextureName + "_" + (active ? "1" : "0");
        }

        private static readonly string[] TexturePathes = new string[4] { "fightbt", "actbt", "itembt", "mercybt" };
        private static readonly string[] TexturePathesCrate = new string[4] { "gifhtbt", "catbt", "tembt", "mecrybt" };

        /// <summary>Call in UIController.Awake()</summary>
        internal void Awake()
        {
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].Awake(GetTexturePath((GlobalControls.crate ? TexturePathesCrate[i] : TexturePathes[i]), true));
            }
        }

        private static readonly string[] ButtonObjectNames = new string[4] { "FightBt", "ActBt", "ItemBt", "MercyBt" };

        private static readonly float[] AbsAdjX = new float[4] { 0, 0, 0, 0 };
        private static readonly float[] AbsAdjY = new float[4] { 0, 0, 0, 0 };


        [MoonSharpHidden]
        /// <summary>Call in UIController.Start()</summary>
        internal void Start()
        {
            StartParent();
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].Start(ButtonObjectNames[i], GetTexturePath(TexturePathesCrate[i], false));
            }
            initialized = true;
        }

        [MoonSharpHidden]
        internal void HideAllOverrideSprite()
        {
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].HideOverrideSprite();
            }
        }

        [MoonSharpHidden]
        internal void ShowOverrideSprite(int buttonID)
        {
            ActionButtons[buttonID].ShowOverrideSprite();
        }

        [MoonSharpHidden]
        internal void SetVisibleOverrideSprite(bool[] visible)
        {
            for (var i = 0; i < 4; i++)
            {
                if (visible[i]) ActionButtons[i].ShowOverrideSprite();
                else            ActionButtons[i].HideOverrideSprite();
            }
        }

        private static readonly float[] DefaultPlayerPosX = new float[4] { 48, 202, 361, 515 };
        private const float DefaultPlayerPosY = 25;

        [MoonSharpHidden]
        internal Vector2 GetPlayerPosition(int buttonID)
        {
            if (ActionButtons[buttonID].playerabs) return ActionButtons[buttonID].RelativePlayerPosition;
            Vector2 vector = parentRelativePosition + ActionButtons[buttonID].RelativePosition + ActionButtons[buttonID].RelativePlayerPosition;
            vector.x += DefaultPlayerPosX[buttonID];
            vector.y += DefaultPlayerPosY;
            return vector;
        }

        // --------------------------------------------------------------------------------
        //                            Asterisk Mod Features
        // --------------------------------------------------------------------------------
        private RectTransform parent;
        private Vector2 parentRelativePosition;
        private bool initialized = false;

        private void StartParent()
        {
            parent = GameObject.Find("UIRect").GetComponent<RectTransform>();
            parentRelativePosition = Vector2.zero;
        }

        [MoonSharpHidden]
        internal void CheckInactivate(int buttonID)
        {
            if (!ActionButtons[buttonID].isactive) return;
            for (var i = 0; i < 4; i++)
            {
                if (i == buttonID) continue;
                if (ActionButtons[i].isactive) return;
            }
            throw new CYFException("button.SetActive(): Attempt to inactivate all buttons.");
        }

        private void CheckInitialized()
        {
            if (!initialized) throw new CYFException("The ButtonUtil object has not been initialized yet.\n\nPlease wait until at least EncounterStarting() to run this code.");
        }

        public ActionButton FIGHT {
            get
            {
                CheckInitialized();
                return ActionButtons[0];
            }
        }
        public ActionButton fight { get { return FIGHT; } }

        public ActionButton ACT
        {
            get
            {
                CheckInitialized();
                return ActionButtons[1];
            }
        }
        public ActionButton act { get { return ACT; } }

        public ActionButton ITEM
        {
            get
            {
                CheckInitialized();
                return ActionButtons[2];
            }
        }
        public ActionButton item { get { return ITEM; } }
        
        public ActionButton MERCY
        {
            get
            {
                CheckInitialized();
                return ActionButtons[3];
            }
        }
        public ActionButton mercy { get { return MERCY; } }

        public ActionButton this[int luaButtonID]
        {
            get
            {
                CheckInitialized();
                if (luaButtonID < 1 || luaButtonID > 4) throw new CYFException("ButtonUtil[int buttonID]: button's ID should be between 1 and 4. (specifed ID: " + luaButtonID + ")");
                return ActionButtons[luaButtonID - 1];
            }
        }

        public void SetActives(bool fight = true, bool act = true, bool item = true, bool mercy = true)
        {
            CheckInitialized();
            if (!(fight || act || item || mercy)) throw new CYFException("ButtonUtil.SetActives(): Attempt to inactivate all buttons.");
            ActionButtons[0].SetActive(fight);
            ActionButtons[1].SetActive(act);
            ActionButtons[2].SetActive(item);
            ActionButtons[3].SetActive(mercy);
        }

        public void SetSprites(string buttonSpritePathRoot, string inactiveSuffix = "_0", string activeSuffix = "_1", bool autoResize = false)
        {
            CheckInitialized();
            if (GlobalControls.crate) return;
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].SetSprite(TexturePathes[i] + inactiveSuffix, TexturePathes[i] + activeSuffix, buttonSpritePathRoot, autoResize);
            }
        }

        public float x
        {
            get { return parentRelativePosition.x; }
            set { MoveTo(value, y); }
        }

        public float y
        {
            get { return parentRelativePosition.y; }
            set { MoveTo(x, value); }
        }

        public void Move(float x, float y)
        {
            MoveTo(x + parentRelativePosition.x, y + parentRelativePosition.y);
        }

        public void MoveTo(float newX, float newY)
        {
            CheckInitialized();
            Vector2 initPos = parent.anchoredPosition - parentRelativePosition;
            parentRelativePosition = new Vector2(newX, newY);
            parent.anchoredPosition = initPos + parentRelativePosition;
            UIController.instance.UpdatePlayerPositionOnAction();
        }

        public void SetColor(float r, float g, float b, float a = 1)
        {
            CheckInitialized();
            float[] color = new[] { r, g, b, a };
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].color = color;
            }
        }

        public void SetColor32(byte r, byte g, byte b, byte a = 255)
        {
            CheckInitialized();
            byte[] color32 = new[] { r, g, b, a };
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].color32 = color32;
            }
        }

        public void Show()
        {
            CheckInitialized();
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].Show();
            }
        }

        public void Hide()
        {
            CheckInitialized();
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].Hide();
            }
        }

        public void Revert(bool revertPosition = true, bool revertActive = true)
        {
            CheckInitialized();
            for (var i = 0; i < 4; i++)
            {
                ActionButtons[i].Revert(revertPosition, revertActive);
            }
            if (revertPosition) MoveTo(0f, 0f);
        }
    }
}
