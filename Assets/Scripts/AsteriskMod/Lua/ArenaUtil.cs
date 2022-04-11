using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod.Lua
{
    public class ArenaUtil
    {
        [MoonSharpHidden]
        public static Vector2 textPosition = Vector2.zero;

        public static float centerabsx { get { return ArenaManager.arenaCenter.x; } }

        public static float centerabsy { get { return ArenaManager.arenaCenter.y; } }

        public static void SetBorderColor(float r, float g, float b, float a = 1.0f)
        {
            ArenaUIManager.Instance.SetBorderColor(new Color(r, g, b, a));
        }

        public static void SetBorderColor32(byte r, byte g, byte b, byte a = 255)
        {
            ArenaUIManager.Instance.SetBorderColor(new Color32(r, g, b, a));
        }

        public static void SetInnerColor(float r, float g, float b, float a = 1.0f)
        {
            ArenaUIManager.Instance.SetInnerColor(new Color(r, g, b, a));
        }

        public static void SetInnerColor32(byte r, byte g, byte b, byte a = 255)
        {
            ArenaUIManager.Instance.SetInnerColor(new Color32(r, g, b, a));
        }

        public static void SetDialogTextPosition(int x, int y)
        {
            Vector3 oldPosition = UIController.instance.mainTextManager.transform.position;
            Vector2 oldRelativePos = textPosition;
            textPosition = new Vector2(x, y);
            UIController.instance.mainTextManager.transform.position = new Vector3(
                oldPosition.x - oldRelativePos.x + textPosition.x,
                oldPosition.y - oldRelativePos.y + textPosition.y,
                oldPosition.z
            );
        }

        public static void SetDialogTextVolume(float value)
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.SetDialogTextVolume");
            ArenaUIManager.Instance.SetTextVolume(value);
        }

        public static float GetDialogTextVolume()
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.GetDialogTextVolume");
            return ArenaUIManager.Instance.GetTextVolume();
        }

        public static void SetDialogTextMute(bool mute)
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.SetDialogTextMute");
            ArenaUIManager.Instance.SetTextMute(mute);
        }

        public static bool GetDialogTextMute()
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.GetDialogTextMute");
            return ArenaUIManager.Instance.GetTextMute();
        }

        public static void SetAsteriskChar(char asterisk)
        {
            ArenaUIManager.asterisk_char = asterisk;
        }

        public static void SetStarChar(char asterisk)
        {
            SetAsteriskChar(asterisk);
        }
    }
}
