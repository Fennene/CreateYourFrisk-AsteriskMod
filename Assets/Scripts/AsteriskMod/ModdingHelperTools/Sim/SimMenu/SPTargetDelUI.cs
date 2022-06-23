using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SPTargetDelUI
    {
        private Dropdown TargetObjectSelecter;
        private Toggle TargetObjectAsBullet;

        private Toggle ReallyRemove;
        private Button Remove;
        private Image Button_Image;

        private bool scriptChange;

        internal int TargetIndex { get { return TargetObjectSelecter.value - 1; } }
        internal bool IsTargetBullet { get { return TargetObjectAsBullet.isOn; } }

        internal void Awake(Transform targetUIParent, Transform removeUIParent)
        {
            TargetObjectSelecter = targetUIParent.Find("Object")   .GetComponent<Dropdown>();
            TargetObjectAsBullet = targetUIParent.Find("ProjCheck").GetComponent<Toggle>();
            ReallyRemove = removeUIParent.Find("DelCheck").GetComponent<Toggle>();
            Remove       = removeUIParent.Find("Delete")  .GetComponent<Button>();
            Button_Image = removeUIParent.Find("Delete")  .GetComponent<Image>();

            scriptChange = false;
        }

        private void CheckRemoveButton()
        {
            bool canPress = (TargetObjectSelecter.value > 0);
            if (canPress)
            {
                if (!ReallyRemove.isOn) canPress = false;
            }
            UnityButtonUtil.SetActive(Remove, Button_Image, canPress);
        }

        internal void Start()
        {
            UnityButtonUtil.SetActive(Remove, Button_Image, false);

            TargetObjectSelecter.onValueChanged.RemoveAllListeners();
            TargetObjectSelecter.onValueChanged.AddListener((value) =>
            {
                //*if (scriptChange) return;

               SimInstance.SPControllerUI.UpdateParameters();

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
                    SimSprProjSimMenu.Instance.RemoveSprite(TargetObjectSelecter.value - 1);
                }
                else
                {
                    SimSprProjSimMenu.Instance.RemoveBullet(TargetObjectSelecter.value - 1);
                }
            });
        }

        internal void UpdateTargetDropDown(bool setToNoSelect = false)
        {
            TargetObjectSelecter.options = new List<Dropdown.OptionData>();
            TargetObjectSelecter.options.Add(new Dropdown.OptionData { text = "< No Select >" });

            if (!TargetObjectAsBullet.isOn)
            {
                for (var i = 0; i < SimSprProjSimMenu.Instance.SpriteLength; i++)
                {
                    TargetObjectSelecter.options.Add(new Dropdown.OptionData { text = SimSprProjSimMenu.Instance.Sprites[i].spritename });
                }
            }
            else
            {
                for (var i = 0; i < SimSprProjSimMenu.Instance.BulletLength; i++)
                {
                    TargetObjectSelecter.options.Add(new Dropdown.OptionData { text = SimSprProjSimMenu.Instance.Bullets[i].sprite.spritename });
                }
            }

            TargetObjectSelecter.RefreshShownValue();

            if (setToNoSelect) TargetObjectSelecter.value = 0;
        }
    }
}
