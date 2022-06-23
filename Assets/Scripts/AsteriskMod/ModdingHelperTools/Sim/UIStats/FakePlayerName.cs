using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakePlayerName : MonoBehaviour
    {
        public FakeLuaStaticTextManager NameTextMan { get; private set; }
        private string nowText = null;

        internal static FakePlayerName instance;

        private void Awake()
        {
            NameTextMan = GetComponent<FakeLuaStaticTextManager>();
            instance = this;
        }

        private void Start()
        {
            if (!FakePlayerLV.instance.Initialized) FakePlayerLV.instance.Start();
            if (Asterisk.language == Languages.Japanese)
            {
                nowText = SimInstance.BattleSimulator.PlayerName;
                SetJP();
                return;
            }
            NameTextMan.SetFont(SimInstance.FakeSpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetName(SimInstance.BattleSimulator.PlayerName);
            //SetJP();
        }

        internal void SetName(string newName, bool force = false)
        {
            if (nowText == newName && !force) return;
            nowText = newName;
            //NameTextMan.SetText(new InstantTextMessage(AsteriskEngine.JapaneseStyleOption.JPName ? nowText : nowText.ToUpper()));
            NameTextMan.SetText(new InstantTextMessage((Asterisk.language == Languages.Japanese) ? nowText : nowText.ToUpper()));
            NameTextMan.enabled = true;
            FakePlayerLV.instance.SetPosition((int)AsteriskUtil.CalcTextWidth(NameTextMan, countEOLSpace: true));
        }

        internal void SetJP()
        {
            NameTextMan.SetFont(SimInstance.FakeSpriteFontRegistry.Get(AsteriskEngine.JapaneseStyleOption.JPName ? SpriteFontRegistry.UI_JP_NAME_NAME : SpriteFontRegistry.UI_SMALLTEXT_NAME));
            NameTextMan.Move(0, 6 * (AsteriskEngine.JapaneseStyleOption.JPName ? 1 : -1));
            SetName(nowText, true);
        }
    }
}
