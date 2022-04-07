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
            script.Globals["retroMode"] = GlobalControls.retroMode;
            script.Globals["isModifiedCYF"] = true;
            //script.Globals["Asterisk"] = true;
            script.Globals["Asterisk"] = Asterisk.active; // *Mod-4
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;
        }

        public static void BoundScriptFunctions(ref Script script)
        {
            script.Globals["GetCurrentAction"] = (Func<string>)GetCurrentAction;
            script.Globals["LayerExists"] = (Func<string, bool>)LayerExists;
            script.Globals["IsEmptyLayer"] = (Func<string, bool?>)IsEmptyLayer;
        }

        public static void BoundScriptUserDatas(ref Script script)
        {
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
            UIStats.instance.Request();
            PlayerUIManager.Instance.Request();
        }
    }
}
