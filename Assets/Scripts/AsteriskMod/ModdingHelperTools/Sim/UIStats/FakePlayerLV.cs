using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakePlayerLV : MonoBehaviour
    {
        public FakeLuaStaticTextManager LoveTextMan { get; private set; }
        private int nowLove;

        internal static FakePlayerLV instance;

        internal bool Initialized { get; private set; }

        private void Awake()
        {
            LoveTextMan = GetComponent<FakeLuaStaticTextManager>();
            instance = this;
        }

        internal void Start()
        {
            if (Initialized) return;
            LoveTextMan.SetFont(SimInstance.FakeSpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetLove(SimInstance.BattleSimulator.PlayerLV);
            Initialized = true;
        }

        internal void SetLove(int newLV)
        {
            if (nowLove == newLV) return;
            nowLove = newLV;
            LoveTextMan.SetText(new InstantTextMessage("LV " + nowLove.ToString().ToUpper()));
            LoveTextMan.enabled = true;
        }

        /*
        public void SetText(string newLV)
        {
            nowLove = 0;
            LoveTextMan.SetText(new InstantTextMessage("LV " + newLV.ToUpper()));
            LoveTextMan.enabled = true;
        }
        */

        internal void SetPosition(int textLength)
        {
            Vector2 pos = GetComponent<RectTransform>().anchoredPosition;
            pos.x = textLength + 30;
            GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
