using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = System.Diagnostics.Debug;

namespace AsteriskMod
{
    /// <summary>This is the alternative class for the text that is not needed any effect, bubble and command and is instant 1 line text (not table of string).</summary>
    public class StaticTextManager : MonoBehaviour
    {
        // SetFont, SetText
        internal Image[] letterReferences;
        internal Vector2[] letterPositions;

        protected UnderFont default_charset;
        public static string[] commandList = new string[] { "color", "alpha", "charspacing", "linespacing", "instant", "font" };
        public int currentLine;
        [MoonSharpHidden] public int _textMaxWidth;
        private int currentCharacter;
        public int currentReferenceCharacter;
        private RectTransform self;
        [MoonSharpHidden] public Vector2 offset;
        private bool offsetSet;
        private float currentX;
        //private float _currentY;
        private float currentY; /* {
        get { return _currentY; }
        set {
            if (GetType() == typeof(LuaStaticTextManager))
                print("Change currentY value: " + _currentY + " => " + value);
            _currentY = value;
        }
    }*/

        // Variables that have to do with "[instant]"
        private bool instantActive; // Will be true if "[instant]" or "[instant:allowcommand]" have been activated
        private bool instantCommand; // Will be true only if "[instant:allowcommand]" has been activated

        private bool skipFromPlayer;
        internal float hSpacing = 3;
        internal float vSpacing;
        private GameObject textframe;
        // private int letterSpeed = 1;

        protected Color currentColor = Color.white;
        private bool colorSet;
        protected Color defaultColor = Color.white;
        //private Color defaultColor = Color.white;

        private float letterTimer;
        private float timePerLetter;
        private const float singleFrameTiming = 1.0f / 20;

        [MoonSharpHidden] public ScriptWrapper caller;

        [MoonSharpHidden] public UnderFont Charset { get; protected set; }
        [MoonSharpHidden] public InstantTextMessage text;
        //public bool overworld;
        [MoonSharpHidden] public bool skipNowIfBlocked = false;
        internal bool noSkip1stFrame = true;

        [MoonSharpHidden] public bool lateStartWaiting = false; // Lua text objects will use a late start
        public StaticTextManager()
        {
            currentLine = 0;
            _textMaxWidth = 0;
            currentCharacter = 0;
            currentReferenceCharacter = 0;
            instantActive = true;
            instantCommand = false;
            skipFromPlayer = false;
            vSpacing = 0;
            colorSet = false;
            letterTimer = 0.0f;
            text = null;
        }

        [MoonSharpHidden] public void SetCaller(ScriptWrapper s) { caller = s; }

        public void SetFont(UnderFont font, bool firstTime = false)
        {
            Charset = font;
            if (default_charset == null)
                default_charset = font;
            vSpacing = 0;
            hSpacing = font.CharSpacing;
            defaultColor = font.DefaultColor;
            if (GetType() == typeof(LuaStaticTextManager))
            {
                if (((LuaStaticTextManager)this).hasColorBeenSet) defaultColor = ((LuaStaticTextManager)this)._color;
                if (((LuaStaticTextManager)this).hasAlphaBeenSet) defaultColor.a = ((LuaStaticTextManager)this).alpha;
            }
            currentColor = defaultColor;
        }

        [MoonSharpHidden] public void SetHorizontalSpacing(float spacing = 3) { hSpacing = spacing; }
        [MoonSharpHidden] public void SetVerticalSpacing(float spacing = 0) { vSpacing = spacing; }

        [MoonSharpHidden]
        public void ResetFont()
        {
            if (Charset == null || default_charset == null)
                if (GetType() == typeof(LuaStaticTextManager))
                    ((LuaStaticTextManager)this).SetFont(SpriteFontRegistry.UI_MONSTERTEXT_NAME);
                else
                    SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_DEFAULT_NAME), true);
            Charset = default_charset;
            Debug.Assert(default_charset != null, "default_charset != null");
            defaultColor = default_charset.DefaultColor;
        }

        protected virtual void Awake()
        {
            self = gameObject.GetComponent<RectTransform>();
            // SetFont(SpriteFontRegistry.F_UI_DIALOGFONT);
            timePerLetter = singleFrameTiming;

            if (!UnitaleUtil.IsOverworld || !GameObject.Find("textframe_border_outer")) return;
            textframe = GameObject.Find("textframe_border_outer");
        }

        private void Start()
        {
            // ResetFont();
            // SetText("the quick brown fox jumps over\rthe lazy dog.\nTHE QUICK BROWN FOX JUMPS OVER\rTHE LAZY DOG.\nJerry.", true, true);
            // SetText(new TextMessage("Here comes Napstablook.", true, false));
            // SetText(new TextMessage(new string[] { "Check", "Compliment", "Ignore", "Steal", "trow temy", "Jerry" }, false));
        }

        public void SetText(InstantTextMessage instantText)
        {
            ResetFont();
            text = instantText;
            currentLine = 0;
            ShowLine(0);
        }

        [MoonSharpHidden]
        public void SetTextQueueAfterValue(int BeginText)
        {
            ResetFont();
            currentLine = BeginText;
            ShowLine(BeginText);
        }

        [MoonSharpHidden]
        public void ResetCurrentCharacter()
        {
            currentCharacter = 0;
            currentReferenceCharacter = 0;
        }

        [MoonSharpHidden]
        public void SetOffset(float xOff, float yOff)
        {
            offset = new Vector2(xOff, yOff);
            offsetSet = true;
        }

        [Obsolete]
        public bool LineComplete()
        {
            if (letterReferences == null)
                return false;
            return (instantActive || currentCharacter == letterReferences.Length);
        }

        [MoonSharpHidden, Obsolete]
        public bool AllLinesComplete()
        {
            return text == null || currentLine == 0 && LineComplete();
        }

        protected void ShowLine(int line, bool forceNoAutoLineBreak = false)
        {
            /*if (overworld) {
                if (mugshotsPath != null)
                    if (mugshotsPath[line] != null || mugshotsPath[line] != "")
                        mugshot.sprite = SpriteRegistry.GetMugshot(mugshotsPath[line]);
                    else
                        mugshot.sprite = null;

                    if (mugshot.sprite == null) {
                        mugshot.color = new Color(mugshot.color.r, mugshot.color.g, mugshot.color.b, 0);
                        self.localPosition = new Vector3(-267, self.localPosition.y, self.localPosition.z);
                    } else {
                        mugshot.color = new Color(mugshot.color.r, mugshot.color.g, mugshot.color.b, 1);
                        self.localPosition = new Vector3(-150, self.localPosition.y, self.localPosition.z);
                    }
            }*/

            if (text == null)
            /*
            if (textQueue == null) return;
            if (line >= textQueue.Length) return;
            if (textQueue[line] == null) return;
            */

            if (!offsetSet)
                SetOffset(0, 0);
            if (GetType() != typeof(LuaStaticTextManager))
                ResetFont();
            currentColor = defaultColor;
            colorSet = false;
            instantCommand = false;
            skipFromPlayer = false;

            timePerLetter = singleFrameTiming;
            letterTimer = 0.0f;
            DestroyChars();
            currentLine = line;
            currentX = self.position.x + offset.x;
            currentY = self.position.y + offset.y;
            // allow Game Over fonts to enjoy the fixed text positioning, too!
            if (GetType() != typeof(LuaStaticTextManager) && gameObject.name != "TextParent" && gameObject.name != "ReviveText")
                currentY -= Charset.LineSpacing;
            /*if (GetType() == typeof(LuaStaticTextManager))
                            print("currentY from ShowLine (" + textQueue[currentLine].Text + ") = " + self.position.y + " + " + offset.y + " - " + Charset.LineSpacing + " = " + currentY);*/
            currentCharacter = 0;
            currentReferenceCharacter = 0;
            instantActive = true;
            SpawnText(forceNoAutoLineBreak);
            //if (!overworld)
            //    UIController.instance.encounter.CallOnSelfOrChildren("AfterText");
        }

        [MoonSharpHidden]
        public void SetTextFrameAlpha(float a)
        {
            Image[] imagesChild;
            Image[] images;

            if (UnitaleUtil.IsOverworld)
            {
                imagesChild = textframe.GetComponentsInChildren<Image>();
                images = new Image[imagesChild.Length + 1];
                images[0] = textframe.GetComponent<Image>();
            }
            else
            {
                imagesChild = GameObject.Find("arena_border_outer").GetComponentsInChildren<Image>();
                images = new Image[imagesChild.Length + 1];
                images[0] = GameObject.Find("arena_border_outer").GetComponent<Image>();
            }

            imagesChild.CopyTo(images, 1);

            foreach (Image img in images)
                img.color = new Color(img.color.r, img.color.g, img.color.b, a);
        }

        [MoonSharpHidden] public void NextLineText() { ShowLine(++currentLine); }

        [MoonSharpHidden]
        public void SkipText()
        {
            if (noSkip1stFrame) return;
            while (currentCharacter < letterReferences.Length)
            {
                if (letterReferences[currentCharacter] != null && Charset.Letters.ContainsKey(text.Text[currentCharacter]))
                //if (letterReferences[currentCharacter] != null && Charset.Letters.ContainsKey(textQueue[currentLine].Text[currentCharacter]))
                {
                    letterReferences[currentCharacter].enabled = true;
                    currentReferenceCharacter++;
                }

                currentCharacter++;
            }
        }

        [MoonSharpHidden]
        public void DoSkipFromPlayer()
        {
            skipFromPlayer = true;

            if ((GlobalControls.isInFight && EnemyEncounter.script.GetVar("playerskipdocommand").Boolean) || !GlobalControls.isInFight)
                instantCommand = true;

            if (!GlobalControls.retroMode)
                InUpdateControlCommand(DynValue.NewString("instant"), currentCharacter);
            else
                SkipText();
        }

        [Obsolete]
        public virtual void SkipLine()
        {
            if (noSkip1stFrame) return;
            while (currentCharacter < letterReferences.Length)
            {
                if (letterReferences[currentCharacter] != null && Charset.Letters.ContainsKey(text.Text[currentCharacter]))
                //if (letterReferences[currentCharacter] != null && Charset.Letters.ContainsKey(textQueue[currentLine].Text[currentCharacter]))
                {
                    letterReferences[currentCharacter].enabled = true;
                    letterReferences[currentCharacter].GetComponent<Letter>().effect = null;
                    currentReferenceCharacter++;
                    switch (text.Text[currentCharacter])
                    //switch (textQueue[currentLine].Text[currentCharacter])
                    {
                        case '\t':
                            {
                                float indice = currentX / 320f;
                                if (currentX - (indice * 320f) < 36)
                                    indice--;
                                currentX = indice * 320 + 356;
                                break;
                            }
                        case '\n':
                            currentX = self.position.x + offset.x;
                            currentY = currentY - vSpacing - Charset.LineSpacing;
                            currentCharacter++;
                            return;
                    }
                }
                currentCharacter++;
            }
        }

        [MoonSharpHidden]
        public void DestroyChars()
        {
            foreach (Transform child in gameObject.transform)
                Destroy(child.gameObject);
        }

        private void SpawnTextSpaceTest(int i, string currentText, out string currentText2)
        {
            currentText2 = currentText;
            //bool decorated = textQueue[currentLine].Decorated;
            //float decorationLength = decorated ? AsteriskUtil.CalcTextWidth(this, 0, 1, true, true) : 0;

            // Gets the first character of the line and the last character after the current space
            int finalIndex = i + 1, beginIndex = i;

            for (; beginIndex > 0; beginIndex--)
                if (currentText[beginIndex] == '\n' || currentText[beginIndex] == '\r')
                    break;
            for (; finalIndex < currentText.Length - 1; finalIndex++)
                if (currentText[finalIndex] == ' ' || currentText[finalIndex] == '\n' || currentText[finalIndex] == '\r')
                    break;

            if (currentText[beginIndex] == '\n' || currentText[beginIndex] == '\r') beginIndex++;
            if (currentText[finalIndex] == '\n' || currentText[finalIndex] == ' ' || currentText[finalIndex] == '\r') finalIndex--;

            if (AsteriskUtil.CalcTextWidth(this, beginIndex, finalIndex, true) > _textMaxWidth && _textMaxWidth > 0)
            {
                // If the line's too long, do something!
                int wordBeginIndex = currentText2[i] == ' ' ? i + 1 : i;
                if (AsteriskUtil.CalcTextWidth(this, wordBeginIndex, finalIndex) > _textMaxWidth)// - decorationLength)
                {
                    // Word is taking the entire line
                    for (int currentIndex = wordBeginIndex; currentIndex <= finalIndex; currentIndex++)
                    {
                        if (!(AsteriskUtil.CalcTextWidth(this, beginIndex, currentIndex) > _textMaxWidth)) continue;
                        currentText2 = currentText2.Substring(0, currentIndex) + "\n" + currentText2.Substring(currentIndex, currentText2.Length - currentIndex);
                        //currentText2 = currentText2.Substring(0, currentIndex) + "\n" + (decorated ? "  " : "") + currentText2.Substring(currentIndex, currentText2.Length - currentIndex);
                        text.Text = currentText2;
                        //textQueue[currentLine].Text = currentText2;
                        finalIndex += 1;
                        //finalIndex += decorated ? 3 : 1;
                        beginIndex = currentIndex;
                    }
                }
                else
                    // Line is too long
                    currentText2 = currentText2.Substring(0, wordBeginIndex - 1) + "\n" + currentText2.Substring(wordBeginIndex, currentText.Length - wordBeginIndex);
                    //currentText2 = currentText2.Substring(0, wordBeginIndex - 1) + "\n" + (decorated ? "  " : "") + currentText2.Substring(wordBeginIndex, currentText.Length - wordBeginIndex);

                Array.Resize(ref letterReferences, currentText2.Length);
                Array.Resize(ref letterPositions, currentText2.Length);
            }
            text.Text = currentText2;
            //textQueue[currentLine].Text = currentText2;
        }

        private void CreateLetter(string currentText, int index, bool insert = false)
        {
            if (insert)
                for (int i = letterReferences.Length - 2; i >= index; i--)
                {
                    letterPositions[i + 1] = letterPositions[i];
                    letterReferences[i + 1] = letterReferences[i];
                }

            GameObject singleLtr = Instantiate(SpriteFontRegistry.LETTER_OBJECT);
            RectTransform ltrRect = singleLtr.GetComponent<RectTransform>();

            bool isLua = GetType() == typeof(LuaStaticTextManager);
            LuaStaticTextManager luaThis = isLua ? ((LuaStaticTextManager)this) : null;

            ltrRect.localScale = new Vector3((isLua ? luaThis.xscale : 1f) + 0.001f, (isLua ? luaThis.yscale : 1f) + 0.001f, ltrRect.localScale.z);

            Image ltrImg = singleLtr.GetComponent<Image>();
            ltrRect.SetParent(gameObject.transform);
            ltrImg.sprite = Charset.Letters[currentText[index]];

            letterReferences[index] = ltrImg;

            MoveLetter(currentText, index, ltrRect);

            ltrImg.SetNativeSize();
            if (isLua)
            {
                Color luaColor = luaThis._color;
                if (!colorSet)
                {
                    ltrImg.color = luaThis.hasColorBeenSet ? luaColor : currentColor;
                    if (luaThis.hasAlphaBeenSet) ltrImg.color = new Color(ltrImg.color.r, ltrImg.color.g, ltrImg.color.b, luaColor.a);
                }
                else ltrImg.color = currentColor;
            }
            else ltrImg.color = currentColor;
            ltrImg.GetComponent<Letter>().colorFromText = currentColor;
            ltrImg.enabled = true;
            //ltrImg.enabled = textQueue[currentLine].ShowImmediate || (GlobalControls.retroMode && instantActive);
        }

        private void MoveLetter(string currentText, int index, RectTransform ltrRect)
        {
            if (GetType() == typeof(LuaStaticTextManager) || gameObject.name == "TextParent" || gameObject.name == "ReviveText")
            {
                // Allow Game Over fonts to enjoy the fixed text positioning, too!
                float diff = (Charset.Letters[currentText[index]].border.w - Charset.Letters[currentText[index]].border.y);
                ltrRect.localPosition = new Vector3(currentX - self.position.x - .9f, (currentY - self.position.y) + diff + .1f, 0);
            }
            else
                // Keep what we already have for all text boxes that are not Text Objects in an encounter
                ltrRect.position = new Vector3(currentX + .1f, (currentY + Charset.Letters[currentText[index]].border.w - Charset.Letters[currentText[index]].border.y + 2) + .1f, 0);

            letterPositions[index] = ltrRect.anchoredPosition;
        }

        private void SpawnText(bool forceNoAutoLineBreak = false)
        {
            noSkip1stFrame = true;
            string currentText = text.Text;
            //string currentText = textQueue[currentLine].Text;
            letterReferences = new Image[currentText.Length];
            letterPositions = new Vector2[currentText.Length];
            if (currentText.Length > 1 && !forceNoAutoLineBreak)
                if (!GlobalControls.isInFight || EnemyEncounter.script.GetVar("autolinebreak").Boolean || GetType() == typeof(LuaStaticTextManager))
                    SpawnTextSpaceTest(0, currentText, out currentText);

            // Work-around for [instant] and [instant:allowcommand] at the beginning of a line of text
            bool skipImmediate = false;
            string skipCommand = "";

            for (int i = 0; i < currentText.Length; i++)
            {
                switch (currentText[i])
                {
                    case '[':
                        int currentChar = i;
                        string command = UnitaleUtil.ParseCommandInline(currentText, ref i);
                        if (command == null || lateStartWaiting)
                            i = currentChar;
                        else
                        {
                            // Work-around for [noskip], [instant] and [instant:allowcommand]
                            if (!GlobalControls.retroMode)
                            {
                                // The goal of this is to allow for commands executed "just before" [instant] on the first frame
                                // Example: "[func:test][instant]..."

                                // Special case for "[noskip]", "[instant]" and "[instant:allowcommand]"
                                if (command == "noskip" || command == "instant" || command == "instant:allowcommand")
                                {
                                    // Copy all text before the command
                                    string precedingText = currentText.Substring(0, i - (command.Length + 1));

                                    // Remove all commands, store them for later if using instant
                                    List<string> commands = command == "noskip" ? null : new List<string>();

                                    while (precedingText.IndexOf('[') > -1)
                                    {
                                        int j = precedingText.IndexOf('['), k = j;
                                        if (UnitaleUtil.ParseCommandInline(precedingText, ref k) == null) break;
                                        if (commands != null)
                                            commands.Add(precedingText.Substring(j + 1, (k - j) - 1));
                                        precedingText = precedingText.Replace(precedingText.Substring(j, (k - j) + 1), "");
                                    }

                                    // Confirm that our command is at the beginning!
                                    if (precedingText.Length == 0)
                                        if (command == "noskip")
                                            PreCreateControlCommand(command);
                                        else
                                        {
                                            // Execute all commands that came before [instant] through InUpdateControlCommand
                                            foreach (string cmd in commands)
                                                InUpdateControlCommand(DynValue.NewString(cmd));

                                            skipImmediate = true;
                                            skipCommand = command;
                                            // InUpdateControlCommand(DynValue.NewString(command), i);
                                        }
                                }
                                else if (command.Length < 7 || command.Substring(0, 7) != "instant")
                                    PreCreateControlCommand(command);
                            }
                            else
                                PreCreateControlCommand(command);
                            continue;
                        }
                        break;
                    case '\n':
                        currentX = self.position.x + offset.x;
                        currentY = currentY - vSpacing - Charset.LineSpacing;
                        break;
                    case '\t':
                        currentX = !GlobalControls.isInFight ? (356 + Camera.main.transform.position.x - 320) : 356; // HACK: bad tab usage
                        break;
                    case ' ':
                        if (i + 1 == currentText.Length || currentText[i + 1] == ' ' || forceNoAutoLineBreak)
                            break;
                        if (!GlobalControls.isInFight || EnemyEncounter.script.GetVar("autolinebreak").Boolean || GetType() == typeof(LuaStaticTextManager))
                        {
                            SpawnTextSpaceTest(i, currentText, out currentText);
                            if (currentText[i] != ' ')
                            {
                                i--;
                                continue;
                            }
                        }
                        break;
                }

                if (!Charset.Letters.ContainsKey(currentText[i]))
                    continue;

                CreateLetter(currentText, i);
                currentX += letterReferences[i].gameObject.GetComponent<RectTransform>().rect.width + hSpacing; // TODO remove hardcoded letter offset
            }

            // Work-around for [instant] and [instant:allowcommand] at the beginning of a line of text
            if (skipImmediate)
                InUpdateControlCommand(DynValue.NewString(skipCommand));

            if (!instantActive)
                Update();
        }

        private bool CheckCommand()
        {
            if (currentLine == 0)
            //if (currentLine >= textQueue.Length)
                return false;
            if (currentCharacter >= text.Text.Length) return false;
            //if (currentCharacter >= textQueue[currentLine].Text.Length) return false;
            if (text.Text[currentCharacter] != '[') return false;
            //if (textQueue[currentLine].Text[currentCharacter] != '[') return false;
            int currentChar = currentCharacter;
            string command = UnitaleUtil.ParseCommandInline(text.Text, ref currentCharacter);
            //string command = UnitaleUtil.ParseCommandInline(textQueue[currentLine].Text, ref currentCharacter);
            if (command != null)
            {
                currentCharacter++; // we're not in a continuable loop so move to the character after the ] manually

                //float lastLetterTimer = letterTimer; // kind of a dirty hack so we can at least release 0.2.0 sigh
                //float lastTimePerLetter = timePerLetter; // i am so sorry

                DynValue commandDV = DynValue.NewString(command);
                InUpdateControlCommand(commandDV, currentCharacter);

                //if (lastLetterTimer != letterTimer || lastTimePerLetter != timePerLetter)
                //if (currentCharacter >= textQueue[currentLine].Text.Length)
                //    return true;

                return true;
            }
            currentCharacter = currentChar;
            return false;
        }

        [Obsolete]
        protected virtual void Update()
        {
            if (text == null || lateStartWaiting)
            //if (textQueue == null || textQueue.Length == 0 || lateStartWaiting)
                return;
            /*if (currentLine >= lineCount() && overworld) {
                endTextEvent();
                return;
            }*/

            if (GlobalControls.retroMode && instantActive || currentCharacter >= letterReferences.Length)
                return;

            /*
            letterTimer += Time.deltaTime;
            if ((letterTimer > timePerLetter || firstChar) && !LineComplete()) {
                firstChar = false;
                letterTimer = 0.0f;
                bool soundPlayed = false;
                int lastLetter = -1;
                if (HandleShowLettersOnce(ref soundPlayed, ref lastLetter))
                    return;
                else
                    for (int i = 0; (instantCommand || i < letterSpeed) && currentCharacter < letterReferences.Length; i++)
                        if (!HandleShowLetter(ref soundPlayed, ref lastLetter)) {
                            HandleShowLettersOnce(ref soundPlayed, ref lastLetter);
                            return;
                        }
            }
            */

            letterTimer += Time.deltaTime;
            if ((letterTimer >= timePerLetter) && !LineComplete())
            {
                int repeats = timePerLetter == 0f ? 1 : (int)Mathf.Floor(letterTimer / timePerLetter);

                int lastLetter = -1;

                for (int i = 0; i < repeats; i++)
                {
                    if (!HandleShowLetter(ref lastLetter))
                    {
                        return;
                    }

                    letterTimer -= timePerLetter;
                }
            }

            noSkip1stFrame = false;
        }

        private bool HandleShowLetter(ref int lastLetter)
        {
            if (lastLetter != currentCharacter && ((!GlobalControls.retroMode && (!instantActive || instantCommand)) || GlobalControls.retroMode))
            {
                float oldLetterTimer = letterTimer;
                lastLetter = currentCharacter;
                while (CheckCommand())
                    if ((GlobalControls.retroMode && instantActive) || letterTimer != oldLetterTimer)
                        return false;
                if (currentCharacter >= letterReferences.Length)
                    return false;
            }
            if (letterReferences[currentCharacter] != null)
            {
                letterReferences[currentCharacter].enabled = true;
                letterReferences[currentCharacter].GetComponent<Letter>().effect = null;
            }
            currentReferenceCharacter++;
            currentCharacter++;
            return true;
        }

        private void PreCreateControlCommand(string command)
        {
            string[] cmds = UnitaleUtil.SpecialSplit(':', command);
            string[] args = new string[0];
            if (cmds.Length == 2)
            {
                args = UnitaleUtil.SpecialSplit(',', cmds[1], true);
                cmds[1] = args[0];
            }
            switch (cmds[0].ToLower())
            {
                case "color":
                    float oldAlpha = currentColor.a;
                    currentColor = ParseUtil.GetColor(cmds[1]);
                    currentColor = new Color(currentColor.r, currentColor.g, currentColor.b, oldAlpha);
                    colorSet = true;
                    break;
                case "alpha":
                    if (cmds[1].Length == 2)
                        currentColor = new Color(currentColor.r, currentColor.g, currentColor.b, ParseUtil.GetByte(cmds[1]) / 255);
                    break;
                case "charspacing":
                    if (cmds.Length > 1 && cmds[1].ToLower() == "default") SetHorizontalSpacing(Charset.CharSpacing);
                    else SetHorizontalSpacing(ParseUtil.GetFloat(cmds[1]));
                    break;
                case "linespacing":
                    if (cmds.Length > 1)
                        SetVerticalSpacing(ParseUtil.GetFloat(cmds[1]));
                    break;

                case "instant":
                    if (GlobalControls.retroMode)
                        instantActive = true;
                    else
                        InUpdateControlCommand(DynValue.NewString(command));
                    break;

                case "font":
                    UnderFont uf = SpriteFontRegistry.Get(cmds[1]);
                    if (uf == null)
                        UnitaleUtil.DisplayLuaError("[font:x] usage", "The font \"" + cmds[1] + "\" doesn't exist.\nYou should check if you made a typo, or if the font really is in your mod.");
                    SetFont(uf);
                    if (GetType() == typeof(LuaStaticTextManager))
                        ((LuaStaticTextManager)this).UpdateBubble();
                    break;
            }
        }

        private void InUpdateControlCommand(DynValue command, int index = 0)
        {
            string[] cmds = UnitaleUtil.SpecialSplit(':', command.String);
            string[] args = new string[0];
            if (cmds.Length >= 2)
            {
                if (cmds.Length == 3)
                {
                    if (cmds[2] == "skipover" && instantCommand) return;
                    if (cmds[2] == "skiponly" && !instantCommand) return;
                }
                else if (cmds[1] == "skipover" && instantCommand) return;
                else if (cmds[1] == "skiponly" && !instantCommand) return;
                args = UnitaleUtil.SpecialSplit(',', cmds[1], true);
                cmds[1] = args[0];
            }
            //print("Frame " + GlobalControls.frame + ": Command " + cmds[0].ToLower() + " found for " + gameObject.name);
            switch (cmds[0].ToLower())
            {
                case "instant":
                    if (args.Length != 0 && (args.Length > 1 || args[0] != "allowcommand"))
                        break;

                    instantActive = true;

                    if (command.String == "instant:allowcommand")
                        instantCommand = true;

                    // First:  Find the active line of text
                    string currentText = text.Text;
                    //string currentText = textQueue[currentLine].Text;

                    // Second: Find the position to "end" at
                    // This will either be: [instant:stop], [instant:stopall] or the end of the string
                    int pos = currentText.Length;

                    for (int i = index; i < pos; i++)
                    {
                        if (!skipFromPlayer)
                        {
                            if ((currentText.Substring(i)).Length < 13 || pos - i < 13 || currentText.Substring(i, 13) != "[instant:stop") continue;
                            pos = i - 1;
                            break;
                        }

                        if ((currentText.Substring(i)).Length < 16 || pos - i < 16 || currentText.Substring(i, 16) != "[instant:stopall") continue;
                        pos = i - 1;
                        break;
                    }

                    // Third: Show all letters (and execute all commands, if applicable) between `index` and `pos`
                    int lastLetter = -1;
                    int destination = System.Math.Min(pos, letterReferences.Length);
                    while (currentCharacter < destination)
                        HandleShowLetter(ref lastLetter);

                    // This is a catch-all.
                    // If a line of text starts with [instant], the above code will not display the letters it passes over,
                    // due to how HandleShowLetter is coded.
                    for (int i = index; i < pos; i++)
                    {
                        if (letterReferences[i] != null)
                            letterReferences[i].enabled = true;
                    }

                    // Fourth:  Update variables
                    if (pos < currentText.Length)
                    {
                        instantActive = false;
                        instantCommand = false;
                        letterTimer = timePerLetter;
                    }

                    skipFromPlayer = false;

                    currentCharacter = pos;
                    currentReferenceCharacter = pos;
                    break;
            }
        }
    }
}
