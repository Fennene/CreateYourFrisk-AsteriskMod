using MoonSharp.Interpreter;
using System;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class StateEditor
    {
        public static void SetDialogText(string text)
        {
            if (AsteriskEngine.JapaneseStyleOption.AutoJPFontStateEditor)
            {
                text = AsteriskEngine.JapaneseStyleOption.FontCommand + text;
            }
            UIController.instance.mainTextManager.SetText(new RegularMessage(text));
        }

        public static void SetChoicesDialogText(string[] texts, bool singleList = true)
        {
            if (AsteriskEngine.JapaneseStyleOption.AutoJPFontStateEditor)
            {
                for (var i = 0; i < texts.Length; i++)
                {
                    texts[i] = AsteriskEngine.JapaneseStyleOption.FontCommand + texts[i];
                }
            }
            UIController.instance.mainTextManager.SetText(new SelectMessage(texts, singleList));
        }

        public static void SetDialogFont(string fontName)
        {
            if (fontName == null)
                throw new CYFException("StateEditor.SetDialogFont: The first argument (the font name) is nil.\n\nSee the documentation for proper usage.");
            UnderFont uf = SpriteFontRegistry.Get(fontName);
            if (uf == null)
                throw new CYFException("The font \"" + fontName + "\" doesn't exist.\nYou should check if you made a typo, or if the font really is in your mod.");
            UIController.instance.mainTextManager.SetFont(uf, false);
            /*
            default_charset = uf;
            UpdateBubble();
            */
        }

        public static void SetDialogTextEffect(string effect, float intensity = -1)
        {
            if (effect == null)
                throw new CYFException("StateEditor.SetEffect: The first argument (the effect name) is nil.\n\nSee the documentation for proper usage.");
            TextEffect targetEffect;
            switch (effect.ToLower())
            {
                case "none":
                    targetEffect = null;
                    break;
                case "twitch":
                    targetEffect = intensity != -1 ? new TwitchEffect(UIController.instance.mainTextManager, intensity) : new TwitchEffect(UIController.instance.mainTextManager);
                    break;
                case "shake":
                    targetEffect = intensity != -1 ? new ShakeEffect(UIController.instance.mainTextManager, intensity) : new ShakeEffect(UIController.instance.mainTextManager);
                    break;
                case "rotate":
                    targetEffect = intensity != -1 ? new RotatingEffect(UIController.instance.mainTextManager, intensity) : new RotatingEffect(UIController.instance.mainTextManager);
                    break;
                default:
                    throw new CYFException("The effect \"" + effect + "\" doesn't exist.\nYou can only choose between \"none\", \"twitch\", \"shake\" and \"rotate\".");
            }
            UIController.instance.mainTextManager.SetEffect(targetEffect);
        }

        public static void SetDialogTextPuase(bool puase) { UIController.instance.mainTextManager.SetPause(puase); }

        public static bool GetLineCompleteDialogText() { return UIController.instance.mainTextManager.LineComplete(); }

        public static void SkipDialogText()
        {
            if (UIController.instance.mainTextManager.CanSkip() && !UIController.instance.mainTextManager.LineComplete())
                UIController.instance.mainTextManager.DoSkipFromPlayer();
        }

        public static void HideDialogText() { UIController.instance.mainTextManager.DestroyChars(); }


        public static void CreateLifeBar(int enemyIndex, int line) { UIController.instance.SimulateFightMenuLifeBar(enemyIndex - 1, line - 1); }

        public static void RemoveLifeBar() { UIController.instance.RemoveFightMenuLifeBar(); }

        /*
        public static void CreateEnemyBubble(string key, int enemyIndex, DynValue customMessage = null, bool usePrefix = true)
        {
            Asterisk.RequireExperimentalFeature("StateEditor.CreateEnemyBubble");
            string[] message;
            if (customMessage == null || customMessage.Type == DataType.Nil || customMessage.Type == DataType.Void)
            {
                message = null;
            }
            else if (customMessage.Type == DataType.Table)
            {
                message = new string[customMessage.Table.Length];
                for (var i = 0; i < customMessage.Table.Length; i++)
                {
                    if (customMessage.Table.Get(i + 1).Type == DataType.String)
                    {
                        message[i] = customMessage.Table.Get(i + 1).String;
                    }
                    else
                    {
                        throw new CYFException("StateEditor.CreateEnemyBubble: customMessage should be nil or a table of string");
                    }
                }
            }
            else
            {
                throw new CYFException("StateEditor.CreateEnemyBubble: customMessage should be nil or a table of string");
            }
            UIController.instance.SimulateEnemyDialogueBubbleAndText(key, enemyIndex, message, usePrefix);
        }

        public static void RemoveEnemyBubble() { UIController.instance.RemoveEnemyDialogueBubbleAndText(); }
        */

        public static void SetPlayerOnSelection(int selection, bool singleList = true) { UIController.instance.SetPlayerOnSelection((selection - 1) * (singleList ? 2 : 1)); }

        public static void SetPlayerVisible(bool visible) { PlayerController.instance.GetComponent<Image>().enabled = visible; }


        public static void SetButtonActive(bool fight = false, bool act = false, bool item = false, bool mercy = false)
        {
            UIController.ActionButtonManager.SetVisibleOverrideSprite(new[] { fight, act, item, mercy });
        }

        public static void SetCurrentAction(string action, bool playerMoveAndUpdateButton = false)
        {
            try
            {
                UIController.Actions nextAction = (UIController.Actions)Enum.Parse(typeof(UIController.Actions), action, true);
                if ((UIController.instance.frozenState == UIController.UIState.PAUSE || !UIController.instance.stateSwitched) && nextAction != UIController.Actions.NONE)
                {
                    UIController.instance.MovePlayerToAction(nextAction, playerMoveAndUpdateButton);
                }
            }
            catch
            {
                throw new CYFException("StateEditor.SetCurrentAction() can only take \"FIGHT\", \"ACT\", \"ITEM\" or \"MERCY\", but you entered \"" + action + "\".");
            }
        }


        public static void ResetArena() { ArenaManager.instance.resetArena(); }
    }
}
