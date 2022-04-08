using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.Lua
{
    public class LuaButton
    {
        private int buttonID;
        private bool isActive;
        private GameObject button;
        private Vector2 position;
        private Vector2 initSize;

        [MoonSharpHidden]
        public LuaButton(int id, GameObject buttonObject)
        {
            buttonID = id;
            isActive = true;
            button = buttonObject;
            position = Vector2.zero;
            initSize = buttonObject.GetComponent<Image>().rectTransform.sizeDelta;
        }

        public void SetActive(bool active)
        {
            isActive = active;
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f, (isActive ? 1f : 0f));
            if (!isActive && !LuaButtonController.CanInactiveButton(buttonID))
            {
                throw new CYFException("All button should not be inactivated.");
            }
        }

        public bool GetActive()
        {
            return isActive;
        }

        public void SetSprite(string inactiveTexturePath, string activeTexturePath, string prefix = "")
        {
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
        }

        public float x
        {
            get { return position.x; }
            set { MoveTo(value, position.y); }
        }

        public float y
        {
            get { return position.y; }
            set { MoveTo(position.x, value); }
        }

        public void MoveTo(float x, float y)
        {
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x - position.x, button.GetComponent<RectTransform>().anchoredPosition.y - position.y);
            position = new Vector2(x, y);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x + x, button.GetComponent<RectTransform>().anchoredPosition.y + y);
        }

        public void Move(float x, float y)
        {
            MoveTo(position.x + x, position.y + y);
        }

        [MoonSharpHidden]
        internal Vector2 GetRelativePosition()
        {
            return position;
        }

        public void SetColor(float r, float g, float b, float a = 1.0f)
        {
            button.GetComponent<Image>().color = new Color(r, g, b, a);
        }

        public void SetColor32(byte r, byte g, byte b, byte a = 255)
        {
            button.GetComponent<Image>().color = new Color32(r, g, b, a);
        }

        public void ResetColor()
        {
            button.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        public void SetSize(int width, int height)
        {
            Asterisk.RequireExperimentalFeature("button.SetSize");
            button.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(width, height);
        }

        public void ResetSize()
        {
            Asterisk.RequireExperimentalFeature("button.SetSize");
            button.GetComponent<Image>().rectTransform.sizeDelta = initSize;
        }

        public void Scale(float xScale, float yScale)
        {
            Asterisk.RequireExperimentalFeature("button.Scale");
            button.GetComponent<Image>().rectTransform.localScale = new Vector3(xScale, yScale, 1);
        }
    }
}
