using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMenuWindowManager : MonoBehaviour
    {
        internal static SimMenuWindowManager Instance;

        private RectTransform Backgrounds;
        private RectTransform Main;
        private RectTransform State;
        private RectTransform Player;
        private RectTransform Screen;
        private RectTransform Arena;
        private RectTransform SprSim;
        private RectTransform STTextSim;

        private bool animationRequester;
        private RectTransform closeTarget;
        private RectTransform openTarget;
        private DisplayingSimMenu target;
        private float animationSpeed;

        private void Awake()
        {
            Backgrounds = transform.Find("Backgrounds")   .GetComponent<RectTransform>();
            Main        = transform.Find("MainMenu")      .GetComponent<RectTransform>();
            State       = transform.Find("GameStateMenu") .GetComponent<RectTransform>();
            Player      = transform.Find("PlayerMenu") .GetComponent<RectTransform>();
            Screen      = transform.Find("ScreenMenu")    .GetComponent<RectTransform>();
            Arena       = transform.Find("DialogBoxMenu") .GetComponent<RectTransform>();
            SprSim      = transform.Find("SprProjSimMenu").GetComponent<RectTransform>();
            STTextSim   = transform.Find("STTextSimMenu") .GetComponent<RectTransform>();

            animationRequester = false;
            closeTarget = null;
            openTarget = null;
            target = DisplayingSimMenu.Main;
            animationSpeed = 0f;

            _currentSimMenu = DisplayingSimMenu.Main;

            Instance = this;
        }

        internal enum DisplayingSimMenu
        {
            Main,
            GameState,
            PlayerStatus,
            Screen,
            DialogBox,
            SprProjSim,
            StaticTextSim
        }
        private DisplayingSimMenu _currentSimMenu = DisplayingSimMenu.Main;
        internal DisplayingSimMenu CurrentSimMenu { get { return _currentSimMenu; } }

        private RectTransform ConvertToRT(DisplayingSimMenu menu)
        {
            switch (menu)
            {
                case DisplayingSimMenu.Main:
                    return Main;
                case DisplayingSimMenu.GameState:
                    return State;
                case DisplayingSimMenu.PlayerStatus:
                    return Player;
                case DisplayingSimMenu.Screen:
                    return Screen;
                case DisplayingSimMenu.DialogBox:
                    return Arena;
                case DisplayingSimMenu.SprProjSim:
                    return SprSim;
                case DisplayingSimMenu.StaticTextSim:
                    return STTextSim;
                default:
                    return Main;
            }
        }
        private RectTransform CurrentSimMenuRT { get { return ConvertToRT(CurrentSimMenu); } }

        internal bool ChangePage(DisplayingSimMenu closeMenu, DisplayingSimMenu openMenu)
        {
            if (!SimInstance.BattleSimulator.MenuOpened) return false;
            closeTarget = ConvertToRT(closeMenu);
            openTarget  = ConvertToRT(openMenu);
            if (closeTarget == openTarget) return false;
            animationRequester = true;
            target = openMenu;
            animationSpeed = 15 * (SimInstance.BattleSimulator.LeftMenu ? 1 : -1);
            AnimFrameCounter.Instance.StartAnimation();
            return true;
        }

        private void Update()
        {
            if (!AnimFrameCounter.Instance.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.Instance.CurrentFrame >= 16)
            {
                closeTarget.anchoredPosition = new Vector2((SimInstance.BattleSimulator.LeftMenu ? -240 : 640), 0);
                openTarget.anchoredPosition  = new Vector2((SimInstance.BattleSimulator.LeftMenu ? 0 : 400), 0);
                _currentSimMenu = target;
                AnimFrameCounter.Instance.EndAnimation();
                animationRequester = false;
                return;
            }
            closeTarget.anchoredPosition -= new Vector2(animationSpeed, 0);
            openTarget .anchoredPosition += new Vector2(animationSpeed, 0);
        }

        internal void AnimOpen(float speed)
        {
            Backgrounds.anchoredPosition += new Vector2(speed, 0);
            CurrentSimMenuRT.anchoredPosition += new Vector2(speed, 0);
        }
        internal void AnimClose(float speed)
        {
            Backgrounds.anchoredPosition += new Vector2(speed, 0);
            CurrentSimMenuRT.anchoredPosition += new Vector2(speed, 0);
        }
        internal void Open()
        {
            Vector2 position = new Vector2((SimInstance.BattleSimulator.LeftMenu ? 0 : 400), 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }
        internal void Close()
        {
            Vector2 position = new Vector2((SimInstance.BattleSimulator.LeftMenu ? -240 : 640), 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }

        internal void AnimGoToRight()
        {
            if (AnimFrameCounter.Instance.CurrentFrame == 8)
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
        internal void AnimGoToLeft()
        {
            if (AnimFrameCounter.Instance.CurrentFrame == 8)
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
        internal void GoToRight()
        {
            Vector2 position = new Vector2(640, 0);
            Backgrounds.anchoredPosition = position;
            Main.anchoredPosition = position;
            State.anchoredPosition = position;
            Player.anchoredPosition = position;
            Screen.anchoredPosition = position;
            Arena.anchoredPosition = position;
            SprSim.anchoredPosition = position;
            STTextSim.anchoredPosition = position;
            if (!SimInstance.BattleSimulator.MenuOpened) return;
            position = new Vector2(400, 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }
        internal void GoToLeft()
        {
            Vector2 position = new Vector2(-240, 0);
            Backgrounds.anchoredPosition = position;
            Main.anchoredPosition = position;
            State.anchoredPosition = position;
            Player.anchoredPosition = position;
            Screen.anchoredPosition = position;
            Arena.anchoredPosition = position;
            SprSim.anchoredPosition = position;
            STTextSim.anchoredPosition = position;
            if (!SimInstance.BattleSimulator.MenuOpened) return;
            position = new Vector2(0, 0);
            Backgrounds.anchoredPosition = position;
            CurrentSimMenuRT.anchoredPosition = position;
        }

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
