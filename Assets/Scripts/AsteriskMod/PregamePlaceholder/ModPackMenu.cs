using AsteriskMod.UnityUI;
using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod
{
    internal class ModPackMenu : MonoBehaviour
    {
        public static ModPackMenu Instance;

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

        private void Awake() { Instance = this; }

        private void Start()
        {
            Mods.Reset();
            
            if (Asterisk.TargetModPack < -1 || Asterisk.TargetModPack >= Mods.modPacks.Length)
            {
                Asterisk.TargetModPack = -1;
                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODPACK, DynValue.NewNumber(Asterisk.TargetModPack), true);
            }

            description.text = EngineLang.Get("ModPack", "Description");

            SelecterLabel.text = EngineLang.Get("ModPack", "SelecterLabel");
            UpdateSelecter();

            toggles = new List<Toggle>(0);

            Reload.onClick.SetListener(() =>
            {
                Mods.LoadModPacks();
                Asterisk.TargetModPack = -1;
                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODPACK, DynValue.NewNumber(Asterisk.TargetModPack), true);
                UpdateSelecter(-1);
            });
            Exit.onClick.SetListener(() => SceneManager.LoadScene("AlternativeModSelect"));

            ModPackSelecter.onValueChanged.SetListener((value) =>
            {
                Asterisk.TargetModPack = value - 1;
                if (value == 0)
                {
                    LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODPACK, DynValue.NewNumber(-1), true);
                    ButtonCover.enabled = true;
                    RemoveToggles();
                    return;
                }
                if (value >= ModPackSelecter.options.Count - 1)
                {
                    window.ResetWindow();
                    window.StartAnimation();
                    return;
                }
                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODPACK, DynValue.NewNumber(Asterisk.TargetModPack), true);
                ButtonCover.enabled = false;
                ShowModPack();
            });
            ModPackSelecter.value = Asterisk.TargetModPack + 1;

            Save.onClick.SetListener(() =>
            {
                if (Asterisk.TargetModPack == -1) return;
                List<string> mods = new List<string>();
                for (var i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i].isOn) mods.Add(Mods.realMods[i].RealName);
                }
                if (mods.Count <= 0) return;
                Mods.modPacks[Asterisk.TargetModPack].SaveToFile(mods.ToArray());
            });
            Revert.onClick.SetListener(() =>
            {
                if (Asterisk.TargetModPack == -1) return;
                for (var i = 0; i < toggles.Count; i++)
                {
                    toggles[i].GetComponent<Toggle>().isOn = Mods.modPacks[Asterisk.TargetModPack].ShowingMods.Contains(Mods.realMods[i].RealName);
                }
            });
            Delete.onClick.SetListener(() =>
            {
                if (Asterisk.TargetModPack == -1) return;
                Mods.modPacks[Asterisk.TargetModPack].DeleteFile();
                Reload.onClick.Invoke();
            });
        }

        private void UpdateSelecter(int value = -2)
        {
            ModPackSelecter.options = new List<Dropdown.OptionData>();
            ModPackSelecter.options.Add(new Dropdown.OptionData { text = EngineLang.Get("ModPack", "SelecterOptionNoSelect") });
            for (var i = 0; i < Mods.modPacks.Length; i++)
            {
                ModPackSelecter.options.Add(new Dropdown.OptionData { text = Mods.modPacks[i].FileName });
            }
            ModPackSelecter.options.Add(new Dropdown.OptionData { text = EngineLang.Get("ModPack", "SelecterOptionCreate") });
            ModPackSelecter.RefreshShownValue();
            if (value > -2) ModPackSelecter.value = value + 1;
        }

        private void ShowModPack()
        {
            toggles = new List<Toggle>(Mods.realMods.Count);
            for (var i = 0; i < Mods.realMods.Count; i++)
            {
                GameObject toggle = Instantiate(TemplateToogle);

                //toggle.transform.parent = ToggleParent.transform;
                toggle.transform.SetParent(ToggleParent.transform);
                toggle.name = "ModName";

                toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 80 - i * 30);

                toggle.transform.Find("Label").GetComponent<Text>().text = Mods.realMods[i].Title;

                toggle.GetComponent<Toggle>().isOn = Mods.modPacks[Asterisk.TargetModPack].ShowingMods.Contains(Mods.realMods[i].RealName);

                toggle.SetActive(true);

                toggles.Add(toggle.GetComponent<Toggle>());
            }
        }

        private void RemoveToggles()
        {
            for (var i = 0; i < toggles.Count; i++) Destroy(toggles[i].gameObject);
            toggles = new List<Toggle>(0);
        }

        public void ReloadModPack()
        {
            Mods.LoadModPacks();
            UpdateSelecter();
        }

        public void SetSelecterIndex(int index = -1)
        {
            if (Asterisk.TargetModPack < -1 || Asterisk.TargetModPack >= Mods.modPacks.Length)
            {
                index = -1;
            }
            //Asterisk.TargetModPack = index;
            //LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODPACK, DynValue.NewNumber(Asterisk.TargetModPack), true);
            //ModPackSelecter.value = Asterisk.TargetModPack + 1;
            ModPackSelecter.value = index + 1;
        }
    }
}
