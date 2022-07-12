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
        private readonly string selfName;

        public GlobalsScripts(string scriptSelfName) { selfName = scriptSelfName; }

        //public class ScriptLoaderOption { }

        private static Dictionary<string, ScriptWrapper> Scripts;
        private static Dictionary<string, DynValue> ScriptDatas;

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

                Scripts = new Dictionary<string, ScriptWrapper>(0);
                ScriptDatas = new Dictionary<string, DynValue>(0);
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

                Scripts = new Dictionary<string, ScriptWrapper>(scriptsFiles.Length);
                ScriptDatas = new Dictionary<string, DynValue>(scriptsFiles.Length);

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
                        DynValue globals = UserData.Create(new GlobalsScripts(scriptsFiles[i]));
                        scriptWrapper.script.Globals.Set("Globals", globals);

                        Scripts.Add(scriptsFiles[i], scriptWrapper);
                        ScriptDatas.Add(scriptsFiles[i], UserData.Create(scriptWrapper));
                    }
                    catch (InterpreterException ex)
                    {
                        UnitaleUtil.DisplayLuaError("Globals/Scripts/" + scriptsFiles[i] + ".lua", UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message);
                    }
                }
            }
        }

        public bool Exists(string scriptName)
        {
            if (scriptName.IsNullOrWhiteSpace()) throw new CYFException("Globals' index shouldn't be nil, empty string or white space string.");
            return Scripts.ContainsKey(scriptName);
        }

        public string[] GetScriptList()
        {
            string[] scriptNames = new string[Scripts.Keys.Count];
            int index = 0;
            foreach (string scriptName in Scripts.Keys)
            {
                scriptNames[index] = scriptName;
                index++;
            }
            return scriptNames;
        }

        private void CheckExists(string scriptName)
        {
            if (!Exists(scriptName)) throw new CYFException("Global Script Globals/Scripts/" + scriptName + ".lua is not found.");
            if (scriptName == selfName) throw new CYFException("Attempted to access script itself.");
        }

        public DynValue this[string scriptName]
        {
            get
            {
                CheckExists(scriptName);
                return ScriptDatas[scriptName];
            }
        }
        public DynValue GetScript(string scriptName) { return this[scriptName]; }

        private ScriptWrapper GetScriptWrapper(string scriptName)
        {
            CheckExists(scriptName);
            return Scripts[scriptName];
        }
        public DynValue GetVar(string scriptName, string key) { return GetScriptWrapper(scriptName).GetVar(null, key); }
        public DynValue Get(string scriptName, string key) { return GetVar(scriptName, key); }
        public void SetVar(string scriptName, string key, DynValue value) { GetScriptWrapper(scriptName).SetVar(key, value); }
        public void Set(string scriptName, string key, DynValue value) { SetVar(scriptName, key, value); }
        public void Call(string scriptName, string function, params DynValue[] args) { GetScriptWrapper(scriptName).Call(function, args); }

        public Table GetRawScript(string scriptName)
        {
            Asterisk.RequireExperimentalFeature("Globals.GetRawScript");
            CheckExists(scriptName);
            return Scripts[scriptName].script.Globals;
        }
    }
}
