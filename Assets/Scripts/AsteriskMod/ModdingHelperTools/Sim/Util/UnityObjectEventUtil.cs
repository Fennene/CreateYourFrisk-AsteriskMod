using AsteriskMod.ModdingHelperTools.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal static class UnityButtonUtil
    {
        internal static void AddListener(Button button, UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        internal static void SetActive(Button button, Image image, bool active)
        {
            button.enabled = active;
            image.color = active ? new Color32(242, 242, 242, 255) : new Color32(192, 192, 192, 255);
        }
    }

    internal static class UnityToggleUtil
    {
        internal static void AddListener(Toggle toggle, UnityAction<bool> action)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(action);
        }
    }

    internal static class CYFInputFieldUtil
    {
        internal static void ShowInputError(CYFInputField cYFInputField, bool noError)
        {
            if (noError) cYFInputField.ResetOuterColor();
            else         cYFInputField.OuterImage.color = new Color32(255, 64, 64, 255);
        }

        internal static void AddListener_OnValueChanged(CYFInputField cYFInputField, UnityAction<string> action)
        {
            cYFInputField.InputField.onValueChanged.RemoveAllListeners();
            cYFInputField.InputField.onValueChanged.AddListener(action);
        }

        internal static void AddListener_OnEndEdit(CYFInputField cYFInputField, UnityAction<string> action)
        {
            cYFInputField.InputField.onEndEdit.RemoveAllListeners();
            cYFInputField.InputField.onEndEdit.AddListener(action);
        }
    }
}
