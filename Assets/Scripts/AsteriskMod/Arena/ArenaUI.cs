﻿using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    // Be attached in Assets/Battle.Unity/Canvas/arena_container
    public class ArenaUI : MonoBehaviour
    {
        private static GameObject border;
        private static GameObject arena;
        private static GameObject mainTextMan;
        private static Vector2 mainTextManPos;

        private static Vector2 _arenaOffset;
        internal static Vector2 ArenaOffset
        {
            get { return _arenaOffset; }
            set
            {
                Vector2 oldArenaOffset = _arenaOffset;
                _arenaOffset = value;
                UIController.UIState state = UIController.instance.GetState();
                if (state == UIController.UIState.DEFENDING || state == UIController.UIState.CUSTOMSTATE)
                {
                    ArenaManager.instance.MoveImmediate(-oldArenaOffset.x + _arenaOffset.x, -oldArenaOffset.y + _arenaOffset.y, true);
                }
                else
                {
                    ArenaManager.instance.resetArena();
                    if (state == UIController.UIState.ENEMYSELECT || state == UIController.UIState.ACTMENU || state == UIController.UIState.ITEMMENU || state == UIController.UIState.MERCYMENU)
                    {
                        float xPos = PlayerController.instance.self.anchoredPosition.x - oldArenaOffset.x + _arenaOffset.x;
                        float yPos = PlayerController.instance.self.anchoredPosition.y - oldArenaOffset.y + _arenaOffset.y;
                        PlayerController.instance.SetPosition(xPos, yPos, false);
                    }
                }
            }
        }

        internal static void Initialize()
        {
            _arenaOffset = Vector2.zero;
        }

        private void Awake()
        {
            border = transform.Find("arena_border_outer").gameObject;
            arena = border.transform.Find("arena").gameObject;
            mainTextMan = arena.transform.Find("TextManager").gameObject;
            mainTextManPos = Vector2.zero;
        }

        public static void SetBorderColor(Color color)
        {
            border.GetComponent<Image>().color = color;
        }

        public static void SetInnerColor(Color color)
        {
            arena.GetComponent<Image>().color = color;
        }

        public static void SetMainTextPosition(int newX, int newY)
        {
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.transform == null) return;
            Vector3 oldPos = UIController.instance.mainTextManager.transform.position;
            Vector2 oldRelativePos = mainTextManPos;
            mainTextManPos = new Vector2(newX, newY);
            UIController.instance.mainTextManager.transform.position = new Vector3(
                oldPos.x - oldRelativePos.x + mainTextManPos.x,
                oldPos.y - oldRelativePos.y + mainTextManPos.y,
                oldPos.z
            );
        }

        public static Vector2 GetMainTextPosition()
        {
            return mainTextManPos;
        }

        public static Table GetMainTextLetters()
        {
            Table table = new Table(null);
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.letterReferences == null) return table;
            int key = 0;
            foreach (Image i in UIController.instance.mainTextManager.letterReferences)
            {
                if (i != null)
                {
                    key++;
                    LuaSpriteController letter = new LuaSpriteController(i) { tag = "letter" };
                    table.Set(key, UserData.Create(letter, LuaSpriteController.data));
                }
            }
            return table;
        }

        public static void SetTextVolume(float value)
        {
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.GetComponent<TextManager>() == null) return;
            if (value < 0) value = 0;
            else if (value > 1) value = 1;
            //mainTextMan.GetComponent<AudioSource>().volume = value;
            UIController.instance.mainTextManager.GetComponent<TextManager>().letterSound.volume = value;
        }

        public static float GetTextVolume()
        {
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.GetComponent<TextManager>() == null) return 1;
            return UIController.instance.mainTextManager.GetComponent<TextManager>().letterSound.volume;
        }

        public static void SetTextFont(string fontName, bool firstTime = false)
        {
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.GetComponent<TextManager>() == null) return;
            if (fontName == null)
                throw new CYFException("ArenaUtil.SetDialogTextFont: The first argument (the font name) is nil.\n\nSee the documentation for proper usage.");
            UnderFont uf = SpriteFontRegistry.Get(fontName);
            if (uf == null)
                throw new CYFException("The font \"" + fontName + "\" doesn't exist.\nYou should check if you made a typo, or if the font really is in your mod.");
            UIController.instance.mainTextManager.GetComponent<TextManager>().SetFont(uf, firstTime);
            //if (!firstTime)
            //    UIController.instance.mainTextManager.GetComponent<TextManager>().default_charset = uf; // impossible.
        }
    }
}
