using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMenuOpener : MonoBehaviour
    {
        //* private static bool _uniqueCheck;

        internal static RectTransform self;
        internal static Image selfImage;
        internal static Button button;
        internal static Image icon;

        private static bool animationRequester;
        private static bool open;
        private static float animationSpeed;

        private void Awake()
        {
            //* if (_uniqueCheck) throw new Exception("SimMenuOpenerが複数存在します。");
            //* _uniqueCheck = true;

            self = GetComponent<RectTransform>();
            selfImage = GetComponent<Image>();
            button = GetComponent<Button>();
            icon = transform.GetChild(0).GetComponent<Image>();

            animationRequester = false;
            open = false;
            animationSpeed = 0f;
        }

        internal static void SetButtonActive(bool active) { button.enabled = selfImage.enabled = icon.enabled = active; }

        private static void SetAnimation(bool isOpenAnim)
        {
            animationRequester = true;
            open = isOpenAnim;
            animationSpeed = 15 * (open ? 1 : -1);
            animationSpeed *= (BattleSimulator.LeftMenu ? 1 : -1);
            AnimFrameCounter.StartAnimation();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(button, () =>
            {
                if (AnimFrameCounter.IsRunningAnimation) return;
                icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_close");
                SetAnimation(true);
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
            self.anchoredPosition = new Vector2((BattleSimulator.LeftMenu ? 250 : 350), -10);
        }
        internal static void Close()
        {
            self.anchoredPosition = new Vector2((BattleSimulator.LeftMenu ? 10 : 590), -10);
        }

        private void Update()
        {
            if (!AnimFrameCounter.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.CurrentFrame >= 16)
            {
                if (open)
                {
                    SimMenuWindowManager.Open();
                    SimMenuOpener.Open();
                    SimMenuMover.Open();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_open");
                        SetAnimation(false);
                    });
                }
                else
                {
                    SimMenuWindowManager.Close();
                    SimMenuOpener.Close();
                    SimMenuMover.Close();
                    UnityButtonUtil.AddListener(button, () =>
                    {
                        if (AnimFrameCounter.IsRunningAnimation) return;
                        icon.sprite = Resources.Load<Sprite>("Sprites/AsteriskMod/sim_close");
                        SetAnimation(true);
                    });
                }
                BattleSimulator.MenuOpened = open;
                AnimFrameCounter.EndAnimation();
                animationRequester = false;
                return;
            }
            if (open)
            {
                SimMenuWindowManager.AnimOpen(animationSpeed);
                SimMenuOpener.AnimOpen(animationSpeed);
                SimMenuMover.AnimOpen(animationSpeed);
            }
            else
            {
                SimMenuWindowManager.AnimClose(animationSpeed);
                SimMenuOpener.AnimClose(animationSpeed);
                SimMenuMover.AnimClose(animationSpeed);
            }
        }

        internal static void AnimGoToRight() { }
        internal static void AnimGoToLeft() { }
        internal static void GoToRight()
        {
            self.anchoredPosition = new Vector2((BattleSimulator.MenuOpened ? 350 : 590), -10);
            SetButtonActive(true);
        }
        internal static void GoToLeft()
        {
            self.anchoredPosition = new Vector2((BattleSimulator.MenuOpened ? 250 : 10), -10);
            SetButtonActive(true);
        }
    }
}
