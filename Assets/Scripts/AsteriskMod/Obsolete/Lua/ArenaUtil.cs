using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod.Lua
{
    public class ArenaUtil
    {
        //[MoonSharpHidden]
        //public static Vector2 textPosition = Vector2.zero;

        // --------------------------------------------------------------------------------
        //                            Asterisk Mod Additions
        // --------------------------------------------------------------------------------
        private AsteriskMod.ArenaUtil NewArenaUtil;

        public ArenaUtil()
        {
            NewArenaUtil = new AsteriskMod.ArenaUtil();
        }
        // --------------------------------------------------------------------------------

        public float centerabsx { get { /*return ArenaManager.arenaCenter.x;*/ return NewArenaUtil.centerabsx; } }

        public float centerabsy { get { /*return ArenaManager.arenaCenter.y;*/ return NewArenaUtil.centerabsy; } }

        public void SetBorderColor(float r, float g, float b, float a = 1.0f)
        {
            //ArenaUIManager.Instance.SetBorderColor(new Color(r, g, b, a));
            NewArenaUtil.SetBorderColor(r, g, b, a);
        }

        public void SetBorderColor32(byte r, byte g, byte b, byte a = 255)
        {
            //ArenaUIManager.Instance.SetBorderColor(new Color32(r, g, b, a));
            NewArenaUtil.SetBorderColor32(r, g, b, a);
        }

        public void SetInnerColor(float r, float g, float b, float a = 1.0f)
        {
            //ArenaUIManager.Instance.SetInnerColor(new Color(r, g, b, a));
            NewArenaUtil.SetInnerColor(r, g, b, a);
        }

        public void SetInnerColor32(byte r, byte g, byte b, byte a = 255)
        {
            //ArenaUIManager.Instance.SetInnerColor(new Color32(r, g, b, a));
            NewArenaUtil.SetInnerColor32(r, g, b, a);
        }

        public void SetDialogTextPosition(int x, int y)
        {
            /*
            Vector3 oldPosition = UIController.instance.mainTextManager.transform.position;
            Vector2 oldRelativePos = textPosition;
            textPosition = new Vector2(x, y);
            UIController.instance.mainTextManager.transform.position = new Vector3(
                oldPosition.x - oldRelativePos.x + textPosition.x,
                oldPosition.y - oldRelativePos.y + textPosition.y,
                oldPosition.z
            );
            */
            NewArenaUtil.DialogTextMoveTo(x, y);
        }

        public void SetDialogTextVolume(float value)
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.SetDialogTextVolume");
            //ArenaUIManager.Instance.SetTextVolume(value);
            NewArenaUtil.SetDialogTextVolume(value);
        }

        public float GetDialogTextVolume()
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.GetDialogTextVolume");
            //return ArenaUIManager.Instance.GetTextVolume();
            return NewArenaUtil.GetDialogTextVolume();
        }

        public void SetDialogTextMute(bool mute)
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.SetDialogTextMute");
            //ArenaUIManager.Instance.SetTextMute(mute);
            NewArenaUtil.SetDialogTextVolume(mute ? 0 : 1);
        }

        public bool GetDialogTextMute()
        {
            Asterisk.RequireExperimentalFeature("ArenaUtil.GetDialogTextMute");
            //return ArenaUIManager.Instance.GetTextMute();
            return NewArenaUtil.GetDialogTextVolume() == 0;
        }

        public void SetAsteriskChar(char asterisk)
        {
            //ArenaUIManager.asterisk_char = asterisk;
            NewArenaUtil.AsteriskChar = asterisk;
        }

        public void SetStarChar(char asterisk)
        {
            SetAsteriskChar(asterisk);
        }
    }
}
