using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class ModGenerator : MonoBehaviour
    {
        private int page;
        private static readonly string[] titles = new string[4] { "Craete New Mod", "Encounter Option", "Monster Option", "Sprite Option" };
        [Header("Common")]
        public Text Title, Page;
        public GameObject prevButton, nextButton, nextMask;
        [Header("Page 1")]
        public GameObject Page1;
        public InputField ModName;
        public Dropdown TargetVersion;
        public Toggle Audio, Sounds, Voices;
        [Header("Page 2")]
        public GameObject Page2;

        private void Awake()
        {
            page = 1;
            ChangePage(true, true);
        }

        private void ChangePage(bool next = true, bool needInitalize = true)
        {
            page += next ? 1 : -1;
            if (page <= 0)
            {
                SceneManager.LoadScene("ModSelect");
                return;
            }
            if (page > titles.Length) return;
            PreparePage1(page == 1, needInitalize);
            PreparePage2(page == 2, needInitalize);
            Title.text = titles[page - 1];
            Page.text = "Page " + page + " / " + titles.Length;
        }

        private void PreparePage1(bool show, bool needInitalize)
        {
            Page1.SetActive(show);
            if (!show || !needInitalize) return;
            ModName.text = "";
            TargetVersion.value = 0;
            Audio.isOn = Sounds.isOn = Voices.isOn = false;
        }

        private void PreparePage2(bool show, bool needInitalize)
        {
            Page2.SetActive(show);
            if (!show || !needInitalize) return;
        }
    }
}