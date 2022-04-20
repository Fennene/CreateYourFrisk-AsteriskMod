using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AsteriskMod
{
    public class UIStatsRelay : MonoBehaviour
    {
        public static bool UseOriginalUIStats
        {
            get { return UnitaleUtil.IsOverworld; }
        }

        private readonly string[] originalUINames = new[] { "NameLv", "HPRect" };
        private readonly string[] modifiedUINames = new[] { "*Name", "*Love", "*HPRect" };

        private void Awake()
        {
            string[] disableTarget = UseOriginalUIStats ? modifiedUINames : originalUINames;
            foreach (string objName in disableTarget)
            {
                Transform target = transform.Find(objName);
                if (target == null) Debug.LogWarning("GameObject \"" + objName + "\" is not found.");
                else target.gameObject.SetActive(false);
            }
            GetComponent<UIStats>().enabled = UseOriginalUIStats;
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

        public static void UpdateHP(float hpCurrent, bool instanceCheck = false)
        {
            if (UseOriginalUIStats)
            {
                if (!instanceCheck || UIStats.instance)
                {
                    UIStats.instance.setHP(hpCurrent);
                }
                return;
            }
            if (!instanceCheck || PlayerLifeUI.instance)
            {
                PlayerLifeUI.instance.SetHP(hpCurrent);
            }
        }

        public static void UpdateMaxHP(bool instanceCheck = false)
        {
            if (UseOriginalUIStats)
            {
                if (!instanceCheck || UIStats.instance)
                {
                    UIStats.instance.setMaxHP();
                }
                return;
            }
            if (!instanceCheck || PlayerLifeUI.instance)
            {
                PlayerLifeUI.instance.SetMaxHP();
            }
        }
    }
}
