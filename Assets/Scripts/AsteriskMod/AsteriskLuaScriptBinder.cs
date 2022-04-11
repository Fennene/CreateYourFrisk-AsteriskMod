using AsteriskMod.Lua;
using AsteriskMod.Lua.GMS;
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
            UserData.RegisterType<Global>();

            UserData.RegisterType<LuaLifeBar>();

            if (!Asterisk.betaTest) return;

            UserData.RegisterType<Engine>();
            UserData.RegisterType<UnityObject>();
            UserData.RegisterType<LuaImage>();
        }

        public static void BoundScriptVariables(ref Script script)
        {
            script.Globals["isModifiedCYF"] = true;
            script.Globals["retroMode"] = GlobalControls.retroMode;
            script.Globals["Asterisk"] = true;
            //script.Globals["AsteriskMod"] = false; //v0.5.2 -> nil  v0.5.3 -> false  v0.5.4 -> true
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;
        }

        public static void BoundScriptFunctions(ref Script script)
        {
            script.Globals["SetAlMightyGlobal"] = (Action<Script, string, DynValue>)SetAlMightySafely;
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
            DynValue global = UserData.Create(new Global());
            script.Globals.Set("Global", global);

            if (!Asterisk.betaTest) return;

            DynValue engine = UserData.Create(new Engine());
            script.Globals.Set("Engine", engine);
            DynValue gameobjectmodifyingsystem = UserData.Create(new GameObjectModifyingSystem());
            script.Globals.Set("GMS", gameobjectmodifyingsystem);
        }

        public static void SetAlMightySafely(Script script, string key, DynValue value)
        {
            if (!Asterisk.optionProtecter)
            {
                LuaScriptBinder.SetAlMighty(script, key, value, true);
                return;
            }
            if (key == null)
                throw new CYFException("SetAlMightyGlobal: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
            byte protect = 0;
            if (key == "CYFSafeMode" || key == "CYFRetroMode" || key == "CYFPerfectFullscreen" || key == "CYFWindowScale" || key == "CYFDiscord") protect = 1;
            if (key == Asterisk.OPTION_EXPERIMENT || key == Asterisk.OPTION_DESC || key == Asterisk.OPTION_DOG || key == Asterisk.OPTION_LANG) protect = 1;
            if (key == Asterisk.OPTION_PROTECT || key == Asterisk.OPTION_PROTECT_ERROR) protect = 1;
            if (key == "CrateYourFrisk" && (value == null || value.Type != DataType.Boolean || !value.Boolean)) protect = 2;
            if (Asterisk.reportProtecter && protect > 0)
            {
                throw new CYFException("SetAlMightyGlobal: " + (protect == 1 ? "Attempted to change the option of system." :
                                                               (GlobalControls.crate ? Temmify.Convert("Attempted to forget what you had done.") : "Hey... what are you trying to do?")));
            }
            if (protect > 0) return;
            LuaScriptBinder.SetAlMighty(script, key, value, true);
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
