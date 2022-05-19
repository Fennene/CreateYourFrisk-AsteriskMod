using UnityEngine;

namespace AsteriskMod
{
    public class PlayerNameText : MonoBehaviour
    {
        public UIStaticTextManager NameTextMan { get; private set; }
        private string nowText = null;

        internal static PlayerNameText instance;

        private void Awake()
        {
            NameTextMan = GetComponent<UIStaticTextManager>();
            instance = this;
        }

        private void Start()
        {
            NameTextMan._SetText = (_ => { PlayerLoveText.instance.SetPosition(NameTextMan.GetTextWidth()); });
            if (AsteriskEngine.JapaneseStyleOption.JPName)
            {
                SetJP();
                return;
            }
            NameTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetName(PlayerCharacter.instance.Name);
            //SetJP();
        }

        internal void SetName(string newName, bool force = false)
        {
            if (nowText == newName && !force) return;
            nowText = newName;
            NameTextMan.SetText(new InstantTextMessage(AsteriskEngine.JapaneseStyleOption.JPName ? nowText : nowText.ToUpper()));
            NameTextMan.enabled = true;
            PlayerLoveText.instance.SetPosition(NameTextMan.GetTextWidth());
        }

        internal void SetJP()
        {
            NameTextMan.SetFont(SpriteFontRegistry.Get(AsteriskEngine.JapaneseStyleOption.JPName ? SpriteFontRegistry.UI_JP_NAME_NAME : SpriteFontRegistry.UI_SMALLTEXT_NAME));
            NameTextMan.Move(0, 6 * (AsteriskEngine.JapaneseStyleOption.JPName ? 1 : -1));
            SetName(nowText, true);
        }
    }
}
