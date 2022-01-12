using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsteriskMod.Lua
{
    public class LuaButton
    {
        private int buttonID;
        private bool isActive;
        private GameObject button;
        private Vector2 position;

        [MoonSharpHidden]
        public LuaButton(int id, GameObject buttonObject)
        {
            buttonID = id;
            isActive = true;
            button = buttonObject;
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

        public void SetSprite(string inactiveTexturePath, string activeTexturePath)
        {
            Image image = button.GetComponent<Image>();
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
    }
}
