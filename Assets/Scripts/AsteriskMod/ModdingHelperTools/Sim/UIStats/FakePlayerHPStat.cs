using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakePlayerHPStat : MonoBehaviour
    {
        internal RectTransform hpRect; // self

        private Image hpLabel;
        //public LuaSpriteController HPLabel { get; private set; }

        public PlayerLifeBar LifeBar { get; private set; }

        private GameObject lifeTextCore;
        private RectTransform lifeTextRT;
        public FakeLuaStaticTextManager LifeTextMan { get; private set; }
        internal bool textOverride;

        internal static FakePlayerHPStat instance;

        private void Awake()
        {
            hpRect = GetComponent<RectTransform>();

            hpLabel = transform.Find("*HPLabel").GetComponent<Image>();

            LifeBar = transform.Find("*HPBar").GetComponent<PlayerLifeBar>();
            LifeBar.Initialize(false);

            lifeTextCore = transform.Find("*LifeTextParent").gameObject;
            lifeTextCore.transform.position = new Vector3(lifeTextCore.transform.position.x, lifeTextCore.transform.position.y - 1, lifeTextCore.transform.position.z);

            lifeTextRT = lifeTextCore.GetComponent<RectTransform>();
            LifeTextMan = lifeTextCore.GetComponent<FakeLuaStaticTextManager>();

            textOverride = false;

            instance = this;
        }

        private void Start()
        {
            LifeTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            //SetMaxHP();
            SetHP();

            //HPLabel = new LuaSpriteController(hpLabel) { tag = "ui", ignoreSet = GlobalControls.crate };
        }

        internal void SetHP()
        {
            //LifeBar.setHP(hpCurrent);
            float hpCurrent = SimInstance.BattleSimulator.PlayerHP;
            LifeBar.SetHP(hpCurrent, SimInstance.BattleSimulator.PlayerMaxHP);
            //if (LifeTextMan._controlOverride) return;
            if (textOverride) return;
            int count = UnitaleUtil.DecimalCount(hpCurrent);
            string sHpCurrent = hpCurrent < 10 ? "0" + hpCurrent.ToString("F" + count) : hpCurrent.ToString("F" + count);
            LifeTextMan.SetText(new InstantTextMessage(sHpCurrent + " / " + SimInstance.BattleSimulator.PlayerMaxHP));
        }

        internal void SetMaxHP()
        {
            //LifeBar.setMaxHP(true);
            SetTextPosition();

            SetHP();
        }

        internal void SetTextPosition(bool force = false)
        {
            if (textOverride && !force) return;
            lifeTextRT.anchoredPosition = new Vector2(LifeBar.self.sizeDelta.x + 30, lifeTextRT.anchoredPosition.y);
        }
    }
}
