using UnityEngine;

namespace AsteriskMod
{
    public class PlayerNameText : MonoBehaviour
    {
        public LimitedLuaStaticTextManager NameTextMan { get; private set; }
        private string nowText = null;

        internal static PlayerNameText instance;

        private void Awake()
        {
            NameTextMan = GetComponent<LimitedLuaStaticTextManager>();
            NameTextMan._BanControlOverride = true;
            instance = this;
        }

        private void Start()
        {
            if (AsteriskEngine.Japanese)
            {
                SetJP(true);
                return;
            }
            NameTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetName(PlayerCharacter.instance.Name);
        }

        internal void SetName(string newName, bool force = false)
        {
            if (nowText == newName && !force) return;
            nowText = newName;
            NameTextMan.SetText(new InstantTextMessage(AsteriskEngine.Japanese ? nowText : nowText.ToUpper()));
            NameTextMan.enabled = true;
            PlayerLoveText.instance.SetPosition(NameTextMan.GetTextWidth());
        }

        internal void SetJP(bool active)
        {
            NameTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_JP_NAME_NAME));
            NameTextMan.Move(0, 6 * (AsteriskEngine.Japanese ? 1 : -1));
            SetName(nowText, true);
        }
    }
}
