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
        public Button _accept;

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
    }
}
