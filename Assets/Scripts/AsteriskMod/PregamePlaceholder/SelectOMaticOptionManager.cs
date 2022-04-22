using System;
using System.Collections;
using System.Collections.Generic;
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

        public static void StartAlt(SelectOMatic selectOMatic)
        {
            if (!GlobalControls.modDev) return;
            instance._selectOMatic = selectOMatic;
            instance._selectOMatic.btnOptions.GetComponent<Button>().onClick.RemoveAllListeners();
            instance._selectOMatic.btnOptions.GetComponent<Button>().onClick.AddListener(() => instance.ToogleOptionSelectWindow());
            //instance.optionButton.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("Options"); });
            opened = false;
            instance.optionSelectWindow.SetActive(false);
        }

        public void ToogleOptionSelectWindow()
        {
            if (!GlobalControls.modDev) return;
            opened = !opened;
            _selectOMatic.btnList.SetActive(!opened);
            _selectOMatic.btnBack.SetActive(!opened);
            _selectOMatic.btnNext.SetActive(!opened);
            if (opened)
            {
                events = _selectOMatic.ModBackground.GetComponent<Button>().onClick;
                _selectOMatic.ModBackground.GetComponent<Button>().onClick.RemoveAllListeners();
                _selectOMatic.ModBackground.GetComponent<Button>().onClick.AddListener(() => instance.ToogleOptionSelectWindow());
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
    }
}
