using AsteriskMod.GameobjectModifyingSystem;
using AsteriskMod.GlobalScript;
using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AsteriskMod
{
    /// <summary>Add AsteriskMod's variables, functions and UserDatas to <see cref="LuaScriptBinder"/></summary>
    public class AsteriskLuaScriptBinder
    {
        public static void Initialize()
        {
            UserData.RegisterType<GlobalsScripts>();

            UserData.RegisterType<CYFEngine>();
            UserData.RegisterType<ActionButton>();
            UserData.RegisterType<ButtonManager>();
            UserData.RegisterType<PlayerUtil>();
            UserData.RegisterType<PlayerLifeBar>();
            UserData.RegisterType<LuaStaticTextManager>();
            UserData.RegisterType<UIStaticTextManager>();
            UserData.RegisterType<ArenaUtil>();
            UserData.RegisterType<StateEditor>();
            UserData.RegisterType<Lang>();

            /*
            UserData.RegisterType<GameObjectModifyingSystem>();
            UserData.RegisterType<UnityObject>();
            UserData.RegisterType<LuaImageComponent>();
            */

            UserData.RegisterType<ExtendedUtil.LuaCYFUtil>();
            UserData.RegisterType<ExtendedUtil.LuaStringUtil>();
            UserData.RegisterType<ExtendedUtil.LuaArrayUtil>();
            UserData.RegisterType<ExtendedUtil.LuaVector>();
            //UserData.RegisterType<ExtendedUtil.LuaVectorClass>();

            // Obsolete Classes
            UserData.RegisterType<Lua.LuaButton>();
            UserData.RegisterType<Lua.LuaButtonController>();
            UserData.RegisterType<Lua.PlayerUtil>();
            UserData.RegisterType<Lua.ArenaUtil>();
            UserData.RegisterType<Lua.StateEditor>();
            UserData.RegisterType<Lua.LuaLifeBar>();
        }

        public static void SetModulePaths(Script script)
        {
            string[] pathes = new string[4 + AsteriskEngine.LuaCodeStyle.libPathes.Length];
            //* pathes = new[] { FileLoader.pathToModFile("Lua/?.lua"), FileLoader.pathToDefaultFile("Lua/?.lua"), FileLoader.pathToModFile("Lua/Libraries/?.lua"), FileLoader.pathToDefaultFile("Lua/Libraries/?.lua") };
            pathes[0] = FileLoader.pathToModFile    ("Lua/?.lua");
            pathes[1] = FileLoader.pathToDefaultFile("Lua/?.lua");
            pathes[2] = FileLoader.pathToModFile    ("Lua/Libraries/?.lua");
            pathes[3] = FileLoader.pathToDefaultFile("Lua/Libraries/?.lua");
            if (AsteriskEngine.LuaCodeStyle.libPathes.Length > 0)
            {
                for (var i = 0; i < AsteriskEngine.LuaCodeStyle.libPathes.Length; i++)
                {
                    pathes[i + 4] = FileLoader.pathToModFile(AsteriskEngine.LuaCodeStyle.libPathes[i]);
                }
            }
            ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = pathes;
        }

        public static void BoundScriptVariables(Script script)
        {
            if (AsteriskUtil.IsCYFOverworld) return;

            script.Globals["retroMode"] = GlobalControls.retroMode;

            /*
            script.Globals["isModifiedCYF"] = true;
            script.Globals["Asterisk"] = true;
            script.Globals["AsteriskMod"] = false; //v0.5.2.x -> nil  v0.5.3.x -> false  v0.5.4.x -> true
            script.Globals["AsteriskVersion"] = Asterisk.ModVersion;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;
            script.Globals["Language"] = Asterisk.language.ToString();
            */

            script.Globals["isModifiedCYF"] = true;
            script.Globals["Asterisk"] = true;
            script.Globals["AsteriskMod"] = false; //v0.5.2.x -> nil  v0.5.3.x -> false  v0.5.4.x -> true
            script.Globals["AsteriskVersion"] = AsteriskUtil.ConvertFromModVersionForLua(AsteriskEngine.ModTarget_AsteriskVersion);
            //script.Globals["AsteriskDevEdition"] = Asterisk.DevEdition;
            script.Globals["AsteriskExperiment"] = Asterisk.experimentMode;

            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.TakeNewStepUpdate)
            {
                script.Globals["Language"] = Asterisk.language.ToString();
            }
        }

        private delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

        public static void BoundScriptFunctions(Script script, bool isGlobalScript)
        {
            if (AsteriskUtil.IsCYFOverworld) return;

            /*
            script.Globals["SetAlMightyGlobal"] = (Action<Script, string, DynValue>)SetAlMightySafely;
            script.Globals["GetCurrentAction"] = (Func<string>)GetCurrentAction;
            script.Globals["LayerExists"] = (Func<string, bool>)LayerExists;

            script.Globals["CreateStaticText"] = (Func<Script, string, string, DynValue, int, string, float?, float, LuaStaticTextManager>)CreateStaticText;
            script.Globals["CreateSTText"] = (Func<Script, string, string, DynValue, int, string, float?, float, LuaStaticTextManager>)CreateStaticText;

            script.Globals["SetJapaneseMode"] = (Action<bool>)AsteriskEngine.SetJapaneseMode;
            script.Globals["SetJapaneseStyle"] = (Action<bool>)AsteriskEngine.SetJapaneseMode;
            script.Globals["SetJPMode"] = (Action<bool>)AsteriskEngine.SetJapaneseMode;
            script.Globals["SetJPStyle"] = (Action<bool>)AsteriskEngine.SetJapaneseMode;
            */

            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.QOLUpdate)
            {
                script.Globals["SetAlMightyGlobal"] = (Action<Script, string, DynValue>)SetAlMightySafely;
                script.Globals["GetCurrentAction"] = (Func<string>)GetCurrentAction;
                script.Globals["LayerExists"] = (Func<string, bool>)LayerExists;

                script.Globals["IsEmptyLayer"] = (Func<string, bool?>)IsEmptyLayer;
            }
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.TakeNewStepUpdate)
            {
                script.Globals["CreateStaticText"] = (Func<Script, string, string, DynValue, int, string, float?, float, LuaStaticTextManager>)CreateStaticText;
                script.Globals["AutoRefreshStaticText"] = (Action<bool>)((value)=> { AsteriskEngine.AutoResetStaticText = value; });

                script.Globals["GetLayerList"] = (Func<string[]>)GetLayerList;
            }
            if (isGlobalScript) return;
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.GlobalsScriptsAddition)
            {
                DynValue globalsScripts = UserData.Create(new GlobalsScripts());
                script.Globals.Set("Globals", globalsScripts);
            }
        }

        public static void BoundScriptUserDatas(Script script)
        {
            if (AsteriskUtil.IsCYFOverworld) return;

            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.TakeNewStepUpdate)
            {
                DynValue buttonUtil = UserData.Create(UIController.ActionButtonManager);
                script.Globals.Set("ButtonUtil", buttonUtil);
                DynValue playerUtil = UserData.Create(PlayerUtil.Instance);
                script.Globals.Set("PlayerUtil", playerUtil);
                DynValue arenaUtil = UserData.Create(new ArenaUtil());
                script.Globals.Set("ArenaUtil", arenaUtil);

                DynValue engine = UserData.Create(new CYFEngine());
                script.Globals.Set("Engine", engine);
                DynValue lang = UserData.Create(new Lang());
                script.Globals.Set("Lang", lang);

                /*
                if (AsteriskEngine.LuaCodeStyle.gms)
                {
                    GameObjectModifyingSystem goms = GameObjectModifyingSystem.Instance;
                    //if (goms == null) goms = new GameObjectModifyingSystem();
                    DynValue gms = UserData.Create(goms);
                    script.Globals.Set("GameObjectModifyingSystem", gms);
                    script.Globals.Set("GMS", gms);
                }
                */

                if (AsteriskEngine.LuaCodeStyle.stringUtil)
                {
                    DynValue stringutil = UserData.Create(new ExtendedUtil.LuaStringUtil());
                    script.Globals.Set("StringUtil", stringutil);
                }
                if (AsteriskEngine.LuaCodeStyle.arrayUtil)
                {
                    DynValue arrayutil = UserData.Create(new ExtendedUtil.LuaArrayUtil());
                    script.Globals.Set("ArrayUtil", arrayutil);
                }
                if (AsteriskEngine.LuaCodeStyle.cyfUtil)
                {
                    DynValue cyfutil = UserData.Create(new ExtendedUtil.LuaCYFUtil());
                    script.Globals.Set("Util", cyfutil);
                }
            }
            else
            {
                // Obsolete Classes
                Lua.LuaButtonController.Initialize();
                DynValue oldButtonUtil = UserData.Create(new Lua.LuaButtonController());
                script.Globals.Set("ButtonUtil", oldButtonUtil);
                DynValue obs_playerUtil = UserData.Create(new Lua.PlayerUtil());
                script.Globals.Set("PlayerUtil", obs_playerUtil);
                DynValue obs_arenaUtil = UserData.Create(new Lua.ArenaUtil());
                script.Globals.Set("ArenaUtil", obs_arenaUtil);
            }
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
            if (key == Asterisk.OPTION_EXPERIMENT || key == Asterisk.OPTION_DESC || key == Asterisk.OPTION_LANG || key == Asterisk.OPTION_UIWITHLANG) protect = 1;
            if (key == Asterisk.OPTION_PROTECT || key == Asterisk.OPTION_PROTECT_ERROR || key == Asterisk.OPTION_MODINFO || key == Asterisk.OPTION_MODPACK) protect = 1;
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

        public static LuaStaticTextManager CreateStaticText(Script scr, string font, string text, DynValue position, int textWidth, string layer = "BelowPlayer", float? charspacing = null, float linespacing = 0)
        {
            if (text == null)
                throw new CYFException("CreateStaticText: The text argument must be a simple string.");
            if (position == null || position.Type != DataType.Table || position.Table.Get(1).Type != DataType.Number || position.Table.Get(2).Type != DataType.Number)
                throw new CYFException("CreateStaticText: The position argument must be a non-empty table of two numbers.");

            GameObject go = Object.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/StTxtContainer"));
            LuaStaticTextManager luatm = go.GetComponentInChildren<LuaStaticTextManager>();
            go.GetComponent<RectTransform>().position = new Vector2((float)position.Table.Get(1).Number, (float)position.Table.Get(2).Number);

            /**
            UnitaleUtil.GetChildPerName(go.transform, "BubbleContainer").GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            UnitaleUtil.GetChildPerName(go.transform, "BubbleContainer").GetComponent<RectTransform>().localPosition = new Vector2(-12, 8);
            UnitaleUtil.GetChildPerName(go.transform, "BubbleContainer").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth + 20, 100);     //Used to set the borders
            UnitaleUtil.GetChildPerName(go.transform, "BackHorz").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth + 20, 100 - 20 * 2);   //BackHorz
            UnitaleUtil.GetChildPerName(go.transform, "BackVert").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth - 20, 100);            //BackVert
            UnitaleUtil.GetChildPerName(go.transform, "CenterHorz").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth + 16, 96 - 16 * 2);  //CenterHorz
            UnitaleUtil.GetChildPerName(go.transform, "CenterVert").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth - 16, 96);           //CenterVert
            */
            foreach (ScriptWrapper scrWrap in ScriptWrapper.instances)
            {
                if (scrWrap.script != scr) continue;
                luatm.SetCaller(scrWrap);
                break;
            }
            // Layers don't exist in the overworld, so we don't set it
            if (!UnitaleUtil.IsOverworld || GlobalControls.isInShop)
                luatm.layer = layer;
            else
                luatm.layer = (layer == "BelowPlayer" ? "Default" : layer);
            luatm.textMaxWidth = textWidth;
            //* luatm.bubbleHeight = bubbleHeight;
            //* luatm.ShowBubble();

            // Converts the text argument into a table if it's a simple string 常にstringのみ
            //* text = text.Type == DataType.String ? DynValue.NewTable(scr, text) : text;

            //////////////////////////////////////////
            ///////////  LATE START SETTER  //////////
            //////////////////////////////////////////

            // テキストの最初の行(table[1])(無いが)の最初の通常の文字の前に"[instant]"がある場合、Late Startが無効になる -> 実質そうなのでLateStartは無視。
            //* bool enableLateStart = true;
            //* bool enableLateStart = false;

            /**
            // if we've made it this far, then the text is valid.

            // so, let's scan the first line of text for [instant]
            string firstLine = text.Table.Get(1).String;

            // if [instant] or [instant:allowcommand] is found, check for the earliest match, and whether it is at the beginning
            if (firstLine.IndexOf("[instant]", StringComparison.Ordinal) > -1 || firstLine.IndexOf("[instant:allowcommand]", StringComparison.Ordinal) > -1)
            {
                // determine whether [instant] or [instant:allowcommand] is first
                string testFor = "[instant]";
                if (firstLine.IndexOf("[instant:allowcommand]", StringComparison.Ordinal) > -1 &&
                    firstLine.IndexOf("[instant:allowcommand]", StringComparison.Ordinal) < firstLine.IndexOf("[instant]") || firstLine.IndexOf("[instant]") == -1)
                    testFor = "[instant:allowcommand]";

                // grab all of the text that comes before the matched command
                string precedingText = firstLine.Substring(0, firstLine.IndexOf(testFor, StringComparison.Ordinal));

                // remove all commands other than the matched command from this variable
                while (precedingText.IndexOf('[') > -1)
                {
                    int i = 0;
                    if (UnitaleUtil.ParseCommandInline(precedingText, ref i) == null) break;
                    precedingText = precedingText.Replace(precedingText.Substring(0, i + 1), "");
                }

                // if the length of the remaining string is 0, then disable late start!
                if (precedingText.Length == 0)
                    enableLateStart = false;
            }
            */

            //////////////////////////////////////////
            /////////// INITIAL FONT SETTER //////////
            //////////////////////////////////////////

            // テキストの最初の行の先頭に"[font]"がある場合 -> コマンド無効なので削除可能。
            /**
            if (firstLine.IndexOf("[font:", StringComparison.Ordinal) > -1 && firstLine.Substring(firstLine.IndexOf("[font:", StringComparison.Ordinal)).IndexOf(']') > -1)
            {
                // grab all of the text that comes before the matched command
                string precedingText = firstLine.Substring(0, firstLine.IndexOf("[font:", StringComparison.Ordinal));

                // remove all commands other than the matched command from this variable
                while (precedingText.IndexOf('[') > -1)
                {
                    int i = 0;
                    if (UnitaleUtil.ParseCommandInline(precedingText, ref i) == null) break;
                    precedingText = precedingText.Replace(precedingText.Substring(0, i + 1), "");
                }

                // if the length of the remaining string is 0, then set the font!
                if (precedingText.Length == 0)
                {
                    int startCommand = firstLine.IndexOf("[font:", StringComparison.Ordinal);
                    string command = UnitaleUtil.ParseCommandInline(precedingText, ref startCommand);
                    if (command != null)
                    {
                        string fontPartOne = command.Substring(6);
                        string fontPartTwo = fontPartOne.Substring(0, fontPartOne.IndexOf("]", StringComparison.Ordinal));
                        UnderFont font = SpriteFontRegistry.Get(fontPartTwo);
                        if (font == null)
                            throw new CYFException("The font \"" + fontPartTwo + "\" doesn't exist.\nYou should check if you made a typo, or if the font really is in your mod.");
                        luatm.SetFont(font, true);
                        luatm.UpdateBubble();
                    }
                    else luatm.ResetFont();
                }
                else luatm.ResetFont();
            }
            else */ luatm.ResetFont();

            //* if (enableLateStart)
                //* luatm.lateStartWaiting = true;
            if (!string.IsNullOrEmpty(font))
                luatm.SetFont(font);
            else
                luatm.ResetFont();
            luatm.SetCharSpacing(charspacing);
            luatm.SetLineSpacing(linespacing);
            luatm.SetText(text);
            /**
            if (!enableLateStart) return luatm;
            luatm.DestroyChars();
            luatm.LateStart();
            */
            return luatm;
        }

        // <summary>
        // Recall the functions that ware called before initialization.<br/>
        // for <see cref="PlayerUtil"/>
        // </summary>
        /*
        public static void LateInitialize()
        {
            //UIStats.instance.Request();
            //PlayerUIManager.Instance.Request();
        }
        */


        [ToDo("Deletes when GMS is Implemented.")]
        public static bool? IsEmptyLayer(string name)
        {
            string canvas = UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/";
            if (name == null || GameObject.Find(canvas + name + "Layer") == null)
                return null;
            return GameObject.Find(canvas + name + "Layer").transform.childCount == 0;
        }

        [ToDo("Deletes when GMS is Implemented.")]
        public static string[] GetLayerList()
        {
            Transform canvas = GameobjectModifyingSystemMain.Instance.Canvas.transform;
            List<string> layers = new List<string>();
            for (var i = 0; i < canvas.childCount; i++)
            {
                string name = canvas.GetChild(i).gameObject.name;
                if (name.EndsWith("Layer")) layers.Add(name.Substring(0, name.Length - 5));
            }
            return layers.ToArray();
        }
    }
}
