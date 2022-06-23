using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakePlayerHPStat : MonoBehaviour
    {
        private Image hpLabel;
        //public LuaSpriteController HPLabel { get; private set; }

        public PlayerLifeBar LifeBar { get; private set; }
        private GameObject lifeTextCore;
        public FakeLuaStaticTextManager LifeTextMan { get; private set; }

        internal static FakePlayerHPStat instance;

        private void Awake()
        {
            hpLabel = transform.Find("*HPLabel").GetComponent<Image>();

            LifeBar = gameObject.GetComponentInChildren<PlayerLifeBar>();
            LifeBar.Initialize(false);

            lifeTextCore = GameObject.Find("*LifeTextParent");
            lifeTextCore.transform.position = new Vector3(lifeTextCore.transform.position.x, lifeTextCore.transform.position.y - 1, lifeTextCore.transform.position.z);

            LifeTextMan = lifeTextCore.GetComponent<FakeLuaStaticTextManager>();

            instance = this;
        }

        private void Start()
        {
            LifeTextMan.SetFont(SimInstance.FakeSpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetHP();

            //HPLabel = new LuaSpriteController(hpLabel) { tag = "ui", ignoreSet = GlobalControls.crate };
        }

        internal void SetHP()
        {
            //LifeBar.setHP(hpCurrent);
            float hpCurrent = SimInstance.BattleSimulator.PlayerHP;
            LifeBar.SetHP(hpCurrent, SimInstance.BattleSimulator.PlayerMaxHP);
            int count = UnitaleUtil.DecimalCount(hpCurrent);
            string sHpCurrent = hpCurrent < 10 ? "0" + hpCurrent.ToString("F" + count) : hpCurrent.ToString("F" + count);
            LifeTextMan.SetText(new InstantTextMessage(SimInstance.BattleSimulator.PlayerHP + " / " + SimInstance.BattleSimulator.PlayerMaxHP));
        }

        /*
        internal void SetMaxHP()
        {
            //LifeBar.setMaxHP(true);
            SetHP();
        }
        */
    }
}
