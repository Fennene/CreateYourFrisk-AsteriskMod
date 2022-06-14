using MoonSharp.Interpreter;
using UnityEngine;

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
        /*
        public static void MoveTo()
        {
            Stats.GetComponent<RectTransform>().anchoredPosition -= relativePosition;
            relativePosition = Vector2.zero;
        }
        */

        public static void MoveToAbs(int newX, int newY)
        {
            Vector2 initPos = Stats.GetComponent<RectTransform>().anchoredPosition - relativePosition;
            Stats.transform.position = new Vector3(newX, newY, Stats.transform.position.z);
            relativePosition = Stats.GetComponent<RectTransform>().anchoredPosition - initPos;
        }

        public static UIStaticTextManager Name { get { return PlayerNameText.instance.NameTextMan; } }
        public static UIStaticTextManager nametext { get { return Name; } } // Could't name "name".

        public static UIStaticTextManager Love { get { return PlayerLoveText.instance.LoveTextMan; } }
        public static UIStaticTextManager lovetext { get { return Love; } }

        public static LuaSpriteController HPLabel { get { return PlayerLifeUI.instance.HPLabel; } }
        public static LuaSpriteController hplabel { get { return HPLabel; } }

        public static PlayerLifeBar HPBar { get { return PlayerLifeUI.instance.LifeBar; } }
        public static PlayerLifeBar hpbar { get { return HPBar; } }

        public static UIStaticTextManager HPText { get { return PlayerLifeUI.instance.LifeTextMan; } }
        public static UIStaticTextManager hptext { get { return HPText; } }

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

        public static int GetSoulAlpha()
        {
            if (PlayerController.instance == null) return 0;
            return PlayerController.instance.selfImg.enabled ? 1 : 0;
        }

        public static void SetTargetSprite(string path)
        {
            ArenaUI.SetTargetSprite(path);
        }

        public static void SetTargetChoiceSprite(string path)
        {
            ArenaUI.SetTargetChoiceSprite(path);
        }

        public static void SetTargetChoiceAnim(string[] anim, float frequency = 1 / 12f, string prefix = "", bool forceSetAnimation = true)
        {
            if (anim.Length == 0)
            {
                //*UIController.instance.fightUI.lineAnim = new[] { "empty" };
                //*UIController.instance.fightUI.lineAnimFrequency = 1 / 12f;
                UIController.instance.fightUI.SetLineAnimation(new[] { "empty" }, 1 / 12f, forceSetAnimation);
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

                //*UIController.instance.fightUI.lineAnim = anim;
                //*UIController.instance.fightUI.lineAnimFrequency = frequency;
                UIController.instance.fightUI.SetLineAnimation(anim, frequency, forceSetAnimation);
            }
        }

        public void ResetTargetChoiceAnim(bool forceSetAnimation = true)
        {
            //*UIController.instance.fightUI.lineAnim = new[] { "UI/Battle/spr_targetchoice_0", "UI/Battle/spr_targetchoice_1" };
            //*UIController.instance.fightUI.lineAnimFrequency = 1 / 12f;
            UIController.instance.fightUI.SetLineAnimation(new[] { "UI/Battle/spr_targetchoice_0", "UI/Battle/spr_targetchoice_1" }, 1 / 12f, forceSetAnimation);
        }

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
