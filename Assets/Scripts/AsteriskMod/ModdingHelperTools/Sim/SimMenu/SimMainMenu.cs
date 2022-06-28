using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMainMenu : MonoBehaviour
    {
        //* private static bool _uniqueCheck;

        private Button GoToStateMenu;
        private Button GoToPlayerMenu;
        private Button GoToScreenMenu;
        private Button GoToDialogBoxMenu;
        private Button GoToSprProjSimMenu;
        private Button GoToSTTextSimMenu;
        private Button Exit;

        private void Awake()
        {
            //* if (_uniqueCheck) throw new Exception("SimMainMenuが複数存在します。");
            //* _uniqueCheck = true;

            GoToStateMenu      = transform.Find("State")     .GetComponent<Button>();
            GoToPlayerMenu     = transform.Find("Player")    .GetComponent<Button>();
            GoToScreenMenu     = transform.Find("Screen")    .GetComponent<Button>();
            GoToDialogBoxMenu  = transform.Find("DialogBox") .GetComponent<Button>();
            GoToSprProjSimMenu = transform.Find("SprProjSim").GetComponent<Button>();
            GoToSTTextSimMenu  = transform.Find("StaticText").GetComponent<Button>();
            Exit               = transform.Find("Exit")      .GetComponent<Button>();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(GoToStateMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.GameState);
            });
            UnityButtonUtil.AddListener(GoToPlayerMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.PlayerStatus);
            });
            UnityButtonUtil.AddListener(GoToScreenMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.Screen);
            });
            UnityButtonUtil.AddListener(GoToDialogBoxMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.DialogBox);
            });
            UnityButtonUtil.AddListener(GoToSprProjSimMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.SprProjSim);
            });
            UnityButtonUtil.AddListener(GoToSTTextSimMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.StaticTextSim);
            });
            UnityButtonUtil.AddListener(Exit, () =>
            {
                FakeSpriteController spr = SimSprProjSimMenu.Instance.CreateSprite("black", childNumber: -1);
                if (GameObject.Find("TopLayer")) spr.layer = "Top";
                spr.Scale(640, 480);
                AsteriskEngine.IsSimulator = false;
                SceneManager.LoadScene("MHTMenu");
                AsteriskEngine.Reset();
                //SimInstance.Dispose();
            });
        }
    }
}
