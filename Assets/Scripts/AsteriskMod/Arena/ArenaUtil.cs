using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod
{
    public class ArenaUtil
    {
        public static float centerabsx { get { return ArenaManager.arenaCenter.x; } }

        public static float centerabsy { get { return ArenaManager.arenaCenter.y; } }

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
    }
}
