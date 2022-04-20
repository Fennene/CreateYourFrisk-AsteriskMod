using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod
{
    public class PlayerUtil
    {
        public static LimitedLuaStaticTextManager Name { get { return PlayerNameText.instance.NameTextMan; } }

        public static LimitedLuaStaticTextManager Love { get { return PlayerLoveText.instance.LoveTextMan; } }

        public static PlayerLifeBar HPBar { get { return PlayerLifeUI.instance.LifeBar; } }

        public static LimitedLuaStaticTextManager HPText { get { return PlayerLifeUI.instance.LifeTextMan; } }

        public static void SetLV(string lv)
        {
            PlayerLoveText.instance.SetText(lv);
        }

        public static void SetHPControlOverride(bool active)
        {
            PlayerLifeUI.instance.SetHPControlOverride(active);
        }

        public static void SetHP(float hp, float maxhp, bool updateText = true)
        {
            PlayerLifeUI.instance.SetHPOverride(hp, maxhp, updateText);
        }

        public static void SetHPText(float hp, float maxhp)
        {
            PlayerLifeUI.instance.SetHPTextFromNumber(hp, maxhp);
        }
    }
}
