using AsteriskMod.Lua;
using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AsteriskMod
{
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
            script.Globals["Asterisk"] = true;
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskGMSUpdate"] = false;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;
        }

        public static void BoundScriptFunctions(ref Script script)
        {
            script.Globals["LayerExists"] = (Func<string, bool>)LayerExists;
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

        public static bool LayerExists(string name)
        {
            return name != null && GameObject.Find((UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/") + name + "Layer") != null;
        }

        public static void LateInitialize()
        {
            UIStats.instance.Request();
            PlayerUIManager.Instance.Request();
        }
    }
}
