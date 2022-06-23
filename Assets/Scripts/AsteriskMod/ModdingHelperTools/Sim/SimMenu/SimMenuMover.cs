using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMenuMover : MonoBehaviour
    {
        internal static SimMenuMover Instance;
        //* private static bool _uniqueCheck;

        private RectTransform self;
        private Image selfImage;
        private Button button;
        private Image icon;

        private bool animationRequester;
        private bool goToRight;

        private void Awake()
        {
            //* if (_uniqueCheck) throw new Exception("SimMenuMoverが複数存在します。");
            //* _uniqueCheck = true;

            self = GetComponent<RectTransform>();
            selfImage = GetComponent<Image>();
            button = GetComponent<Button>();
            icon = transform.GetChild(0).GetComponent<Image>();

            animationRequester = false;
            goToRight = false;

            Instance = this;
        }

        internal void SetButtonActive(bool active) { button.enabled = selfImage.enabled = icon.enabled = active; }

        private void SetAnimation(bool willGoToRight)
        {
            SimMenuOpener.Instance.SetButtonActive(false);
            Instance.SetButtonActive(false);
            animationRequester = true;
            goToRight = willGoToRight;
            AnimFrameCounter.Instance.StartAnimation();
        }

        private void TryMove(bool willGoToRight)
        {
            if (SimInstance.BattleSimulator.MenuOpened)
            {
                SetAnimation(willGoToRight);
                return;
            }
            AnimFrameCounter.Instance.StartAnimation();
            if (willGoToRight)
            {
                SimMenuWindowManager.Instance.GoToRight();
                SimMenuOpener.Instance.GoToRight();
                Instance.GoToRight();
                UnityButtonUtil.AddListener(button, () =>
                {
                    if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                    icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toright");
                    TryMove(false);
                });
            }
            else
            {
                SimMenuWindowManager.Instance.GoToLeft();
                SimMenuOpener.Instance.GoToLeft();
                Instance.GoToLeft();
                UnityButtonUtil.AddListener(button, () =>
                {
                    if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                    icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toleft");
                    TryMove(true);
                });
            }
            SimInstance.BattleSimulator.LeftMenu = !willGoToRight;
            AnimFrameCounter.Instance.EndAnimation();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(button, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toleft");
                TryMove(true);
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
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.LeftMenu ? 250 : 350), -60);
        }
        internal void Close()
        {
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.LeftMenu ? 10 : 590), -60);
        }

        internal void AnimGoToRight() { }
        internal void AnimGoToLeft() { }
        internal void GoToRight()
        {
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.MenuOpened ? 350 : 590), -60);
            SetButtonActive(true);
        }
        internal void GoToLeft()
        {
            self.anchoredPosition = new Vector2((SimInstance.BattleSimulator.MenuOpened ? 250 : 10), -60);
            SetButtonActive(true);
        }

        private void Update()
        {
            if (!AnimFrameCounter.Instance.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.Instance.CurrentFrame >= 16)
            {
                if (goToRight)
                {
                    SimMenuWindowManager.Instance.GoToRight();
                    SimMenuOpener.Instance.GoToRight();
                    Instance.GoToRight();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toright");
                        TryMove(false);
                    });
                }
                else
                {
                    SimMenuWindowManager.Instance.GoToLeft();
                    SimMenuOpener.Instance.GoToLeft();
                    Instance.GoToLeft();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toleft");
                        TryMove(true);
                    });
                }
                SimInstance.BattleSimulator.LeftMenu = !goToRight;
                AnimFrameCounter.Instance.EndAnimation();
                animationRequester = false;
                return;
            }
            if (goToRight)
            {
                SimMenuWindowManager.Instance.AnimGoToRight();
                SimMenuOpener.Instance.AnimGoToRight();
                Instance.AnimGoToRight();
            }
            else
            {
                SimMenuWindowManager.Instance.AnimGoToLeft();
                SimMenuOpener.Instance.AnimGoToLeft();
                Instance.AnimGoToLeft();
            }
        }

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
