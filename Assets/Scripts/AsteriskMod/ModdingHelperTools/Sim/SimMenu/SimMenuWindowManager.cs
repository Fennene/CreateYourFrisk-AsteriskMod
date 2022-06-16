using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMenuWindowManager : MonoBehaviour
    {
        private static bool _uniqueCheck;

        private static RectTransform Backgrounds;
        private static RectTransform Main;
        private static RectTransform Screen;

        private static bool animationRequester;
        private static RectTransform closeTarget;
        private static RectTransform openTarget;
        private static DisplayingSimMenu target;
        private static float animationSpeed;

        private void Awake()
        {
            if (_uniqueCheck) throw new Exception("SimMenuWindowManagerが複数存在します。");
            _uniqueCheck = true;

            Backgrounds = transform.Find("Backgrounds").GetComponent<RectTransform>();
            Main        = transform.Find("MainMenu")   .GetComponent<RectTransform>();
            Screen      = transform.Find("ScreenMenu") .GetComponent<RectTransform>();
        }

        internal enum DisplayingSimMenu
        {
            Main,
            Screen
        }
        private static DisplayingSimMenu _currentSimMenu = DisplayingSimMenu.Main;
        internal static DisplayingSimMenu CurrentSimMenu { get { return _currentSimMenu; } }

        private static RectTransform ConvertToRT(DisplayingSimMenu menu)
        {
            switch (menu)
            {
                case DisplayingSimMenu.Main:
                    return Main;
                case DisplayingSimMenu.Screen:
                    return Screen;
                default:
                    return Main;
            }
        }
        private static RectTransform CurrentSimMenuRT { get { return ConvertToRT(CurrentSimMenu); } }

        internal static bool ChangePage(DisplayingSimMenu closeMenu, DisplayingSimMenu openMenu)
        {
            if (!BattleSimulator.MenuOpened) return false;
            closeTarget = ConvertToRT(closeMenu);
            openTarget  = ConvertToRT(openMenu);
            if (closeTarget == openTarget) return false;
            animationRequester = true;
            target = openMenu;
            animationSpeed = 15 * (BattleSimulator.LeftMenu ? 1 : -1);
            AnimFrameCounter.StartAnimation();
            return true;
        }

        private void Update()
        {
            if (!AnimFrameCounter.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.CurrentFrame >= 16)
            {
                closeTarget.anchoredPosition = new Vector2((BattleSimulator.LeftMenu ? -240 : 640), 0);
                openTarget.anchoredPosition  = new Vector2((BattleSimulator.LeftMenu ? 0 : 400), 0);
                _currentSimMenu = target;
                AnimFrameCounter.EndAnimation();
                animationRequester = false;
                return;
            }
            closeTarget.anchoredPosition -= new Vector2(animationSpeed, 0);
            openTarget .anchoredPosition += new Vector2(animationSpeed, 0);
        }

        internal static void AnimOpen(float speed)
        {
            Backgrounds.anchoredPosition += new Vector2(speed, 0);
            CurrentSimMenuRT.anchoredPosition += new Vector2(speed, 0);
        }
        internal static void AnimClose(float speed)
        {
            Backgrounds.anchoredPosition += new Vector2(speed, 0);
            CurrentSimMenuRT.anchoredPosition += new Vector2(speed, 0);
        }
        internal static void Open()
        {
            Vector2 position = new Vector2((BattleSimulator.LeftMenu ? 0 : 400), 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }
        internal static void Close()
        {
            Vector2 position = new Vector2((BattleSimulator.LeftMenu ? -240 : 640), 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }

        internal static void AnimGoToRight()
        {
            if (AnimFrameCounter.CurrentFrame == 8)
            {
                Vector2 position = new Vector2(640, 0);
                Backgrounds.anchoredPosition = position;
                CurrentSimMenuRT.anchoredPosition = position;
                return;
            }
            Vector2 velocity = new Vector2(-30, 0);
            Backgrounds.anchoredPosition += velocity;
            CurrentSimMenuRT.anchoredPosition += velocity;
        }
        internal static void AnimGoToLeft()
        {
            if (AnimFrameCounter.CurrentFrame == 8)
            {
                Vector2 position = new Vector2(-240, 0);
                Backgrounds.anchoredPosition = position;
                CurrentSimMenuRT.anchoredPosition = position;
                return;
            }
            Vector2 velocity = new Vector2(30, 0);
            Backgrounds.anchoredPosition += velocity;
            CurrentSimMenuRT.anchoredPosition += velocity;
        }
        internal static void GoToRight()
        {
            Vector2 position = new Vector2(640, 0);
            Backgrounds.anchoredPosition = position;
            Main.anchoredPosition = position;
            Screen.anchoredPosition = position;
            if (!BattleSimulator.MenuOpened) return;
            position = new Vector2(400, 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }
        internal static void GoToLeft()
        {
            Vector2 position = new Vector2(-240, 0);
            Backgrounds.anchoredPosition = position;
            Main.anchoredPosition = position;
            Screen.anchoredPosition = position;
            if (!BattleSimulator.MenuOpened) return;
            position = new Vector2(0, 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }
    }
}
