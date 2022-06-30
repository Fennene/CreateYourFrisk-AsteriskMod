using AsteriskMod.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModPack
{
    internal class TextBoxWindow : FrameAnimation
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
            rectTransform.anchoredPosition += new Vector2(0, opened ? 30 : -30);
        }

        public Text title;
        public Text description;
        public TextBox textBox;
        public Text error;
        public Button cancel;
        public Button accept;
    }
}
