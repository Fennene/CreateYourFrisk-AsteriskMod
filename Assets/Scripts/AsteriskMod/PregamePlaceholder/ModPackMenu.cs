using AsteriskMod.UnityUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod
{
    internal class ModPackMenu : MonoBehaviour
    {
        private static List<DirectoryInfo> modDirs;
        private static List<LegacyModInfo> modInfos;

        public Text description;

        public Text SelecterLabel;
        public Dropdown ModPackSelecter;

        public GameObject ToggleParent;
        public GameObject TemplateToogle;
        private List<Toggle> toggles;

        public Button Save, Revert, Delete;
        public Image ButtonCover;

        public Button Reload, Exit;
        
        public ModPackWindow window;

        private void Start()
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods"));
            var modDirsTemp = di.GetDirectories();
            List<DirectoryInfo> purged = (from modDir in modDirsTemp
                                          where new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + modDir.Name + "/Lua/Encounters")).Exists
                                          let hasEncounters = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + modDir.Name + "/Lua/Encounters")).GetFiles("*.lua").Any()
                                          where hasEncounters && (modDir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && !modDir.Name.StartsWith("@")
                                          select modDir).ToList();
            modDirs = purged;
            modDirs.Sort((a, b) => a.Name.CompareTo(b.Name));
            modInfos = new List<LegacyModInfo>();
            for (var i = 0; i < modDirs.Count; i++) modInfos.Add(LegacyModInfo.Get(modDirs[i].Name));

            description.text = EngineLang.Get("ModPack", "Description");

            SelecterLabel.text = EngineLang.Get("ModPack", "SelecterLabel");
            UpdateSelecter();

            toggles = new List<Toggle>(0);

            ButtonCover.enabled = false;

            Reload.onClick.SetListener(() =>
            {
                Asterisk.ModPackDatas = ModPack.GetModPacks();
                Asterisk.TargetModPack = -1;
                UpdateSelecter();
            });
            Exit.onClick.SetListener(() => SceneManager.LoadScene("ModSelect"));

            ModPackSelecter.onValueChanged.SetListener((value) =>
            {
                Asterisk.TargetModPack = value - 1;
                if (value == 0)
                {
                    RemoveToggles();
                    return;
                }
                if (value >= ModPackSelecter.options.Count - 1)
                {
                    Asterisk.TargetModPack = -1;
                    //window.Title = "";
                    window.StartAnimation();
                    return;
                }
                ShowModPack();
            });
            ModPackSelecter.value = Asterisk.TargetModPack + 1;
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

        private void ShowModPack()
        {
            toggles = new List<Toggle>(modDirs.Count);
            for (var i = 0; i < modDirs.Count; i++)
            {
                GameObject toggle = Instantiate(TemplateToogle);

                toggle.transform.parent = ToggleParent.transform;
                toggle.name = "ModName";

                toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 80 - i * 30);

                string text = modDirs[i].Name;
                if (modInfos[i].title != "" && Asterisk.displayModInfo) text = modInfos[i].title;
                toggle.transform.Find("Label").GetComponent<Text>().text = text;

                toggle.GetComponent<Toggle>().isOn = Asterisk.ModPackDatas[Asterisk.TargetModPack].ShowingMods.Contains(modDirs[i].Name);

                toggle.SetActive(true);

                toggles.Add(toggle.GetComponent<Toggle>());
            }
        }

        private void RemoveToggles()
        {
            for (var i = 0; i < toggles.Count; i++) Destroy(toggles[i]);
            toggles = new List<Toggle>(0);
        }
    }
}
