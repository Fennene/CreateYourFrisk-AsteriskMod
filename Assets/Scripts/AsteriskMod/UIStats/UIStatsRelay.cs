using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod
{
    public class UIStatsRelay
    {
        public static bool UseOriginalUIStats
        {
            get { return UnitaleUtil.IsOverworld; }
        }

        public static void UpdatePlayerInfo(string newName, int newLv, bool instanceCheck = false)
        {
            if (UseOriginalUIStats)
            {
                if (!instanceCheck || UIStats.instance)
                {
                    UIStats.instance.setPlayerInfo(newName, newLv);
                }
                return;
            }
            if (!instanceCheck || PlayerNameText.instance)
            {
                PlayerNameText.instance.SetName(newName);
            }
            if (!instanceCheck || PlayerLoveText.instance)
            {
                PlayerLoveText.instance.SetLove(newLv);
            }
        }

        public static void UpdateHP(float hpCurrent)
        {
            if (UseOriginalUIStats) UIStats.instance.setHP(hpCurrent);
        }

        public static void UpdateMaxHP()
        {
            if (UseOriginalUIStats) UIStats.instance.setMaxHP();
        }
    }
}
