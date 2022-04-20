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

        public static void UpdatePlayerInfo(string newName, int newLv)
        {
            if (UseOriginalUIStats)
            {
                UIStats.instance.setPlayerInfo(newName, newLv);
                return;
            }
            PlayerNameText.instance.SetName(newName);
            PlayerLoveText.instance.SetLove(newLv);
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
