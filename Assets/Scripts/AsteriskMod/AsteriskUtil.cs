using System;
using System.IO;
using UnityEngine;

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

        public static bool IsV053 { get { return AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.TakeNewStepUpdate; } }

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

        public static string GetModsFolder()
        {
            return Path.Combine(FileLoader.DataRoot, "Mods");
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

        public static readonly string[] SpecialInvalidPathNames = new[]
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

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

        public static void ThrowFakeNonexistentFunctionError(string className, string functionName)
        {
            throw new CYFException("cannot access field " + functionName + " of userdata<AsteriskMod.Lua." + className + ">");
        }
    }
}
