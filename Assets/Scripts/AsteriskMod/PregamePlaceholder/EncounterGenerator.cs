using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class EncounterGenerator : MonoBehaviour
    {
        private string[] encounterFileNames;
        private string music = "";
        private string encountertext = "";
        private string nextwaves = "";
        private string wavetimer = "";
        private string arenasize = "";
        private string enemies = "";
        private string enemypositions = "";
        private string autolinebreak = "";
        private string playerskipdocommand = "";
        private string unescape = "";
        private string sparetext = "";
        private string flee = "";
        private string fleetext = "";
        private string fleesuccess = "";
        private string fleetexts = "";
        private string revive = "";
        private string deathtext = "";
        private string deathmusic = "";
        private string EncounterStarting = "";
        private string EnemyDialogueStarting = "";
        private string EnemyDialogueEnding = "";
        private string DefenceEnding = "";
        private string HandleSpare = "";
        private string HandleItem = "";
        private string EnteringState = "";
        private string Update = "";
        private string BeforeDeath = "";

        private ModGenerator mainGen;

        public Dropdown uiGeneratePreset;
        public CheckBox uiMusic, uiEncountertext, uiEncountertextEmpty, uiNextwaves, uiWavetimer, uiWavetimerInf, uiArenasize, uiEnemies, uiEnemypositions,
                        uiAutolinebreak, uiPlayerskipdocommand, uiPlayerskipdocommandValue, uiUnescape,
                        uiSparetext, uiFlee, uiFleeValue, uiFleetext, uiFleesuccess, uiFleetexts, uiRevive, uiDeathtext, uiDeathmusic;
        public CheckBox uiEncounterStarting, uiEnemyDialogueStarting, uiEnemyDialogueEnding, uiEnemyDialogueEndingExample, uiDefenceEnding, uiDefenceEndingExample,
                        uiHandleSpare, uiHandleItem, uiEnteringState, uiUpdate, uiBeforeDeath;

        private void InitializeCheckBox(CheckBox self, CheckBox child, bool linkActivate)
        {
            self.onValueChanged.RemoveAllListeners();
            self.onValueChangedFromUser.RemoveAllListeners();
            //self.onValueChangedFromScript.RemoveAllListeners();
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
            InitializeCheckBox(uiMusic);
            InitializeCheckBox(uiEncountertext, uiEncountertextEmpty, true);
            InitializeCheckBox(uiNextwaves);
            InitializeCheckBox(uiWavetimer, uiWavetimerInf, true);
            InitializeCheckBox(uiArenasize);
            InitializeCheckBox(uiEnemies);
            InitializeCheckBox(uiEnemypositions);
            InitializeCheckBox(uiAutolinebreak);
            InitializeCheckBox(uiPlayerskipdocommand, uiPlayerskipdocommandValue, true);
            InitializeCheckBox(uiUnescape);
            InitializeCheckBox(uiSparetext);
            InitializeCheckBox(uiFlee, uiFleeValue, true);
            InitializeCheckBox(uiFleetext);
            InitializeCheckBox(uiFleesuccess);
            InitializeCheckBox(uiFleetexts);
            InitializeCheckBox(uiRevive);
            InitializeCheckBox(uiDeathtext);
            InitializeCheckBox(uiDeathmusic);
            InitializeCheckBox(uiEncounterStarting);
            InitializeCheckBox(uiEnemyDialogueStarting);
            InitializeCheckBox(uiEnemyDialogueEnding, uiEnemyDialogueEndingExample, true);
            InitializeCheckBox(uiDefenceEnding, uiDefenceEndingExample, true);
            InitializeCheckBox(uiHandleSpare);
            InitializeCheckBox(uiHandleItem);
            InitializeCheckBox(uiEnteringState);
            InitializeCheckBox(uiUpdate);
            InitializeCheckBox(uiBeforeDeath);
        }

        private void Preset(int value)
        {
            switch (value)
            {
                case ModGenerator.PRESET_ENCOUNTER_SKELETON:
                    uiMusic.Checked = false;
                    uiEncountertext.Checked = true;
                        uiEncountertextEmpty.Checked = false;
                    uiNextwaves.Checked = true;
                    uiWavetimer.Checked = true;
                        uiWavetimerInf.Checked = false;
                    uiArenasize.Checked = true;
                    uiEnemies.Checked = true;
                    uiEnemypositions.Checked = true;
                    uiAutolinebreak.Checked = false;
                    uiPlayerskipdocommand.Checked = false;
                        uiPlayerskipdocommandValue.Checked = false;
                    uiUnescape.Checked = false;
                    uiSparetext.Checked = false;
                    uiFlee.Checked = false;
                        uiFleeValue.Checked = true;
                    uiFleetext.Checked = false;
                    uiFleesuccess.Checked = false;
                    uiFleetexts.Checked = false;
                    uiRevive.Checked = false;
                    uiDeathtext.Checked = false;
                    uiDeathmusic.Checked = false;

                    uiEncounterStarting.Checked = true;
                    uiEnemyDialogueStarting.Checked = true;
                    uiEnemyDialogueEnding.Checked = true;
                        uiEnemyDialogueEndingExample.Checked = true;
                    uiDefenceEnding.Checked = true;
                        uiDefenceEndingExample.Checked = true;
                    uiHandleSpare.Checked = true;
                    uiHandleItem.Checked = true;
                    uiEnteringState.Checked = false;
                    uiUpdate.Checked = false;
                    uiBeforeDeath.Checked = false;
                    break;
                case ModGenerator.PRESET_EMPTY:
                    uiMusic.Checked = false;
                    uiEncountertext.Checked = false;
                        //uiEncountertextEmpty.Checked = false;
                    uiNextwaves.Checked = false;
                    uiWavetimer.Checked = false;
                        //uiWavetimerInf.Checked = false;
                    uiArenasize.Checked = false;
                    uiEnemies.Checked = false;
                    uiEnemypositions.Checked = false;
                    uiAutolinebreak.Checked = false;
                    uiPlayerskipdocommand.Checked = false;
                        //uiPlayerskipdocommandValue.Checked = false;
                    uiUnescape.Checked = false;
                    uiSparetext.Checked = false;
                    uiFlee.Checked = false;
                        //uiFleeValue.Checked = false;
                    uiFleetext.Checked = false;
                    uiFleesuccess.Checked = false;
                    uiFleetexts.Checked = false;
                    uiRevive.Checked = false;
                    uiDeathtext.Checked = false;
                    uiDeathmusic.Checked = false;

                    uiEncounterStarting.Checked = false;
                    uiEnemyDialogueStarting.Checked = false;
                    uiEnemyDialogueEnding.Checked = false;
                        //uiEnemyDialogueEndingExample.Checked = false;
                    uiDefenceEnding.Checked = false;
                        //uiDefenceEndingExample.Checked = false;
                    uiHandleSpare.Checked = false;
                    uiHandleItem.Checked = false;
                    uiEnteringState.Checked = false;
                    uiUpdate.Checked = false;
                    uiBeforeDeath.Checked = false;
                    break;
                case ModGenerator.PRESET_NIL256:
                    uiMusic.Checked = true;
                    uiEncountertext.Checked = true;
                        uiEncountertextEmpty.Checked = true;
                    uiNextwaves.Checked = true;
                    uiWavetimer.Checked = true;
                        uiWavetimerInf.Checked = true;
                    uiArenasize.Checked = true;
                    uiEnemies.Checked = true;
                    uiEnemypositions.Checked = true;
                    uiAutolinebreak.Checked = false;
                    uiPlayerskipdocommand.Checked = true;
                        uiPlayerskipdocommandValue.Checked = true;
                    uiUnescape.Checked = false;
                    uiSparetext.Checked = false;
                    uiFlee.Checked = true;
                        uiFleeValue.Checked = false;
                    uiFleetext.Checked = false;
                    uiFleesuccess.Checked = false;
                    uiFleetexts.Checked = false;
                    uiRevive.Checked = false;
                    uiDeathtext.Checked = false;
                    uiDeathmusic.Checked = false;

                    uiEncounterStarting.Checked = true;
                    uiEnemyDialogueStarting.Checked = true;
                    uiEnemyDialogueEnding.Checked = true;
                        uiEnemyDialogueEndingExample.Checked = false;
                    uiDefenceEnding.Checked = true;
                        uiDefenceEndingExample.Checked = false;
                    uiHandleSpare.Checked = true;
                    uiHandleItem.Checked = true;
                    uiEnteringState.Checked = true;
                    uiUpdate.Checked = true;
                    uiBeforeDeath.Checked = true;
                    break;
                case ModGenerator.PRESET_ALL:
                    uiMusic.Checked = true;
                    uiEncountertext.Checked = true;
                        //uiEncountertextEmpty.Checked = true;
                    uiNextwaves.Checked = true;
                    uiWavetimer.Checked = true;
                        //uiWavetimerInf.Checked = true;
                    uiArenasize.Checked = true;
                    uiEnemies.Checked = true;
                    uiEnemypositions.Checked = true;
                    uiAutolinebreak.Checked = true;
                    uiPlayerskipdocommand.Checked = true;
                        //uiPlayerskipdocommandValue.Checked = true;
                    uiUnescape.Checked = true;
                    uiSparetext.Checked = true;
                    uiFlee.Checked = true;
                        //uiFleeValue.Checked = true;
                    uiFleetext.Checked = true;
                    uiFleesuccess.Checked = true;
                    uiFleetexts.Checked = true;
                    uiRevive.Checked = true;
                    uiDeathtext.Checked = true;
                    uiDeathmusic.Checked = true;

                    uiEncounterStarting.Checked = true;
                    uiEnemyDialogueStarting.Checked = true;
                    uiEnemyDialogueEnding.Checked = true;
                        uiEnemyDialogueEndingExample.Checked = true;
                    uiDefenceEnding.Checked = true;
                        uiDefenceEndingExample.Checked = true;
                    uiHandleSpare.Checked = true;
                    uiHandleItem.Checked = true;
                    uiEnteringState.Checked = true;
                    uiUpdate.Checked = true;
                    uiBeforeDeath.Checked = true;
                    break;
            }
        }


        private string CommentOut(string comment) { return mainGen.commentout ? comment : ""; }
        public void Generate()
        {
            // file-prefix comment
            if (uiGeneratePreset.value == ModGenerator.PRESET_ENCOUNTER_SKELETON) music = CommentOut("-- A basic encounter script skeleton you can copy and modify for your own creations.\n\n");
            // music
            if (uiMusic.Checked) music = "music = \"mus_battle1\"" + CommentOut(" --Either OGG or WAV. Extension is added automatically. Uncomment for custom music.") + "\n";
            // encountertext
            if (uiEncountertext.Checked) encountertext = "encountertext = \"" + (uiEncountertextEmpty.Checked ? "" : "Poseur strikes a pose!") + "\"" + CommentOut(" --Modify as necessary. It will only be read out in the action select screen.") + "\n";
            // nextwaves
            if (uiNextwaves.Checked) nextwaves = "nextwaves = {\"" + (mainGen.generateExampleWaveScripts ? "bullettest_chaserorb" : "") + "\"}\n";
            // wavetimer
            if (uiWavetimer.Checked) wavetimer = "wavetimer = " + (uiWavetimerInf.Checked ? "math.huge" : "4.0") + "\n";
            // arenasize
            if (uiArenasize.Checked) arenasize = "arenasize = {155, 130}\n\n";

            // enemies
            if (uiEnemies.Checked) enemies = "";
        }
    }
}
