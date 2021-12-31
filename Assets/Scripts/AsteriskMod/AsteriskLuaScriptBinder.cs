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
        }

        public static void BoundScriptGlobal(ref Script script)
        {
            script.Globals["isModifiedCYF"] = true;
            script.Globals["Asterisk"] = true;
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
        }

        public static void BoundScriptUserData(ref Script script)
        {
            DynValue ui = UserData.Create(new LuaButtonController());
            script.Globals.Set("ButtonUtil", ui);
        }
    }
}
