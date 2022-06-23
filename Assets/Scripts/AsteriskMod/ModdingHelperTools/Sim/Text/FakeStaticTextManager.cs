using System;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeStaticTextManager : MonoBehaviour
    {
        public bool IsUI;

        internal Image[] letterReferences;
        internal Vector2[] letterPositions;

        protected UnderFont default_charset;
        [MoonSharpHidden] public int _textMaxWidth;
        private bool decoratedTextOffset;
        private RectTransform self;
        [MoonSharpHidden] public Vector2 offset;
        private bool offsetSet;
        private float currentX;
        private float currentY;

        internal float hSpacing = 3;
        internal float vSpacing;
        private GameObject textframe;

        protected Color currentColor = Color.white;
        private bool colorSet;
        protected Color defaultColor = Color.white;

        //[MoonSharpHidden] public ScriptWrapper caller;

        [MoonSharpHidden] public UnderFont Charset { get; protected set; }
        [MoonSharpHidden] public InstantTextMessage instantText;
        [MoonSharpHidden] public bool blockSkip;

        private float newhSpacing = 3;
        private bool newhSpacingSet;
        private float newvSpacing;
        private bool newvSpacingSet;

        public FakeStaticTextManager()
        {
            _textMaxWidth = 0;
            decoratedTextOffset = false;
            vSpacing = 0;
            colorSet = false;
            instantText = null;
            blockSkip = false;
        }

        //[MoonSharpHidden] public void SetCaller(ScriptWrapper s) { caller = s; }

        [MoonSharpHidden]
        public void SetFont(UnderFont font, bool firstTime = false)
        {
            Charset = font;
            default_charset = font;

            vSpacing = 0;
            hSpacing = font.CharSpacing;
            defaultColor = font.DefaultColor;
            if (GetType() == typeof(FakeLuaStaticTextManager) && !IsUI)
            {
                if (((FakeLuaStaticTextManager)this).hasColorBeenSet) defaultColor = ((FakeLuaStaticTextManager)this)._color;
                if (((FakeLuaStaticTextManager)this).hasAlphaBeenSet) defaultColor.a = ((FakeLuaStaticTextManager)this).alpha;
            }
            currentColor = defaultColor;
        }

        [MoonSharpHidden] public void SetHorizontalSpacing(float spacing = 3) { hSpacing = spacing; }
        [MoonSharpHidden] public void SetVerticalSpacing(float spacing = 0) { vSpacing = spacing; }

        [MoonSharpHidden]
        public void ResetFont()
        {
            if (Charset == null || default_charset == null)
                if (GetType() == typeof(FakeLuaStaticTextManager) && !IsUI)
                    ((FakeLuaStaticTextManager)this).SetFont(SpriteFontRegistry.UI_DEFAULT_NAME);
                else
                    SetFont(SimInstance.FakeSpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME), true);
            Charset = default_charset;
            Debug.Assert(default_charset != null, "default_charset != null");
            defaultColor = default_charset.DefaultColor;
        }

        protected virtual void Awake()
        {
            self = gameObject.GetComponent<RectTransform>();

            /*
            if (!UnitaleUtil.IsOverworld || !GameObject.Find("textframe_border_outer")) return;
            textframe = GameObject.Find("textframe_border_outer");
            */
        }

        [MoonSharpHidden]
        public void SetText(InstantTextMessage text)
        {
            /*
            if (UnitaleUtil.IsOverworld && (gameObject.name == "TextManager OW"))
                PlayerOverworld.AutoSetUIPos();
            */

            ResetFont();
            instantText = text;
            ShowLine();
        }

        [MoonSharpHidden]
        public void SetOffset(float xOff, float yOff)
        {
            offset = new Vector2(xOff, yOff);
            offsetSet = true;
        }

        protected void ShowLine(/**int line, */bool forceNoAutoLineBreak = false)
        {
            if (instantText == null) return;

            if (!offsetSet)
                SetOffset(0, 0);
            if (GetType() != typeof(FakeLuaStaticTextManager) || IsUI)
                ResetFont();
            currentColor = defaultColor;
            colorSet = false;
            if (newvSpacingSet) vSpacing = newvSpacing;
            if (newhSpacingSet) hSpacing = newhSpacing;
            newvSpacingSet = false;
            newhSpacingSet = false;

            DestroyChars();
            currentX = self.position.x + offset.x;
            currentY = self.position.y + offset.y;
            if ((GetType() != typeof(FakeLuaStaticTextManager) || IsUI) && gameObject.name != "TextParent" && gameObject.name != "ReviveText")
                currentY -= Charset.LineSpacing;
            SpawnText(forceNoAutoLineBreak);
            /*
            if (UnitaleUtil.IsOverworld && textframe != null && this == PlayerOverworld.instance.textmgr)
            {
                if (instantText.ActualText)
                {
                    if (textframe.GetComponent<Image>().color.a == 0)
                        SetTextFrameAlpha(1);
                    blockSkip = false;
                }
                else
                {
                    if ((textframe.GetComponent<Image>().color.a == 1))
                        SetTextFrameAlpha(0);
                    blockSkip = true;
                    DestroyChars();
                }
            }

            if (!GlobalControls.retroMode && !UnitaleUtil.IsOverworld && UIController.instance && this == UIController.instance.mainTextManager)
            {
                int lines = (instantText.Text.Split('\n').Length > 3 && (UIController.instance.state == UIController.UIState.ACTIONSELECT || UIController.instance.state == UIController.UIState.DIALOGRESULT)) ? 4 : 3;
                Vector3 pos = self.localPosition;

                self.localPosition = new Vector3(pos.x, pos.y - (decoratedTextOffset ? 9 : 0), pos.z);
                pos = self.localPosition;
                decoratedTextOffset = false;

                if (lines != 4) return;
                decoratedTextOffset = true;
                self.localPosition = new Vector3(pos.x, pos.y + (decoratedTextOffset ? 9 : 0), pos.z);
            }
            else if (gameObject.name == "TextManager OW")
            {
                int lines = instantText.Text.Split('\n').Length;
                lines = lines >= 4 ? 4 : 3;
                Vector3 pos = gameObject.GetComponent<RectTransform>().localPosition;
                gameObject.GetComponent<RectTransform>().localPosition = new Vector3(pos.x, 22 + ((lines - 1) * Charset.LineSpacing / 2), pos.z);
            }
            */
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

        [MoonSharpHidden]
        public void DestroyChars()
        {
            foreach (Transform child in gameObject.transform)
                Destroy(child.gameObject);
        }

        private void SpawnTextSpaceTest(int i, string currentText, out string currentText2)
        {
            currentText2 = currentText;

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
                if (AsteriskUtil.CalcTextWidth(this, wordBeginIndex, finalIndex) > _textMaxWidth)
                {
                    // Word is taking the entire line
                    for (int currentIndex = wordBeginIndex; currentIndex <= finalIndex; currentIndex++)
                    {
                        if (!(AsteriskUtil.CalcTextWidth(this, beginIndex, currentIndex) > _textMaxWidth)) continue;
                        currentText2 = currentText2.Substring(0, currentIndex) + "\n" + currentText2.Substring(currentIndex, currentText2.Length - currentIndex);
                        instantText.Text = currentText2;
                        finalIndex += 1;
                        beginIndex = currentIndex;
                    }
                }
                else
                    // Line is too long
                    currentText2 = currentText2.Substring(0, wordBeginIndex - 1) + "\n" + currentText2.Substring(wordBeginIndex, currentText.Length - wordBeginIndex);

                Array.Resize(ref letterReferences, currentText2.Length);
                Array.Resize(ref letterPositions, currentText2.Length);
            }
            instantText.Text = currentText2;
        }

        private void CreateLetter(string currentText, int index, bool insert = false)
        {
            if (insert)
                for (int i = letterReferences.Length - 2; i >= index; i--)
                {
                    letterPositions[i + 1] = letterPositions[i];
                    letterReferences[i + 1] = letterReferences[i];
                }

            GameObject singleLtr = Instantiate(SimInstance.FakeSpriteFontRegistry.LETTER_OBJECT);
            RectTransform ltrRect = singleLtr.GetComponent<RectTransform>();

            bool isLua = GetType() == typeof(FakeLuaStaticTextManager);
            FakeLuaStaticTextManager luaThis = isLua ? ((FakeLuaStaticTextManager)this) : null;

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
        }

        private void MoveLetter(string currentText, int index, RectTransform ltrRect)
        {
            if ((GetType() == typeof(FakeLuaStaticTextManager) && !IsUI) || gameObject.name == "TextParent" || gameObject.name == "ReviveText")
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
            string currentText = instantText.Text;
            letterReferences = new Image[currentText.Length];
            letterPositions = new Vector2[currentText.Length];
            if (currentText.Length > 1 && !forceNoAutoLineBreak)
                if (SimInstance.BattleSimulator.autoLineBreak || (GetType() == typeof(FakeLuaStaticTextManager) && !IsUI))
                    SpawnTextSpaceTest(0, currentText, out currentText);

            for (int i = 0; i < currentText.Length; i++)
            {
                switch (currentText[i])
                {
                    case '\n':
                        currentX = self.position.x + offset.x;
                        currentY = currentY - vSpacing - Charset.LineSpacing;
                        break;
                    case '\t':
                        currentX = 356;
                        break;
                    case ' ':
                        if (i + 1 == currentText.Length || currentText[i + 1] == ' ' || forceNoAutoLineBreak)
                            break;
                        if (SimInstance.BattleSimulator.autoLineBreak || (GetType() == typeof(FakeLuaStaticTextManager) && !IsUI))
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
        }

        /// <summary>必ず<see cref="SetFont(UnderFont, bool)"/>または<see cref="ResetFont"/>後に呼び出すこと</summary>
        public void SetCharSpacing(float? space = null)
        {
            if (!space.HasValue) newhSpacing = Charset.CharSpacing;
            else newhSpacing = space.Value;
            newhSpacingSet = true;
            SetText(instantText);
        }

        public void SetLineSpacing(float space = 0)
        {
            newvSpacing = space;
            newvSpacingSet = true;
            SetText(instantText);
        }
    }
}
