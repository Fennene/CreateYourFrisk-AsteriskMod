using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AsteriskMod.GlobalScript
{
    public class GlobalsScripts
    {
        //public class ScriptLoaderOption { }

        private static Dictionary<string, DynValue> Scripts;

        private static List<string> GetFiles(string rootDirFullPath, string relativeDirPath)
        {
            List<string> relativeFileNames = new List<string>();

            string realDirPath = rootDirFullPath;
            if (!relativeDirPath.IsNullOrWhiteSpace()) realDirPath = Path.Combine(rootDirFullPath, relativeDirPath);

            string[] files = Directory.GetFiles(realDirPath, "*.lua", SearchOption.TopDirectoryOnly);
            for (var i = 0; i < files.Length; i++)
            {
                string path = Path.GetFileNameWithoutExtension(files[i]);
                if (!relativeDirPath.IsNullOrWhiteSpace()) path = Path.Combine(relativeDirPath, path).Replace('\\', '/');
                relativeFileNames.Add(path);
            }

            string[] directories = Directory.GetDirectories(realDirPath);
            for (var i = 0; i < directories.Length; i++)
            {
                string newRelativePath = Path.GetFileName(directories[i]);
                if (!relativeDirPath.IsNullOrWhiteSpace()) newRelativePath = Path.Combine(relativeDirPath, newRelativePath);
                List<string> dirFiles = GetFiles(rootDirFullPath, newRelativePath);
                for (var j = 0; j < dirFiles.Count; j++)
                {
                    relativeFileNames.Add(dirFiles[j]);
                }
            }

            return relativeFileNames;
        }

        internal static void Load()
        {
            //string globalsDirPath = FileLoader.pathToModFile("Lua/Globals");

            string scriptsDirPath = FileLoader.pathToModFile("Lua/Globals/Scripts");
            if (!Directory.Exists(scriptsDirPath))
            {
                Debug.Log("AsteriskMod.GlobalScripts - Globals/Scripts luaFiles\nNo Lua Files.\n");

                Scripts = new Dictionary<string, DynValue>(0);
            }
            else
            {
                string[] scriptsFiles = GetFiles(scriptsDirPath, "").ToArray();

                string logText = "AsteriskMod.GlobalScripts - Globals/Scripts luaFiles";
                for (var i = 0; i < scriptsFiles.Length; i++)
                {
                    logText += "\n" + scriptsFiles[i] + ".lua";
                }
                Debug.Log(logText + "\n");

                Scripts = new Dictionary<string, DynValue>(scriptsFiles.Length);

                for (var i = 0; i < scriptsFiles.Length; i++)
                {
                    try
                    {
                        ScriptWrapper scriptWrapper = new ScriptWrapper(true) { scriptname = scriptsFiles[i] };
                        try
                        {
                            scriptWrapper.DoString(ScriptRegistry.Get(ScriptRegistry.GLOBAL_SCRIPT_PREFIX + scriptsFiles[i]));
                        }
                        catch (InterpreterException ex)
                        {
                            UnitaleUtil.DisplayLuaError("Globals/Scripts/" + scriptsFiles[i] + ".lua", UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            if (!ScriptRegistry.dict.ContainsKey(ScriptRegistry.GLOBAL_SCRIPT_PREFIX + scriptsFiles[i]))
                            {
                                UnitaleUtil.DisplayLuaError(StaticInits.ENCOUNTER, "<Internal Error> The Global Script " + scriptsFiles[i] + ".lua isn't found.");
                            }
                            else
                            {
                                UnitaleUtil.DisplayLuaError("<UNKNOWN LOCATION>", ex.Message + "\n\n" + ex.StackTrace);
                            }
                        }
                        scriptWrapper.script.Globals["State"] = (Action<Script, string>)UIController.SwitchStateOnString;
                        Scripts.Add(scriptsFiles[i], UserData.Create(scriptWrapper));
                    }
                    catch (InterpreterException ex)
                    {
                        UnitaleUtil.DisplayLuaError("Globals/Scripts/" + scriptsFiles[i] + ".lua", UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message);
                    }
                }
            }
        }

        public DynValue this[string scriptName]
        {
            get
            {
                if (scriptName.IsNullOrWhiteSpace()) throw new CYFException("Globals' index shouldn't be nil, empty string or white space string.");
                if (!Scripts.ContainsKey(scriptName)) throw new CYFException("Global Script Globals/Scripts/" + scriptName + ".lua is not found.");
                return Scripts[scriptName];
            }
        }

        public DynValue GetScript(string scriptName) { return this[scriptName]; }

        public DynValue GetVar(string scriptName, string key) { return ((ScriptWrapper)this[scriptName].UserData.Object).GetVar(null, key); }
        public void SetVar(string scriptName, string key, DynValue value) { ((ScriptWrapper)this[scriptName].UserData.Object).SetVar(key, value); }
        public void Call(string scriptName, string function, params DynValue[] args) { ((ScriptWrapper)this[scriptName].UserData.Object).Call(function, args); }
    }
}
