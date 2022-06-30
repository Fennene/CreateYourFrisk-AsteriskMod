using AsteriskMod.UnityUI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod.ModPack
{
    internal class ModPackMenu : MonoBehaviour
    {
        public Text description;

        public Text SelecterLabel;
        public Dropdown ModPackSelecter;

        public Button Save, Revert, Delete;
        public Image ButtonCover;

        public Button Exit;
        
        public TextBoxWindow window;

        private List<ModPack> modPacks;

        private void Start()
        {
            description.text = EngineLang.Get("ModPack", "Description");

            SelecterLabel.text = EngineLang.Get("ModPack", "Description");
            UpdateSelecter();

            ButtonCover.enabled = false;

            Exit.onClick.SetListener(() => SceneManager.LoadScene("ModSelect"));
        }

        private void GetModPackFiles()
        {
            modPacks = new List<ModPack>();
            string modPackDir = Path.Combine(Application.persistentDataPath, "ModPack").Replace('\\', '/');
            if (!Directory.Exists(modPackDir))
            {
                try   { Directory.CreateDirectory(modPackDir); }
                catch { return; }
            }
            string[] filePathes;
            try   { filePathes = Directory.GetFiles(modPackDir, "*.modpack", SearchOption.TopDirectoryOnly); }
            catch { return; }
            for (var i = 0; i < filePathes.Length; i++)
            {
                ModPack _;
                try { _ = new ModPack(filePathes[i]); }
                catch { continue; }
                modPacks.Add(_);
            }
        }

        private void UpdateSelecter()
        {
            GetModPackFiles();
            ModPackSelecter.options = new List<Dropdown.OptionData>();
            ModPackSelecter.options.Add(new Dropdown.OptionData { text = EngineLang.Get("ModPack", "SelecterOptionNoSelect") });
            for (var i = 0; i < modPacks.Count; i++)
            {
                ModPackSelecter.options.Add(new Dropdown.OptionData { text = modPacks[i].fileName });
            }
            ModPackSelecter.options.Add(new Dropdown.OptionData { text = EngineLang.Get("ModPack", "SelecterOptionCreate") });
            ModPackSelecter.RefreshShownValue();
        }

        private int TargetPackIndex { get { return ModPackSelecter.value - 1; } }
    }
}
