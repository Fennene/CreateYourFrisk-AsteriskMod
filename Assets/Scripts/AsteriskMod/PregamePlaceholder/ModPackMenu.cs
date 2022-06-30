using AsteriskMod.UnityUI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod
{
    internal class ModPackMenu : MonoBehaviour
    {
        public Text description;

        public Text SelecterLabel;
        public Dropdown ModPackSelecter;

        public Button Save, Revert, Delete;
        public Image ButtonCover;

        public Button Reload, Exit;
        
        public ModPackWindow window;

        private List<ModPack> modPacks;

        private void Start()
        {
            description.text = EngineLang.Get("ModPack", "Description");

            SelecterLabel.text = EngineLang.Get("ModPack", "Description");
            UpdateSelecter();

            ButtonCover.enabled = false;

            Reload.onClick.SetListener(() =>
            {
                Asterisk.ModPackDatas = ModPack.GetModPacks();
                Asterisk.TargetModPack = -1;
                UpdateSelecter(-2);
            });
            Exit.onClick.SetListener(() => SceneManager.LoadScene("ModSelect"));
        }

        private void UpdateSelecter(int value = -2)
        {
            ModPackSelecter.options = new List<Dropdown.OptionData>();
            ModPackSelecter.options.Add(new Dropdown.OptionData { text = EngineLang.Get("ModPack", "SelecterOptionNoSelect") });
            for (var i = 0; i < Asterisk.ModPackDatas.Length; i++)
            {
                ModPackSelecter.options.Add(new Dropdown.OptionData { text = Asterisk.ModPackDatas[i].FileName });
            }
            ModPackSelecter.options.Add(new Dropdown.OptionData { text = EngineLang.Get("ModPack", "SelecterOptionCreate") });
            ModPackSelecter.RefreshShownValue();
            if (value > -2) ModPackSelecter.value = value + 1;
        }
    }
}
