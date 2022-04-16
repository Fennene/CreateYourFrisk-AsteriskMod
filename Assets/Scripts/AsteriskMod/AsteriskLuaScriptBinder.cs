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
            UserData.RegisterType<ActionButton>();
            UserData.RegisterType<ButtonManager>();
            UserData.RegisterType<GameObjectModifyingSystem>();
            UserData.RegisterType<UnityObject>();
            UserData.RegisterType<LuaImageComponent>();
            UserData.RegisterType<CYFEngine>();

            // Obsolete Classes
            UserData.RegisterType<Lua.LuaButton>();
            UserData.RegisterType<Lua.LuaButtonController>();
            UserData.RegisterType<Lua.PlayerUtil>();
            UserData.RegisterType<Lua.ArenaUtil>();
            UserData.RegisterType<Lua.StateEditor>();
            UserData.RegisterType<Lua.LuaLifeBar>();
        }

        public static void BoundScriptVariables(ref Script script)
        {
            script.Globals["retroMode"] = GlobalControls.retroMode;

            script.Globals["isModifiedCYF"] = true;
            script.Globals["Asterisk"] = true;
            script.Globals["AsteriskMod"] = false; //v0.5.2.x -> nil  v0.5.3.x -> false  v0.5.4.x -> true
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;
            script.Globals["Language"] = Asterisk.language.ToString();
        }

        public static void BoundScriptFunctions(ref Script script)
        {
            script.Globals["SetAlMightyGlobal"] = (Action<Script, string, DynValue>)SetAlMightySafely;
            script.Globals["GetCurrentAction"] = (Func<string>)GetCurrentAction;
            script.Globals["LayerExists"] = (Func<string, bool>)LayerExists;
        }

        public static void BoundScriptUserDatas(ref Script script)
        {
            if (UnitaleUtil.IsOverworld) return;
            DynValue buttonUtil = UserData.Create(UIController.ActionButtonManager);
            script.Globals.Set("ButtonUtil", buttonUtil);

            GameObjectModifyingSystem goms = GameObjectModifyingSystem.Instance;
            //if (goms == null) goms = new GameObjectModifyingSystem();
            DynValue gms = UserData.Create(goms);
            script.Globals.Set("GameObjectModifyingSystem", gms);
            script.Globals.Set("GMS", gms);
            DynValue engine = UserData.Create(new CYFEngine());
            script.Globals.Set("Engine", engine);

            // Obsolete Classes
            //Lua.LuaButtonController.Initialize();
            //DynValue oldButtonUtil = UserData.Create(new Lua.LuaButtonController());
            //script.Globals.Set("ButtonUtil", oldButtonUtil);
            DynValue obs_playerUtil = UserData.Create(new Lua.PlayerUtil());
            script.Globals.Set("PlayerUtil", obs_playerUtil);
            DynValue obs_arenaUtil = UserData.Create(new Lua.ArenaUtil());
            script.Globals.Set("ArenaUtil", obs_arenaUtil);
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

        /* v0.5.2.9
        public static bool? IsEmptyLayer(string name)
        {
            string canvas = UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/";
            if (name == null || GameObject.Find(canvas + name + "Layer") == null)
                return null;
            return GameObject.Find(canvas + name + "Layer").transform.childCount == 0;
        }
        */

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
