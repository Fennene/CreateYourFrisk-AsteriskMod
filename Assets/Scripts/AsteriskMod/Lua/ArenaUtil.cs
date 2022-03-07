using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod.Lua
{
    public class ArenaUtil
    {
        [MoonSharpHidden]
        public static Vector2 textPosition = Vector2.zero;

        public static void SetBorderColor(float r, float g, float b, float a = 1.0f)
        {
            ArenaUIManager.Instance.SetBorderColor(new Color(r, g, b, a));
        }

        public static void SetBorderColor32(byte r, byte g, byte b, byte a = 255)
        {
            ArenaUIManager.Instance.SetBorderColor(new Color32(r, g, b, a));
        }

        public static void SetDialogTextPosition(int x, int y)
        {
            Vector3 oldPosition = new Vector3(UIController.instance.mainTextManager.transform.position.x, UIController.instance.mainTextManager.transform.position.y, UIController.instance.mainTextManager.transform.position.z);
            Vector2 oldRelativePos = new Vector2(textPosition.x, textPosition.y);
            textPosition = new Vector2(x, y);
            Vector3 newPosition = new Vector3(oldPosition.x - oldRelativePos.x + textPosition.x, oldPosition.y - oldRelativePos.y + textPosition.y, oldPosition.z);
            UIController.instance.mainTextManager.transform.position = newPosition;
        }
    }
}
