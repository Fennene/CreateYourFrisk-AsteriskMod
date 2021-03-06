using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class UIStatsRelay : MonoBehaviour
    {
        private readonly string[] originalUINames = new[] { "NameLv", "HPRect" };
        private readonly string[] modifiedUINames = new[] { "*Name", "*Love", "*HPRect" };

        private void Awake()
        {
            string[] disableTarget = AsteriskUtil.IsCYFOverworld ? modifiedUINames : originalUINames;
            foreach (string objName in disableTarget)
            {
                Transform target = transform.Find(objName);
                if (target == null) Debug.LogWarning("GameObject \"" + objName + "\" is not found.");
                else target.gameObject.SetActive(false);
            }
            GetComponent<UIStats>().enabled = AsteriskUtil.IsCYFOverworld;
        }

        public static void UpdatePlayerInfo(string newName, int newLv, bool instanceCheck = false)
        {
            if (AsteriskUtil.IsCYFOverworld)
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
            if (AsteriskUtil.IsCYFOverworld)
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
            if (AsteriskUtil.IsCYFOverworld)
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

        public static void ChangeHPLabel(bool instanceCheck = false)
        {
            if (AsteriskUtil.IsCYFOverworld)
            {
                GameObject.Find("HPLabelCrate").GetComponent<Image>().enabled = true;
                GameObject.Find("HPLabel").GetComponent<Image>().enabled = false;
                return;
            }
            if (!instanceCheck || PlayerLifeUI.instance)
            {
                PlayerLifeUI.instance.ChangeHPLabel();
            }
        }
    }
}
