using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace AsteriskMod.ModdingHelperTools
{
    public class Menu : MonoBehaviour
    {
        // used to update the Description periodically
        private int DescriptionTimer;

        // game objects
        public GameObject Document, Sim, Exit;
        public Text TargetModName, Description, ErrorText;


        // For Document //

        private void OpenDocument()
        {
            string documentPath = FileLoader.DataRoot;
#if UNITY_EDITOR
            documentPath = Path.Combine(documentPath, "..");
            documentPath = Path.Combine(documentPath, "Documentation CYF 1.0");
#else
            documentPath = Path.Combine(documentPath, "Documentation CYF 0.6.5 Asterisk " + Asterisk.ModVersion);
#endif
            documentPath = Path.Combine(documentPath, "documentation.html");
            try { Process.Start(documentPath); }
            catch { /* ignore */ }
        }


        // For Simulator //

        private bool EncounterExists(string encounterFileName)
        {
            return File.Exists(Path.Combine(FileLoader.DataRoot, "Mods/" + FakeStaticInits.MODFOLDER + "/Lua/Encounters/" + encounterFileName + ".lua"));
        }

        private void ShowErrorMessage(string message)
        {
            ErrorText.text = "Error: " + message;
            ErrorText.enabled = true;
        }

        private bool CanLaunch()
        {
            if (!new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + FakeStaticInits.MODFOLDER + "/Lua/Encounters/")).Exists)
            {
                ShowErrorMessage("Mods/" + FakeStaticInits.MODFOLDER + " is not found.");
                return false;
            }
            if (!EncounterExists(FakeStaticInits.ENCOUNTER))
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(FakeFileLoader.ModDataPath, "Lua/Encounters"));
                List<string> encounters = di.GetFiles("*.lua").Select(f => Path.GetFileNameWithoutExtension(f.Name)).ToList();
                if (encounters.Count > 1) FakeStaticInits.ENCOUNTER = encounters[0];
                if (!EncounterExists(FakeStaticInits.ENCOUNTER))
                {
                    ShowErrorMessage("Mods/" + FakeStaticInits.MODFOLDER + " does not contains any encounters.");
                    return false;
                }
            }
            return true;
        }

        private IEnumerator LaunchSimulator()
        {
            if (!CanLaunch()) yield break;

            yield return new WaitForEndOfFrame();

            FakeStaticInits.Initialized = false;
            try
            {
                AsteriskEngine.IsSimulator = true;
                //FakeStaticInits.InitAll(/*true*/);
                FakeStaticInits.Start();

                if (UnitaleUtil.firstErrorShown)
                    throw new Exception();

                Debug.Log("Loading " + FakeStaticInits.ENCOUNTER);

                //GlobalControls.isInFight = true; //No.
                //DiscordControls.StartBattle(FakeStaticInits.MODFOLDER, FakeStaticInits.ENCOUNTER); //No.

                //AsteriskEngine.PrepareMod(); //Not needed
                SceneManager.LoadScene("MHTSim");
            }
            catch (Exception e)
            {
                ShowErrorMessage("Some error has occured while loading a mod!\nSee log file.");
                Debug.LogError("An error occured while loading a mod (Sim):\n" + e.Message + "\n\n" + e.StackTrace);
            }
        }


        // System //

        private void Start()
        {
            TargetModName.text = "Target Mod: " + FakeStaticInits.MODFOLDER;

            Document.GetComponent<Button>().onClick.AddListener(() => OpenDocument());
            Sim.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(LaunchSimulator()));
            Exit.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("ModSelect"));

            if (!GlobalControls.crate) return;
            Exit.GetComponentInChildren<Text>().text = "EXIT TOO MAD SELCT";
        }

        private string GetDescription(string buttonName)
        {
            string response;
            switch (buttonName)
            {
                case "Document":
                    response = "<color=#FF0>Open CYF Documentation</color>\n\n"
                             + "Opens the documentation of\nCreate Your Frisk.\n"
                             + "It is written about all of CYF.\n\n"
                             + "You should read document before asking\nany question to CYF Discord.";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Sim":
                    response = "<color=#FF0>Battle Simulator</color>\n\n"
                             + "simulates encountertext,\nsprite(bullet) object, text object\nand etc..."
                             + "\n\n<color=#FF0>Recommended to set window scale to 2!</color>";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                case "Exit":
                    response = "Returns to the Mod Select screen.";
                    return !GlobalControls.crate ? response : Temmify.Convert(response);
                default:
                    return !GlobalControls.crate ? "Hover over an option and its description will appear here!" : "HOVR OVR DA TING N GET TEXT HEAR!!";
            }
        }

        private void Update()
        {
            // update the description every 1/6th of a second
            if (DescriptionTimer > 0)
            {
                DescriptionTimer--;
            }
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
                    // Document
                    if (mousePosY <= 340 && mousePosY > 300)
                        hoverItem = "Document";
                    // Sim
                    else if (mousePosY <= 300 && mousePosY > 260)
                        hoverItem = "Sim";
                    /*
                    // Retro
                    else if (mousePosY <= 260 && mousePosY > 220)
                        hoverItem = "Retro";
                    // Fullscreen
                    else if (mousePosY <= 220 && mousePosY > 180)
                        hoverItem = "Fullscreen";
                    // Scale
                    else if (mousePosY <= 180 && mousePosY > 140)
                        hoverItem = "Scale";
                    // Discord
                    else if (mousePosY <= 140 && mousePosY > 100)
                        hoverItem = "Discord";
                    */
                    // Exit
                    else if (mousePosY <= 60 && mousePosY > 20)
                        hoverItem = "Exit";
                }
                Description.GetComponent<Text>().text = GetDescription(hoverItem);
            }
        }
    }
}