using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsteriskMod
{
    public class ButtonManager
    {
        public static readonly int[] PlayerButtonDefaultPositionX = new int[4] {
            48,
            202,
            361,
            515
        };

        public static readonly int PlayerButtonDefaultPositionY = 25;

        /*
        private static GameObject fight;
        private static GameObject act;
        private static GameObject item;
        private static GameObject mercy;

        private static bool activeFight;
        private static bool activeAct;
        private static bool activeItem;
        private static bool activeMercy;

        public static void Load(GameObject[] buttons)
        {
            fight = buttons[0];
            act = buttons[1];
            item = buttons[2];
            mercy = buttons[3];
            activeFight = true;
            activeAct = true;
            activeItem = true;
            activeMercy = true;
        }

        public static void SetFightSprite(string inactiveTextureFileName, string activeTextureFileName)
        {
            Image fightImage = fight.GetComponent<Image>();
            SpriteUtil.SwapSpriteFromFile(fightImage, inactiveTextureFileName);
            UIController.fightButtonSprite = SpriteRegistry.Get(activeTextureFileName);
        }

        public static void SetActSprite(string inactiveTextureFileName, string activeTextureFileName)
        {
            Image actImage = act.GetComponent<Image>();
            SpriteUtil.SwapSpriteFromFile(actImage, inactiveTextureFileName);
            UIController.actButtonSprite = SpriteRegistry.Get(activeTextureFileName);
        }

        public static void SetItemSprite(string inactiveTextureFileName, string activeTextureFileName)
        {
            Image itemImage = item.GetComponent<Image>();
            SpriteUtil.SwapSpriteFromFile(itemImage, inactiveTextureFileName);
            UIController.itemButtonSprite = SpriteRegistry.Get(activeTextureFileName);
        }

        public static void SetMercySprite(string inactiveTextureFileName, string activeTextureFileName)
        {
            Image mercyImage = mercy.GetComponent<Image>();
            SpriteUtil.SwapSpriteFromFile(mercyImage, inactiveTextureFileName);
            UIController.mercyButtonSprite = SpriteRegistry.Get(activeTextureFileName);
        }
        */
    }
}
