using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AsteriskMod.ModdingHelperTools
{
    public class Menu : MonoBehaviour
    {
        // used to update the Description periodically
        private int DescriptionTimer;

        // game objects
        public GameObject Document, Sim, Exit;
        public Text TargetModName, Description;

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

        private bool CanLaunch()
        {
            return new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + FakeStaticInits.MODFOLDER + "/Lua/Encounters/")).Exists
                      && File.Exists(Path.Combine(FileLoader.DataRoot, "Mods/" + FakeStaticInits.MODFOLDER + "/Lua/Encounters/" + FakeStaticInits.ENCOUNTER + ".lua"));
        }

        private IEnumerator LaunchSimulator()
        {
            if (!CanLaunch())
            {
                yield break;
            }
            yield return new WaitForEndOfFrame();
            //SceneManager.LoadScene("MHTSim");
        }

        private void Start()
        {
            TargetModName.text = "Target Mod: " + FakeStaticInits.MODFOLDER;

            Document.GetComponent<Button>().onClick.AddListener(() => OpenDocument());
            //Sim.GetComponent<Button>().onClick.AddListener(() => LaunchSimulator());
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
                             + "simulates encountertext,\nsprite(bullet) object, text object\nand etc...";
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