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
            AsteriskMod.PlayerUtil.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            //PlayerUIManager.Instance.MovePosition(x, y);
        }

        public static void UIMoveTo(float x, float y)
        {
            AsteriskMod.PlayerUtil.MoveTo(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            //PlayerUIManager.Instance.SetPosition(x, y);
        }

        public static Table GetUIPosition()
        {
            //Vector2 pos = PlayerUIManager.Instance.GetPosition();
            Vector2 pos = new Vector2(AsteriskMod.PlayerUtil.x, AsteriskMod.PlayerUtil.y);
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void NameUIMove(float x, float y)
        {
            AsteriskMod.PlayerUtil.Name.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            //PlayerUIManager.Instance.MoveNameLVPosition(x, y);
        }

        public static void NameUIMoveTo(float x, float y)
        {
            AsteriskMod.PlayerUtil.Name.MoveTo(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            //PlayerUIManager.Instance.SetNameLVPosition(x, y);
        }

        public static Table GetNameUIPosition()
        {
            //Vector2 pos = PlayerUIManager.Instance.GetNameLVPosition();
            Vector2 pos = new Vector2(AsteriskMod.PlayerUtil.Name.x, AsteriskMod.PlayerUtil.Name.y);
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void SetLV(string lv)
        {
            AsteriskMod.PlayerUtil.SetLV(lv);
            //PlayerUIManager.Instance.SetLVText(lv);
        }

        public static void SetNameColor(float r, float g, float b, float a = 1.0f)
        {
            AsteriskMod.PlayerUtil.Name.color = new[] { r, g, b, a };
            /*
            nameTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(true, nameTextColor);
            */
        }

        public static void SetNameColor32(byte r, byte g, byte b, byte a = 255)
        {
            AsteriskMod.PlayerUtil.Name.color = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
            /*
            nameTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(true, nameTextColor);
            */
        }

        public static void SetLVColor(float r, float g, float b, float a = 1.0f)
        {
            AsteriskMod.PlayerUtil.Love.color = new[] { r, g, b, a };
            /*
            lvTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(false, lvTextColor);
            */
        }

        public static void SetLVColor32(byte r, byte g, byte b, byte a = 255)
        {
            AsteriskMod.PlayerUtil.Love.color = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
            /*
            lvTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(false, lvTextColor);
            */
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
            DevelopHint.ToDo();
            PlayerUIManager.Instance.MoveHPPosition(x, y);
        }

        public static void HPUIMoveTo(float x, float y)
        {
            PlayerUIManager.Instance.SetHPPosition(x, y);
        }

        public static Table GetHPUIPosition()
        {
            Vector2 pos = PlayerUIManager.Instance.GetHPPosition();
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void SetHPLabelColor(float r, float g, float b, float a = 1.0f)
        {
            DevelopHint.ToDo();
            PlayerUIManager.Instance.SetHPLabelColor(new Color(r, g, b, a));
        }

        public static void SetHPLabelColor32(byte r, byte g, byte b, byte a = 255)
        {
            PlayerUIManager.Instance.SetHPLabelColor(new Color32(r, g, b, a));
        }

        public static void SetHPBarBGColor(float r, float g, float b, float a = 1.0f)
        {
            AsteriskMod.PlayerUtil.HPBar.bgcolor = new[] { r, g, b, a };
            //PlayerUIManager.Instance.SetHPBarColor(true, new Color(r, g, b, a));
        }

        public static void SetHPBarBGColor32(byte r, byte g, byte b, byte a = 255)
        {
            AsteriskMod.PlayerUtil.HPBar.bgcolor = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
            //PlayerUIManager.Instance.SetHPBarColor(true, new Color32(r, g, b, a));
        }

        public static void SetHPBarFillColor(float r, float g, float b, float a = 1.0f)
        {
            AsteriskMod.PlayerUtil.HPBar.fillcolor = new[] { r, g, b, a };
            //PlayerUIManager.Instance.SetHPBarColor(false, new Color(r, g, b, a));
        }

        public static void SetHPBarFillColor32(byte r, byte g, byte b, byte a = 255)
        {
            AsteriskMod.PlayerUtil.HPBar.fillcolor = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
            //PlayerUIManager.Instance.SetHPBarColor(false, new Color32(r, g, b, a));
        }

        public static void HPTextMove(float x, float y)
        {
            AsteriskMod.PlayerUtil.HPText.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            //PlayerUIManager.Instance.MoveHPTextPosition(x, y);
        }

        public static void HPTextMoveTo(float x, float y)
        {
            AsteriskMod.PlayerUtil.HPText.Move(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            PlayerUIManager.Instance.SetHPTextPosition(x, y);
        }

        public static Table GetHPTextPosition()
        {
            //Vector2 pos = PlayerUIManager.Instance.GetHPTextPosition();
            Vector2 pos = new Vector2(AsteriskMod.PlayerUtil.HPText.x, AsteriskMod.PlayerUtil.HPText.y);
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void SetHPTextColor(float r, float g, float b, float a = 1.0f)
        {
            AsteriskMod.PlayerUtil.HPText.color = new[] { r, g, b, a };
            /*
            hpTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetHPTextColor(hpTextColor);
            */
        }

        public static void SetHPTextColor32(byte r, byte g, byte b, byte a = 255)
        {
            AsteriskMod.PlayerUtil.HPText.color = new[] { r / 255f, g / 255f, b / 255f, a / 255f };
            /*
            hpTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetHPTextColor(hpTextColor);
            */
        }

        public static void SetHPControlOverride(bool active)
        {
            AsteriskMod.PlayerUtil.SetHPControlOverride(active);
            //PlayerUIManager.Instance.hpbarControlOverride = active;
        }

        public static void SetHP(int newHP, int newMaxHP, bool updateHPText = false)
        {
            float percentage = (float)newHP / (float)newMaxHP;
            DevelopHint.ToDo("前の挙動");
            AsteriskMod.PlayerUtil.SetHP(newHP, newMaxHP, updateHPText);
            //UIStats.instance.setHPOverride(newHP, newMaxHP, updateHPText);
        }

        public static void SetHPBarLength(int newMaxHP)
        {
            DevelopHint.ToDo("前の挙動");
            AsteriskMod.PlayerUtil.HPBar.maxhp = newMaxHP;
            //UIStats.instance.setMaxHPOverride(newMaxHP);
        }

        public static void SetHPText(string hpText)
        {
            AsteriskMod.PlayerUtil.HPText.SetText(hpText);
            //UIStats.instance.setHPTextOverride(hpText);
        }

        public static int GetSoulAlpha()
        {
            if (PlayerController.instance == null) return 1;
            return AsteriskMod.PlayerUtil.GetSoulAlpha();
            //return PlayerController.instance.selfImg.enabled ? 1 : 0;
        }


        public static LuaLifeBar CreateLifeBar(bool below = false)
        {
            GameObject parent = below ? GameObject.Find("BelowHPBar"): GameObject.Find("AboveHPBar");
            if (parent == null) return null;
            return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/HPBar"), parent.transform).GetComponent<LuaLifeBar>();
        }
    }
}
