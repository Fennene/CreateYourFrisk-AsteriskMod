using AsteriskMod.ModdingHelperTools.UI;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SPCreateUI
    {
        private CYFInputField SpriteName;
        private Dropdown SpriteLayer;
        private Toggle AsBullet;
        private Button Run;
        private Image Button_Image;

        private bool scriptChange;

        internal void Awake(Transform createUIParent)
        {
            SpriteName   = createUIParent.Find("SpriteName").GetComponent<CYFInputField>();
            SpriteLayer  = createUIParent.Find("LayerName") .GetComponent<Dropdown>();
            AsBullet     = createUIParent.Find("ProjCheck") .GetComponent<Toggle>();
            Run          = createUIParent.Find("Create")    .GetComponent<Button>();
            Button_Image = createUIParent.Find("Create")    .GetComponent<Image>();

            scriptChange = false;
        }

        private string ConvertLayerName()
        {
            switch (SpriteLayer.value)
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
                    return "";
                    //* return "BulletPool";
                case 6:
                    return "Top";
                default:
                    return "";
            }
        }

        private void CheckButtonEnable(bool prerequisites = true)
        {
            bool canPress = prerequisites;
            if (canPress)
            {
                if (!AsBullet.isOn && !SimSprProjSimMenu.Instance.CanCreateSprite) canPress = false;
                if (AsBullet.isOn && !SimSprProjSimMenu.Instance.CanCreateBullet)  canPress = false;
            }
            if (canPress)
            {
                if (!AsBullet.isOn && SpriteLayer.value == 5) canPress = false;
            }
            UnityButtonUtil.SetActive(Run, Button_Image, canPress);
        }

        internal void Start()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(SpriteName, (value) =>
            {
                FileInfo fi = new FileInfo(SimInstance.FakeFileLoader.pathToModFile("Sprites/" + value + ".png"));
                if (!fi.Exists) fi = new FileInfo(SimInstance.FakeFileLoader.pathToDefaultFile("Sprites/" + value + ".png"));
                CYFInputFieldUtil.ShowInputError(SpriteName, fi.Exists);
                CheckButtonEnable(fi.Exists);
            });

            SpriteLayer.onValueChanged.RemoveAllListeners();
            SpriteLayer.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                CheckButtonEnable();
            });

            UnityToggleUtil.AddListener(AsBullet, (value) =>
            {
                scriptChange = true;
                SpriteLayer.value = value ? 5 : 2;
                scriptChange = false;
                CheckButtonEnable();
            });

            UnityButtonUtil.AddListener(Run, () =>
            {
                if (!AsBullet.isOn)
                {
                    SimSprProjSimMenu.Instance.AddSprite(SpriteName.InputField.text, ConvertLayerName());
                }
                else
                {
                    SimSprProjSimMenu.Instance.AddBullet(SpriteName.InputField.text, ConvertLayerName());
                }
            });
        }
    }
}
