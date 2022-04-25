using UnityEngine;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;

namespace AsteriskMod
{
    public static class AsteriskUtil
    {
        public static Languages ConvertToLanguage(string languageName)
        {
            languageName = languageName.ToLower();
            switch (languageName)
            {
                case "jp":
                case "ja":
                case "japan":
                case "japanese":
                    return Languages.Japanese;
                case "en":
                //case "en-US":
                //case "en-GB":
                //case "en-CA":
                //case "en-AU":
                case "english":
                    return Languages.English;
            }
            return Languages.English;
        }

        public static string ConvertFromLanguage(Languages language, bool shortName = true)
        {
            switch (language)
            {
                case Languages.Japanese:
                    return shortName ? "ja" : "Japanese";
                case Languages.English:
                    return shortName ? "en" : "English";
            }
            return "???";
        }

        public static Languages SwitchLanguage()
        {
            if (Asterisk.language == Languages.English) Asterisk.language = Languages.Japanese;
            else                                        Asterisk.language = Languages.English;
            return Asterisk.language;
        }

        public static string GetSafeSetAlMightyGlobalStatus()
        {
            if (!Asterisk.optionProtecter) return "Allow";
            return Asterisk.reportProtecter ? "Error" : "Ignore";
        }

        public static string SwitchSafeSetAlMightyGlobalStatus()
        {
            if (!Asterisk.optionProtecter) // Allow -> Error
            {
                Asterisk.optionProtecter = true;
                Asterisk.reportProtecter = true;
            }
            else if (Asterisk.reportProtecter) // Error -> Ignore
            {
                Asterisk.reportProtecter = false;
            }
            else // Ignore -> Allow
            {
                Asterisk.optionProtecter = false;
            }
            return GetSafeSetAlMightyGlobalStatus();
        }

        public static float CalcTextWidth(StaticTextManager txtmgr, int fromLetter = -1, int toLetter = -1, bool countEOLSpace = false, bool getLastSpace = false)
        {
            float totalWidth = 0, totalWidthSpaceTest = 0, totalMaxWidth = 0, hSpacing = txtmgr.Charset.CharSpacing;
            if (fromLetter == -1) fromLetter = 0;
            if (txtmgr.instantText == null) return 0;
            if (toLetter == -1) toLetter = txtmgr.instantText.Text.Length - 1;
            if (fromLetter > toLetter || fromLetter < 0 || toLetter > txtmgr.instantText.Text.Length) return -1;

            for (int i = fromLetter; i <= toLetter; i++)
            {
                switch (txtmgr.instantText.Text[i])
                {
                    /**
                    case '[':

                        if (txtmgr.Charset.Letters.ContainsKey(txtmgr.instantText.Text[i]))
                            totalWidth += txtmgr.Charset.Letters[txtmgr.instantText.Text[i]].textureRect.size.x + hSpacing;
                        break;
                    */
                    case '\r':
                    case '\n':
                        if (totalMaxWidth < totalWidthSpaceTest - hSpacing)
                            totalMaxWidth = totalWidthSpaceTest - hSpacing;
                        totalWidth = 0;
                        totalWidthSpaceTest = 0;
                        break;
                    default:
                        if (txtmgr.Charset.Letters.ContainsKey(txtmgr.instantText.Text[i]))
                        {
                            totalWidth += txtmgr.Charset.Letters[txtmgr.instantText.Text[i]].textureRect.size.x + hSpacing;
                            // Do not count end of line spaces
                            if (txtmgr.instantText.Text[i] != ' ' || countEOLSpace)
                                totalWidthSpaceTest = totalWidth;
                        }
                        break;
                }
            }
            if (totalMaxWidth < totalWidthSpaceTest - hSpacing)
                totalMaxWidth = totalWidthSpaceTest - hSpacing;
            return totalMaxWidth + (getLastSpace ? hSpacing : 0);
        }

        public static float CalcTextHeight(StaticTextManager txtmgr, int fromLetter = -1, int toLetter = -1)
        {
            float maxY = -999, minY = 999;
            if (fromLetter == -1) fromLetter = 0;
            if (toLetter == -1) toLetter = txtmgr.instantText.Text.Length;
            if (fromLetter > toLetter || fromLetter < 0 || toLetter > txtmgr.instantText.Text.Length) return -1;
            if (fromLetter == toLetter) return 0;
            for (int i = fromLetter; i < toLetter; i++)
            {
                if (!txtmgr.Charset.Letters.ContainsKey(txtmgr.instantText.Text[i])) continue;
                if (txtmgr.letterPositions[i].y < minY)
                    minY = txtmgr.letterPositions[i].y;
                if (txtmgr.letterPositions[i].y + txtmgr.Charset.Letters[txtmgr.instantText.Text[i]].textureRect.size.y > maxY)
                    maxY = txtmgr.letterPositions[i].y + txtmgr.Charset.Letters[txtmgr.instantText.Text[i]].textureRect.size.y;
            }
            return maxY - minY;
        }
    }
}
