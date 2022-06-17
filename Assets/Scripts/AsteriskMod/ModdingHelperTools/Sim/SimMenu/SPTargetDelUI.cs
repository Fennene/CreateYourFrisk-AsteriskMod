using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal static class SPTargetDelUI
    {
        private static Dropdown TargetObjectSelecter;
        private static Toggle TargetObjectAsBullet;

        private static Toggle ReallyRemove;
        private static Button Remove;
        private static Image Button_Image;

        private static bool scriptChange;

        internal static int TargetIndex { get { return TargetObjectSelecter.value - 1; } }
        internal static bool IsTargetBullet { get { return TargetObjectAsBullet.isOn; } }

        internal static void Awake(Transform targetUIParent, Transform removeUIParent)
        {
            TargetObjectSelecter = targetUIParent.Find("Object")   .GetComponent<Dropdown>();
            TargetObjectAsBullet = targetUIParent.Find("ProjCheck").GetComponent<Toggle>();
            ReallyRemove = removeUIParent.Find("DelCheck").GetComponent<Toggle>();
            Remove       = removeUIParent.Find("Delete")  .GetComponent<Button>();
            Button_Image = removeUIParent.Find("Delete")  .GetComponent<Image>();

            scriptChange = false;
        }

        private static void CheckRemoveButton()
        {
            bool canPress = (TargetObjectSelecter.value > 0);
            if (canPress)
            {
                if (!ReallyRemove.isOn) canPress = false;
            }
            UnityButtonUtil.SetActive(Remove, Button_Image, canPress);
        }

        internal static void Start()
        {
            UnityButtonUtil.SetActive(Remove, Button_Image, false);

            TargetObjectSelecter.onValueChanged.RemoveAllListeners();
            TargetObjectSelecter.onValueChanged.AddListener((value) =>
            {
                //*if (scriptChange) return;

                SPControllerUI.UpdateParameters();

                CheckRemoveButton();
            });

            UnityToggleUtil.AddListener(TargetObjectAsBullet, (value) =>
            {
                UpdateTargetDropDown(true);
                CheckRemoveButton();
            });

            UnityToggleUtil.AddListener(ReallyRemove, (value) =>
            {
                CheckRemoveButton();
            });

            UnityButtonUtil.AddListener(Remove, () =>
            {
                ReallyRemove.isOn = false;
                if (TargetObjectSelecter.value <= 0) return;
                if (!TargetObjectAsBullet.isOn)
                {
                    SimSprProjSimMenu.RemoveSprite(TargetObjectSelecter.value - 1);
                }
                else
                {
                    SimSprProjSimMenu.RemoveBullet(TargetObjectSelecter.value - 1);
                }
            });
        }

        internal static void UpdateTargetDropDown(bool setToNoSelect = false)
        {
            TargetObjectSelecter.options = new List<Dropdown.OptionData>();
            TargetObjectSelecter.options.Add(new Dropdown.OptionData { text = "< No Select >" });

            if (!TargetObjectAsBullet.isOn)
            {
                for (var i = 0; i < SimSprProjSimMenu.SpriteLength; i++)
                {
                    TargetObjectSelecter.options.Add(new Dropdown.OptionData { text = SimSprProjSimMenu.Sprites[i].spritename });
                }
            }
            else
            {
                for (var i = 0; i < SimSprProjSimMenu.BulletLength; i++)
                {
                    TargetObjectSelecter.options.Add(new Dropdown.OptionData { text = SimSprProjSimMenu.Bullets[i].sprite.spritename });
                }
            }

            TargetObjectSelecter.RefreshShownValue();

            if (setToNoSelect) TargetObjectSelecter.value = 0;
        }
    }
}
