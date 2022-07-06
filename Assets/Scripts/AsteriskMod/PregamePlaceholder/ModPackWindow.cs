using AsteriskMod.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    internal class ModPackWindow : FrameAnimation
    {
        private bool opened;

        protected override void PostAwake()
        {
            opened = true;
        }

        private void Start()
        {
            PreStopAnimation();

            _title.text = EngineLang.Get("ModPack", "WindowTitle");
            _description.text = EngineLang.Get("ModPack", "WindowDescription");
            _textBox.onValueChanged.SetListener((name) =>
            {
                string message;
                bool invalid = AsteriskUtil.IsInvalidPath(name + ".modpack", out message);
                if (!invalid) invalid = AsteriskUtil.PathExists(ModPack.ModPackDirectory + "/" + name + ".modpack", out message);
                _textBox.SetErrorOuterColor(invalid);
                _error.text = message;
            });
            _cancelText = _cancel.GetComponentInChildren<Text>();
            _acceptText = _accept.GetComponentInChildren<Text>();
            _cancelText.text = EngineLang.Get("ModPack", "WindowCancel");
            _acceptText.text = EngineLang.Get("ModPack", "WindowAccept");
            _cancel.onClick.SetListener(() =>
            {
                ModPackMenu.Instance.SetSelecterIndex();
                StartAnimation();
            });
            _accept.onClick.SetListener(() =>
            {
                string message;
                string fileName = _textBox.text;
                if (AsteriskUtil.IsInvalidPath(fileName + ".modpack", out message))
                {
                    _error.text = message;
                    return;
                }
                if (AsteriskUtil.PathExists(ModPack.ModPackDirectory + "/" + name + ".modpack", out message))
                {
                    _error.text = message;
                    return;
                }
                ModPack.CreateFile(fileName);
                ModPackMenu.Instance.ReloadModPack();
                ModPackMenu.Instance.SetSelecterIndex(Mods.FindModPackIndex(fileName));
                StartAnimation();
            });
        }

        protected override void PreStopAnimation()
        {
            opened = !opened;
            rectTransform.anchoredPosition = new Vector2(0, opened ? 0 : -480);
        }

        protected override void UpdateAnimation(uint frame)
        {
            if (frame == 16)
            {
                StopAnimation();
                FrameCounter.StopCount();
                return;
            }
            rectTransform.anchoredPosition += new Vector2(0, opened ? -30 : 30);
        }

        public Text _title;
        public Text _description;
        public TextBox _textBox;
        public Text _error;
        public Button _cancel;
        private Text _cancelText;
        public Button _accept;
        private Text _acceptText;

        public void ResetWindow()
        {
            _textBox.text = "";
        }

        /*
        public string Title
        {
            get { return _title.text; }
            set { _title.text = value; }
        }

        public string Description
        {
            get { return _description.text; }
            set { _description.text = value; }
        }
        */
    }
}
