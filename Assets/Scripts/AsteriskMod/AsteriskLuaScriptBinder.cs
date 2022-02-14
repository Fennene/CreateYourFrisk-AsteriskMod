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
        }

        public static void BoundScriptGlobal(ref Script script)
        {
            script.Globals["isModifiedCYF"] = true;
            script.Globals["Asterisk"] = true;
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;
        }

        public static void BoundScriptUserData(ref Script script)
        {
            DynValue buttonUtil = UserData.Create(new LuaButtonController());
            script.Globals.Set("ButtonUtil", buttonUtil);
            DynValue playerUtil = UserData.Create(new PlayerUtil());
            script.Globals.Set("PlayerUtil", playerUtil);
        }
    }
}
