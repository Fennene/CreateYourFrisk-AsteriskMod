using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakePlayerUtil : MonoBehaviour
    {
        internal static FakePlayerUtil Instance;

        private GameObject Stats;
        private Vector2 relativePosition;
        private Vector2 relativeHPRectPosition;

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

        public void MoveToAbs(float newX, float newY)
        {
            Vector2 initPos = Stats.GetComponent<RectTransform>().anchoredPosition - relativePosition;
            Stats.transform.position = new Vector3(newX, newY, Stats.transform.position.z);
            relativePosition = Stats.GetComponent<RectTransform>().anchoredPosition - initPos;
        }

        /*
        public FakeLuaStaticTextManager Name { get { return FakePlayerName.instance.NameTextMan; } }

        public void SetJapaneseNameActive(bool active)
        {
            AsteriskEngine.JapaneseStyleOption.SetJapaneseNameActive(active);
        }

        public FakeLuaStaticTextManager Love { get { return FakePlayerLV.instance.LoveTextMan; } }

        public void SetLV(string lv)
        {
            FakePlayerLV.instance.SetText(lv);
        }
        */

        public float hpx
        {
            get { return relativeHPRectPosition.x; }
            set { HPMoveTo(value, hpy); }
        }

        public float hpy
        {
            get { return relativeHPRectPosition.y; }
            set { HPMoveTo(hpx, value); }
        }

        public void HPMove(float x, float y)
        {
            Vector2 add = new Vector2(x, y);
            relativeHPRectPosition += add;
            FakePlayerHPStat.instance.hpRect.anchoredPosition += add;
        }

        public void HPMoveTo(float newX, float newY)
        {
            Vector2 initPos = FakePlayerHPStat.instance.hpRect.anchoredPosition - relativeHPRectPosition;
            relativeHPRectPosition = new Vector2(newX, newY);
            FakePlayerHPStat.instance.hpRect.anchoredPosition = initPos + relativeHPRectPosition;
        }

        /*
        public FakeLuaStaticTextManager HPLabel { get { return FakePlayerHPStat.instance.HPLabel; } }

        public PlayerLifeBar HPBar { get { return FakePlayerHPStat.instance.LifeBar; } }

        public FakeLuaStaticTextManager HPText { get { return FakePlayerHPStat.instance.LifeTextMan; } }
        */


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
        public PlayerLifeBar CreateLifeBar(string layer = "BelowHPBar")
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
                Debug.LogWarning("GameObject\"" + layerName + "\" is not found.");
                UnitaleUtil.DisplayLuaError("Creating a lifeBar", "The lifeBar layer " + layer + " doesn't exist.");
                return null;
            }
            PlayerLifeBar lifeBar = GameObject.Instantiate(Resources.Load<GameObject>(prefabName), parent.transform).GetComponent<PlayerLifeBar>();
            lifeBar.Initialize(false);
            return lifeBar;
        }
    }
}
