using UnityEngine;

namespace AsteriskMod
{
    public class PlayerLoveText : MonoBehaviour
    {
        public UIStaticTextManager LoveTextMan { get; private set; }
        private int nowLove;

        internal static PlayerLoveText instance;

        private void Awake()
        {
            LoveTextMan = GetComponent<UIStaticTextManager>();
            instance = this;
        }

        private void Start()
        {
            LoveTextMan._UpdatePosition = () => { SetPosition((int)AsteriskUtil.CalcTextWidth(PlayerNameText.instance.NameTextMan, countEOLSpace: true)); };
            LoveTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetLove(PlayerCharacter.instance.LV);
        }

        internal void SetLove(int newLV)
        {
            if (LoveTextMan._controlOverride) return;
            if (nowLove == newLV) return;
            nowLove = newLV;
            LoveTextMan.SetText(new InstantTextMessage("LV " + nowLove.ToString().ToUpper()));
            LoveTextMan.enabled = true;
        }

        public void SetText(string newLV)
        {
            nowLove = 0;
            LoveTextMan.SetText(new InstantTextMessage("LV " + newLV.ToUpper()));
            LoveTextMan.enabled = true;
        }

        internal void SetPosition(int textLength, bool force = false)
        {
            if (LoveTextMan._controlOverride && !force) return;
            Vector2 pos = GetComponent<RectTransform>().anchoredPosition;
            pos.x = textLength + 30;
            GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
