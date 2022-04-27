using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AsteriskMod
{
    public class SelectOMaticOptionManager : MonoBehaviour
    {
        public static SelectOMaticOptionManager instance;

        private void Awake() { instance = this; }

        private SelectOMatic _selectOMatic;
        private Button.ButtonClickedEvent events;

        public GameObject optionSelectWindow;
        public GameObject newMod, openDocument, cyfOption, asteriskOption;
        public GameObject descName, descDesc;

        public static bool opened;

        public void StartAlt(SelectOMatic selectOMatic)
        {
            if (!GlobalControls.modDev) return;
            newMod.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("NewMod"); });
            openDocument.GetComponent<Button>().onClick.AddListener(() => { OpenDocument(); });
            cyfOption.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("Options"); });
            asteriskOption.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("AsteriskOptions"); });
            _selectOMatic = selectOMatic;
            _selectOMatic.btnExit.GetComponent<Button>().onClick.AddListener(() => { opened = false; });
            _selectOMatic.btnOptions.GetComponent<Button>().onClick.RemoveAllListeners();
            _selectOMatic.btnOptions.GetComponent<Button>().onClick.AddListener(() => { ToggleOptionSelectWindow(); });
            optionSelectWindow.SetActive(false);
            if (opened)
            {
                opened = false;
                ToggleOptionSelectWindow();
            }
        }

        private void ToggleOptionSelectWindow()
        {
            if (!GlobalControls.modDev) return;
            opened = !opened;
            _selectOMatic.btnList.SetActive(!opened);
            _selectOMatic.btnBack.SetActive(!opened);
            _selectOMatic.btnNext.SetActive(!opened);
            if (opened)
            {
                events = _selectOMatic.ModBackground.GetComponent<Button>().onClick;
                _selectOMatic.ModBackground.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                _selectOMatic.ModBackground.GetComponent<Button>().onClick.AddListener(() => ToggleOptionSelectWindow());
                _selectOMatic.OptionsText.text = GlobalControls.crate ? "CLOES →" : "Close →";
            }
            else
            {
                _selectOMatic.ModBackground.GetComponent<Button>().onClick.RemoveAllListeners();
                _selectOMatic.ModBackground.GetComponent<Button>().onClick = events;
                _selectOMatic.OptionsText.text = "";
                _selectOMatic.OptionsText.text = GlobalControls.crate ? "OPSHUNZ →" : "Options →";
            }
            _selectOMatic.OptionsShadow.text = _selectOMatic.OptionsText.text;
            optionSelectWindow.SetActive(opened);
        }

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

        private void Update()
        {
            if (!opened) return;
            int mousePosX = (int)((ScreenResolution.mousePosition.x / ScreenResolution.displayedSize.x) * 640);
            int mousePosY = (int)((Input.mousePosition.y / ScreenResolution.displayedSize.y) * 480);
            string descriptionTitle = "Option";
            string description = "Hover over an option and its description will appear here!";
            if (90 <= mousePosX && mousePosX <= 310)
            {
                if (335 < mousePosY && mousePosY <= 375)
                {
                    descriptionTitle = "Create New Mod";
                    description = "Creates your new mod.\n\nGenerates skeleton of a mod\nin this option.";
                }
                else if (295 < mousePosY && mousePosY <= 335)
                {
                    descriptionTitle = "Open Documentation";
                    description = "Opens the documentation of Create Your Frisk.";
                }
                else if (255 < mousePosY && mousePosY <= 295)
                {
                    descriptionTitle = "CYF Option";
                    description = "Goes to the normal option.";
                }
                else if (215 < mousePosY && mousePosY <= 255)
                {
                    descriptionTitle = "Asterisk Mod Option";
                    description = "Goes to the option that AsteriskMod adds.";
                }
            }
            if (GlobalControls.crate)
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
        }
    }
}
