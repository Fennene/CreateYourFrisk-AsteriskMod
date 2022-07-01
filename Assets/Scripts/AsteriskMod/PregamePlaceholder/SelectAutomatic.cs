using AsteriskMod.UnityUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class SelectAutomatic : MonoBehaviour
    {
        private static int CurrentSelectedMod;
        private Dictionary<string, Sprite> bgs = new Dictionary<string, Sprite>();
        private bool animationDone = true;
        private float animationTimer;

        private static float modListScroll;          // Used to keep track of the position of the mod list specifically. Resets if you press escape
        private static float encounterListScroll;    // Used to keep track of the position of the encounter list. Resets if you press escape

        private float ExitButtonAlpha = 5f;                 // Used to fade the "Exit" button in and out
        private float OptionsButtonAlpha = 5f;              // Used to fade the "Options" button in and out

        private static int selectedItem;                // Used to let users navigate the mod and encounter menus with the arrow keys!

        public GameObject encounterBox, devMod, content;
        public GameObject btnList, btnBack, btnNext, btnExit, btnOptions;
        public Text ListText, ListShadow, BackText, BackShadow, NextText, NextShadow, ExitText, ExitShadow, OptionsText, OptionsShadow;
        public GameObject ModContainer, ModBackground, ModTitle, ModTitleShadow, EncounterCount, EncounterCountShadow;
        public GameObject AnimContainer, AnimModBackground, AnimModTitle, AnimModTitleShadow, AnimEncounterCount, AnimEncounterCountShadow;

        public GameObject ModDescShadow, ModDesc, ExistDescInfoShadow, ExistDescInfo;
        public GameObject AnimModDescShadow, AnimModDesc;
        public GameObject ENLabelShadow, ENLabel, JPLabelShadow, JPLabel, RetroWarningTextShadow, RetroWarningText;
        public GameObject NoEncounterLabelShadow, NoEncounterLabel;

        private void Start()
        {
            Destroy(GameObject.Find("Player"));
            Destroy(GameObject.Find("Main Camera OW"));
            Destroy(GameObject.Find("Canvas OW"));
            Destroy(GameObject.Find("Canvas Two"));
            UnitaleUtil.firstErrorShown = false;

            if (!Mods.Reset())
            {
                GlobalControls.modDev = false;
                UnitaleUtil.DisplayLuaError("loading", "<b>Your mod folder is empty!</b>\nYou need at least 1 playable mod to use the Mod Selector.\n\n"
                    + "Remember:\n1. Mods whose names start with \"@\" do not count\n2. Folders without encounter files do not count");
                return;
            }

            // Bind button functions
            btnBack.GetComponent<Button>().onClick.SetListener(() =>
            {
                if (!animationDone) return;
                modFolderSelection();
                ScrollMods(-1);
            });
            btnNext.GetComponent<Button>().onClick.SetListener(() =>
            {
                if (!animationDone) return;
                modFolderSelection();
                ScrollMods( 1);
            });

            // Give the mod list button a function
            btnList.GetComponent<Button>().onClick.SetListener(() =>
            {
                if (animationDone) modFolderMiniMenu();
            });
            // Grab the exit button, and give it some functions
            btnExit.GetComponent<Button>().onClick.SetListener(() =>
            {
                SceneManager.LoadScene("Disclaimer");
                DiscordControls.StartTitle();
            });

            // Add devMod button functions
            if (GlobalControls.modDev) btnOptions.GetComponent<Button>().onClick.SetListener(() => SceneManager.LoadScene("Options"));

            // Crate Your Frisk initializer
            if (GlobalControls.crate)
            {
                //Exit button
                ExitText.text   = "← BYEE";
                ExitShadow.text = ExitText.text;

                //Options button
                if (GlobalControls.modDev)
                {
                    OptionsText.text   = "OPSHUNZ →";
                    OptionsShadow.text = OptionsText.text;
                }

                //Back button within scrolling list
                content.transform.Find("Back/Text").GetComponent<Text>().text = "← BCAK";

                //Mod list button
                ListText.gameObject.GetComponent<Text>().text   = "MDO LITS";
                ListShadow.gameObject.GetComponent<Text>().text = "MDO LITS";
            }

            // This check will be true if we just exited out of an encounter
            // If that's the case, we want to open the encounter list so the user only has to click once to re enter
            modFolderSelection();
            if (StaticInits.ENCOUNTER != "")
            {
                //Check to see if there is more than one encounter in the mod just exited from
                List<string>  encounters = new List<string>();
                DirectoryInfo di2        = new DirectoryInfo(Path.Combine(FileLoader.ModDataPath, "Lua/Encounters"));
                foreach (FileInfo f in di2.GetFiles("*.lua")) {
                    if (encounters.Count < 2)
                        encounters.Add(Path.GetFileNameWithoutExtension(f.Name));
                }

                if (encounters.Count > 1) {
                    // Highlight the chosen encounter whenever the user exits the mod menu
                    int temp = selectedItem;
                    encounterSelection();
                    selectedItem = temp;
                    content.transform.GetChild(selectedItem).GetComponent<MenuButton>().StartAnimation(1);
                }

                // Move the scrolly bit to where it was when the player entered the encounter
                content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, encounterListScroll);

                // Start the Exit button at half transparency
                ExitButtonAlpha                       = 0.5f;
                ExitText.GetComponent<Text>().color   = new Color(1f, 1f, 1f, 0.5f);
                ExitShadow.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0.5f);

                // Start the Options button at half transparency
                if (GlobalControls.modDev) {
                    OptionsButtonAlpha                       = 0.5f;
                    OptionsText.GetComponent<Text>().color   = new Color(1f, 1f, 1f, 0.5f);
                    OptionsShadow.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0.5f);
                }

                // Reset it to let us accurately tell if the player just came here from the Disclaimer scene or the Battle scene
                StaticInits.ENCOUNTER = "";
                // Player is coming here from the Disclaimer scene
            }
            else
            {
                // When the player enters from the Disclaimer screen, reset stored scroll positions
                modListScroll       = 0.0f;
                encounterListScroll = 0.0f;
            }
        }
    }
}
