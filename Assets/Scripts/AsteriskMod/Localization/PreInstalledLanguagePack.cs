using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod
{
    public static class PreInstalledLanguagePack
    {
        public static readonly Dictionary<string, TransText> English;
        public static readonly Dictionary<string, TransText> Japanese;

        static PreInstalledLanguagePack()
        {
            English = new Dictionary<string, TransText>();
            PrepareEnglish();
            Japanese = new Dictionary<string, TransText>();
            PrepareJapanese();
        }

        private static void PrepareEnglish()
        {
            English["DisclaimerText1"] = new TransText (
                "<b>Lua experience is <color='red'>REQUIRED</color> to make your own mods!!</b>",
                "<b><color='red'>KNOW YUOR CODE</color> R U'LL HVAE A BED TMIE!!!</b>"
            );
            English["DisclaimerText2"] = new TransText (
                "For support and newest versions, go to <b>/r/Unitale</b>.",
                "GO TO /R/UNITLAE. FOR UPDTAES!!!!!"
            );
            English["DisclaimerText3"] = new TransText (
                "Create Your Frisk is not owned by Toby Fox. Selling this engine or its mods is <b>illegal</b>.",
                "NO RELESLING HERE!!! IT'S RFEE!!! OR TUBY FEX WILL BE ANGER!!! U'LL HVAE A BED TMIE!!!"
            );

            English["ModSelectText1"] = new TransText (
                "Has N Encounters"
            );
            English["ModSelectText2"] = new TransText (
                " Press V to show/hide the description",
                " PRSES V TO SOHW/HIED THE DECSRIPTOIN"
            );
            English["ModSelectText3"] = new TransText (
                "Mod List",
                "MDO LITS"
            );
            English["ModSelectText4"] = new TransText (
                "Exit",
                "BYEE"
            );
            English["ModSelectText5"] = new TransText (
                "Back",
                "BCAK"
            );
            English["ModSelectText6"] = new TransText (
                "Options",
                "OPSHUNZ"
            );

            English["OptionWinText1"] = new TransText (
                "Create New Mod"
            );
            English["OptionWinText1Desc"] = new TransText (
                "Creates your new mod.\n\nGenerates skeleton of a mod\nin this option."
            );
            English["OptionWinText2"] = new TransText (
                "Open Documentation"
            );
            English["OptionWinText2Desc"] = new TransText (
                "Opens the documentation of Create Your Frisk."
            );
            English["OptionWinText3"] = new TransText (
                "CYF Option"
            );
            English["OptionWinText3Desc"] = new TransText (
                "Goes to the normal option."
            );
            English["OptionWinText4"] = new TransText (
                "Asterisk Mod Option"
            );
            English["OptionWinText4Desc"] = new TransText (
                "Goes to the option that AsteriskMod adds."
            );
            English["OptionWinText5"] = new TransText (
                "Restore Default Folder"
            );

            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
            English[""] = new TransText (
                "",
                ""
            );
        }

        private static void PrepareJapanese()
        {
        }
    }
}
