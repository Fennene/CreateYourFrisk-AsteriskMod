using UnityEngine.UI;

namespace AsteriskMod.Lua
{
    public class StateEditor
    {
        public static void SetDialogText(string text)
        {
            UIController.instance.mainTextManager.SetText(new RegularMessage(text));
        }

        public static void SetChoicesDialogText(string[] texts, bool singleList = true)
        {
            UIController.instance.mainTextManager.SetText(new SelectMessage(texts, singleList));
        }

        public static void SetDialogTextPuase(bool puase)
        {
            UIController.instance.mainTextManager.SetPause(puase);
        }

        public static void HideDialogText()
        {
            UIController.instance.mainTextManager.DestroyChars();
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

        public static bool GetLineCompleteDialogText()
        {
            return UIController.instance.mainTextManager.LineComplete();
        }

        public static void SetButtonActive(bool fight = false, bool act = false, bool item = false, bool mercy = false)
        {
            UIController.instance.SetButtonActive(fight, act, item, mercy);
        }

        public static void SetPlayerOnSelection(int selection, bool singleList = true)
        {
            UIController.instance.SetPlayerOnSelection((selection - 1) * (singleList ? 2 : 1));
        }

        public static void SetPlayerVisible(bool visible)
        {
            PlayerController.instance.GetComponent<Image>().enabled = visible;
        }

        public static void ForceResetArena()
        {
            ArenaManager.instance.resetArena();
        }
    }
}
