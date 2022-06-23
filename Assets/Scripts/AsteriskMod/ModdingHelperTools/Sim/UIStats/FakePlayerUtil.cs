using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakePlayerUtil : MonoBehaviour
    {
        internal static FakePlayerUtil Instance;
        private static GameObject Stats;
        private static Vector2 relativePosition;

        private void Awake()
        {
            Stats = gameObject;
            relativePosition = Vector2.zero;
            Instance = this;
        }

        public static int x
        {
            get { return Mathf.RoundToInt(relativePosition.x); }
            set { MoveTo(value, y); }
        }

        public static int y
        {
            get { return Mathf.RoundToInt(relativePosition.y); }
            set { MoveTo(x, value); }
        }

        public static void Move(int x, int y)
        {
            Vector2 add = new Vector2(x, y);
            relativePosition += add;
            Stats.GetComponent<RectTransform>().anchoredPosition += add;
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

        //public static FakeLuaStaticTextManager Name { get { return FakePlayerName.instance.NameTextMan; } }
        //public static FakeLuaStaticTextManager Love { get { return FakePlayerLV.instance.LoveTextMan; } }
        //public static LuaSpriteController HPLabel { get { return FakePlayerHPStat.instance.HPLabel; } }
        //public static PlayerLifeBar HPBar { get { return FakePlayerHPStat.instance.LifeBar; } }
        //public static FakeLuaStaticTextManager HPText { get { return FakePlayerHPStat.instance.LifeTextMan; } }

        public static void SetTargetSprite(string path)
        {
            FakeArenaUtil.Instance.SetTargetSprite(path);
        }

        public static void SetTargetChoiceSprite(string path)
        {
            FakeArenaUtil.Instance.SetTargetChoiceSprite(path);
        }

        /*
        public static void SetTargetChoiceAnim(string[] anim, float frequency = 1 / 12f, string prefix = "")
        {
            if (anim.Length == 0)
            {
                UIController.instance.fightUI.lineAnim = new[] { "empty" };
                UIController.instance.fightUI.lineAnimFrequency = 1 / 12f;
            }
            else
            {
                if (prefix != "")
                {
                    while (prefix.StartsWith("/"))
                        prefix = prefix.Substring(1);

                    if (!prefix.EndsWith("/"))
                        prefix += "/";

                    for (int i = 0; i < anim.Length; i++)
                        anim[i] = prefix + anim[i];
                }

                UIController.instance.fightUI.lineAnim = anim;
                UIController.instance.fightUI.lineAnimFrequency = frequency;
            }
        }

        public void ResetTargetChoiceAnim()
        {
            UIController.instance.fightUI.lineAnim = new[] { "UI/Battle/spr_targetchoice_0", "UI/Battle/spr_targetchoice_1" };
            UIController.instance.fightUI.lineAnimFrequency = 1 / 12f;
        }
        */

        public static PlayerLifeBar CreateLifeBar(bool below = false)
        {
            string findName = below ? "*BelowHPBar" : "*AboveHPBar";
            GameObject parent = GameObject.Find(findName);
            if (parent == null)
            {
                Debug.LogWarning("GameObject\"" + findName + "\" is not found.");
                return null;
            }
            PlayerLifeBar lifeBar = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AsteriskMod/LifeBar"), parent.transform).GetComponent<PlayerLifeBar>();
            lifeBar.Initialize(false);
            return lifeBar;
        }
    }
}
