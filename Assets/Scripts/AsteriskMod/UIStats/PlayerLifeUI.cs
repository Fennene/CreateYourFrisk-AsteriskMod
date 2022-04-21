using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class PlayerLifeUI : MonoBehaviour
    {
        private GameObject hpRect; // self
        //* private RectTransform lifebarRt;

        private Image hpLabel;

        public PlayerLifeBar LifeBar { get; private set; }
        private GameObject lifeTextCore;
        public LimitedLuaStaticTextManager LifeTextMan { get; private set; }

        internal static PlayerLifeUI instance;

        private void Awake()
        {
            hpRect = gameObject;

            LifeBar = gameObject.GetComponentInChildren<PlayerLifeBar>();
            LifeBar.Initialize(true);
            //* lifebarRt = lifebar.GetComponent<RectTransform>();

            LifeTextMan = LifeBar.gameObject.GetComponentInChildren<LimitedLuaStaticTextManager>();
            LifeTextMan._SetText = ((text) => { LifeTextMan.SetText(new InstantTextMessage(text)); });

            //* lifeTextCore = GameObject.Find("*LifeTextParent");
            lifeTextCore = LifeTextMan.gameObject;
            lifeTextCore.transform.position = new Vector3(lifeTextCore.transform.position.x, lifeTextCore.transform.position.y - 1, lifeTextCore.transform.position.z);

            //* lifeTextMan = lifeTextCore.GetComponent<LimitedLuaStaticTextManager>();

            instance = this;
        }

        private void Start()
        {
            LifeTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetMaxHP();

            // In Undertale, the position of HP's object is fixed. UndertaleではHPを表示するオブジェクトの位置は固定されている。
            //hpRect.transform.position = new Vector3(hpRect.transform.parent.position.x + (PlayerCharacter.instance.Name.Length > 6 ? 286.1f : 215.1f), hpRect.transform.position.y, hpRect.transform.position.z);

            Image hpl = transform.Find("*HPLabel").GetComponent<Image>();
            Image phl = transform.Find("*HPLabelCrate").GetComponent<Image>();
            hpl.enabled = !GlobalControls.crate;
            phl.enabled = GlobalControls.crate;
            hpLabel = GlobalControls.crate ? phl : hpl;
        }

        internal void SetHP(float hpCurrent)
        {
            /**
            float hpMax = PlayerCharacter.instance.MaxHP,
                  hpFrac = hpCurrent / hpMax;
            lifebar.setInstant(hpFrac);
            */
            LifeBar.setHP(hpCurrent);
            if (LifeTextMan._controlOverride) return;
            int count = UnitaleUtil.DecimalCount(hpCurrent);
            string sHpCurrent = hpCurrent < 10 ? "0" + hpCurrent.ToString("F" + count) : hpCurrent.ToString("F" + count);
            //* string sHpMax = hpMax < 10 ? "0" + hpMax : "" + hpMax;
            //* lifeTextMan.SetText(new InstantTextMessage(sHpCurrent + " / " + sHpMax));
            // In Undertale, it displays Max HP as it is even if Max HP is less than 10. Undertaleでは最大HPは10未満であろうとそのまま表示する。(先頭に0がつかない)
            LifeTextMan.SetText(new InstantTextMessage(sHpCurrent + " / " + PlayerCharacter.instance.MaxHP));
        }

        internal void SetMaxHP()
        {
            //* lifebarRt.sizeDelta = new Vector2(Mathf.Min(120, PlayerCharacter.instance.MaxHP * 1.2f), lifebarRt.sizeDelta.y);
            //* SetHP(PlayerCharacter.instance.HP);
            LifeBar.setMaxHP(true);
            SetHP(PlayerCharacter.instance.HP);
        }

        public void SetHPControlOverride(bool active)
        {
            LifeBar.SetControlOverride(active);
            LifeTextMan.SetControlOverride(active);
        }

        public void SetHPOverride(float hp, float maxhp, bool updateText = true)
        {
            LifeBar.SetHP(hp, maxhp);
            if (!updateText) return;
            SetHPTextFromNumber(hp, maxhp);
        }

        public void SetHPTextFromNumber(float hp, float maxhp)
        {
            int count = UnitaleUtil.DecimalCount(hp);
            string text = hp < 10 ? "0" + hp.ToString("F" + count) : hp.ToString("F" + count);
            count = UnitaleUtil.DecimalCount(maxhp);
            text += " / " + maxhp.ToString("F" + count);
            LifeTextMan.SetText(text);
        }
    }
}
