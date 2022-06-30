using UnityEngine;

namespace AsteriskMod.UnityUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class FrameAnimation : MonoBehaviour
    {
        protected bool animationRequester;

        public RectTransform rectTransform { get; private set; }

        protected virtual void PostAwake() { }

        private void Awake()
        {
            if (!FrameCounter.Exists) throw new UINotFoundException("このシーンにFrameCounterコンポーネントを持つGameObjectがありません！");
            animationRequester = false;
            rectTransform = GetComponent<RectTransform>();
            PostAwake();
        }

        protected virtual void PreStartAnimation() { }

        public bool StartAnimation()
        {
            if (FrameCounter.IsCounting) return false;
            PreStartAnimation();
            animationRequester = true;
            FrameCounter.StartCount();
            return true;
        }

        protected virtual void PreStopAnimation() { }

        public void StopAnimation()
        {
            PreStartAnimation();
            animationRequester = false;
            FrameCounter.StopCount();
        }

        protected virtual void UpdateAnimation(uint frame) { }

        private void Update()
        {
            if (!FrameCounter.IsCounting) return;
            if (!animationRequester) return;
            UpdateAnimation(FrameCounter.CurrentFrame);
        }
    }
}
