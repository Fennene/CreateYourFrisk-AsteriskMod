using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeUIController : MonoBehaviour
    {
        public static FakeUIController instance;
        //* public TextManager mainTextManager; // Main text manager in the arena

        /*
        private static Sprite fightButtonSprite, actButtonSprite, itemButtonSprite, mercyButtonSprite;  // UI button sprites when the soul is selecting them
        private Image fightButton, actButton, itemButton, mercyButton;                                  // UI button objects in the scene
        */
        //* internal static ButtonManager ActionButtonManager { get; private set; }

        private GameObject arenaParent; // Arena's parent, which will be used to manipulate it
        //* public GameObject psContainer;  // Container for any particle effect used when using sprite.Dust() and when sparing or killing an enemy
        private AudioSource uiAudio;    // AudioSource only used to play the sound menumove when the Player moves in menus

        [HideInInspector] public FakeFightUIController fightUI; // Main Player attack handler

        //* private bool[] spareList;                                       // Includes a list telling which enemies have just been spared
        //* public Dictionary<int, string[]> messages;                      // Stores the messages enemies will say in the state ENEMYDIALOGUE
        public bool stateSwitched;                                      // True if the state has been changed this frame, false otherwise

        public delegate void Message();
        public static event Message SendToStaticInit;

        private void Awake()
        {
            /**
            fightButtonSprite = SpriteRegistry.Get("UI/Buttons/fightbt_1");
            actButtonSprite = SpriteRegistry.Get("UI/Buttons/actbt_1");
            itemButtonSprite = SpriteRegistry.Get("UI/Buttons/itembt_1");
            mercyButtonSprite = SpriteRegistry.Get("UI/Buttons/mercybt_1");

            ActionButtonManager.Awake();
            */

            arenaParent = GameObject.Find("arena_border_outer");
            //canvasParent = GameObject.Find("Canvas");
            uiAudio = GetComponent<AudioSource>();
            uiAudio.clip = AudioClipRegistry.GetSound("menumove");

            instance = this;
        }

        private void Start()
        {
            //* messages = new Dictionary<int, string[]>(); //?

            // reset GlobalControls' frame timer
            GlobalControls.frame = 0; // maybe not need...?

            /**
            mainTextManager = GameObject.Find("TextManager").GetComponent<TextManager>();
            mainTextManager.SetEffect(new TwitchEffect(mainTextManager));
            mainTextManager.ResetFont();
            mainTextManager.SetCaller(EnemyEncounter.script);
            */
            //*encounter = FindObjectOfType<EnemyEncounter>();
            /**
            fightButton = GameObject.Find("FightBt").GetComponent<Image>();
            actButton = GameObject.Find("ActBt").GetComponent<Image>();
            itemButton = GameObject.Find("ItemBt").GetComponent<Image>();
            mercyButton = GameObject.Find("MercyBt").GetComponent<Image>();

            ActionButtonManager.Start();
            */
            FakeArenaManager.instance.ResizeImmediate(ArenaManager.UIWidth, ArenaManager.UIHeight);

            //* MusicManager.src = Camera.main.GetComponent<AudioSource>();

            //* ProjectileController.globalPixelPerfectCollision = false;
            //* ControlPanel.instance.FrameBasedMovement = false;
            //* LuaScriptBinder.CopyToBattleVar(); //?
            /**
            spareList = new bool[encounter.enemies.Length];
            for (int i = 0; i < spareList.Length; i++)
                spareList[i] = false;
            if (EnemyEncounter.script.GetVar("Update") != null)
                encounterHasUpdate = true;
            */
            GameObject.Find("Main Camera").GetComponent<ProjectileHitboxRenderer>().enabled = false;//* !GameObject.Find("Main Camera").GetComponent<ProjectileHitboxRenderer>().enabled;

            SimInstance.FakeStaticInits.SendLoaded();
            /**
            psContainer = new GameObject("psContainer");
            // The following is a trick to make psContainer spawn within the battle scene, rather than the overworld scene, if in the overworld
            psContainer.transform.SetParent(mainTextManager.transform);
            psContainer.transform.SetParent(null);
            psContainer.transform.SetAsFirstSibling();
            */

            //Play that funky music
            //* if (MusicManager.IsStoppedOrNull(PlayerOverworld.audioKept))
            //*     GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();

            if (SendToStaticInit != null)
                SendToStaticInit();

            // PlayerController.instance.Awake();
            FakePlayerController.instance.playerAbs = new Rect(0, 0,
                                                            FakePlayerController.instance.selfImg.sprite.texture.width - 8,
                                                            FakePlayerController.instance.selfImg.sprite.texture.height - 8);
            FakePlayerController.instance.setControlOverride(true);
            FakePlayerController.instance.SetPosition(48, 25, true);
            fightUI = GameObject.Find("FightUI").GetComponent<FakeFightUIController>();
            fightUI.gameObject.SetActive(false);

            if (UnitaleUtil.firstErrorShown) return;
            //* encounter.CallOnSelfOrChildren("EncounterStarting");

            if (!stateSwitched)
                SwitchState(UIController.UIState.ACTIONSELECT, true);
        }

        public void SwitchState(UIController.UIState newState, bool first = false)
        {
            stateSwitched = true;

            /**
            // Quick and dirty addition to add some humor to the Run away command.
            if (musicPausedFromRunning)
            {
                Camera.main.GetComponent<AudioSource>().UnPause();
                musicPausedFromRunning = false;
            }
            */

            if (newState == UIController.UIState.DEFENDING || newState == UIController.UIState.ENEMYDIALOGUE)
            {
                FakePlayerController.instance.setControlOverride(newState != UIController.UIState.DEFENDING);
                //* mainTextManager.DestroyChars();
                FakePlayerController.instance.SetPosition(320, 160, true);
                FakePlayerController.instance.GetComponent<Image>().enabled = true;
                /*
                fightButton.overrideSprite = null;
                actButton.overrideSprite = null;
                itemButton.overrideSprite = null;
                mercyButton.overrideSprite = null;

                ActionButtonManager.HideAllOverrideSprite();
                */
                //* mainTextManager.SetPause(true);
            }
            else
            {
                if (!first && !FakeArenaManager.instance.firstTurn)
                    FakeArenaManager.instance.resetArena();
                FakePlayerController.instance.invulTimer = 0.0f;
                FakePlayerController.instance.setControlOverride(true);
            }

            UIController.UIState oldState = SimInstance.BattleSimulator.CurrentState;
            SimInstance.BattleSimulator.CurrentState = newState;

            switch (SimInstance.BattleSimulator.CurrentState)
            {
                case UIController.UIState.ACTIONSELECT:
                    //* forcedAction = Actions.NONE;
                    FakePlayerController.instance.setControlOverride(true);
                    FakePlayerController.instance.GetComponent<Image>().enabled = true;

                    FakePlayerController.instance.SetPosition(48, 25, true);
                    break;
                case UIController.UIState.DEFENDING:
                    FakeArenaManager.instance.Resize((int)SimInstance.BattleSimulator.arenaSize.x, (int)SimInstance.BattleSimulator.arenaSize.y);
                    FakePlayerController.instance.setControlOverride(false);
                    //* encounter.NextWave();
                    // ActionDialogResult(new TextMessage("This is where you'd\rdefend yourself.\nBut the code was spaghetti.", true, false), UIState.ACTIONSELECT);
                    break;
            }

            if (!first) SimDialogBoxMenu.Instance.UpdateState();
        }
    }
}
