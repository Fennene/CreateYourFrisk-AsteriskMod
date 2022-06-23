using AsteriskMod.ModdingHelperTools.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class STTargetUI
    {
        private bool scriptChange;

        internal void Awake(Transform targetView)
        {
            AwakeCreate(targetView.Find("ObjCreate"));
            AwakeTarget(targetView.Find("Obj"));
            AwakeRemove(targetView.Find("ObjDel"));
        }

        internal void Start()
        {
            StartCreate();
            StartTarget();
            StartRemove();
        }

        internal int TargetIndex { get { return Target_ObjectSelecter.value - 1; } }

        internal void UpdateTargetDropDown(bool setToNoSelect = false)
        {
            Target_ObjectSelecter.options = new List<Dropdown.OptionData>();
            Target_ObjectSelecter.options.Add(new Dropdown.OptionData { text = "< No Select >" });

            for (var i = 0; i < SimStaticTextSimMenu.Instance.StaticTextLength; i++)
            {
                Target_ObjectSelecter.options.Add(new Dropdown.OptionData { text = "\"" + SimStaticTextSimMenu.Instance.StaticTexts[i].text + "\"" });
            }

            Target_ObjectSelecter.RefreshShownValue();

            if (setToNoSelect) Target_ObjectSelecter.value = 0;
        }

        // --------------------------------------------------------------------------------

        private CYFInputField Create_Text;
        private Dropdown Create_TextLayer;
        private Button Create_Run;
        private Image Create_RunButtonImage;

        private void AwakeCreate(Transform createParent)
        {
            Create_Text           = createParent.Find("Text")     .GetComponent<CYFInputField>();
            Create_TextLayer      = createParent.Find("LayerName").GetComponent<Dropdown>();
            Create_Run            = createParent.Find("Create")   .GetComponent<Button>();
            Create_RunButtonImage = createParent.Find("Create")   .GetComponent<Image>();
        }

        private string ConvertLayerName()
        {
            switch (Create_TextLayer.value)
            {
                case 0:
                    return "Bottom";
                case 1:
                    return "BelowUI";
                case 2:
                    return "BelowArena";
                case 3:
                    return "BelowPlayer";
                case 4:
                    return "BelowBullet";
                case 5:
                    return "Top";
                default:
                    return "";
            }
        }

        private void StartCreate()
        {
            UnityButtonUtil.AddListener(Create_Run, () =>
            {
                SimStaticTextSimMenu.Instance.AddStaticText(Create_Text.InputField.text, ConvertLayerName());
                UnityButtonUtil.SetActive(Create_Run, Create_RunButtonImage, SimStaticTextSimMenu.Instance.CanCreateStaticText);
            });
        }

        // --------------------------------------------------------------------------------

        private Dropdown Target_ObjectSelecter;

        private void AwakeTarget(Transform targetParent)
        {
            Target_ObjectSelecter = targetParent.Find("Object").GetComponent<Dropdown>();
        }

        private void StartTarget()
        {
            Target_ObjectSelecter.onValueChanged.RemoveAllListeners();
            Target_ObjectSelecter.onValueChanged.AddListener((value) =>
            {
                //*if (scriptChange) return;

                SimInstance.STControllerUI.UpdateParameters();

                CheckRemoveButton();
            });
        }

        // --------------------------------------------------------------------------------

        private Toggle Remove_Really;
        private Button Remove_Run;
        private Image Remove_RunButton_Image;

        private void AwakeRemove(Transform removeParent)
        {
            Remove_Really          = removeParent.Find("DelCheck").GetComponent<Toggle>();
            Remove_Run             = removeParent.Find("Delete")  .GetComponent<Button>();
            Remove_RunButton_Image = removeParent.Find("Delete")  .GetComponent<Image>();
        }

        private void CheckRemoveButton()
        {
            bool canPress = (Target_ObjectSelecter.value > 0);
            if (canPress)
            {
                if (!Remove_Really.isOn) canPress = false;
            }
            UnityButtonUtil.SetActive(Remove_Run, Remove_RunButton_Image, canPress);
        }

        private void StartRemove()
        {
            UnityToggleUtil.AddListener(Remove_Really, (value) => CheckRemoveButton());

            UnityButtonUtil.AddListener(Remove_Run, () =>
            {
                Remove_Really.isOn = false;
                if (Target_ObjectSelecter.value <= 0) return;
                SimStaticTextSimMenu.Instance.RemoveStaticText(Target_ObjectSelecter.value - 1);
            });
        }
    }
}
