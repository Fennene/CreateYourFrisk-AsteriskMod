using AsteriskMod.Lua;
using System;
using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod
{
    /// <summary>Add AsteriskMod's variables, functions and UserDatas to <see cref="LuaScriptBinder"/></summary>
    public class AsteriskLuaScriptBinder
    {
        public static void Initialize()
        {
            UserData.RegisterType<LuaButton>();
            UserData.RegisterType<LuaButtonController>();
            UserData.RegisterType<PlayerUtil>();
            UserData.RegisterType<ArenaUtil>();
            UserData.RegisterType<StateEditor>();
        }

        public static void BoundScriptVariables(ref Script script)
        {
            script.Globals["isModifiedCYF"] = true;
            script.Globals["Asterisk"] = Asterisk.active; // *Mod-4
            script.Globals["null"] = null; // *Mod-4
            script.Globals["None"] = null; // *Mod-4
            script.Globals["UnsupportedVersion"] = true; // *Mod-4

            if (!Asterisk.active) return; // *Mod-4

            script.Globals["retroMode"] = GlobalControls.retroMode;
            //script.Globals["Asterisk"] = true;
            //script.Globals["AsteriskMod"] = false; //v0.5.2 -> nil  v0.5.3 -> false  v0.5.4 -> true
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;
        }

        public static void BoundScriptFunctions(ref Script script)
        {
            script.Globals["IsThisReallyCreateYourFriskAsteriskModAprilVersion"] = (Func<bool>)(() => { return true; }); // *Mod-4
            script.Globals["GetMayBeTrue"] = (Func<bool>)(() => { if (Math.RandomRange(1, 5) == 1) return true; else return false; }); // *Mod-4
            script.Globals["StringStartsWith"] = (Func<string, string, bool>)((text, value) => { return text.StartsWith(value); }); // *Mod-4
            script.Globals["StringEndsWith"] = (Func<string, string, bool>)((text, value) => { return text.EndsWith(value); }); // *Mod-4
            script.Globals["StringContains"] = (Func<string, string, bool>)((text, value) => { return text.Contains(value); }); // *Mod-4
            script.Globals["StringSplit"] = (Func<string, char, string[]>)((text, pattern) => { return text.Split(pattern); }); // *Mod-4

            if (!Asterisk.active) return; // *Mod-4

            script.Globals["GetCurrentAction"] = (Func<string>)GetCurrentAction;
            script.Globals["LayerExists"] = (Func<string, bool>)LayerExists;
            script.Globals["IsEmptyLayer"] = (Func<string, bool?>)IsEmptyLayer;
        }

        public static void BoundScriptUserDatas(ref Script script)
        {
            if (!Asterisk.active) return; // *Mod-4

            DynValue buttonUtil = UserData.Create(new LuaButtonController());
            script.Globals.Set("ButtonUtil", buttonUtil);
            DynValue playerUtil = UserData.Create(new PlayerUtil());
            script.Globals.Set("PlayerUtil", playerUtil);
            DynValue arenaUtil = UserData.Create(new ArenaUtil());
            script.Globals.Set("ArenaUtil", arenaUtil);
        }

        public static string GetCurrentAction()
        {
            try   { return UIController.instance.GetAction().ToString(); }
            catch { return "NONE (error)"; }
        }

        public static bool LayerExists(string name)
        {
            return name != null && GameObject.Find((UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/") + name + "Layer") != null;
        }

        public static bool? IsEmptyLayer(string name)
        {
            string canvas = UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/";
            if (name == null || GameObject.Find(canvas + name + "Layer") == null)
                return null;
            return GameObject.Find(canvas + name + "Layer").transform.childCount == 0;
        }

        /// <summary>
        /// Recall the functions that ware called before initialization.<br/>
        /// for <see cref="PlayerUtil"/>
        /// </summary>
        public static void LateInitialize()
        {
            if (!Asterisk.active) return; // *Mod-4

            UIStats.instance.Request();
            PlayerUIManager.Instance.Request();
        }
    }
}
