using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class PlayerLoveText : MonoBehaviour
    {
        public LimitedLuaStaticTextManager LoveTextMan { get; private set; }
        private int nowLove;

        internal static PlayerLoveText instance;

        private void Awake()
        {
            instance = this;
            LoveTextMan = GetComponent<LimitedLuaStaticTextManager>();
            LoveTextMan._SetText = SetText;
        }

        private void Start()
        {
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

        internal void SetPosition(string newName)
        {
        }
    }
}
