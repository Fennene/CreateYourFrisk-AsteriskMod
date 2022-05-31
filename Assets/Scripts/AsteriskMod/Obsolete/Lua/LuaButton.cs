using MoonSharp.Interpreter;
//using UnityEngine;
//using UnityEngine.UI;

namespace AsteriskMod.Lua
{
    // I know that below codes are very too awesome f stupid idea. I will fix it in v0.5.3.
    public class LuaButton
    {
        private int buttonID;
        /*
        private bool isActive;
        private GameObject button;
        private Vector2 position;
        private Vector2 initSize;
        */

        [MoonSharpHidden]
        public LuaButton(int id/*, GameObject buttonObject*/)
        {
            buttonID = id;
            /*
            isActive = true;
            button = buttonObject;
            position = Vector2.zero;
            initSize = buttonObject.GetComponent<Image>().rectTransform.sizeDelta;
            */
        }

        public void SetActive(bool active)
        {
            UIController.ActionButtonManager[buttonID + 1].SetActive(active);
            /*
            isActive = active;
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f, (isActive ? 1f : 0f));
            if (!isActive && !LuaButtonController.CanInactiveButton(buttonID))
            {
                throw new CYFException("All button should not be inactivated.");
            }
            */
        }

        public bool GetActive()
        {
            return UIController.ActionButtonManager[buttonID + 1].isactive;
        }

        public void SetSprite(string inactiveTexturePath, string activeTexturePath, string prefix = "")
        {
            UIController.ActionButtonManager[buttonID + 1].SetSprite(inactiveTexturePath, activeTexturePath, prefix, true);
            /*
            Image image = button.GetComponent<Image>();
            if (!string.IsNullOrEmpty(prefix)) prefix += "/";
            inactiveTexturePath = prefix + inactiveTexturePath;
            activeTexturePath = prefix + activeTexturePath;
            SpriteUtil.SwapSpriteFromFile(image, inactiveTexturePath);
            Sprite activeTexture = SpriteRegistry.Get(activeTexturePath);
            switch (buttonID)
            {
                case 0:
                    if (image.overrideSprite == UIController.fightButtonSprite)
                    {
                        image.overrideSprite = activeTexture;
                    }
                    UIController.fightButtonSprite = activeTexture;
                    break;
                case 1:
                    if (image.overrideSprite == UIController.actButtonSprite)
                    {
                        image.overrideSprite = activeTexture;
                    }
                    UIController.actButtonSprite = activeTexture;
                    break;
                case 2:
                    if (image.overrideSprite == UIController.itemButtonSprite)
                    {
                        image.overrideSprite = activeTexture;
                    }
                    UIController.itemButtonSprite = activeTexture;
                    break;
                case 3:
                    if (image.overrideSprite == UIController.mercyButtonSprite)
                    {
                        image.overrideSprite = activeTexture;
                    }
                    UIController.mercyButtonSprite = activeTexture;
                    break;
            }
            */
        }

        public float x
        {
            get { /*return position.x;*/ return UIController.ActionButtonManager[buttonID + 1].x; }
            set { /*MoveTo(value, position.y);*/ UIController.ActionButtonManager[buttonID + 1].x = (int)value; }
        }

        public float y
        {
            get { /*return position.y;*/ return UIController.ActionButtonManager[buttonID + 1].y; }
            set { /*MoveTo(position.x, value);*/ UIController.ActionButtonManager[buttonID + 1].y = (int)value; }
        }

        public void MoveTo(float x, float y)
        {
            UIController.ActionButtonManager[buttonID + 1].MoveTo((int)x, (int)y);
            /*
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x - position.x, button.GetComponent<RectTransform>().anchoredPosition.y - position.y);
            position = new Vector2(x, y);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x + x, button.GetComponent<RectTransform>().anchoredPosition.y + y);
            */
        }

        public void Move(float x, float y)
        {
            UIController.ActionButtonManager[buttonID + 1].Move((int)x, (int)y);
            // MoveTo(position.x + x, position.y + y);
        }

        public void SetColor(float r, float g, float b, float a = 1.0f)
        {
            UIController.ActionButtonManager[buttonID + 1].color = new[] { r, g, b, a };
            //button.GetComponent<Image>().color = new Color(r, g, b, a);
        }

        public void SetColor32(byte r, byte g, byte b, byte a = 255)
        {
            UIController.ActionButtonManager[buttonID + 1].color32 = new[] { r, g, b, a };
            //button.GetComponent<Image>().color = new Color32(r, g, b, a);
        }

        public void ResetColor()
        {
            UIController.ActionButtonManager[buttonID + 1].color = new float[4] { 1, 1, 1, 1 };
            //button.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        public void SetSize(int width, int height)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.QOLUpdate) AsteriskUtil.ThrowFakeNonexistentFunctionError("LuaButton", "SetSize");
            Asterisk.RequireExperimentalFeature("button.SetSize");
            UIController.ActionButtonManager[buttonID + 1].SetSize(width, height);
            /*
            Asterisk.RequireExperimentalFeature("button.SetSize");
            button.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(width, height);
            */
        }

        public void ResetSize()
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.QOLUpdate) AsteriskUtil.ThrowFakeNonexistentFunctionError("LuaButton", "ResetSize");
            Asterisk.RequireExperimentalFeature("button.ResetSize");
            UIController.ActionButtonManager[buttonID + 1].SetSize(110, 42);
            /*
            Asterisk.RequireExperimentalFeature("button.SetSize");
            button.GetComponent<Image>().rectTransform.sizeDelta = initSize;
            */
        }

        public void Scale(float xScale, float yScale)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.QOLUpdate) AsteriskUtil.ThrowFakeNonexistentFunctionError("LuaButton", "Scale");
            Asterisk.RequireExperimentalFeature("button.Scale");
            UIController.ActionButtonManager[buttonID + 1].Scale(xScale, yScale);
            //button.GetComponent<Image>().rectTransform.localScale = new Vector3(xScale, yScale, 1);
        }

        public void Revert()
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion < Asterisk.Versions.QOLUpdate) AsteriskUtil.ThrowFakeNonexistentFunctionError("LuaButton", "Revert");
            UIController.ActionButtonManager[buttonID + 1].Revert(true);
            /*
            ResetColor();
            string btname = "";
            switch (buttonID)
            {
                case 0:
                    btname = "fight";
                    break;
                case 1:
                    btname = "act";
                    break;
                case 2:
                    btname = "item";
                    break;
                case 3:
                    btname = "mercy";
                    break;
            }
            SetSprite(btname + "bt_0", btname + "bt_1", "UI/Buttons");
            MoveTo(0, 0);
            SetActive(true);
            if (!Asterisk.RequireExperimentalFeature("button.Revert", false)) return;
            Scale(1, 1);
            ResetSize();
            */
        }
    }
}
