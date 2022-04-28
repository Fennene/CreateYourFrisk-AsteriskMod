using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class MonsterGenerator : MonoBehaviour
    {
        private string varComments = "";
        private string varCommands = "";
        private string varRandomDialogue = "";
        private string varCurrentDialogue = "";
        private string varDefencemisstext = "";
        private string varNoattackmisstext = "";
        private string varCancheck = "";
        private string varCanspare = "";
        private string varSprite = "";
        private string varDialogbubble = "";
        private string varDialogueprefix = "";
        private string varName = "";
        private string varHp = "";
        private string varMaxhp = "";
        private string varAtk = "";
        private string varDef = "";
        private string varXp = "";
        private string varGold = "";
        private string varCheck = "";
        private string varUnkillable = "";
        private string varPosx = "";
        private string varPosy = "";
        private string varFont = "";
        private string varVoice = "";
        private string funcHandleAttack = "";
        private string funcOnDeath = "";
        private string funcOnSpare = "";
        private string funcBeforeDamageCalculation = "";
        private string funcBeforeDamageValues = "";
        private string funcHandleCustomCommand = "";

        private ModGenerator mainGen;

        public Dropdown uiGeneratePreset;
        public CheckBox uiComments, uiCommentsEmpty, uiCommands, uiCommandsCheck, uiCommandsEmpty, uiRandomdialogue, uiCurrentdialogue,
                        uiDefencemisstext, uiNoattackmisstext,
                        uiCancheck, uiCancheckValue, uiCanspare, uiCanspareValue, uiSprite, uiSpriteEmpty,
                        uiDialogbubble, uiDialogueprefix,
                        uiName, uiHp, uiMaxhp, uiAtk, uiDef, uiXp, uiGold, uiCheck, uiUnkillable, uiuiPosx, uiPosy, uiFont, uiVoice;
        public CheckBox uiHandleAttack, uiOnDeath, uiOnSpare, uiBeforeDamageCalculation, uiBeforeDamageValues,
                        uiHandleCustomCommand, uiHandleCustomCommandCheck, uiHandleCustomCommandExample;

        private void InitializeCheckBox(CheckBox self, CheckBox child, bool linkActivate)
        {
            self.onValueChanged.RemoveAllListeners();
            self.onValueChangedFromUser.RemoveAllListeners();
            self.onValueChangedFromUser.AddListener(_ => { uiGeneratePreset.value = ModGenerator.PRESET_CUSTOM; });
            if (child == null) return;
            child.onValueChanged.RemoveAllListeners();
            child.onValueChangedFromUser.RemoveAllListeners();
            child.onValueChangedFromUser.AddListener(_ => { uiGeneratePreset.value = ModGenerator.PRESET_CUSTOM; });
            self.AddChildCheckBox(child, linkActivate);
        }
        private void InitializeCheckBox(CheckBox self, CheckBox child = null) { InitializeCheckBox(self, child, true); }

        public void Initialize(ModGenerator main)
        {
            mainGen = main;
            uiGeneratePreset.onValueChanged.RemoveAllListeners();
            uiGeneratePreset.onValueChanged.AddListener(newValue => Preset(newValue));
            uiGeneratePreset.value = ModGenerator.PRESET_ENCOUNTER_SKELETON;
            InitializeCheckBox(uiComments, uiCommentsEmpty, true);

            InitializeCheckBox(uiCommands);
            InitializeCheckBox(uiCommandsCheck);
            InitializeCheckBox(uiCommandsEmpty);

            InitializeCheckBox(uiRandomdialogue);
            InitializeCheckBox(uiCurrentdialogue);
            InitializeCheckBox(uiDefencemisstext);
            InitializeCheckBox(uiNoattackmisstext);
        }

        private void Preset(int value)
        {
            switch (value)
            {
                case ModGenerator.PRESET_ENCOUNTER_SKELETON:
                    break;
                case ModGenerator.PRESET_EMPTY:
                    break;
                case ModGenerator.PRESET_NIL256:
                    break;
                case ModGenerator.PRESET_ALL:
                    break;
            }
        }
    }
}
