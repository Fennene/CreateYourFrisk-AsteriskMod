using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimStateMenu : MonoBehaviour
    {
        internal Button BackButton;

        private Button State_ActionSelect;
        private Button State_Defending;

        private void Awake()
        {
            BackButton = transform.Find("MenuNameLabel").Find("BackButton").GetComponent<Button>();

            Transform State = transform.Find("State");
            State_ActionSelect = State.Find("ACTIONSELECT").GetComponent<Button>();
            State_Defending    = State.Find("DEFENDING")   .GetComponent<Button>();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.GameState, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
            UnityButtonUtil.AddListener(State_ActionSelect, () =>
            {
                if (SimInstance.BattleSimulator.CurrentState != UIController.UIState.ACTIONSELECT)
                    FakeUIController.instance.SwitchState(UIController.UIState.ACTIONSELECT);
            });
            UnityButtonUtil.AddListener(State_Defending, () =>
            {
                if (SimInstance.BattleSimulator.CurrentState != UIController.UIState.DEFENDING)
                    FakeUIController.instance.SwitchState(UIController.UIState.DEFENDING);
            });
        }
    }
}
