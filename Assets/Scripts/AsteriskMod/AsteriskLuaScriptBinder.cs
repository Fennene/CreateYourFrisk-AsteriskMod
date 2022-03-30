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

        public static void BoundScriptGlobal(ref Script script)
        {
            script.Globals["retroMode"] = GlobalControls.retroMode;
            script.Globals["isModifiedCYF"] = true;
            script.Globals["Asterisk"] = true;
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskCustomStateUpdate"] = false;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;

            if (!UnitaleUtil.IsOverworld)
            {
                script.Globals["ExistsLayer"] = (Func<string, bool>)ExistsLayer;
            }
        }

        public static void BoundScriptUserData(ref Script script)
        {
            DynValue buttonUtil = UserData.Create(new LuaButtonController());
            script.Globals.Set("ButtonUtil", buttonUtil);
            DynValue playerUtil = UserData.Create(new PlayerUtil());
            script.Globals.Set("PlayerUtil", playerUtil);
            DynValue arenaUtil = UserData.Create(new ArenaUtil());
            script.Globals.Set("ArenaUtil", arenaUtil);
        }

        public static bool ExistsLayer(string name)
        {
            return name != null && GameObject.Find((UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/") + name + "Layer") != null;
        }

        public static void LateInitialize()
        {
            PlayerUIManager.Instance.Request();
            UIStats.instance.Request();
        }
    }
}
