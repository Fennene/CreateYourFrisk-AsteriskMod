using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsteriskMod.Lua
{
    // I know that below codes are very too awesome f stupid idea. I will fix it in v0.5.3.
    public class PlayerUtil
    {
        public static Color nameTextColor;
        public static Color lvTextColor;
        public static Color hpTextColor;

        public static void Initialize()
        {
            nameTextColor = new Color(1, 1, 1, 1);
            lvTextColor = new Color(1, 1, 1, 1);
            hpTextColor = new Color(1, 1, 1, 1);
        }

        public static void UIMove(float x, float y)
        {
            PlayerUIManager.Instance.MovePosition(x, y);
        }

        public static void UIMoveTo(float x, float y)
        {
            PlayerUIManager.Instance.SetPosition(x, y);
        }

        public static Table GetUIPosition()
        {
            Vector2 pos = PlayerUIManager.Instance.GetPosition();
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void NameUIMove(float x, float y)
        {
            PlayerUIManager.Instance.MoveNameLVPosition(x, y);
        }

        public static void NameUIMoveTo(float x, float y)
        {
            PlayerUIManager.Instance.SetNameLVPosition(x, y);
        }

        public static Table GetNameUIPosition()
        {
            Vector2 pos = PlayerUIManager.Instance.GetNameLVPosition();
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void SetLV(string lv)
        {
            PlayerUIManager.Instance.SetLVText(lv);
        }

        public static void SetNameColor(float r, float g, float b, float a = 1.0f)
        {
            nameTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(true, nameTextColor);
        }

        public static void SetNameColor32(byte r, byte g, byte b, byte a = 255)
        {
            nameTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(true, nameTextColor);
        }

        public static void SetLVColor(float r, float g, float b, float a = 1.0f)
        {
            lvTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(false, lvTextColor);
        }

        public static void SetLVColor32(byte r, byte g, byte b, byte a = 255)
        {
            lvTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetNameLVColor(false, lvTextColor);
        }

        public static void SetNameLVColorManually(int start, int end, float r, float g, float b, float a = 1.0f)
        {
            PlayerUIManager.Instance.SetNameLVColorManually(start, end, new Color(r, g, b, a));
        }

        public static void SetNameLVColor32Manually(int start, int end, byte r, byte g, byte b, byte a = 255)
        {
            PlayerUIManager.Instance.SetNameLVColorManually(start, end, new Color32(r, g, b, a));
        }

        public static void HPUIMove(float x, float y)
        {
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
            PlayerUIManager.Instance.SetHPLabelColor(new Color(r, g, b, a));
        }

        public static void SetHPLabelColor32(byte r, byte g, byte b, byte a = 255)
        {
            PlayerUIManager.Instance.SetHPLabelColor(new Color32(r, g, b, a));
        }

        public static void SetHPBarBGColor(float r, float g, float b, float a = 1.0f)
        {
            PlayerUIManager.Instance.SetHPBarColor(true, new Color(r, g, b, a));
        }

        public static void SetHPBarBGColor32(byte r, byte g, byte b, byte a = 255)
        {
            PlayerUIManager.Instance.SetHPBarColor(true, new Color32(r, g, b, a));
        }

        public static void SetHPBarFillColor(float r, float g, float b, float a = 1.0f)
        {
            PlayerUIManager.Instance.SetHPBarColor(false, new Color(r, g, b, a));
        }

        public static void SetHPBarFillColor32(byte r, byte g, byte b, byte a = 255)
        {
            PlayerUIManager.Instance.SetHPBarColor(false, new Color32(r, g, b, a));
        }

        public static void HPTextMove(float x, float y)
        {
            PlayerUIManager.Instance.MoveHPTextPosition(x, y);
        }

        public static void HPTextMoveTo(float x, float y)
        {
            PlayerUIManager.Instance.SetHPTextPosition(x, y);
        }

        public static Table GetHPTextPosition()
        {
            Vector2 pos = PlayerUIManager.Instance.GetHPTextPosition();
            Table table = new Table(null);
            table.Append(DynValue.NewNumber(pos.x));
            table.Append(DynValue.NewNumber(pos.y));
            return table;
        }

        public static void SetHPTextColor(float r, float g, float b, float a = 1.0f)
        {
            hpTextColor = new Color(r, g, b, a);
            PlayerUIManager.Instance.SetHPTextColor(hpTextColor);
        }

        public static void SetHPTextColor32(byte r, byte g, byte b, byte a = 255)
        {
            hpTextColor = new Color32(r, g, b, a);
            PlayerUIManager.Instance.SetHPTextColor(hpTextColor);
        }

        public static void SetHPControlOverride(bool active)
        {
            PlayerUIManager.Instance.hpbarControlOverride = active;
        }

        public static void SetHP(int newHP, int newMaxHP, bool updateHPText = false)
        {
            UIStats.instance.setHPOverride(newHP, newMaxHP, updateHPText);
        }

        public static void SetHPBarLength(int newMaxHP)
        {
            UIStats.instance.setMaxHPOverride(newMaxHP);
        }

        public static void SetHPText(string hpText)
        {
            UIStats.instance.setHPTextOverride(hpText);
        }

        public static int GetSoulAlpha()
        {
            if (PlayerController.instance == null) return 1;
            return PlayerController.instance.selfImg.enabled ? 1 : 0;
        }


        public static LuaLifeBar CreateLifeBar(bool below = false)
        {
            GameObject parent = below ? GameObject.Find("BelowHPBar"): GameObject.Find("AboveHPBar");
            if (parent == null) return null;
            return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/HPBar"), parent.transform).GetComponent<LuaLifeBar>();
        }
    }
}
