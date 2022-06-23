using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMenuOpener : MonoBehaviour
    {
        internal static SimMenuOpener Instance;

        private RectTransform self;
        private Image selfImage;
        private Button button;
        private Image icon;

        private bool animationRequester;
        private bool open;
        private float animationSpeed;

        private void Awake()
        {
            self = GetComponent<RectTransform>();
            selfImage = GetComponent<Image>();
            button = GetComponent<Button>();
            icon = transform.GetChild(0).GetComponent<Image>();

            animationRequester = false;
            open = false;
            animationSpeed = 0f;

            Instance = this;
        }

        internal void SetButtonActive(bool active) { button.enabled = selfImage.enabled = icon.enabled = active; }

        private void SetAnimation(bool isOpenAnim)
        {
            animationRequester = true;
            open = isOpenAnim;
            animationSpeed = 15 * (open ? 1 : -1);
            animationSpeed *= (SimInstance.BattleSimulator.LeftMenu ? 1 : -1);
            AnimFrameCounter.Instance.StartAnimation();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(button, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_close");
                SetAnimation(true);
            });
        }

        internal void AnimOpen(float speed)
        {
            self.anchoredPosition += new Vector2(speed, 0);
        }
        internal void AnimClose(float speed)
        {
            self.anchoredPosition += new Vector2(speed, 0);
        }
        internal void Open()
        {
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.LeftMenu ? 250 : 350), -10);
        }
        internal void Close()
        {
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.LeftMenu ? 10 : 590), -10);
        }

        private void Update()
        {
            if (!AnimFrameCounter.Instance.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.Instance.CurrentFrame >= 16)
            {
                if (open)
                {
                    SimMenuWindowManager.Instance.Open();
                    Instance.Open();
                    SimMenuMover.Instance.Open();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_open");
                        SetAnimation(false);
                    });
                }
                else
                {
                    SimMenuWindowManager.Instance.Close();
                    Instance.Close();
                    SimMenuMover.Instance.Close();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_close");
                        SetAnimation(true);
                    });
                }
                SimInstance.BattleSimulator.MenuOpened = open;
                AnimFrameCounter.Instance.EndAnimation();
                animationRequester = false;
                return;
            }
            if (open)
            {
                SimMenuWindowManager.Instance.AnimOpen(animationSpeed);
                Instance.AnimOpen(animationSpeed);
                SimMenuMover.Instance.AnimOpen(animationSpeed);
            }
            else
            {
                SimMenuWindowManager.Instance.AnimClose(animationSpeed);
                Instance.AnimClose(animationSpeed);
                SimMenuMover.Instance.AnimClose(animationSpeed);
            }
        }

        internal void AnimGoToRight() { }
        internal void AnimGoToLeft() { }
        internal void GoToRight()
        {
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.MenuOpened ? 350 : 590), -10);
            SetButtonActive(true);
        }
        internal void GoToLeft()
        {
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.MenuOpened ? 250 : 10), -10);
            SetButtonActive(true);
        }

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
