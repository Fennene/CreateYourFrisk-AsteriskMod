﻿using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod
{
    public class ArenaUtil
    {
        public static float centerabsx { get { return ArenaManager.arenaCenter.x; } }

        public static float centerabsy { get { return ArenaManager.arenaCenter.y; } }

        public static float offsetx
        {
            get { return ArenaUI.ArenaOffset.x; }
            set { ArenaUI.ArenaOffset = new Vector2(value, offsety); }
        }

        public static float offsety
        {
            get { return ArenaUI.ArenaOffset.y; }
            set { ArenaUI.ArenaOffset = new Vector2(offsetx, value); }
        }

        public static void SetOffset(float x, float y)
        {
            ArenaUI.ArenaOffset = new Vector2(x, y);
        }

        public static float relativewidth
        {
            get { return ArenaUI.ArenaOffsetSize.x; }
            set { ArenaUI.ArenaOffsetSize = new Vector2(value, relativeheight); }
        }

        public static float relativeheight
        {
            get { return ArenaUI.ArenaOffsetSize.y; }
            set { ArenaUI.ArenaOffsetSize = new Vector2(relativewidth, value); }
        }

        public static void SetRelativeSize(float width, float height, bool immediate = false)
        {
            ArenaUI.SetArenaOffsetSize(width, height, immediate);
        }

        public static void SetBorderColor(float r, float g, float b, float a = 1.0f)
        {
            ArenaUI.SetBorderColor(new Color(r, g, b, a));
        }

        public static void SetBorderColor32(byte r, byte g, byte b, byte a = 255)
        {
            ArenaUI.SetBorderColor(new Color32(r, g, b, a));
        }

        /*
        public static void SetBorderColor()
        {
            ArenaUI.SetBorderColor(new Color32(255, 255, 255, 255));
        }
        */

        public static void SetInnerColor(float r, float g, float b, float a = 1.0f)
        {
            ArenaUI.SetInnerColor(new Color(r, g, b, a));
        }

        public static void SetInnerColor32(byte r, byte g, byte b, byte a = 255)
        {
            ArenaUI.SetInnerColor(new Color32(r, g, b, a));
        }

        /*
        public static void SetInnerColor()
        {
            ArenaUI.SetInnerColor(new Color32(0, 0, 0, 255));
        }
        */


        public static int dialogtextx
        {
            get { return Mathf.RoundToInt(ArenaUI.GetMainTextPosition().x); }
            set { DialogTextMoveTo(value, dialogtexty); }
        }

        public static int dialogtexty
        {
            get { return Mathf.RoundToInt(ArenaUI.GetMainTextPosition().y); }
            set { DialogTextMoveTo(dialogtextx, value); }
        }

        public static void DialogTextMove(int x, int y)
        {
            Vector2 pos = ArenaUI.GetMainTextPosition();
            int newX = Mathf.RoundToInt(pos.x) + x;
            int newY = Mathf.RoundToInt(pos.y) + y;
            ArenaUI.SetMainTextPosition(newX, newY);
        }

        public static void DialogTextMoveTo(int newX, int newY)
        {
            ArenaUI.SetMainTextPosition(newX, newY);
        }
        /*
        public static void DialogTextMoveTo()
        {
            ArenaUI.SetMainTextPosition(0, 0);
        }
        public static void SetDialogTextPosition() { DialogTextMoveTo(); }
        */

        public static DynValue GetDialogTextLetters()
        {
            return DynValue.NewTable(ArenaUI.GetMainTextLetters());
        }

        [MoonSharpHidden]
        public static void SetDialogTextFont(string fontName, bool firstTime = false)
        {
            DevelopHint.ToDo("It does not work.");
            ArenaUI.SetTextFont(fontName, firstTime);
        }

        public static char AsteriskChar
        {
            get { return AsteriskEngine.AsteriskChar; }
            set { AsteriskEngine.AsteriskChar = value; }
        }

        public static void SetDialogTextVolume(float value)
        {
            ArenaUI.SetTextVolume(value);
        }

        public static float GetDialogTextVolume()
        {
            return ArenaUI.GetTextVolume();
        }

        public static void SetAutoFontCommandActive(bool active)
        {
            AsteriskEngine.JapaneseStyleOption.SetAutoJapaneseFontStyle(active);
        }

        public static void SetAutoFontCommandFont(string fontName)
        {
            if (SpriteFontRegistry.Get(fontName) == null) throw new CYFException("The font \"" + fontName + "\" doesn't exist.\nYou should check if you made a typo, or if the font really is in your mod.");
            AsteriskEngine.JapaneseStyleOption.JapaneseFontName = fontName;
        }

        public static void SetFullWidthAsterisk(bool active)
        {
            AsteriskEngine.JapaneseStyleOption.SetAutoFontCoordinatingActive(active);
        }

        public static float playeroffsetx
        {
            get { return ArenaUI.PlayerOffset.x; }
            set { ArenaUI.PlayerOffset = new Vector2(value, playeroffsety); }
        }

        public static float playeroffsety
        {
            get { return ArenaUI.PlayerOffset.y; }
            set { ArenaUI.PlayerOffset = new Vector2(playeroffsetx, value); }
        }

        public static void SetPlayerOffsetOnSelection(float x, float y)
        {
            ArenaUI.PlayerOffset = new Vector2(x, y);
        }
    }
}
