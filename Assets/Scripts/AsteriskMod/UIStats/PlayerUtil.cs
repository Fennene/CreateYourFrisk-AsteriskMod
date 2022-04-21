using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class PlayerUtil : MonoBehaviour
    {
        [MoonSharpHidden] internal static PlayerUtil Instance;
        private static GameObject Stats;
        private static Vector2 relativePosition;

        private void Awake()
        {
            Stats = gameObject;
            relativePosition = Vector2.zero;
            Instance = this;
        }

        public static void Move(int x, int y)
        {
            Vector2 add = new Vector2(x, y);
            relativePosition += add;
            Stats.GetComponent<RectTransform>().anchoredPosition += add;
        }

        public static void MoveTo()
        {
            Stats.GetComponent<RectTransform>().anchoredPosition -= relativePosition;
            relativePosition = Vector2.zero;
        }

        public static void MoveTo(int newX, int newY)
        {
            Vector2 initPos = Stats.GetComponent<RectTransform>().anchoredPosition - relativePosition;
            relativePosition = new Vector2(newX, newY);
            Stats.GetComponent<RectTransform>().anchoredPosition = initPos + relativePosition;
        }

        public static void MoveToAbs(int newX, int newY)
        {
            Vector2 initPos = Stats.GetComponent<RectTransform>().anchoredPosition - relativePosition;
            Stats.transform.position = new Vector3(newX, newY, Stats.transform.position.z);
            relativePosition = Stats.GetComponent<RectTransform>().anchoredPosition - initPos;
        }

        public static LimitedLuaStaticTextManager Name { get { return PlayerNameText.instance.NameTextMan; } }

        public static LimitedLuaStaticTextManager Love { get { return PlayerLoveText.instance.LoveTextMan; } }

        public static PlayerLifeBar HPBar { get { return PlayerLifeUI.instance.LifeBar; } }

        public static LimitedLuaStaticTextManager HPText { get { return PlayerLifeUI.instance.LifeTextMan; } }

        public static void SetControlOverride(bool active)
        {
            Name.SetControlOverride(active);
            Love.SetControlOverride(active);
            SetHPControlOverride(active);
        }

        public static void SetLV(string lv)
        {
            PlayerLoveText.instance.SetText(lv);
        }

        public static void SetHPControlOverride(bool active)
        {
            PlayerLifeUI.instance.SetHPControlOverride(active);
        }

        public static void SetHP(float hp, float maxhp, bool updateText = true)
        {
            PlayerLifeUI.instance.SetHPOverride(hp, maxhp, updateText);
        }

        public static void SetHPText(float hp, float maxhp)
        {
            PlayerLifeUI.instance.SetHPTextFromNumber(hp, maxhp);
        }

        public static PlayerLifeBar CreateLifeBar(bool below = false)
        {
            return null;
        }
    }
}
