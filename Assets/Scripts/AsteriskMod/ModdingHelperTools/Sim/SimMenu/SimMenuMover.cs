using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMenuMover : MonoBehaviour
    {
        private static bool _uniqueCheck;

        internal static RectTransform self;
        internal static Image selfImage;
        internal static Button button;
        internal static Image icon;

        private static bool animationRequester;
        private static bool goToRight;

        private void Awake()
        {
            if (_uniqueCheck) throw new Exception("SimMenuMoverが複数存在します。");
            _uniqueCheck = true;

            self = GetComponent<RectTransform>();
            selfImage = GetComponent<Image>();
            button = GetComponent<Button>();
            icon = transform.GetChild(0).GetComponent<Image>();
        }

        internal static void SetButtonActive(bool active) { button.enabled = selfImage.enabled = icon.enabled = active; }

        private static void SetAnimation(bool willGoToRight)
        {
            SimMenuOpener.SetButtonActive(false);
            SimMenuMover.SetButtonActive(false);
            animationRequester = true;
            goToRight = willGoToRight;
            AnimFrameCounter.StartAnimation();
        }

        private static void TryMove(bool willGoToRight)
        {
            if (BattleSimulator.MenuOpened)
            {
                SetAnimation(willGoToRight);
                return;
            }
            AnimFrameCounter.StartAnimation();
            if (willGoToRight)
            {
                SimMenuWindowManager.GoToRight();
                SimMenuOpener.GoToRight();
                SimMenuMover.GoToRight();
                UnityButtonUtil.AddListener(button, () =>
                {
                    if (AnimFrameCounter.IsRunningAnimation) return;
                    icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toright");
                    TryMove(false);
                });
            }
            else
            {
                SimMenuWindowManager.GoToLeft();
                SimMenuOpener.GoToLeft();
                SimMenuMover.GoToLeft();
                UnityButtonUtil.AddListener(button, () =>
                {
                    if (AnimFrameCounter.IsRunningAnimation) return;
                    icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toleft");
                    TryMove(true);
                });
            }
            BattleSimulator.LeftMenu = !willGoToRight;
            AnimFrameCounter.EndAnimation();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(button, () =>
            {
                if (AnimFrameCounter.IsRunningAnimation) return;
                icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toleft");
                TryMove(true);
            });
        }

        internal static void AnimOpen(float speed)
        {
            self.anchoredPosition += new Vector2(speed, 0);
        }
        internal static void AnimClose(float speed)
        {
            self.anchoredPosition += new Vector2(speed, 0);
        }
        internal static void Open()
        {
            self.anchoredPosition = new Vector2((BattleSimulator.LeftMenu ? 250 : 350), -60);
        }
        internal static void Close()
        {
            self.anchoredPosition = new Vector2((BattleSimulator.LeftMenu ? 10 : 590), -60);
        }

        internal static void AnimGoToRight() { }
        internal static void AnimGoToLeft() { }
        internal static void GoToRight()
        {
            self.anchoredPosition = new Vector2((BattleSimulator.MenuOpened ? 350 : 590), -60);
            SetButtonActive(true);
        }
        internal static void GoToLeft()
        {
            self.anchoredPosition = new Vector2((BattleSimulator.MenuOpened ? 250 : 10), -60);
            SetButtonActive(true);
        }

        private void Update()
        {
            if (!AnimFrameCounter.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.CurrentFrame >= 16)
            {
                if (goToRight)
                {
                    SimMenuWindowManager.GoToRight();
                    SimMenuOpener.GoToRight();
                    SimMenuMover.GoToRight();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toright");
                        TryMove(false);
                    });
                }
                else
                {
                    SimMenuWindowManager.GoToLeft();
                    SimMenuOpener.GoToLeft();
                    SimMenuMover.GoToLeft();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_toleft");
                        TryMove(true);
                    });
                }
                BattleSimulator.LeftMenu = !goToRight;
                AnimFrameCounter.EndAnimation();
                animationRequester = false;
                return;
            }
            if (goToRight)
            {
                SimMenuWindowManager.AnimGoToRight();
                SimMenuOpener.AnimGoToRight();
                SimMenuMover.AnimGoToRight();
            }
            else
            {
                SimMenuWindowManager.AnimGoToLeft();
                SimMenuOpener.AnimGoToLeft();
                SimMenuMover.AnimGoToLeft();
            }
        }
    }
}
