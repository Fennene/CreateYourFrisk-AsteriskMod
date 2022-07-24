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
            LoveTextMan._UpdatePosition = (force) => { SetPosition((int)AsteriskUtil.CalcTextWidth(PlayerNameText.instance.NameTextMan, countEOLSpace: true), force); };
            LoveTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetLove(PlayerCharacter.instance.LV);
        }

        internal void SetLove(int newLV)
        {
            if (LoveTextMan._textOverride) return;
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
            if (LoveTextMan._positionOverride && !force) return;
            Vector2 pos = GetComponent<RectTransform>().anchoredPosition;
            pos.x = textLength + 30;

            pos.x += Mathf.RoundToInt(LoveTextMan._relativePosition.x);
            pos.y += Mathf.RoundToInt(LoveTextMan._relativePosition.y);

            GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
