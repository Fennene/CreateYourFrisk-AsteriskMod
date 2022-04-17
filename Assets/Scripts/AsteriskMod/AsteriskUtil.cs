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
        public static float CalcTextWidth(StaticTextManager txtmgr, int fromLetter = -1, int toLetter = -1, bool countEOLSpace = false, bool getLastSpace = false)
        {
            float totalWidth = 0, totalWidthSpaceTest = 0, totalMaxWidth = 0, hSpacing = txtmgr.Charset.CharSpacing;
            if (fromLetter == -1) fromLetter = 0;
            if (txtmgr.text == null) return 0;
            if (toLetter == -1) toLetter = txtmgr.text.Text.Length - 1;
            if (fromLetter > toLetter || fromLetter < 0 || toLetter > txtmgr.text.Text.Length) return -1;

            for (int i = fromLetter; i <= toLetter; i++)
            {
                switch (txtmgr.text.Text[i])
                {
                    case '[':

                        if (txtmgr.Charset.Letters.ContainsKey(txtmgr.text.Text[i]))
                            totalWidth += txtmgr.Charset.Letters[txtmgr.text.Text[i]].textureRect.size.x + hSpacing;
                        break;
                    case '\r':
                    case '\n':
                        if (totalMaxWidth < totalWidthSpaceTest - hSpacing)
                            totalMaxWidth = totalWidthSpaceTest - hSpacing;
                        totalWidth = 0;
                        totalWidthSpaceTest = 0;
                        break;
                    default:
                        if (txtmgr.Charset.Letters.ContainsKey(txtmgr.text.Text[i]))
                        {
                            totalWidth += txtmgr.Charset.Letters[txtmgr.text.Text[i]].textureRect.size.x + hSpacing;
                            // Do not count end of line spaces
                            if (txtmgr.text.Text[i] != ' ' || countEOLSpace)
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
            if (toLetter == -1) toLetter = txtmgr.text.Text.Length;
            if (fromLetter > toLetter || fromLetter < 0 || toLetter > txtmgr.text.Text.Length) return -1;
            if (fromLetter == toLetter) return 0;
            for (int i = fromLetter; i < toLetter; i++)
            {
                if (!txtmgr.Charset.Letters.ContainsKey(txtmgr.text.Text[i])) continue;
                if (txtmgr.letterPositions[i].y < minY)
                    minY = txtmgr.letterPositions[i].y;
                if (txtmgr.letterPositions[i].y + txtmgr.Charset.Letters[txtmgr.text.Text[i]].textureRect.size.y > maxY)
                    maxY = txtmgr.letterPositions[i].y + txtmgr.Charset.Letters[txtmgr.text.Text[i]].textureRect.size.y;
            }
            return maxY - minY;
        }
    }
}
