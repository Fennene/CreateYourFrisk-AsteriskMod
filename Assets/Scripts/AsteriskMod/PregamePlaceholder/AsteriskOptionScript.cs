using MoonSharp.Interpreter;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AsteriskMod
{
    public class AsteriskOptionScript : MonoBehaviour
    {
        // used to prevent the player from erasing real/almighty globals or their save by accident
        private int RealGlobalCooldown;
        private int AlMightyGlobalCooldown;
        private int SaveCooldown;

        // used to update the Description periodically
        private int DescriptionTimer;

        // game objects
        public GameObject Language, UIChangeWithLang, ReplaceTitle, DescVisible, Protect, Experiment, Exit, Git;
        public Text Description;

        // Use this for initialization
        private void Start()
        {
            // add button functions

            // language
            Language.GetComponent<Button>().onClick.AddListener(() =>
            {
                AsteriskUtil.SwitchLanguage();

                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_LANG, DynValue.NewString(AsteriskUtil.ConvertFromLanguage(Asterisk.language, true)), true);

                Language.GetComponentInChildren<Text>().text = "Language: " + AsteriskUtil.ConvertFromLanguage(Asterisk.language, false);
            });
            Language.GetComponentInChildren<Text>().text = "Language: " + AsteriskUtil.ConvertFromLanguage(Asterisk.language, false);

            // UIを日本語表記にするかどうか

            UIChangeWithLang.GetComponent<Button>().onClick.AddListener(() =>
            {
                Asterisk.changeUIwithLanguage = !Asterisk.changeUIwithLanguage;

                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_UIWITHLANG, DynValue.NewBoolean(Asterisk.changeUIwithLanguage), true);

                UIChangeWithLang.GetComponentInChildren<Text>().text = "UIを日本語表記にするかどうか: " + (Asterisk.changeUIwithLanguage ? "有効" : "無効");
            });
            UIChangeWithLang.GetComponentInChildren<Text>().text = "UIを日本語表記にするかどうか: " + (Asterisk.changeUIwithLanguage ? "有効" : "無効");

            // replace modders mod title
            ReplaceTitle.GetComponent<Button>().onClick.AddListener(() =>
            {
                Asterisk.displayModInfo = !Asterisk.displayModInfo;

                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODINFO, DynValue.NewBoolean(Asterisk.displayModInfo), true);

                ReplaceTitle.GetComponentInChildren<Text>().text = "Replace Prepared Mods' Name: " + (Asterisk.displayModInfo ? "On" : "Off");
            });
            ReplaceTitle.GetComponentInChildren<Text>().text = "Replace Prepared Mods' Name: " + (Asterisk.displayModInfo ? "On" : "Off");

            // show always desc
            DescVisible.GetComponent<Button>().onClick.AddListener(() =>
            {
                Asterisk.alwaysShowDesc = !Asterisk.alwaysShowDesc;

                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_DESC, DynValue.NewBoolean(Asterisk.alwaysShowDesc), true);

                DescVisible.GetComponentInChildren<Text>().text = "Show Always Mods' Descriptions: " + (Asterisk.alwaysShowDesc ? "On" : "Off");
            });
            DescVisible.GetComponentInChildren<Text>().text = "Show Always Mods' Descriptions: " + (Asterisk.alwaysShowDesc ? "On" : "Off");

            // safe set almighty global
            Protect.GetComponent<Button>().onClick.AddListener(() =>
            {
                string text = AsteriskUtil.SwitchSafeSetAlMightyGlobalStatus();

                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_PROTECT, DynValue.NewBoolean(Asterisk.optionProtecter), true);
                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_PROTECT_ERROR, DynValue.NewBoolean(Asterisk.reportProtecter), true);

                Protect.GetComponentInChildren<Text>().text = "Allow illegal change option: " + text;
            });
            Protect.GetComponentInChildren<Text>().text = "Allow illegal change option: " + AsteriskUtil.GetSafeSetAlMightyGlobalStatus();

            // experiment
            Experiment.GetComponent<Button>().onClick.AddListener(() =>
            {
                Asterisk.experimentMode = !Asterisk.experimentMode;

                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_EXPERIMENT, DynValue.NewBoolean(Asterisk.experimentMode), true);

                Experiment.GetComponentInChildren<Text>().text = "Experimental Features: " + (Asterisk.experimentMode ? "On" : "Off");
            });
            Experiment.GetComponentInChildren<Text>().text = "Experimental Features: " + (Asterisk.experimentMode ? "On" : "Off");

            // exit
            Exit.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("ModSelect"); });

            // github
            Git.GetComponent<Button>().onClick.AddListener(() => { try { Process.Start("https://github.com/Fennene/CreateYourFrisk-AsteriskMod"); } catch { /* ignore */ } });

            // Crate Your Frisk
            if (!GlobalControls.crate) return;
            // labels
            GameObject.Find("OptionsLabel").GetComponent<Text>().text = "OPSHUNS";
            GameObject.Find("DescriptionLabel").GetComponent<Text>().text = "MORE TXET";

            // buttons
            //ResetRG.GetComponentInChildren<Text>().text = "RESTE RELA GOLBALZ";
            //ResetAG.GetComponentInChildren<Text>().text = "RESTE ALMIGTY GOLBALZ";
            //ClearSave.GetComponentInChildren<Text>().text = "WYPE SAV";
            Exit.GetComponentInChildren<Text>().text = "EXIT TOO MAD SELCT";
        }

        // Gets the text the description should use based on what button is currently being hovered over
        private string GetDescription(string buttonName)
        {
            string response;
            switch (buttonName)
            {
                case "Lang":
                    response = "Display Language\n\n"
                             + "Switches display language\nto English or Japanese.";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "LangWithUI":
                    response = "This option is for when engine's language is Japanese.";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Title":
                    response = "Replace Prepared Mods' Title\n\n"
                             + "Whether replace mods' title and subtitle to prepared from modders.\n\n"
                             + "Default by On";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Desc":
                    response = "Always Description Visible\n\n"
                             + "Whether mods' descriptions are always shown or not.\n\n"
                             + "Default by On";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "SafeSetAlMightGlobal":
                    response = "Permission of changing system option's data from mods' code.\n\n"
                             + "Save data of option also uses AlMighty Global, so mod can change that data forcely.\n"
                             + "This feature will prevent that illegal change.\n\n"
                             + "You can select option: Error, Ignore, Allow.\n"
                             + "<b>Error</b> shows error message if that change happens.\n"
                             + "<b>Ignore</b> prevents that change and continues mod running.\n"
                             + "<b>Allow</b> allows that change.\n\n"
                             + "Default by Error";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Experiment":
                    response = "Experimental Features\n\n"
                             + "This feature allows the mod uses experimental features.\n"
                             + "Experimental Features contains the feature that is added in the future update or may be unstable or occur error.\n\n"
                             + "Default by Off";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Template":
                    response = "\n\n"
                             + "\n\n"
                             + "Default by ";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Exit":
                    response = "Returns to the Mod Select screen.";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Git":
                    response = "Goes to AsteriskMod's GitHub page.";
                    return response; //!GlobalControls.crate ? response : Temmify.Convert(response);
                default:
                    return !GlobalControls.crate ? "Hover over an option and its description will appear here!" : "HOVR OVR DA TING N GET TEXT HEAR!!";
            }
        }

        private string GetDescriptionNew(string buttonName)
        {
            if (GlobalControls.crate) return GetDescription(buttonName);
            if (EngineLang.Exists("AsteriskOption", buttonName)) return EngineLang.Get("AsteriskOption", buttonName);
            return EngineLang.Get("AsteriskOption", "Hover");
        }

        // Used to animate scrolling left or right.
        private void Update()
        {
            // update the description every 1/6th of a second
            if (DescriptionTimer > 0)
                DescriptionTimer--;
            else
            {
                DescriptionTimer = 10;

                // try to find which button the player is hovering over
                string hoverItem = "";
                // if the player is within the range of the buttons
                int mousePosX = (int)((ScreenResolution.mousePosition.x / ScreenResolution.displayedSize.x) * 640);
                int mousePosY = (int)((Input.mousePosition.y / ScreenResolution.displayedSize.y) * 480);
                if (mousePosX >= 40 && mousePosX <= 290)
                {
                    // Language
                    if (mousePosY <= 420 && mousePosY > 380)
                        hoverItem = "Lang";
                    else if (mousePosY <= 380 && mousePosY > 340)
                        hoverItem = "LangWithUI";
                    // ReplaceTitle
                    else if (mousePosY <= 340 && mousePosY > 300)
                        hoverItem = "Title";
                    // ShowAlwaysDesc
                    else if (mousePosY <= 300 && mousePosY > 260)
                        hoverItem = "Desc";
                    // SafeAlMightyGlobal
                    else if (mousePosY <= 260 && mousePosY > 220)
                        hoverItem = "SafeSetAlMightGlobal";
                    // Experiment
                    else if (mousePosY <= 220 && mousePosY > 180)
                        hoverItem = "Experiment";
                    /*
                    else if (mousePosY <= 180 && mousePosY > 140)
                        hoverItem = "";
                    else if (mousePosY <= 140 && mousePosY > 100)
                        hoverItem = "";
                    */
                    // Exit
                    else if (mousePosY <= 60 && mousePosY > 20)
                        hoverItem = "Exit";
                }
                if (mousePosX >= 350 && mousePosX <= 600)
                {
                    if (mousePosY <= 60 && mousePosY > 20)
                        hoverItem = "Git";
                }

                Git.SetActive(hoverItem != "SafeSetAlMightGlobal");

                Description.GetComponent<Text>().text = GetDescriptionNew(hoverItem);
            }

            /*
            // make the player click twice to reset RG or AG, or to wipe their save
            if (RealGlobalCooldown > 0)
                RealGlobalCooldown -= 1;
            else if (RealGlobalCooldown == 0)
            {
                RealGlobalCooldown = -1;
                ResetRG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Reset Real Globals" : "RSETE RAEL GLOBALS";
            }

            if (AlMightyGlobalCooldown > 0)
                AlMightyGlobalCooldown -= 1;
            else if (AlMightyGlobalCooldown == 0)
            {
                AlMightyGlobalCooldown = -1;
                ResetAG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Reset AlMighty Globals" : "RESET ALIMGHTY";
            }

            if (SaveCooldown > 0)
                SaveCooldown -= 1;
            else if (SaveCooldown == 0)
            {
                SaveCooldown = -1;
                ClearSave.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Wipe Save" : "WYPE SAV";
            }
            */
        }
    }
}
