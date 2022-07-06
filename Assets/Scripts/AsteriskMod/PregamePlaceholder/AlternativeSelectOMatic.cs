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
    public class AlternativeSelectOMatic : MonoBehaviour
    {
        private static int CurrentSelectedMod;
        private static int CurrentSelectedEncounter;
        private Dictionary<string, Sprite> bgs = new Dictionary<string, Sprite>();
        private bool animationDone = true;
        private float animationTimer;

        private static float modListScroll;          // Used to keep track of the position of the mod list specifically. Resets if you press escape
        private static float encounterListScroll;    // Used to keep track of the position of the encounter list. Resets if you press escape

        private float ExitButtonAlpha = 5f;                 // Used to fade the "Exit" button in and out
        private float OptionsButtonAlpha = 5f;              // Used to fade the "Options" button in and out

        private static int selectedItem;                // Used to let users navigate the mod and encounter menus with the arrow keys!

        public static bool openOptionWindow;
        private Button.ButtonClickedEvent events;
        private string[] CantOpenMHT = new string[] {
            "@0.5.0_SEE_CRATE", "@OverWorld Test", "@Title",
            "Encounter Skeleton", "Examples", "Examples 2", "RTLGeno",
            "Encounter Skeleton (Asterisk Mod)",
            "Asterisk Mod Features", "Asterisk Mod Features 2", "Asterisk Mod Features 3"
        };

        public GameObject encounterBox, devMod, content;
        public GameObject btnList, btnBack, btnNext, btnExit, btnOptions;
        public Text ListText, ListShadow, BackText, BackShadow, NextText, NextShadow, ExitText, ExitShadow, OptionsText, OptionsShadow;
        public GameObject ModContainer, ModBackground, ModTitle, ModTitleShadow, EncounterCount, EncounterCountShadow;
        public GameObject AnimContainer, AnimModBackground, AnimModTitle, AnimModTitleShadow, AnimEncounterCount, AnimEncounterCountShadow;

        public GameObject ModDescShadow, ModDesc, ExistDescInfoShadow, ExistDescInfo;
        public GameObject AnimModDescShadow, AnimModDesc;
        public GameObject ENLabelShadow, ENLabel, JPLabelShadow, JPLabel, RetroWarningTextShadow, RetroWarningText;
        public GameObject NoEncounterLabelShadow, NoEncounterLabel;

        public GameObject optionSelectWindow;
        public GameObject modPackMan, newMod, moddingHelper, cyfOption, asteriskOption;
        public GameObject descName, descDesc;

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

            ExistDescInfoShadow.GetComponent<Text>().text = EngineLang.Get("ModSelect", "DescVisible");
            ExistDescInfo      .GetComponent<Text>().text = EngineLang.Get("ModSelect", "DescVisible");
            NoEncounterLabelShadow.GetComponent<Text>().text = EngineLang.Get("ModSelect", "NoEncounter");
            NoEncounterLabel      .GetComponent<Text>().text = EngineLang.Get("ModSelect", "NoEncounter");

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
                ScrollMods(1);
            });

            // Give the mod list button a function
            btnList.GetComponent<Button>().onClick.SetListener(() =>
            {
                if (animationDone) modFolderMiniMenu();
            });
            // Grab the exit button, and give it some functions
            btnExit.GetComponent<Button>().onClick.SetListener(() =>
            {
                openOptionWindow = false;
                SceneManager.LoadScene("Disclaimer");
                DiscordControls.StartTitle();
            });

            // Add devMod button functions
            if (GlobalControls.modDev) btnOptions.GetComponent<Button>().onClick.SetListener(() => ToggleOptionSelectWindow());

            modPackMan.GetComponent<Button>().onClick.SetListener(() => SceneManager.LoadScene("ModPack"));
            //newMod.GetComponent<Button>().onClick.SetListener(() => SceneManager.LoadScene("NewMod"));
            moddingHelper.GetComponent<Button>().onClick.SetListener(() => SceneManager.LoadScene("MHTMenu"));
            cyfOption.GetComponent<Button>().onClick.SetListener(() => SceneManager.LoadScene("Options"));
            asteriskOption.GetComponent<Button>().onClick.SetListener(() => SceneManager.LoadScene("AsteriskOptions"));

            // Crate Your Frisk initializer
            if (GlobalControls.crate)
            {
                //Exit button
                ExitText.text = "← BYEE";
                ExitShadow.text = ExitText.text;

                //Options button
                if (GlobalControls.modDev)
                {
                    OptionsText.text = "OPSHUNZ →";
                    OptionsShadow.text = OptionsText.text;
                }

                //Back button within scrolling list
                content.transform.Find("Back/Text").GetComponent<Text>().text = "← BCAK";

                //Mod list button
                ListText.gameObject.GetComponent<Text>().text = "MDO LITS";
                ListShadow.gameObject.GetComponent<Text>().text = "MDO LITS";
            }

            // This check will be true if we just exited out of an encounter
            // If that's the case, we want to open the encounter list so the user only has to click once to re enter
            modFolderSelection();
            if (StaticInits.ENCOUNTER != "")
            {
                //Check to see if there is more than one encounter in the mod just exited from
                List<string> encounters = new List<string>();
                DirectoryInfo di2 = new DirectoryInfo(Path.Combine(FileLoader.ModDataPath, "Lua/Encounters"));
                foreach (FileInfo f in di2.GetFiles("*.lua"))
                {
                    if (encounters.Count < 2)
                        encounters.Add(Path.GetFileNameWithoutExtension(f.Name));
                }

                if (encounters.Count > 1)
                {
                    // Highlight the chosen encounter whenever the user exits the mod menu
                    int temp = selectedItem;
                    encounterSelection();
                    selectedItem = temp;
                    content.transform.GetChild(selectedItem).GetComponent<MenuButton>().StartAnimation(1);
                }

                // Move the scrolly bit to where it was when the player entered the encounter
                content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, encounterListScroll);

                // Start the Exit button at half transparency
                ExitButtonAlpha = 0.5f;
                ExitText.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.5f);
                ExitShadow.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0.5f);

                // Start the Options button at half transparency
                if (GlobalControls.modDev)
                {
                    OptionsButtonAlpha = 0.5f;
                    OptionsText.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.5f);
                    OptionsShadow.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0.5f);
                }

                // Reset it to let us accurately tell if the player just came here from the Disclaimer scene or the Battle scene
                StaticInits.ENCOUNTER = "";
                // Player is coming here from the Disclaimer scene
            }
            else
            {
                // When the player enters from the Disclaimer screen, reset stored scroll positions
                modListScroll = 0.0f;
                encounterListScroll = 0.0f;
            }

            optionSelectWindow.SetActive(false);
            if (openOptionWindow)
            {
                openOptionWindow = false;
                ToggleOptionSelectWindow();
            }
        }

        // A special function used specifically for error handling
        // It re-generates the mod list, and selects the first mod
        // Used for cases where the player selects a mod or encounter that no longer exists
        private void HandleErrors()
        {
            Debug.LogWarning("Mod or Encounter not found! Resetting mod list...");
            CurrentSelectedMod = 0;
            bgs = new Dictionary<string, Sprite>();
            Start();
        }

        private IEnumerator LaunchMod()
        {
            // First: make sure the mod is still here and can be opened
            if (!Mods.mods[CurrentSelectedMod].ReloadEncounters(false))
            {
                HandleErrors();
                yield break;
            }

            // Mods.mods[CurrentSelectedMod].ReloadModInfo();

            // Dim the background to indicate loading
            ModBackground.GetComponent<Image>().color = Mods.mods[CurrentSelectedMod].RawInfoData.LaunchingBackgroundColor;

            // Store the current position of the scrolly bit
            encounterListScroll = content.GetComponent<RectTransform>().anchoredPosition.y;

            yield return new WaitForEndOfFrame();
            StaticInits.Initialized = false;
            try
            {
                // --------------------------------------------------------------------------------
                //                          Asterisk Mod Modification
                // --------------------------------------------------------------------------------
                AsteriskEngine.IsSimulator = false;
                AsteriskEngine.PrepareMod(Mods.mods[CurrentSelectedMod].RealName, Mods.mods[CurrentSelectedMod].RawInfoData);
                // --------------------------------------------------------------------------------
                StaticInits.InitAll(true);
                if (UnitaleUtil.firstErrorShown)
                    throw new Exception();
                Debug.Log("Loading " + StaticInits.ENCOUNTER);
                GlobalControls.isInFight = true;
                DiscordControls.StartBattle(Mods.mods[CurrentSelectedMod].Title, Mods.mods[CurrentSelectedMod].Encounters[CurrentSelectedEncounter]);
                SceneManager.LoadScene("Battle");
            }
            catch (Exception e)
            {
                ModBackground.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f);
                Debug.LogError("An error occured while loading a mod:\n" + e.Message + "\n\n" + e.StackTrace);
            }
        }

        // Shows a mod's "page".
        private void ShowMod(int id)
        {
            // Error handler
            // If current index is now out of range OR currently selected mod is not present:
            if (CurrentSelectedMod < 0 || CurrentSelectedMod > Mods.mods.Count - 1 || !Mods.mods[CurrentSelectedMod].ReloadEncounters(false))
            {
                HandleErrors();
                return;
            }

            // Update currently selected mod folder
            StaticInits.MODFOLDER = Mods.mods[id].RealName;

            // Make clicking the background go to the encounter select screen
            ModBackground.GetComponent<Button>().onClick.RemoveAllListeners();
            ModBackground.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (animationDone)
                    encounterSelection();
            });

            // Update the background
            var ImgComp = ModBackground.GetComponent<Image>();
            // First, check if we already have this mod's background loaded in memory
            if (bgs.ContainsKey(Mods.mods[CurrentSelectedMod].RealName))
            {
                ImgComp.sprite = bgs[Mods.mods[CurrentSelectedMod].RealName];
            }
            else
            {
                // if not, find it and store it
                try
                {
                    Sprite thumbnail = SpriteUtil.FromFile(FileLoader.pathToModFile("Sprites/preview.png"));
                    ImgComp.sprite = thumbnail;
                }
                catch
                {
                    try
                    {
                        Sprite bg = SpriteUtil.FromFile(FileLoader.pathToModFile("Sprites/bg.png"));
                        ImgComp.sprite = bg;
                    }
                    catch { ImgComp.sprite = SpriteUtil.FromFile("Sprites/black.png"); }
                }
                bgs.Add(Mods.mods[CurrentSelectedMod].RealName, ImgComp.sprite);
            }

            // Get all encounters in the mod's Encounters folder
            Mods.mods[CurrentSelectedMod].Reload();

            // Update the text
            ModTitle.GetComponent<Text>().text = Mods.mods[id].RealName;
            // Crate your frisk version
            if (GlobalControls.crate)
                ModTitle.GetComponent<Text>().text = Temmify.Convert(Mods.mods[id].Title, true);
            ModTitleShadow.GetComponent<Text>().text = ModTitle.GetComponent<Text>().text;

            // List # of encounters, or name of encounter if there is only one
            if (Mods.mods[CurrentSelectedMod].RealEncounters.Length == 1)
            {
                // Make clicking the bg directly open the encounter
                ModBackground.GetComponent<Button>().onClick.SetListener(() =>
                {
                    if (!animationDone) return;
                    StaticInits.ENCOUNTER = Mods.mods[CurrentSelectedMod].RealEncounters[0];
                    CurrentSelectedEncounter = 0;
                    StartCoroutine(LaunchMod());
                });
            }
            EncounterCount.GetComponent<Text>().text = Mods.mods[CurrentSelectedMod].SubTitle;
            EncounterCountShadow.GetComponent<Text>().text = EncounterCount.GetComponent<Text>().text;

            // Update the color of the arrows
            if (CurrentSelectedMod == 0 && Mods.mods.Count == 1)
                BackText.color = new Color(0.25f, 0.25f, 0.25f, 1f);
            else
                BackText.color = new Color(1f, 1f, 1f, 1f);
            if (CurrentSelectedMod == Mods.mods.Count - 1 && Mods.mods.Count == 1)
                NextText.color = new Color(0.25f, 0.25f, 0.25f, 1f);
            else
                NextText.color = new Color(1f, 1f, 1f, 1f);


            // Set BG Color
            ModBackground.GetComponent<Image>().color = Mods.mods[CurrentSelectedMod].RawInfoData.BackgroundColor;

            // RetroModeCheck
            /**
            if (info.retroMode.HasValue)
            {
                RetroWarningTextShadow.GetComponent<Text>().enabled = (info.retroMode.Value ^ GlobalControls.retroMode);
                RetroWarningText      .GetComponent<Text>().enabled = (info.retroMode.Value ^ GlobalControls.retroMode);
                RetroWarningTextShadow.GetComponent<Text>().text = "! You should " + (info.retroMode.Value ? "enable" : "disable") + " Retrocompatibility Mode in CYF option !";
                RetroWarningText      .GetComponent<Text>().text = "! You should " + (info.retroMode.Value ? "enable" : "disable") + " Retrocompatibility Mode in CYF option !";
            }
            else
            {
                RetroWarningTextShadow.GetComponent<Text>().enabled = false;
                RetroWarningText      .GetComponent<Text>().enabled = false;
            }
            */

            ModTitleShadow.GetComponent<Text>().font = Mods.mods[CurrentSelectedMod].Font;
            ModTitle      .GetComponent<Text>().font = Mods.mods[CurrentSelectedMod].Font;
            EncounterCountShadow.GetComponent<Text>().font = Mods.mods[CurrentSelectedMod].Font;
            EncounterCount      .GetComponent<Text>().font = Mods.mods[CurrentSelectedMod].Font;

            // RichText
            /**
            ModTitleShadow.GetComponent<Text>().supportRichText = info.richText;
            ModTitle      .GetComponent<Text>().supportRichText = info.richText;
            EncounterCountShadow.GetComponent<Text>().supportRichText = info.richText;
            EncounterCount      .GetComponent<Text>().supportRichText = info.richText;
            */

            // Set Description's Alignment
            ModDescShadow.GetComponent<Text>().alignment = Mods.mods[CurrentSelectedMod].RawInfoData.DescriptionAlign;
            ModDesc      .GetComponent<Text>().alignment = Mods.mods[CurrentSelectedMod].RawInfoData.DescriptionAlign;
            // Set Description's Active
            bool hasDescription = !Mods.mods[CurrentSelectedMod].Description.IsNullOrWhiteSpace();
            ModDescShadow.SetActive(hasDescription && Asterisk.alwaysShowDesc);
            ModDesc      .SetActive(hasDescription && Asterisk.alwaysShowDesc);
            // Set Description
            ModDescShadow.GetComponent<Text>().text = Regex.Replace(Mods.mods[CurrentSelectedMod].Description, "<[^>]*?>", "");
            ModDesc      .GetComponent<Text>().text = Mods.mods[CurrentSelectedMod].Description;
            ExistDescInfoShadow.SetActive(hasDescription);
            ExistDescInfo      .SetActive(hasDescription);

            // Languages
            if (Mods.mods[CurrentSelectedMod].SupportedLanguages[0])
            {
                ENLabelShadow.GetComponent<Text>().color = new Color32(  0,   0,   0, 255);
                ENLabel      .GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                ENLabelShadow.GetComponent<Text>().color = new Color32( 0,  0,  0,  64);
                ENLabel      .GetComponent<Text>().color = new Color32(64, 64, 64, 255);
            }
            if (Mods.mods[CurrentSelectedMod].SupportedLanguages[1])
            {
                JPLabelShadow.GetComponent<Text>().color = new Color32(  0,   0,   0, 255);
                JPLabel      .GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                JPLabelShadow.GetComponent<Text>().color = new Color32( 0,  0,  0,  64);
                JPLabel      .GetComponent<Text>().color = new Color32(64, 64, 64, 255);
            }

            ModdingHelperTools.FakeStaticInits.MODFOLDER = Mods.mods[CurrentSelectedMod].RealName;
            ModdingHelperTools.FakeStaticInits.ENCOUNTER = Mods.mods[CurrentSelectedMod].RealEncounters[0];
            if (CantOpenMHT.Contains(ModdingHelperTools.FakeStaticInits.MODFOLDER))
            {
                moddingHelper.GetComponentInChildren<Text>().color = Color.gray;
                moddingHelper.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            else
            {
                moddingHelper.GetComponentInChildren<Text>().color = Color.white;
                moddingHelper.GetComponent<Button>().onClick.SetListener(() => SceneManager.LoadScene("MHTMenu"));
            }
        }

        // Goes to the next or previous mod with a little scrolling animation.
        // -1 for left, 1 for right
        private void ScrollMods(int dir)
        {
            // First, determine if the next mod should be shown
            bool animate = Mods.mods.Count > 1;
            //if ((dir == -1 && CurrentSelectedMod > 0) || (dir == 1 && CurrentSelectedMod < modDirs.Count - 1)) {

            // If the new mod is being shown, start the animation!
            if (!animate) return;
            animationTimer = dir / 10f;
            animationDone = false;

            // Enable the "ANIM" assets
            AnimContainer.SetActive(true);
            AnimContainer.transform.localPosition = new Vector2(0, 0);
            AnimModBackground.GetComponent<Image>().sprite = ModBackground.GetComponent<Image>().sprite;
            AnimModTitleShadow.GetComponent<Text>().font = ModTitleShadow.GetComponent<Text>().font;
            AnimModTitle      .GetComponent<Text>().font = ModTitle      .GetComponent<Text>().font;
            AnimModTitleShadow.GetComponent<Text>().text = ModTitleShadow.GetComponent<Text>().text;
            AnimModTitle      .GetComponent<Text>().text = ModTitle      .GetComponent<Text>().text;
            AnimEncounterCountShadow.GetComponent<Text>().font = EncounterCountShadow.GetComponent<Text>().font;
            AnimEncounterCount      .GetComponent<Text>().font = EncounterCount      .GetComponent<Text>().font;
            AnimEncounterCountShadow.GetComponent<Text>().text = EncounterCountShadow.GetComponent<Text>().text;
            AnimEncounterCount      .GetComponent<Text>().text = EncounterCount      .GetComponent<Text>().text;
            AnimModDescShadow.GetComponent<Text>().alignment = ModDescShadow.GetComponent<Text>().alignment;
            AnimModDesc      .GetComponent<Text>().alignment = ModDesc.GetComponent<Text>().alignment;
            AnimModDescShadow.GetComponent<Text>().text = ModDesc.activeSelf ? ModDescShadow.GetComponent<Text>().text : "";
            AnimModDesc      .GetComponent<Text>().text = ModDesc.activeSelf ? ModDesc      .GetComponent<Text>().text : "";

            // Move all real assets to the side
            ModBackground.transform.Translate(640 * dir, 0, 0);
            ModTitleShadow.transform.Translate(640 * dir, 0, 0);
            ModTitle      .transform.Translate(640 * dir, 0, 0);
            EncounterCountShadow.transform.Translate(640 * dir, 0, 0);
            EncounterCount      .transform.Translate(640 * dir, 0, 0);
            ModDescShadow.transform.Translate(640 * dir, 0, 0);
            ModDesc      .transform.Translate(640 * dir, 0, 0);

            // Actually choose the new mod
            CurrentSelectedMod = (CurrentSelectedMod + dir) % Mods.mods.Count;
            if (CurrentSelectedMod < 0) CurrentSelectedMod += Mods.mods.Count;

            ShowMod(CurrentSelectedMod);
        }

        // Used to animate scrolling left or right.
        private void Update()
        {
            // Animation updating section
            if (AnimContainer.activeSelf)
            {
                animationTimer = animationTimer > 0 ? Mathf.Floor(animationTimer + 1) : Mathf.Ceil(animationTimer - 1);

                int distance = (int)((20 - Mathf.Abs(animationTimer)) * 3.4 * -Mathf.Sign(animationTimer));

                AnimContainer.transform.Translate(distance, 0, 0);
                ModContainer.transform.Translate(distance, 0, 0);

                if (Mathf.Abs(animationTimer) == 20)
                {
                    AnimContainer.SetActive(false);

                    // Manual movement because I can't change the movement multiplier to a precise enough value
                    ModContainer.transform.Translate((int)(2 * -Mathf.Sign(animationTimer)), 0, 0);

                    animationTimer = 0;
                    animationDone = true;
                }
            }

            // Prevent scrolling too far in the encounter box
            if (encounterBox.activeSelf)
            {
                if (content.GetComponent<RectTransform>().anchoredPosition.y < -200)
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
                else if (content.GetComponent<RectTransform>().anchoredPosition.y > (content.transform.childCount - 1) * 30)
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (content.transform.childCount - 1) * 30);
            }

            // Detect hovering over the Exit button and handle fading
            if (ScreenResolution.mousePosition.x / ScreenResolution.displayedSize.x * 640 < 70 && Input.mousePosition.y / ScreenResolution.displayedSize.y * 480 > 450 && ExitButtonAlpha < 1f)
            {
                ExitButtonAlpha += 0.05f;
                ExitText.color = new Color(1f, 1f, 1f, ExitButtonAlpha);
                ExitShadow.color = new Color(0f, 0f, 0f, ExitButtonAlpha);
            }
            else if (ExitButtonAlpha > 0.5f)
            {
                ExitButtonAlpha -= 0.05f;
                ExitText.color = new Color(1f, 1f, 1f, ExitButtonAlpha);
                ExitShadow.color = new Color(0f, 0f, 0f, ExitButtonAlpha);
            }

            // Detect hovering over the Options button and handle fading
            if (GlobalControls.modDev)
            {
                if (ScreenResolution.mousePosition.x / ScreenResolution.displayedSize.x * 640 > 550 && Input.mousePosition.y / ScreenResolution.displayedSize.y * 480 > 450 && OptionsButtonAlpha < 1f)
                {
                    OptionsButtonAlpha += 0.05f;
                    OptionsText.color = new Color(1f, 1f, 1f, OptionsButtonAlpha);
                    OptionsShadow.color = new Color(0f, 0f, 0f, OptionsButtonAlpha);
                }
                else if (OptionsButtonAlpha > 0.5f)
                {
                    OptionsButtonAlpha -= 0.05f;
                    OptionsText.color = new Color(1f, 1f, 1f, OptionsButtonAlpha);
                    OptionsShadow.color = new Color(0f, 0f, 0f, OptionsButtonAlpha);
                }
            }

            if (openOptionWindow)
            {
                UpdateOptionWindow();
                return;
            }

            // Controls:

            ////////////////// Main: ////////////////////////////////////
            //    Z or Return: Start encounter (if mod has only one    //
            //                 encounter), or open encounter list      //
            //     Shift or X: Return to Disclaimer screen             //
            //        Up or C: Open the mod list                       //
            //           Left: Scroll left                             //
            //          Right: Scroll right                            //
            ////////////////// Encounter or Mod list: ///////////////////
            //    Z or Return: Start an encounter, or select a mod     //
            //     Shift or X: Exit                                    //
            //             Up: Move up                                 //
            //           Down: Move down                               //
            /////////////////////////////////////////////////////////////


            // Main controls:
            if (!encounterBox.activeSelf)
            {
                if (animationDone)
                {
                    //scroll left
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                        ScrollMods(-1);
                    //scroll right
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                        ScrollMods(1);
                    //open the mod list
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.C))
                    {
                        modFolderMiniMenu();
                        content.transform.GetChild(selectedItem).GetComponent<MenuButton>().StartAnimation(1);
                        // Open the encounter list or start the encounter (if there is only one encounter)
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                        ModBackground.GetComponent<Button>().onClick.Invoke();
                    //content.transform.GetChild(selectedItem).GetComponent<MenuButton>().StartAnimation(1);
                    // Show/Hide Description
                    if (Input.GetKeyDown(KeyCode.V))
                    {
                        ModDesc.SetActive(!ModDesc.activeSelf);
                        ModDescShadow.SetActive(!ModDescShadow.activeSelf);
                    }
                }

                // Return to the Disclaimer screen
                if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                    btnExit.GetComponent<Button>().onClick.Invoke();

            // Encounter or Mod List controls:
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    // Store previous value of selectedItem
                    int previousSelectedItem = selectedItem;

                    //move up
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                        selectedItem -= 1;
                    //move down
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                        selectedItem += 1;

                    // Keep the selector in-bounds!
                    if (selectedItem < 0)
                        selectedItem = content.transform.childCount - 1;
                    else if (selectedItem > content.transform.childCount - 1)
                        selectedItem = 0;

                    // Update the buttons!

                    // Animate the old button
                    GameObject previousButton = content.transform.GetChild(previousSelectedItem).gameObject;
                    previousButton.GetComponent<MenuButton>().StartAnimation(-1);
                    //previousButton.spriteState = SpriteState.

                    // Animate the new button
                    GameObject newButton = content.transform.GetChild(selectedItem).gameObject;
                    newButton.GetComponent<MenuButton>().StartAnimation(1);

                    // Scroll to the newly chosen button if it is hidden!
                    float buttonTopEdge = -newButton.GetComponent<RectTransform>().anchoredPosition.y + 100;
                    float buttonBottomEdge = -newButton.GetComponent<RectTransform>().anchoredPosition.y + 100 + 30;

                    float topEdge = content.GetComponent<RectTransform>().anchoredPosition.y;
                    float bottomEdge = content.GetComponent<RectTransform>().anchoredPosition.y + 230;

                    //button is above the top of the scrolly bit
                    if (topEdge > buttonTopEdge)
                        content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonTopEdge);
                    //button is below the bottom of the scrolly bit
                    else if (bottomEdge < buttonBottomEdge)
                        content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonBottomEdge - 230);
                }

                // Exit
                if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                    ModBackground.GetComponent<Button>().onClick.Invoke();
                // Select the mod or encounter
                else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                    content.transform.GetChild(selectedItem).gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }

        // Shows the "mod page" screen.
        private void modFolderSelection()
        {
            UnitaleUtil.printDebuggerBeforeInit = "";
            ShowMod(CurrentSelectedMod);

            //hide the 4 buttons if needed
            if (!GlobalControls.modDev)
                devMod.SetActive(false);

            //show the mod list button
            btnList.SetActive(true);

            // If the encounter box is visible, remove all encounter buttons before hiding
            if (encounterBox.activeSelf)
            {
                foreach (Transform b in content.transform)
                {
                    if (b.gameObject.name != "Back")
                        Destroy(b.gameObject);
                    else
                        b.GetComponent<MenuButton>().Reset();
                }
            }
            //hide the encounter selection box
            encounterBox.SetActive(false);
        }

        // Shows the list of available encounters in a mod.
        private void encounterSelection()
        {
            //hide the mod list button
            btnList.SetActive(false);

            //automatically choose "back"
            selectedItem = 0;

            // Make clicking the background exit the encounter selection screen
            ModBackground.GetComponent<Button>().onClick.SetListener(() =>
            {
                if (animationDone)
                    modFolderSelection();
            });
            //show the encounter selection box
            encounterBox.SetActive(true);
            //reset the encounter box's position
            content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            //give the back button its function
            GameObject back = content.transform.Find("Back").gameObject;
            back.GetComponent<Button>().onClick.SetListener(modFolderSelection);

            int id = Mods.FindIndex(StaticInits.MODFOLDER);
            Mods.mods[id].ReloadEncounters(true);

            if (Mods.mods[id].Encounters.Length == 0)
            {
                NoEncounterLabelShadow.GetComponent<Text>().enabled = true;
                NoEncounterLabel      .GetComponent<Text>().enabled = true;
                return;
            }
            NoEncounterLabelShadow.GetComponent<Text>().enabled = false;
            NoEncounterLabel      .GetComponent<Text>().enabled = false;

            for (var i = 0; i < Mods.mods[id].Encounters.Length; i++)
            {
                //create a button for each encounter file
                GameObject button = Instantiate(back);

                //set parent and name
                button.transform.SetParent(content.transform);
                button.name = "EncounterButton";

                //set position
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 70 - i * 30);

                //set color
                button.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                button.GetComponent<MenuButton>().NormalColor = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                button.GetComponent<MenuButton>().HoverColor = new Color(0.75f, 0.75f, 0.75f, 1f);
                button.transform.Find("Fill").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

                // set text
                button.transform.Find("Text").GetComponent<Text>().font = Mods.mods[id].Font;
                button.transform.Find("Text").GetComponent<Text>().text = Mods.mods[id].Encounters[i];
                if (GlobalControls.crate)
                    button.transform.Find("Text").GetComponent<Text>().text = Temmify.Convert(Mods.mods[id].Encounters[i], true);

                //finally, set function!
                string filename = Path.GetFileNameWithoutExtension(Mods.mods[id].RealEncounters[Mods.mods[id].EncounterIndexes[i]]);
                int tempCount = i + 1;

                button.GetComponent<Button>().onClick.SetListener(() =>
                {
                    selectedItem = tempCount;
                    StaticInits.ENCOUNTER = filename;
                    CurrentSelectedEncounter = selectedItem - 1;
                    StartCoroutine(LaunchMod());
                });
            }
        }

        // Opens the scrolling interface and lets the user browse their mods.
        private void modFolderMiniMenu()
        {
            // Hide the mod list button
            btnList.SetActive(false);

            // Automatically select the current mod when the mod list appears
            selectedItem = CurrentSelectedMod + 1;

            // Give the back button its function
            GameObject back = content.transform.Find("Back").gameObject;
            back.GetComponent<Button>().onClick.SetListener(() =>
            {
                // Reset the encounter box's position
                modListScroll = 0.0f;
                modFolderSelection();
            });

            // Make clicking the background exit this menu
            ModBackground.GetComponent<Button>().onClick.SetListener(() =>
            {
                if (!animationDone) return;
                // Store the encounter box's position so it can be remembered upon exiting a mod
                modListScroll = content.GetComponent<RectTransform>().anchoredPosition.y;
                modFolderSelection();
            });
            // Show the encounter selection box
            encounterBox.SetActive(true);
            // Move the encounter box to the stored position, for easier mod browsing
            content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, modListScroll);

            NoEncounterLabelShadow.GetComponent<Text>().enabled = false;
            NoEncounterLabel      .GetComponent<Text>().enabled = false;

            for (var i = 0; i < Mods.mods.Count; i++)
            {
                // Create a button for each mod
                GameObject button = Instantiate(back);

                //set parent and name
                button.transform.SetParent(content.transform);
                button.name = "ModButton";

                //set position
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 70 - i * 30);

                //set color
                button.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                button.GetComponent<MenuButton>().NormalColor = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                button.GetComponent<MenuButton>().HoverColor = new Color(0.75f, 0.75f, 0.75f, 1f);
                button.transform.Find("Fill").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

                button.transform.Find("Text").GetComponent<Text>().font = ModTitle.GetComponent<Text>().font;
                button.transform.Find("Text").GetComponent<Text>().text = Mods.mods[i].Title;
                if (GlobalControls.crate)
                    button.transform.Find("Text").GetComponent<Text>().text = Temmify.Convert(Mods.mods[i].Title, true);

                //finally, set function!
                int tempCount = i;

                button.GetComponent<Button>().onClick.SetListener(() =>
                {
                    // Store the encounter box's position so it can be remembered upon exiting a mod
                    modListScroll = content.GetComponent<RectTransform>().anchoredPosition.y;

                    CurrentSelectedMod = tempCount;
                    modFolderSelection();
                    ShowMod(CurrentSelectedMod);
                });
            }
        }

        private void ToggleOptionSelectWindow()
        {
            if (!GlobalControls.modDev) return;
            openOptionWindow = !openOptionWindow;
            btnList.SetActive(!openOptionWindow);
            btnBack.SetActive(!openOptionWindow);
            btnNext.SetActive(!openOptionWindow);
            if (openOptionWindow)
            {
                events = ModBackground.GetComponent<Button>().onClick;
                ModBackground.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                ModBackground.GetComponent<Button>().onClick.RemoveAllListeners();
                ModBackground.GetComponent<Button>().onClick.AddListener(() => ToggleOptionSelectWindow());
                OptionsText.text = GlobalControls.crate ? "CLOES →" : "Close →";
            }
            else
            {
                ModBackground.GetComponent<Button>().onClick.RemoveAllListeners();
                ModBackground.GetComponent<Button>().onClick = events;
                OptionsText.text = "";
                OptionsText.text = GlobalControls.crate ? "OPSHUNZ →" : "Options →";
            }
            OptionsShadow.text = OptionsText.text;
            optionSelectWindow.SetActive(openOptionWindow);
        }

        private void UpdateOptionWindow()
        {
            int mousePosX = (int)((ScreenResolution.mousePosition.x / ScreenResolution.displayedSize.x) * 640);
            int mousePosY = (int)((Input.mousePosition.y / ScreenResolution.displayedSize.y) * 480);
            string descriptionTitle = "Option";
            string description = EngineLang.Get("ModSelect", "OptionHover");
            if (90 <= mousePosX && mousePosX <= 310)
            {
                if (335 < mousePosY && mousePosY <= 375)
                {
                    descriptionTitle = "Manage ModPack";
                    description = EngineLang.Get("ModSelect", "OptionModPack");
                }
                else if (295 < mousePosY && mousePosY <= 335)
                {
                    descriptionTitle = "Create New Mod";
                    description = EngineLang.Get("ModSelect", "OptionNewMod");
                }
                else if (255 < mousePosY && mousePosY <= 295)
                {
                    descriptionTitle = "Modding Helper Tools";
                    description = EngineLang.Get("ModSelect", "OptionHelper");
                }
                else if (215 < mousePosY && mousePosY <= 255)
                {
                    descriptionTitle = "CYF Option";
                    description = EngineLang.Get("ModSelect", "OptionCYF");
                }
                else if (175 < mousePosY && mousePosY <= 215)
                {
                    descriptionTitle = "Asterisk Mod Option";
                    description = EngineLang.Get("ModSelect", "OptionAsterisk");
                }
            }
            if (GlobalControls.crate && Asterisk.language != Languages.Japanese)
            {
                if (descriptionTitle.StartsWith("Create"))
                {
                    descriptionTitle = descriptionTitle.Replace("Create", "");
                    description = description.Replace("Creates", "");
                    descriptionTitle = "CRATE" + Temmify.Convert(descriptionTitle);
                    description = "CRATES" + Temmify.Convert(description);
                }
                else
                {
                    descriptionTitle = Temmify.Convert(descriptionTitle);
                    description = Temmify.Convert(description);
                }
            }
            descName.GetComponent<Text>().text = descriptionTitle;
            descDesc.GetComponent<Text>().text = description;

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                ToggleOptionSelectWindow();
        }
    }
}