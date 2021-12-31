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
            switch (buttonID)
            {
                case 0:
                    UIController.fightButtonSprite = SpriteRegistry.Get(activeTexturePath);
                    break;
                case 1:
                    UIController.actButtonSprite = SpriteRegistry.Get(activeTexturePath);
                    break;
                case 2:
                    UIController.itemButtonSprite = SpriteRegistry.Get(activeTexturePath);
                    break;
                case 3:
                    UIController.mercyButtonSprite = SpriteRegistry.Get(activeTexturePath);
                    break;
            }
        }
    }
}
