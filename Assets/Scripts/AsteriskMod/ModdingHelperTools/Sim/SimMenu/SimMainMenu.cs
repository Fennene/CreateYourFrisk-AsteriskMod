using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimMainMenu : MonoBehaviour
    {
        //* private static bool _uniqueCheck;

        //internal static Button StateMenu;
        internal static Button GoToScreenMenu;
        //internal static Button GoToDialogBoxMenu;
        internal static Button GoToSprProjSimMenu;
        internal static Button Exit;

        private void Awake()
        {
            //* if (_uniqueCheck) throw new Exception("SimMainMenuが複数存在します。");
            //* _uniqueCheck = true;

            GoToScreenMenu     = transform.Find("Screen")    .GetComponent<Button>();
            GoToSprProjSimMenu = transform.Find("SprProjSim").GetComponent<Button>();
            Exit               = transform.Find("Exit")      .GetComponent<Button>();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(GoToScreenMenu, () =>
            {
                if (AnimFrameCounter.IsRunningAnimation) return;
                SimMenuWindowManager.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.Screen);
            });
            UnityButtonUtil.AddListener(GoToSprProjSimMenu, () =>
            {
                if (AnimFrameCounter.IsRunningAnimation) return;
                SimMenuWindowManager.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Main, SimMenuWindowManager.DisplayingSimMenu.SprProjSim);
            });
            UnityButtonUtil.AddListener(Exit, () =>
            {
                FakeSpriteController spr = SimSprProjSimMenu.CreateSprite("black", childNumber: -1);
                if (GameObject.Find("TopLayer")) spr.layer = "Top";
                spr.Scale(640, 480);
                AsteriskEngine.IsSimulator = false;
                SceneManager.LoadScene("MHTMenu");
                AsteriskEngine.Reset();
            });
        }
    }
}
