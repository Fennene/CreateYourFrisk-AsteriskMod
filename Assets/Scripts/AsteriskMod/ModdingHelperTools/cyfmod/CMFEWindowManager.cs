using AsteriskMod.ModdingHelperTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class CMFEWindowManager : MonoBehaviour
    {
        internal static CMFEWindowManager Insatnce;

        private RectTransform self;

        internal enum ParameterWindows
        {
            TargetVersion,
            Title,
            Subtitle,
            Description,
            Language,
            File
        }

        private Text title;
        private Text description;

        private GameObject textEditor;

        private Button Cancel;
        private Button Delete;
        private Button Revert;
        private Button Accept;

        private bool animationRequester;
        private bool openAnim;

        private void Awake()
        {
            self = GetComponent<RectTransform>();

            title = transform.Find("Title").Find("TitleLabel").GetComponent<Text>();
            description = transform.Find("Main").Find("Description").GetComponent<Text>();

            textEditor = transform.Find("Main").Find("TextEditor").gameObject;

            Cancel = transform.Find("Cancel").GetComponent<Button>();
            Delete = transform.Find("Delete").GetComponent<Button>();
            Revert = transform.Find("Revert").GetComponent<Button>();
            Accept = transform.Find("Accept").GetComponent<Button>();

            animationRequester = false;

            Insatnce = this;
        }

        private void CloseWindow()
        {
            if (AnimFrameCounter.Instance.IsRunningAnimation) return;
            openAnim = false;
            animationRequester = true;
            AnimFrameCounter.Instance.StartAnimation();
        }

        private void Start()
        {
            self.anchoredPosition = new Vector2(0, openAnim ? 0 : -480);
            UnityButtonUtil.AddListener(Cancel, CloseWindow);
        }

        private void Update()
        {
            if (Insatnce == null) return;
            if (!AnimFrameCounter.Instance.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.Instance.CurrentFrame == 16)
            {
                self.anchoredPosition = new Vector2(0, openAnim ? 0 : -480);
                AnimFrameCounter.Instance.EndAnimation();
                animationRequester = false;
                return;
            }
            self.anchoredPosition += new Vector2(0, openAnim ? 30 : -30);
        }

        internal static void Dispose() { Insatnce = null; }

        internal void OpenWindow(ParameterWindows target)
        {
            if (AnimFrameCounter.Instance.IsRunningAnimation) return;
            SetEditingParameter(target);
            openAnim = true;
            animationRequester = true;
            AnimFrameCounter.Instance.StartAnimation();
        }

        private void SetEditingParameter(ParameterWindows parameter)
        {
            string t = "Error!";
            string d = "No Description";
            switch (parameter)
            {
                case ParameterWindows.TargetVersion:
                    t = "AsteriskMod's Target Version";
                    break;
                case ParameterWindows.Title:
                    t = "Mod's Title";
                    break;
                case ParameterWindows.Subtitle:
                    t = "Mod's Subtitle";
                    break;
                case ParameterWindows.Description:
                    t = "Mod's Description";
                    break;
                case ParameterWindows.Language:
                    t = "Supporting Language Override";
                    break;
                case ParameterWindows.File:
                    t = "Accept Change or Manages File";
                    break;
            }
            title.text = t;
            description.text = d;
        }
    }
}
