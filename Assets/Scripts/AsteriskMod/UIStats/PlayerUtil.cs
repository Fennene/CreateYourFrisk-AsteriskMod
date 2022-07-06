using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod
{
    public class PlayerUtil : MonoBehaviour
    {
        [MoonSharpHidden] internal static PlayerUtil Instance;
        private GameObject Stats;
        private Vector2 relativePosition;
        private Vector2 relativeHPRectPosition;

        public static float defaultdamage;
        public static float defaultinvtime;

        [MoonSharpHidden]
        internal static void Initialize()
        {
            defaultdamage = 3;
            defaultinvtime = 1.7f;
        }

        private void Awake()
        {
            Stats = gameObject;
            relativePosition = Vector2.zero;
            relativeHPRectPosition = Vector2.zero;
            Instance = this;
        }

        public float x
        {
            get { return relativePosition.x; }
            set { MoveTo(value, y); }
        }

        public float y
        {
            get { return relativePosition.y; }
            set { MoveTo(x, value); }
        }

        public void Move(float x, float y)
        {
            Vector2 add = new Vector2(x, y);
            relativePosition += add;
            Stats.GetComponent<RectTransform>().anchoredPosition += add;
        }

        public void MoveTo(float newX, float newY)
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

        public void MoveToAbs(float newX, float newY)
        {
            Vector2 initPos = Stats.GetComponent<RectTransform>().anchoredPosition - relativePosition;
            Stats.transform.position = new Vector3(newX, newY, Stats.transform.position.z);
            relativePosition = Stats.GetComponent<RectTransform>().anchoredPosition - initPos;
        }


        public UIStaticTextManager Name { get { return PlayerNameText.instance.NameTextMan; } }
        public UIStaticTextManager nametext { get { return Name; } } // Could't name "name".

        public void SetJapaneseNameActive(bool active)
        {
            AsteriskEngine.JapaneseStyleOption.SetJapaneseNameActive(active);
        }

        public UIStaticTextManager Love { get { return PlayerLoveText.instance.LoveTextMan; } }
        public UIStaticTextManager lovetext { get { return Love; } }

        public void SetLV(string lv)
        {
            PlayerLoveText.instance.SetText(lv);
        }


        public float hpx
        {
            get { return relativeHPRectPosition.x; }
            set { HPMoveTo(value, hpy); }
        }
        public float HPx
        {
            get { return hpx; }
            set { hpx = value; }
        }

        public float hpy
        {
            get { return relativeHPRectPosition.y; }
            set { HPMoveTo(hpx, value); }
        }
        public float HPy
        {
            get { return hpy; }
            set { hpy = value; }
        }

        public void HPMove(float x, float y)
        {
            Vector2 add = new Vector2(x, y);
            relativeHPRectPosition += add;
            PlayerLifeUI.instance.hpRect.anchoredPosition += add;
        }

        public void HPMoveTo(float newX, float newY)
        {
            Vector2 initPos = PlayerLifeUI.instance.hpRect.anchoredPosition - relativeHPRectPosition;
            relativeHPRectPosition = new Vector2(newX, newY);
            PlayerLifeUI.instance.hpRect.anchoredPosition = initPos + relativeHPRectPosition;
        }

        /*
        public void HPMoveToAbs(float newX, float newY)
        {
            Vector2 initPos = PlayerLifeUI.instance.hpRect.anchoredPosition - relativeHPRectPosition;
            PlayerLifeUI.instance.hpRect.gameObject.transform.position = new Vector3(newX, newY, PlayerLifeUI.instance.hpRect.gameObject.transform.position.z);
            relativeHPRectPosition = PlayerLifeUI.instance.hpRect.anchoredPosition - initPos;
        }
        */

        public LuaSpriteController HPLabel { get { return PlayerLifeUI.instance.HPLabel; } }
        public LuaSpriteController hplabel { get { return HPLabel; } }

        public PlayerLifeBar HPBar { get { return PlayerLifeUI.instance.LifeBar; } }
        public PlayerLifeBar hpbar { get { return HPBar; } }

        public UIStaticTextManager HPText { get { return PlayerLifeUI.instance.LifeTextMan; } }
        public UIStaticTextManager hptext { get { return HPText; } }

        public void SetHP(float hp, float maxhp, bool updateText = true)
        {
            PlayerLifeUI.instance.SetHPOverride(hp, maxhp, updateText);
        }

        public void SetHPText(float hp, float maxhp)
        {
            PlayerLifeUI.instance.SetHPTextFromNumber(hp, maxhp);
        }


        public void SetHPControlOverride(bool active)
        {
            PlayerLifeUI.instance.SetHPControlOverride(active);
        }

        public void SetControlOverride(bool active)
        {
            Name.SetControlOverride(active);
            Love.SetControlOverride(active);
            SetHPControlOverride(active);
        }


        public int GetSoulAlpha()
        {
            if (PlayerController.instance == null) return 0;
            return PlayerController.instance.selfImg.enabled ? 1 : 0;
        }


        public void SetTargetSprite(string path)
        {
            ArenaUI.SetTargetSprite(path);
        }

        public void SetTargetChoiceSprite(string path)
        {
            ArenaUI.SetTargetChoiceSprite(path);
        }

        public void SetTargetChoiceAnim(string[] anim, float frequency = 1 / 12f, string prefix = "", bool forceSetAnimation = true)
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

        public float GetTargetChoiceX()
        {
            return UIController.instance.fightUI.GetTargetRTX();
        }

        public int SimulateDamage(int playerATK, int playerWeaponATK, int enemyDEF, float targetChoiceX = 0f, float randomValue = -1f)
        {
            return UIController.instance.fightUI.SimulateDamage(playerATK, playerWeaponATK, enemyDEF, targetChoiceX, randomValue);
        }

        public PlayerLifeBar CreateLifeBar(string layer = "AboveHPBar")
        {
            string layerName = layer;
            string prefabName;
            if (layer == "BelowHPBar" || layer == "AboveHPBar")
            {
                layerName = "*" + layer;
                prefabName = "Prefabs/AsteriskMod/LifeBar";
            }
            else
            {
                layerName += "Layer";
                prefabName = "Prefabs/AsteriskMod/LifeBarOnSpriteL";
            }
            GameObject parent = GameObject.Find(layerName);
            if (parent == null)
            {
                UnitaleUtil.DisplayLuaError("Creating a lifeBar", "The lifeBar layer " + layer + " doesn't exist.");
                return null;
            }
            PlayerLifeBar lifeBar = GameObject.Instantiate(Resources.Load<GameObject>(prefabName), parent.transform).GetComponent<PlayerLifeBar>();
            lifeBar.Initialize(false);
            return lifeBar;
        }
        /*
        public PlayerLifeBar CreateLifeBar(bool below = false)
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
        */
    }
}
