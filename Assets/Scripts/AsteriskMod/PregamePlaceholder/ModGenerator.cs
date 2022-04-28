using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class ModGenerator : MonoBehaviour
    {
        private static readonly string[] titles = new string[4] { "Craete New Mod", "Encounter Option", "Monster Option", "Sprite Option" };
        private int page;

        /// <summary>AsteriskModのターゲットバージョン - 指定無し(= 常に最新版を使用)</summary>
        public const int TARGET_ALWAYS_LATEST = 0;
        /// <summary>AsteriskModのターゲットバージョン - v0.5.3</summary>
        public const int TARGET_V053 = 1;
        /// <summary>AsteriskModのターゲットバージョン - v0.5.2.9</summary>
        public const int TARGET_V0529 = 2;
        /// <summary>AsteriskModのターゲットバージョン - v0.5.2.8</summary>
        public const int TARGET_V0528 = 3;

        /// <summary>プリセット - Encounter Skeleton</summary>
        public const int PRESET_ENCOUNTER_SKELETON = 0;
        /// <summary>プリセット - 空ファイル</summary>
        public const int PRESET_EMPTY = 1;
        /// <summary>プリセット - おすすめ</summary>
        public const int PRESET_NIL256 = 2;
        /// <summary>プリセット - 全てのオプション</summary>
        public const int PRESET_ALL = 3;
        /// <summary>プリセット - ユーザーカスタム</summary>
        public const int PRESET_CUSTOM = 4;

        /// <summary>Mod名</summary>
        internal string modName;
        /// <summary>Audioフォルダを生成するかどうか</summary>
        internal bool generateAudio;
        /// <summary>Soundsフォルダを生成するかどうか</summary>
        internal bool generateSounds;
        /// <summary>Sounds/Voicesフォルダを生成するかどうか</summary>
        internal bool generateSoundsVoices;

        /// <summary>Encounterファイル名</summary>
        internal string[] encounterScriptNames;
        /// <summary>Monsterファイル名</summary>
        internal string[] monsterScriptNames;
        /// <summary>EncounterSkeletonのWaveスクリプト例を生成するかどうか</summary>
        internal bool generateExampleWaveScripts;
        /// <summary>(EncounterSkeletonにあるような)コメントを追加するかどうか</summary>
        internal bool commentout;

        [Header("Each Generators")]
        public EncounterGenerator encounterGen;

        [Header("Common Objects")]
        public Text Title, Page;
        public GameObject prevButton, nextButton;

        [Header("Page 1")]
        public GameObject Page1;
        public InputField uiModName;
        public Text uiModNameDesc;
        public Dropdown uiTargetVersion;
        public CheckBox uiAudio, uiSounds, uiVoices, uiLibraries, uiStates;

        [Header("Page 2")]
        public GameObject Page2;
        public InputField uiEncounterNames;
        public Text uiEncounterNamesDesc;

        private void Start()
        {
            Initialize();
            InitializeModGen();
            InitializeEncounterGen();

            page = 1;
            ShowPage();
        }

        private void Initialize()
        {
            prevButton.GetComponent<Button>().onClick.RemoveAllListeners();
            nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        private void SetPrevButton(string text, UnityAction action)
        {
            prevButton.GetComponentInChildren<Text>().text = text;
            prevButton.GetComponent<Button>().onClick.RemoveAllListeners();
            prevButton.GetComponent<Button>().onClick.AddListener(action);
        }

        private void SetNextButton(string text, UnityAction action)
        {
            nextButton.GetComponentInChildren<Text>().text = text;
            nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
            nextButton.GetComponent<Button>().onClick.AddListener(action);
        }

        private void BackModSelect() { SceneManager.LoadScene("ModSelect"); }

        private void ShowPage()
        {
            Title.text = titles[page - 1];
            Page.text = "Page " + page + " / " + titles.Length;
            Page1SetActive(page == 1);
            Page2SetActive(page == 2);
        }


        private void InitializeModGen()
        {
            Page1.SetActive(true);
            uiModName.text = "";
            uiModName.onValueChanged.RemoveAllListeners();
            uiModName.onValueChanged.AddListener(_ => CheckInvalidModName(true));
            CheckInvalidModName(true);
            uiTargetVersion.value = 0;
            uiAudio.Checked = true;
            uiSounds.Checked = true;
            uiVoices.Checked = false;
            uiSounds.AddChildCheckBox(uiVoices, true);
            uiLibraries.Checked = true;
            uiStates.Checked = true;
            Page1.SetActive(false);
        }

        private bool CheckInvalidModName(bool restore = false)
        {
            string text;
            if (!restore && AsteriskUtil.IsInvalidPath(uiModName.text, false, out text))
            {
                text = text.Replace("The path", "The name");
                text = "<color=#f44>" + text + "</color>";
                uiModNameDesc.text = text + "\nThis is same as the name of mod's root folder, so you can not use invalid characters for folder's name.";
                return true;
            }
            else
            {
                uiModNameDesc.text = "Your new mod's name.\nThis is same as the name of mod's root folder, so you can not use invalid characters for folder's name.";
                return false;
            }
        }

        private void Page1SetActive(bool active)
        {
            Page1.SetActive(active);
            if (!active) return;
            SetPrevButton("Cancel", () => BackModSelect());
            SetNextButton("Next", () =>
            {
                if (CheckInvalidModName(false)) return;
                page++;
                ShowPage();
            });
        }


        private void InitializeEncounterGen()
        {
            Page2.SetActive(true);
            uiEncounterNames.text = "encounter";
            uiEncounterNames.onValueChanged.RemoveAllListeners();
            uiEncounterNames.onValueChanged.AddListener(_ => CheckInvalidEncounterName(true));
            CheckInvalidEncounterName(true);
            encounterGen.Initialize(this);
            Page2.SetActive(false);
        }

        private bool CheckInvalidEncounterName(bool restore = false)
        {
            if (restore)
            {
                uiEncounterNamesDesc.text = "Names The Encounter Files. (without extension)\nYou can specify multiple file names separated by slashes(/).";
                return false;
            }
            string[] fileNames = uiEncounterNames.text.Split('/');
            string[] checkArray = uiEncounterNames.text.Split('/');
            for (var i = 0; i < checkArray.Length; i++)
            {
                checkArray[i] = checkArray[i].ToLower();
            }
            int index = 0;
            foreach (string fileName in fileNames)
            {
                string text;
                if (AsteriskUtil.IsInvalidPath(fileName, true, out text))
                {
                    text = text.Replace("The path", "The name");
                    text = "<color=#f44>" + text + "</color>";
                    uiEncounterNamesDesc.text = text + "\nYou can specify multiple file names separated by slashes(/).";
                    return true;
                }
                checkArray[index] = "";
                if (checkArray.Contains(fileName.ToLower()))
                {
                    uiEncounterNamesDesc.text = "<color=#f44>There are above 2 files of the same name.</color>\nYou can specify multiple file names separated by slashes(/).";
                    return true;
                }
                checkArray[index] = fileName;
                index++;
            }
            uiEncounterNamesDesc.text = "Names The Encounter Files. (without extension)\nYou can specify multiple file names separated by slashes(/).";
            return false;
        }

        private void Page2SetActive(bool active)
        {
            Page2.SetActive(active);
            if (!active) return;
            SetPrevButton("Prev", () =>
            {
                page--;
                ShowPage();
            });
            SetNextButton("Next", () =>
            {
                if (CheckInvalidEncounterName(false)) return;
                page++;
                ShowPage();
            });
        }
    }
}