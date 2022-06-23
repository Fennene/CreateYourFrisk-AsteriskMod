using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimStaticTextSimMenu : MonoBehaviour
    {
        internal static SimStaticTextSimMenu Instance;

        private Button BackButton;

        private void Awake()
        {
            BackButton = transform.Find("MenuNameLabel").Find("BackButton").GetComponent<Button>();

            StaticTexts = new FakeLuaStaticTextManager[MAX_STATIC_TEXT_OBJECT];

            Instance = this;

            SimInstance.STTargetUI.Awake(transform.Find("ObjectManagerWindow").Find("View"));
            SimInstance.STControllerUI.Awake(transform.Find("ObjControllerWindow").Find("View"));
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.StaticTextSim, SimMenuWindowManager.DisplayingSimMenu.Main);
            });

            SimInstance.STTargetUI.Start();
            SimInstance.STControllerUI.Start();
        }

        internal FakeLuaStaticTextManager CreateStaticText(string font, string text, float[] position, int textWidth, string layer = "BelowPlayer", float? charspacing = null, float linespacing = 0)
        {
            if (text == null)
                throw new CYFException("CreateStaticText: The text argument must be a simple string.");
            //* if (position == null || position.Type != DataType.Table || position.Table.Get(1).Type != DataType.Number || position.Table.Get(2).Type != DataType.Number)
            //*     throw new CYFException("CreateStaticText: The position argument must be a non-empty table of two numbers.");

            GameObject go = Object.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/FakeStTxtContainer"));
            FakeLuaStaticTextManager luatm = go.GetComponentInChildren<FakeLuaStaticTextManager>();
            go.GetComponent<RectTransform>().position = new Vector2(position[0], position[1]);

            luatm.IsUI = false;
            /*
            UnitaleUtil.GetChildPerName(go.transform, "BubbleContainer").GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            UnitaleUtil.GetChildPerName(go.transform, "BubbleContainer").GetComponent<RectTransform>().localPosition = new Vector2(-12, 8);
            UnitaleUtil.GetChildPerName(go.transform, "BubbleContainer").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth + 20, 100);     //Used to set the borders
            UnitaleUtil.GetChildPerName(go.transform, "BackHorz").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth + 20, 100 - 20 * 2);   //BackHorz
            UnitaleUtil.GetChildPerName(go.transform, "BackVert").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth - 20, 100);            //BackVert
            UnitaleUtil.GetChildPerName(go.transform, "CenterHorz").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth + 16, 96 - 16 * 2);  //CenterHorz
            UnitaleUtil.GetChildPerName(go.transform, "CenterVert").GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth - 16, 96);           //CenterVert
            */
            /**
            foreach (ScriptWrapper scrWrap in ScriptWrapper.instances)
            {
                if (scrWrap.script != scr) continue;
                luatm.SetCaller(scrWrap);
                break;
            }
            */
            // Layers don't exist in the overworld, so we don't set it
            //* if (!UnitaleUtil.IsOverworld || GlobalControls.isInShop)
            luatm.layer = layer;
            //* else
            //*    luatm.layer = (layer == "BelowPlayer" ? "Default" : layer);
            luatm.textMaxWidth = textWidth;
            // luatm.bubbleHeight = bubbleHeight;
            // luatm.ShowBubble();

            // Converts the text argument into a table if it's a simple string 常にstringのみ
            // text = text.Type == DataType.String ? DynValue.NewTable(scr, text) : text;

            //////////////////////////////////////////
            ///////////  LATE START SETTER  //////////
            //////////////////////////////////////////

            // テキストの最初の行(table[1])(無いが)の最初の通常の文字の前に"[instant]"がある場合、Late Startが無効になる -> 実質そうなのでLateStartは無視。
            // bool enableLateStart = true;
            // bool enableLateStart = false;

            /*
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
            /*
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
            else */
            luatm.ResetFont();

            // if (enableLateStart)
            // luatm.lateStartWaiting = true;
            if (!string.IsNullOrEmpty(font))
            {
                luatm.SetFont(font);
                luatm.font = font;
            }
            else
            {
                luatm.ResetFont();
            }
            luatm.SetCharSpacing(charspacing);
            luatm.SetLineSpacing(linespacing);
            luatm.SetText(text);
            /*
            if (!enableLateStart) return luatm;
            luatm.DestroyChars();
            luatm.LateStart();
            */
            return luatm;
        }

        private readonly byte MAX_STATIC_TEXT_OBJECT = 16;

        internal FakeLuaStaticTextManager[] StaticTexts;

        internal bool CanCreateStaticText { get { return StaticTexts[MAX_STATIC_TEXT_OBJECT - 1] == null; } }

        private int GetEmptyStaticTextIndex()
        {
            for (var i = 0; i < MAX_STATIC_TEXT_OBJECT; i++)
            {
                if (StaticTexts[i] == null) return i;
            }
            return -1;
        }

        internal int StaticTextLength
        {
            get
            {
                int _ = GetEmptyStaticTextIndex();
                if (_ == -1) return MAX_STATIC_TEXT_OBJECT;
                return _;
            }
        }

        internal bool AddStaticText(string text, string layer = "BelowArena")
        {
            int index = GetEmptyStaticTextIndex();
            if (index == -1) return false;
            StaticTexts[index] = CreateStaticText("uidialog", text, new[]{320f,240f}, 16383, layer);
            SimInstance.STTargetUI.UpdateTargetDropDown();
            return true;
        }

        internal void RemoveStaticText(int index)
        {
            if (index < 0) return;
            if (index >= MAX_STATIC_TEXT_OBJECT) return;
            StaticTexts[index].DestroyText();
            StaticTexts[index] = null;
            for (var i = index; i < MAX_STATIC_TEXT_OBJECT - 1; i++)
            {
                StaticTexts[i] = StaticTexts[i + 1];
            }
            SimInstance.STTargetUI.UpdateTargetDropDown(true);
        }

        internal FakeLuaStaticTextManager Target
        {
            get
            {
                if (SimInstance.STTargetUI.TargetIndex < 0) return null;
                if (SimInstance.STTargetUI.TargetIndex >= StaticTextLength) return null;
                return StaticTexts[SimInstance.STTargetUI.TargetIndex];
            }
        }

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
