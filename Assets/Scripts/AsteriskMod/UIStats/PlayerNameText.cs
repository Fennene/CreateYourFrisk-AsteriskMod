using UnityEngine;

namespace AsteriskMod
{
    public class PlayerNameText : MonoBehaviour
    {
        public LimitedLuaStaticTextManager NameTextMan { get; private set; }
        private string nowText = null;

        private bool initialized;
        internal static PlayerNameText instance;

        private void Awake()
        {
            instance = this;
            NameTextMan = GetComponent<LimitedLuaStaticTextManager>();
            NameTextMan._BanControlOverride = true;
        }

        private void Start()
        {
            NameTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetName(PlayerCharacter.instance.Name);
        }

        internal void SetName(string newName)
        {
            if (nowText == newName) return;
            nowText = newName;
            NameTextMan.SetText(new InstantTextMessage(nowText.ToUpper()));
            NameTextMan.enabled = true;
        }
    }
}
