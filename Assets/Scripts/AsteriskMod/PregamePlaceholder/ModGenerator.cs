using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class ModGenerator : MonoBehaviour
    {
        /// <summary>プリセット - Encounter Skeleton</summary>
        public const int PRESET_ENCOUNTER_SKELETON = 0;
        /// <summary>プリセット - 空ファイル</summary>
        public const int PRESET_EMPTY = 1;
        /// <summary>プリセット - 必要最小限</summary>
        public const int PRESET_MIN = 2;
        /// <summary>プリセット - おすすめ</summary>
        public const int PRESET_NIL256 = 3;
        /// <summary>プリセット - 全てのオプション</summary>
        public const int PRESET_ALL = 4;
        /// <summary>プリセット - ユーザーカスタム</summary>
        public const int PRESET_CUSTOM = 5;

        /// <summary>(EncounterSkeletonにあるような)コメントを追加するかどうか</summary>
        public bool commentout;
        /// <summary>EncounterSkeletonのWaveスクリプト例を追加するかどうか</summary>
        public bool addWaveExamples;




        private int page;
        private static readonly string[] titles = new string[4] { "Craete New Mod", "Encounter Option", "Monster Option", "Sprite Option" };
        private static bool[] initialized;
        [Header("Common")]
        public Text Title, Page;
        public GameObject prevButton, nextButton;
        [Header("Page 1")]
        public GameObject Page1;
        public InputField ModName;
        public Text ModNameDesc;
        public Dropdown TargetVersion;
        public Toggle Audio, Sounds, Voices;
        [Header("Page 2")]
        public GameObject Page2;
        public InputField EncounterFiles;
        public Text EncounterFilesDesc;
        public Dropdown E_VarFuncPreset;
        private bool E_changeFromPreset;
        public CheckBox E_music, E_encountertext, E_nextwaves, E_wavetimer, E_wavetimer_huge, E_arenasize, E_enemies, E_enwmypositions,
                        E_autolinebreak, E_playerskipcommand, E_unescape, E_sparetext, E_flee, E_fleetext, E_fleesuccess, E_fleetexts, E_revive,
                        E_deathtext, E_deathmusic;
        public CheckBox E_EncounterStarting, /*E_FirstFrameUpdate,*/ E_EnemyDialogueStarting, E_EnemyDialogueEnding, E_DefenceEnding, E_HandleSpare, E_HandleItem,
                        E_EnteringState, E_Update, E_BeforeDeath;


        // Common //

        private void Start()
        {
            initialized = new bool[titles.Length];
            page = 0;
            prevButton.GetComponent<Button>().onClick.RemoveAllListeners();
            prevButton.GetComponent<Button>().onClick.AddListener(() => ChangePage(false));
            ChangePage(true);
        }

        private void ChangePage(bool next = true)
        {
            page += next ? 1 : -1;
            if (page <= 0)
            {
                SceneManager.LoadScene("ModSelect");
                return;
            }
            if (page > titles.Length) return;
            Title.text = titles[page - 1];
            Page.text = "Page " + page + " / " + titles.Length;
            prevButton.GetComponentInChildren<Text>().text = (page == 1) ? "Cancel" : "Prev";
            nextButton.GetComponentInChildren<Text>().text = (page == titles.Length) ? "Generate" : "Next";
            PreparePage1(page == 1);
            PreparePage2(page == 2);
        }

        private void SetNextButtonAction(UnityAction customAction = null)
        {
            nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
            if (customAction == null)
            {
                nextButton.GetComponent<Button>().onClick.AddListener(() => ChangePage(true));
            }
            else
            {
                nextButton.GetComponent<Button>().onClick.AddListener(customAction);
            }
        }


        // Page 1 //

        private void PreparePage1(bool show)
        {
            Page1.SetActive(show);

            if (!show) return;

            SetNextButtonAction(() =>
            {
                if (CheckInvalidModName()) return;
                ChangePage(true);
            });

            if (initialized[0]) return;

            ModName.text = "";
            ModName.onValueChanged.AddListener((_) => CheckInvalidModName(true));
            TargetVersion.value = 0;
            Audio.isOn = Sounds.isOn = true;
            Voices.isOn = false;
            initialized[0] = true;
        }

        /// <summary>Checks that users input invalid mod's name</summary>
        /// <returns>Any Error has occurred</returns>
        private bool CheckInvalidModName(bool restore = false)
        {
            string text;
            if (!restore && AsteriskUtil.IsInvalidPath(ModName.text, false, out text))
            {
                text = text.Replace("The path", "The name");
                text = "<color=#f44>" + text + "</color>";
                ModNameDesc.text = text + "\nThis is same as the name of mod's root folder, so you can not use invalid characters for folder's name.";
                return true;
            }
            else
            {
                ModNameDesc.text = "Your new mod's name.\nThis is same as the name of mod's root folder, so you can not use invalid characters for folder's name.";
                return false;
            }
        }


        // Page 2 //

        private void PreparePage2(bool show)
        {
            Page2.SetActive(show);

            if (!show) return;

            SetNextButtonAction(() =>
            {
                if (CheckInvalidEncounterName(false)) return;
                ChangePage(true);
            });

            if (initialized[1]) return;

            EncounterFiles.text = "encounter";
            EncounterFiles.onValueChanged.AddListener((value) => CheckInvalidEncounterName(true));

            E_wavetimer.onValueChanged.AddListener((value) =>
            {
                if (value) E_wavetimer_huge.Enable();
                else       E_wavetimer_huge.Disable();
            });

            E_music.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_encountertext.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_nextwaves.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_wavetimer.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_wavetimer_huge.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_arenasize.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_enemies.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_enwmypositions.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_autolinebreak.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_unescape.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_sparetext.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_flee.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_fleetext.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_fleesuccess.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_fleetexts.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_revive.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_deathtext.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_deathmusic.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });

            E_EncounterStarting.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_EnemyDialogueStarting.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_EnemyDialogueEnding.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_DefenceEnding.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_HandleSpare.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_HandleItem.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_EnteringState.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_Update.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });
            E_BeforeDeath.onValueChangedFromUser.AddListener((value) => { E_VarFuncPreset.value = 4; });

            E_VarFuncPreset.onValueChanged.AddListener((value) => SetEncounterVarFuncPreset(value));
            E_VarFuncPreset.value = 0;

            initialized[1] = true;
        }

        private void SetEncounterVarFuncPreset(int value)
        {
            switch (value)
            {
                case 0: // Encounter Skeleton
                    E_music.Checked = false;
                    E_encountertext.Checked = true;
                    E_nextwaves.Checked = true;
                    E_wavetimer.Checked = true;
                    E_wavetimer_huge.Checked = false;
                    E_arenasize.Checked = true;
                    E_enemies.Checked = true;
                    E_enwmypositions.Checked = true;
                    E_autolinebreak.Checked = false;
                    E_playerskipcommand.Checked = false;
                    E_unescape.Checked = false;
                    E_sparetext.Checked = false;
                    E_flee.Checked = false;
                    E_fleetext.Checked = false;
                    E_fleesuccess.Checked = false;
                    E_fleetexts.Checked = false;
                    E_revive.Checked = false;
                    E_deathtext.Checked = false;
                    E_deathmusic.Checked = false;

                    E_EncounterStarting.Checked = true;
                    E_EnemyDialogueStarting.Checked = true;
                    E_EnemyDialogueEnding.Checked = true;
                    E_DefenceEnding.Checked = true;
                    E_HandleSpare.Checked = true;
                    E_HandleItem.Checked = true;
                    E_EnteringState.Checked = false;
                    E_Update.Checked = false;
                    E_BeforeDeath.Checked = false;
                    break;
                case 1: // Empty
                    E_music.Checked = false;
                    E_encountertext.Checked = false;
                    E_nextwaves.Checked = false;
                    E_wavetimer.Checked = false;
                    E_wavetimer_huge.Checked = false;
                    E_arenasize.Checked = false;
                    E_enemies.Checked = false;
                    E_enwmypositions.Checked = false;
                    E_autolinebreak.Checked = false;
                    E_playerskipcommand.Checked = false;
                    E_unescape.Checked = false;
                    E_sparetext.Checked = false;
                    E_flee.Checked = false;
                    E_fleetext.Checked = false;
                    E_fleesuccess.Checked = false;
                    E_fleetexts.Checked = false;
                    E_revive.Checked = false;
                    E_deathtext.Checked = false;
                    E_deathmusic.Checked = false;

                    E_EncounterStarting.Checked = false;
                    E_EnemyDialogueStarting.Checked = false;
                    E_EnemyDialogueEnding.Checked = false;
                    E_DefenceEnding.Checked = false;
                    E_HandleSpare.Checked = false;
                    E_HandleItem.Checked = false;
                    E_EnteringState.Checked = false;
                    E_Update.Checked = false;
                    E_BeforeDeath.Checked = false;
                    break;
                case 2: // All
                    E_music.Checked = true;
                    E_encountertext.Checked = true;
                    E_nextwaves.Checked = true;
                    E_wavetimer.Checked = true;
                    E_wavetimer_huge.Checked = true;
                    E_arenasize.Checked = true;
                    E_enemies.Checked = true;
                    E_enwmypositions.Checked = true;
                    E_autolinebreak.Checked = true;
                    E_playerskipcommand.Checked = true;
                    E_unescape.Checked = true;
                    E_sparetext.Checked = true;
                    E_flee.Checked = true;
                    E_fleetext.Checked = true;
                    E_fleesuccess.Checked = true;
                    E_fleetexts.Checked = true;
                    E_revive.Checked = true;
                    E_deathtext.Checked = true;
                    E_deathmusic.Checked = true;

                    E_EncounterStarting.Checked = true;
                    E_EnemyDialogueStarting.Checked = true;
                    E_EnemyDialogueEnding.Checked = true;
                    E_DefenceEnding.Checked = true;
                    E_HandleSpare.Checked = true;
                    E_HandleItem.Checked = true;
                    E_EnteringState.Checked = true;
                    E_Update.Checked = true;
                    E_BeforeDeath.Checked = true;
                    break;
                case 3: // Recommend
                    E_music.Checked = true;
                    E_encountertext.Checked = true;
                    E_nextwaves.Checked = true;
                    E_wavetimer.Checked = true;
                    E_wavetimer_huge.Checked = true;
                    E_arenasize.Checked = true;
                    E_enemies.Checked = true;
                    E_enwmypositions.Checked = true;
                    E_autolinebreak.Checked = false;
                    E_playerskipcommand.Checked = true;
                    E_unescape.Checked = false;
                    E_sparetext.Checked = false;
                    E_flee.Checked = true;
                    E_fleetext.Checked = false;
                    E_fleesuccess.Checked = false;
                    E_fleetexts.Checked = false;
                    E_revive.Checked = false;
                    E_deathtext.Checked = false;
                    E_deathmusic.Checked = false;

                    E_EncounterStarting.Checked = true;
                    E_EnemyDialogueStarting.Checked = true;
                    E_EnemyDialogueEnding.Checked = true;
                    E_DefenceEnding.Checked = true;
                    E_HandleSpare.Checked = true;
                    E_HandleItem.Checked = true;
                    E_EnteringState.Checked = true;
                    E_Update.Checked = true;
                    E_BeforeDeath.Checked = true;
                    break;
                case 4: // Custom
                    break;
            }
            return;
        }

        /// <summary>Checks that users input invalid mod's name</summary>
        /// <returns>Any Error has occurred</returns>
        private bool CheckInvalidEncounterName(bool restore = false)
        {
            if (restore)
            {
                EncounterFilesDesc.text = "Names The Encounter Files. (without extension)\nYou can specify multiple file names separated by slashes(/).";
                return false;
            }
            string[] fileNames = EncounterFiles.text.Split('/');
            foreach (string fileName in EncounterFiles.text.Split('/'))
            {
                string text;
                if (AsteriskUtil.IsInvalidPath(fileName, true, out text))
                {
                    text = text.Replace("The path", "The name");
                    text = "<color=#f44>" + text + "</color>";
                    EncounterFilesDesc.text = text + "\nYou can specify multiple file names separated by slashes(/).";
                    return true;
                }
            }
            EncounterFilesDesc.text = "Names The Encounter Files. (without extension)\nYou can specify multiple file names separated by slashes(/).";
            return false;
        }
    }
}