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
        private PlayerLifeBar lifebar;
        private GameObject lifeTextCore;
        private LimitedLuaStaticTextManager lifeTextMan;

        internal static PlayerLifeUI instance;

        private void Awake()
        {
            hpRect = gameObject;

            lifebar = gameObject.GetComponentInChildren<PlayerLifeBar>();
            //* lifebarRt = lifebar.GetComponent<RectTransform>();

            lifeTextMan = lifebar.gameObject.GetComponentInChildren<LimitedLuaStaticTextManager>();

            //* lifeTextCore = GameObject.Find("*LifeTextParent");
            lifeTextCore = lifeTextMan.gameObject;
            lifeTextCore.transform.position = new Vector3(lifeTextCore.transform.position.x, lifeTextCore.transform.position.y - 1, lifeTextCore.transform.position.z);

            //* lifeTextMan = lifeTextCore.GetComponent<LimitedLuaStaticTextManager>();

            instance = this;
        }

        private void Start()
        {
            lifeTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
            SetMaxHP();
        }

        internal void SetHP(float hpCurrent)
        {
            /**
            float hpMax = PlayerCharacter.instance.MaxHP,
                  hpFrac = hpCurrent / hpMax;
            lifebar.setInstant(hpFrac);
            */
            lifebar.setHP(hpCurrent);
            int count = UnitaleUtil.DecimalCount(hpCurrent);
            string sHpCurrent = hpCurrent < 10 ? "0" + hpCurrent.ToString("F" + count) : hpCurrent.ToString("F" + count);
            //* string sHpMax = hpMax < 10 ? "0" + hpMax : "" + hpMax;
            //* lifeTextMan.SetText(new InstantTextMessage(sHpCurrent + " / " + sHpMax));
            // In Undertale, It displays Max HP as it is even if Max HP is less than 10. Undertaleでは最大HPは10未満であろうとそのまま表示する。(先頭に0がつかない)
            lifeTextMan.SetText(new InstantTextMessage(sHpCurrent + " / " + PlayerCharacter.instance.MaxHP));
        }

        internal void SetMaxHP()
        {
            //* lifebarRt.sizeDelta = new Vector2(Mathf.Min(120, PlayerCharacter.instance.MaxHP * 1.2f), lifebarRt.sizeDelta.y);
            //* SetHP(PlayerCharacter.instance.HP);
            lifebar.setMaxHP();
        }
    }
}
