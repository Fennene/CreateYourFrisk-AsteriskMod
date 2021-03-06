using AsteriskMod.ModdingHelperTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public static class AsteriskUtil
    {
        public static string ToStringAlt(this object obj)
        {
            if (obj is string) return "\"" + obj + "\"";
            return obj.ToString();
        }

        public static bool IsNullOrWhiteSpace(this string text) { return string.IsNullOrEmpty(text) || text.Trim().Length == 0; }

        public static string TrimOnce(this string text, char startPattern, char endPattern)
        {
            if (string.IsNullOrEmpty(text)) return text;
            int min = 0;
            int max = text.Length;
            if (text.StartsWith(startPattern.ToString())) min++;
            if (text.EndsWith(endPattern.ToString())) max--;
            string _ = "";
            for (var i = min; i < max; i++) _ += text[i];
            return _;
        }
        public static string TrimOnce(this string text, char pattern) { return text.TrimOnce(pattern, pattern); }

        public static bool StartsAndEndsWith(this string text, string startPattern, string endPattern) { return text.StartsWith(startPattern) && text.EndsWith(endPattern); }
        public static bool StartsAndEndsWith(this string text, string pattern) { return text.StartsAndEndsWith(pattern, pattern); }

        public static T[] Copy<T>(this T[] array)
        {
            T[] copied = new T[array.Length];
            for (var i = 0; i < array.Length; i++) copied[i] = array[i];
            return copied;
        }

        public static string ConvertArrayToString(object[] array)
        {
            string log = "";
            for (var i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrEmpty(log)) log += "\n";
                log += "[" + i.ToString() + "] = " + array[i].ToStringAlt();
            }
            return log;
        }


        public static bool IsCYFOverworld { get { return !GlobalControls.modDev; } }


        public static Languages ConvertToLanguage(string languageName, bool ignoreUnknown = true)
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
            return ignoreUnknown ? Languages.English : Languages.Unknown;
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

        public static string ConvertFromModVersionForLua(Asterisk.Versions version)
        {
            switch (version)
            {
                case Asterisk.Versions.InitialVersion:    return "v0.5";
                case Asterisk.Versions.CustomStateUpdate: return "v0.5.2.7";
                case Asterisk.Versions.UtilUpdate:        return "v0.5.2.8";
                case Asterisk.Versions.QOLUpdate:         return  "0.5.2.9";
                case Asterisk.Versions.TakeNewStepUpdate: return  "0.5.3";
            }
            return Asterisk.ModVersion;
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


        [ToDo("Translate Error")]
        public static Sprite GetSpriteFromFont(string fontName, char character)
        {
            if (fontName == null) throw new CYFException("SpriteFromFont: The first argument (the font name) is nil.");//\n\nSee the documentation for proper usage.");
            UnderFont uf = SpriteFontRegistry.Get(fontName);
            if (uf == null) throw new CYFException("The font \"" + fontName + "\" doesn't exist.\nYou should check if you made a typo, or if the font really is in your mod.");
            if (!uf.Letters.ContainsKey(character)) throw new CYFException("The font \"" + fontName + "\" doesn't contain character\'" + character + "\'");
            return uf.Letters[character];
        }

        public static void SwapSprite(Component target, Sprite sprite)
        {
            /*
            try
            {
                UIController.instance.encounter.EnabledEnemies[-1].bubbleWidth = 0;
            }
            catch (Exception)
            {
                UIController.instance.encounter.EnabledEnemies[-1].bubbleWidth = 0;
            }
            */
            if (sprite== null)
            {
                Debug.LogError("SwapSprite: Sprite is empty!");
            }
            Image img = target.GetComponent<Image>();
            if (!img)
            {
                SpriteRenderer img2 = target.GetComponent<SpriteRenderer>();
                Vector2 pivot = img2.GetComponent<RectTransform>().pivot;
                img2.sprite = sprite;
                img2.GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
                img2.GetComponent<RectTransform>().pivot = pivot;
            }
            else
            {
                Vector2 pivot = img.rectTransform.pivot;
                img.sprite = sprite;
                //enemyImg.SetNativeSize();
                img.rectTransform.sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
                img.rectTransform.pivot = pivot;
            }

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

        internal static float CalcTextWidth(FakeStaticTextManager txtmgr, int fromLetter = -1, int toLetter = -1, bool countEOLSpace = false, bool getLastSpace = false)
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

        internal static float CalcTextHeight(FakeStaticTextManager txtmgr, int fromLetter = -1, int toLetter = -1)
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


        public static string GetModsFolder() { return Path.Combine(FileLoader.DataRoot, "Mods"); }

        public static bool IsIgnoreFile(string fullpath) { return !File.Exists(fullpath) || new FileInfo(fullpath).Length > 1024 * 1024; } // 1MB

        public static readonly string[] SpecialInvalidPathNames = new[]
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

        public static bool IsInvalidPath(string path, out string errorMessage)
        {
            errorMessage = "";
            // Empty or White Space Check
            if (path.IsNullOrWhiteSpace())
            {
                errorMessage = EngineLang.Get("InvalidPathMessage", "Null");
                return true;
            }
            // Check StartsWith "." [for Mac]
            if (path.StartsWith("."))
            {
                errorMessage = EngineLang.Get("InvalidPathMessage", "StartWithDot");
                return true;
            }
            // Path.InvalidFileNameChars
            if (path.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                errorMessage = EngineLang.Get("InvalidPathMessage", "InvalidChar");
                return true;
            }
            // SpecialInvalidPathNames
            for (var i = 0; i < SpecialInvalidPathNames.Length; i++)
            {
                if (Path.GetFileNameWithoutExtension(path).ToUpper() == SpecialInvalidPathNames[i])
                {
                    errorMessage = "\"" + SpecialInvalidPathNames[i] + "\"" + EngineLang.Get("InvalidPathMessage", "SpecialName");
                    return true;
                }
            }
            return false;
        }

        public static bool PathExists(string fullPath, out string errorMessage)
        {
            errorMessage = "";
            //string parentDir = Path.GetDirectoryName(path);
            if (File.Exists(fullPath) || Directory.Exists(fullPath))
            {
                errorMessage = EngineLang.Get("InvalidPathMessage", "Duplication");
                return true;
            }
            return false;
        }

        public static List<string> GetFilesWithoutExtension(string rootDirFullPath, string relativeDirPath, string searchPattern = "*.lua")
        {
            List<string> relativeFileNames = new List<string>();

            string realDirPath = rootDirFullPath;
            if (!relativeDirPath.IsNullOrWhiteSpace()) realDirPath = Path.Combine(rootDirFullPath, relativeDirPath);

            string[] files = Directory.GetFiles(realDirPath, searchPattern, SearchOption.TopDirectoryOnly);
            for (var i = 0; i < files.Length; i++)
            {
                string path = Path.GetFileNameWithoutExtension(files[i]);
                if (!relativeDirPath.IsNullOrWhiteSpace()) path = Path.Combine(relativeDirPath, path).Replace('\\', '/');
                relativeFileNames.Add(path);
            }

            string[] directories = Directory.GetDirectories(realDirPath);
            for (var i = 0; i < directories.Length; i++)
            {
                string newRelativePath = Path.GetFileName(directories[i]);
                if (!relativeDirPath.IsNullOrWhiteSpace()) newRelativePath = Path.Combine(relativeDirPath, newRelativePath);
                List<string> dirFiles = GetFilesWithoutExtension(rootDirFullPath, newRelativePath, searchPattern);
                for (var j = 0; j < dirFiles.Count; j++)
                {
                    relativeFileNames.Add(dirFiles[j]);
                }
            }

            return relativeFileNames;
        }


        public static readonly char[] ValidVariableNameChars = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
            '_'
        };

        public static bool StartsWithNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            char first = value[0];
            return (first == '0' || first == '1' || first == '2' || first == '3' || first == '4' || first == '5' || first == '6' || first == '7' || first == '8' || first == '9');
        }

        public static bool IsValidVariableName(string value)
        {
            if (value.StartsWithNumber()) return false;
            for (var i = 0; i < value.Length; i++)
            {
                if (!ValidVariableNameChars.Contains(value[i])) return false;
            }
            return true;
        }

        public static string ConvertToVariableName(string value)
        {
            string varName = value.Replace(' ', '_').Replace('　', '_').Replace('\'', '_');
            if (varName.StartsWithNumber()) varName = "_" + varName;
            string temp = varName;
            varName = "";
            for (var i = 0; i < temp.Length; i++)
            {
                if (ValidVariableNameChars.Contains(temp[i])) varName += temp[i];
            }
            return varName;
        }



        public static void ThrowFakeNonexistentFunctionError(string className, string functionName)
        {
            throw new CYFException("cannot access field " + functionName + " of userdata<AsteriskMod.Lua." + className + ">");
        }


        private static readonly string[] TempDirNames = new[]
        {
            "T", "Tem", "Tempo", "Tempora", "Temporary",
            "P", "Ph", "Place", "Placehold", "Placeholder",
            "WD", "Gaster", "WD_Gaster", "Wingdings",
            "Null", "NullReferenceException", "Nil",
            "Void", "try", "Test", "ForTest",
            "CYF", "Unitale", "Asterisk", "AsteriskMod",
            "Dog", "TestDog", "Bark",
            "Nil256"
        };

        internal static string CreateTemporaryDirectory()
        {
            string modDir = Path.Combine(FileLoader.DataRoot, "Mods");
            string path = Path.Combine(modDir, "@Temp");
            int index = 0;
            while (Directory.Exists(path))
            {
                path = Path.Combine(modDir, "@" + TempDirNames[index]);
                index++;
                if (index >= TempDirNames.Length)
                {
                    throw new IOException("The engine can not prepare temporary folder\n" +
                                          "All options of temporary directory names are created by someone already!!\n" +
                                          "WHY.");
                }
            }
            Directory.CreateDirectory(path);
            return path;
        }

        private static bool ContainsInvalidCharOrName(string path, out string message)
        {
            message = "";
            // Empty
            if (string.IsNullOrEmpty(path))
            {
                message = "The path should not be empty string.";
                return true;
            }
            // Check Invalid
            if (path.StartsWith("."))
            {
                message = "The path can not start with \".\".";
                return true;
            }
            if (path.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                message = "The path contains invalid characters.";
                return true;
            }
            for (var i = 0; i < SpecialInvalidPathNames.Length; i++)
            {
                if (Path.GetFileNameWithoutExtension(path).ToUpper() == SpecialInvalidPathNames[i])
                {
                    message = SpecialInvalidPathNames[i] + " can not use to name of the path.";
                    return true;
                }
            }
            return false;
        }

        public static bool IsInvalidPath(string path, bool isFile, out string message)
        {
            message = "";
            if (ContainsInvalidCharOrName(path, out message)) return true;
            // Check Exist
            if (/*(isFile && File.Exists(Path.Combine(GetModsFolder(), path))) || */(!isFile && Directory.Exists(Path.Combine(GetModsFolder(), path))))
            {
                message = "That mod exists already.";
                return true;
            }
            // Actually, tries to create file/directory
            if (isFile)
            {
                string tempDir = CreateTemporaryDirectory();
                string tryPath = Path.Combine(tempDir, path);
                try { File.WriteAllText(tryPath, ""); }
                catch (ArgumentException)     { message = "The path contains invalid characters."; }
                catch (PathTooLongException)  { message = "The path is too long."; } // Check Length
                catch (NotSupportedException) { message = "The path contains \":\"."; }
                catch (IOException)           { message = "< UNKNWON IO EXCEPTION >"; }
                catch (Exception)             { message = "< UNKNWON EXCEPTION >"; }
                if (File.Exists(tryPath))
                {
                    try { File.Delete(tryPath); }
                    catch { /* ignore */ }
                }
                if (Directory.Exists(tempDir))
                {
                    try { Directory.Delete(tempDir, true); }
                    catch { /* ignore */ }
                }
            }
            else
            {
                string tryPath = Path.Combine(GetModsFolder(), path);
                try { Directory.CreateDirectory(tryPath); }
                catch (ArgumentException)     { message = "The path contains invalid characters."; }
                catch (PathTooLongException)  { message = "The path is too long."; } // Check Length
                catch (NotSupportedException) { message = "The path contains \":\"."; }
                catch (IOException)           { message = "< UNKNWON IO EXCEPTION >"; }
                catch (Exception)             { message = "< UNKNWON EXCEPTION >"; }
                if (Directory.Exists(tryPath))
                {
                    try { Directory.Delete(tryPath); }
                    catch { /* ignore */ }
                }
            }
            return (message != "");
        }
    }
}
