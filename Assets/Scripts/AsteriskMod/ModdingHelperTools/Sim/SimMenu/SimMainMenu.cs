using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMainMenu : MonoBehaviour
    {
        //* private static bool _uniqueCheck;

        private Button StateMenu;
        private Button GoToScreenMenu;
        //internal Button GoToDialogBoxMenu;
        private Button GoToSprProjSimMenu;
        private Button GoToSTTextSimMenu;
        private Button Exit;

        private void Awake()
        {
            //* if (_uniqueCheck) throw new Exception("SimMainMenuが複数存在します。");
            //* _uniqueCheck = true;

            StateMenu          = transform.Find("State")     .GetComponent<Button>();
            GoToScreenMenu     = transform.Find("Screen")    .GetComponent<Button>();
            GoToSprProjSimMenu = transform.Find("SprProjSim").GetComponent<Button>();
            GoToSTTextSimMenu  = transform.Find("StaticText").GetComponent<Button>();
            Exit               = transform.Find("Exit")      .GetComponent<Button>();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(StateMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.GameState);
            });
            UnityButtonUtil.AddListener(GoToScreenMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.Screen);
            });

            UnityButtonUtil.AddListener(GoToSprProjSimMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.SprProjSim);
            });
            UnityButtonUtil.AddListener(GoToSTTextSimMenu, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.STTextSim);
            });
            UnityButtonUtil.AddListener(Exit, () =>
            {
                FakeSpriteController spr = SimSprProjSimMenu.CreateSprite("black", childNumber: -1);
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
