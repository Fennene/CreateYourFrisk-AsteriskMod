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
    }
}
