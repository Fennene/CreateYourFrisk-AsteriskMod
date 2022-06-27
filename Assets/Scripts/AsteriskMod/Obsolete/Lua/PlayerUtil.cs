using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsteriskMod.Lua
{
    // I know that below codes are very too awesome f stupid idea. I will fix it in v0.5.3.
    public class PlayerUtil
    {
        /*
        public static Color nameTextColor;
        public static Color lvTextColor;
        public static Color hpTextColor;
        */

        /*
        public static void Initialize()
        {
            nameTextColor = new Color(1, 1, 1, 1);
            lvTextColor = new Color(1, 1, 1, 1);
            hpTextColor = new Color(1, 1, 1, 1);
        }
        */

        public static void UIMove(float x, float y)
        {
            //PlayerUIManager.Instance.MovePosition(x, y);
            AsteriskMod.PlayerUtil.Instance.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public static void UIMoveTo(float x, float y)
        {
            //PlayerUIManager.Instance.SetPosition(x, y);
            AsteriskMod.PlayerUtil.Instance.MoveTo(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public static Table GetUIPosition()
        {
            //Vector2 pos = PlayerUIManager.Instance.GetPosition();
            Vector2 pos = new Vector2(AsteriskMod.PlayerUtil.Instance.x, AsteriskMod.PlayerUtil.Instance.y);
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void NameUIMove(float x, float y)
        {
            //PlayerUIManager.Instance.MoveNameLVPosition(x, y);
            AsteriskMod.PlayerUtil.Instance.Name.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            AsteriskMod.PlayerUtil.Instance.Love.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public static void NameUIMoveTo(float x, float y)
        {
            //PlayerUIManager.Instance.SetNameLVPosition(x, y);
            AsteriskMod.PlayerUtil.Instance.Name.MoveTo(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            AsteriskMod.PlayerUtil.Instance.Love.MoveTo(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public static Table GetNameUIPosition()
        {
            //Vector2 pos = PlayerUIManager.Instance.GetNameLVPosition();
            Vector2 pos = new Vector2(AsteriskMod.PlayerUtil.Instance.Name.x, AsteriskMod.PlayerUtil.Instance.Name.y);
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void SetLV(string lv)
        {
            //PlayerUIManager.Instance.SetLVText(lv);
            AsteriskMod.PlayerUtil.Instance.SetLV(lv);
        }

        public static void SetNameColor(float r, float g, float b, float a = 1.0f)
        {
            /*
            nameTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(true, nameTextColor);
            */
            AsteriskMod.PlayerUtil.Instance.Name.color = new[] { r, g, b, a };
        }

        public static void SetNameColor32(byte r, byte g, byte b, byte a = 255)
        {
            /*
            nameTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(true, nameTextColor);
            */
            AsteriskMod.PlayerUtil.Instance.Name.color = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
        }

        public static void SetLVColor(float r, float g, float b, float a = 1.0f)
        {
            /*
            lvTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(false, lvTextColor);
            */
            AsteriskMod.PlayerUtil.Instance.Love.color = new[] { r, g, b, a };
        }

        public static void SetLVColor32(byte r, byte g, byte b, byte a = 255)
        {
            /*
            lvTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(false, lvTextColor);
            */
            AsteriskMod.PlayerUtil.Instance.Love.color = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
        }

        public static void SetNameLVColorManually(int start, int end, float r, float g, float b, float a = 1.0f)
        {
            throw new CYFException("PlayerUtil.SetNameLVColorManually: Why do you know this function?\nThis function is for dev.\nThe issue is solved already.\nYou should use normal functions that is written in documentation.");
            //PlayerUIManager.Instance.SetNameLVColorManually(start, end, new Color(r, g, b, a));
        }

        public static void SetNameLVColor32Manually(int start, int end, byte r, byte g, byte b, byte a = 255)
        {
            throw new CYFException("PlayerUtil.SetNameLVColor32Manually: Why do you know this function?\nThis function is for dev.\nThe issue is solved already.\nYou should use normal functions that is written in documentation.");
            //PlayerUIManager.Instance.SetNameLVColorManually(start, end, new Color32(r, g, b, a));
        }

        public static void HPUIMove(float x, float y)
        {
            //PlayerUIManager.Instance.MoveHPPosition(x, y);
            AsteriskMod.PlayerUtil.Instance.HPMove(x, y);
        }

        public static void HPUIMoveTo(float x, float y)
        {
            //PlayerUIManager.Instance.SetHPPosition(x, y);
            AsteriskMod.PlayerUtil.Instance.HPMoveTo(x, y);
        }

        public static Table GetHPUIPosition()
        {
            //Vector2 pos = PlayerUIManager.Instance.GetHPPosition();
            Table table = new Table(null);
            //table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(AsteriskMod.PlayerUtil.Instance.hpx));
            //table.Append(DynValue.NewNumber(pos.y));
            table.Append(DynValue.NewNumber(AsteriskMod.PlayerUtil.Instance.hpy));
            return table;
        }

        public static void SetHPLabelColor(float r, float g, float b, float a = 1.0f)
        {
            //PlayerUIManager.Instance.SetHPLabelColor(new Color(r, g, b, a));
            AsteriskMod.PlayerUtil.Instance.HPLabel.color = new[] { r, g, b, a };
        }

        public static void SetHPLabelColor32(byte r, byte g, byte b, byte a = 255)
        {
            //PlayerUIManager.Instance.SetHPLabelColor(new Color32(r, g, b, a));
            AsteriskMod.PlayerUtil.Instance.HPLabel.color = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
        }

        public static void SetHPBarBGColor(float r, float g, float b, float a = 1.0f)
        {
            //PlayerUIManager.Instance.SetHPBarColor(true, new Color(r, g, b, a));
            AsteriskMod.PlayerUtil.Instance.HPBar.bgcolor = new[] { r, g, b, a };
        }

        public static void SetHPBarBGColor32(byte r, byte g, byte b, byte a = 255)
        {
            //PlayerUIManager.Instance.SetHPBarColor(true, new Color32(r, g, b, a));
            AsteriskMod.PlayerUtil.Instance.HPBar.bgcolor = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
        }

        public static void SetHPBarFillColor(float r, float g, float b, float a = 1.0f)
        {
            //PlayerUIManager.Instance.SetHPBarColor(false, new Color(r, g, b, a));
            AsteriskMod.PlayerUtil.Instance.HPBar.fillcolor = new[] { r, g, b, a };
        }

        public static void SetHPBarFillColor32(byte r, byte g, byte b, byte a = 255)
        {
            //PlayerUIManager.Instance.SetHPBarColor(false, new Color32(r, g, b, a));
            AsteriskMod.PlayerUtil.Instance.HPBar.fillcolor = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
        }

        public static void HPTextMove(float x, float y)
        {
            //PlayerUIManager.Instance.MoveHPTextPosition(x, y);
            AsteriskMod.PlayerUtil.Instance.HPText.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public static void HPTextMoveTo(float x, float y)
        {
            //PlayerUIManager.Instance.SetHPTextPosition(x, y);
            AsteriskMod.PlayerUtil.Instance.HPText.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public static Table GetHPTextPosition()
        {
            //Vector2 pos = PlayerUIManager.Instance.GetHPTextPosition();
            Vector2 pos = new Vector2(AsteriskMod.PlayerUtil.Instance.HPText.x, AsteriskMod.PlayerUtil.Instance.HPText.y);
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void SetHPTextColor(float r, float g, float b, float a = 1.0f)
        {
            /*
            hpTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetHPTextColor(hpTextColor);
            */
            AsteriskMod.PlayerUtil.Instance.HPText.color = new[] { r, g, b, a };
        }

        public static void SetHPTextColor32(byte r, byte g, byte b, byte a = 255)
        {
            /*
            hpTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetHPTextColor(hpTextColor);
            */
            AsteriskMod.PlayerUtil.Instance.HPText.color = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
        }

        public static void SetHPControlOverride(bool active)
        {
            //PlayerUIManager.Instance.hpbarControlOverride = active;
            AsteriskMod.PlayerUtil.Instance.SetHPControlOverride(active);
        }

        public static void SetHP(int newHP, int newMaxHP, bool updateHPText = false)
        {
            //UIStats.instance.setHPOverride(newHP, newMaxHP, updateHPText);
            PlayerLifeUI.instance.LegacySetHPOverride(newHP, newMaxHP, updateHPText);
        }

        public static void SetHPBarLength(int newMaxHP)
        {
            //UIStats.instance.setMaxHPOverride(newMaxHP);
            AsteriskMod.PlayerUtil.Instance.HPBar.maxhp = newMaxHP;
        }

        public static void SetHPText(string hpText)
        {
            //UIStats.instance.setHPTextOverride(hpText);
            AsteriskMod.PlayerUtil.Instance.HPText.SetText(hpText);
        }

        public static int GetSoulAlpha()
        {
            //return PlayerController.instance.selfImg.enabled ? 1 : 0;
            if (PlayerController.instance == null) return 1;
            return AsteriskMod.PlayerUtil.Instance.GetSoulAlpha();
        }


        public static LuaLifeBar CreateLifeBar(bool below = false)
        {
            /*
            GameObject parent = below ? GameObject.Find("BelowHPBar"): GameObject.Find("AboveHPBar");
            if (parent == null) return null;
            return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/HPBar"), parent.transform).GetComponent<LuaLifeBar>();
            */
            string findName = below ? "*BelowHPBar" : "*AboveHPBar";
            GameObject parent = GameObject.Find(findName);
            if (parent == null) return null;
            return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/HPBar"), parent.transform).GetComponent<LuaLifeBar>();
        }
    }
}
