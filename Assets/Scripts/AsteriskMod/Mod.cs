using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AsteriskMod
{
    public class Mod
    {
        private DirectoryInfo _dir;
        private ModInfo _modInfo;
        private DirectoryInfo _encounterDir;
        private FileInfo[] _encounters;

        public Mod(DirectoryInfo dir)
        {
            _dir = dir;
            _modInfo = ModInfo.GetFromFile(_dir.Name, false);
            _encounterDir = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + _dir.Name + "/Lua/Encounters"));
            if (!_encounterDir.Exists || _encounterDir.GetFiles().Length <= 0) _encounters = new FileInfo[0];
            else                                                               _encounters = _encounterDir.GetFiles("*.lua");

            RealName = _dir.Name;
            RealEncounters = new string[_encounters.Length];
            for (var i = 0; i < _encounters.Length; i++)
            {
                RealEncounters[i] = Path.GetFileNameWithoutExtension(_encounters[i].Name);
            }

            Font = _modInfo.ScreenFont;
            Title = _dir.Name;
            SubTitle = null;
            Description = _modInfo.Description ?? "";

            if (RealEncounters.Length == 1)
            {
                Encounters = new string[1] { RealEncounters[0] };
                EncounterIndexes = new int[1] { 0 };
            }
            else
            {
                List<string> encountersTemp = new List<string>(RealEncounters.Length);
                List<int> encounterIndexesTemp = new List<int>(RealEncounters.Length);
                for (var i = 0; i < RealEncounters.Length; i++)
                {
                    string fileName = RealEncounters[i];
                    string showName = fileName;
                    if (_modInfo.EncounterNames != null && i < _modInfo.EncounterNames.Length && !string.IsNullOrEmpty(_modInfo.EncounterNames[i]))
                    {
                        showName = _modInfo.EncounterNames[i];
                    }
                    if (_modInfo.ShowEncounters != null)
                    {
                        if (!_modInfo.ShowEncounters.Contains(fileName)) continue;
                    }
                    else if (_modInfo.HideEncounters != null)
                    {
                        if (_modInfo.HideEncounters.Contains(fileName)) continue;
                    }
                    encountersTemp.Add(showName);
                    encounterIndexesTemp.Add(i);
                }
                Encounters = encountersTemp.ToArray();
                EncounterIndexes = encounterIndexesTemp.ToArray();
            }

            if (Asterisk.displayModInfo)
            {
                if (!string.IsNullOrEmpty(_modInfo.TitleOverride))    Title = _modInfo.TitleOverride;
                if (!string.IsNullOrEmpty(_modInfo.SubtitleOverride)) SubTitle = _modInfo.SubtitleOverride;
            }

            if (SubTitle == null)
            {
                if (RealEncounters.Length == 1)
                {
                    SubTitle = (!GlobalControls.crate) ? Encounters[0] : Temmify.Convert(Encounters[0], true);
                }
                else
                {
                    if (Encounters.Length <= 0)
                    {
                        SubTitle = (!GlobalControls.crate) ? "No  encounters" : "NO ENCUOTNERS";
                    }
                    else
                    {
                        SubTitle = "Has " + Encounters.Length + " encounter" + ((Encounters.Length > 1) ? "s" : "");
                        if (GlobalControls.crate)
                        {
                            SubTitle = "HSA " + Encounters.Length + " ENCUOTNER" + (Encounters.Length > 1 ? "S" : "");
                        }
                    }
                }
            }

            SupportedLanguages = new bool[2];
            if (_modInfo.SupportedLanguagesOverride == null)
            {
                Lang.Exist(RealName, out SupportedLanguages[0], out SupportedLanguages[1]);
            }
            else
            {
                SupportedLanguages = _modInfo.SupportedLanguagesOverride.Copy();
            }

            if (!GlobalControls.crate) return;

            Title = Temmify.Convert(Title, true);
            Description = Temmify.Convert(Description);
        }

        public string RealName { get; private set; }
        public string[] RealEncounters { get; private set; }
        public ModInfo RawInfoData { get { return _modInfo; } }

        public Font Font { get; private set; }
        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public string Description { get; private set; }
        public string[] Encounters { get; private set; }
        public int[] EncounterIndexes { get; private set; }
        public bool[] SupportedLanguages { get; private set; }

        public void Reload()
        {
            _modInfo = ModInfo.GetFromFile(_dir.Name, false);
            _encounterDir = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + _dir.Name + "/Lua/Encounters"));
            if (!_encounterDir.Exists || _encounterDir.GetFiles().Length <= 0) _encounters = new FileInfo[0];
            else                                                               _encounters = _encounterDir.GetFiles("*.lua");

            RealName = _dir.Name;
            RealEncounters = new string[_encounters.Length];
            for (var i = 0; i < _encounters.Length; i++)
            {
                RealEncounters[i] = Path.GetFileNameWithoutExtension(_encounters[i].Name);
            }

            Font = _modInfo.ScreenFont;
            Title = _dir.Name;
            SubTitle = null;
            Description = _modInfo.Description ?? "";

            if (RealEncounters.Length == 1)
            {
                Encounters = new string[1] { RealEncounters[0] };
                EncounterIndexes = new int[1] { 0 };
            }
            else
            {
                List<string> encountersTemp = new List<string>(RealEncounters.Length);
                List<int> encounterIndexesTemp = new List<int>(RealEncounters.Length);
                for (var i = 0; i < RealEncounters.Length; i++)
                {
                    string fileName = RealEncounters[i];
                    string showName = fileName;
                    if (_modInfo.EncounterNames != null && i < _modInfo.EncounterNames.Length && !string.IsNullOrEmpty(_modInfo.EncounterNames[i]))
                    {
                        showName = _modInfo.EncounterNames[i];
                    }
                    if (_modInfo.ShowEncounters != null)
                    {
                        if (!_modInfo.ShowEncounters.Contains(fileName)) continue;
                    }
                    else if (_modInfo.HideEncounters != null)
                    {
                        if (_modInfo.HideEncounters.Contains(fileName)) continue;
                    }
                    encountersTemp.Add(showName);
                    encounterIndexesTemp.Add(i);
                }
                Encounters = encountersTemp.ToArray();
                EncounterIndexes = encounterIndexesTemp.ToArray();
            }

            if (Asterisk.displayModInfo)
            {
                if (!string.IsNullOrEmpty(_modInfo.TitleOverride))    Title = _modInfo.TitleOverride;
                if (!string.IsNullOrEmpty(_modInfo.SubtitleOverride)) SubTitle = _modInfo.SubtitleOverride;
            }

            if (SubTitle == null)
            {
                if (RealEncounters.Length == 1)
                {
                    SubTitle = (!GlobalControls.crate) ? Encounters[0] : Temmify.Convert(Encounters[0], true);
                }
                else
                {
                    if (Encounters.Length <= 0)
                    {
                        SubTitle = (!GlobalControls.crate) ? "No  encounters" : "NO ENCUOTNERS";
                    }
                    else
                    {
                        SubTitle = "Has " + Encounters.Length + " encounter" + ((Encounters.Length > 1) ? "s" : "");
                        if (GlobalControls.crate)
                        {
                            SubTitle = "HSA " + Encounters.Length + " ENCUOTNER" + (Encounters.Length > 1 ? "S" : "");
                        }
                    }
                }
            }

            if (!GlobalControls.crate) return;

            Title = Temmify.Convert(Title, true);
            Description = Temmify.Convert(Description);
        }

        public bool ReloadEncounters(bool updateEncountersList)
        {
            ReloadModInfo();

            DirectoryInfo encounterDir = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + RealName + "/Lua/Encounters"));
            if (!encounterDir.Exists || encounterDir.GetFiles("*.lua").Length <= 0) return false;

            if (!updateEncountersList) return true;

            _encounterDir = encounterDir;
            _encounters = _encounterDir.GetFiles("*.lua");

            RealEncounters = new string[_encounters.Length];
            for (var i = 0; i < _encounters.Length; i++)
            {
                RealEncounters[i] = Path.GetFileNameWithoutExtension(_encounters[i].Name);
            }

            SubTitle = null;

            if (RealEncounters.Length == 1)
            {
                Encounters = new string[1] { RealEncounters[0] };
                EncounterIndexes = new int[1] { 0 };
            }
            else
            {
                List<string> encountersTemp = new List<string>(RealEncounters.Length);
                List<int> encounterIndexesTemp = new List<int>(RealEncounters.Length);
                for (var i = 0; i < RealEncounters.Length; i++)
                {
                    string fileName = RealEncounters[i];
                    string showName = fileName;
                    if (_modInfo.EncounterNames != null && i < _modInfo.EncounterNames.Length && !string.IsNullOrEmpty(_modInfo.EncounterNames[i]))
                    {
                        showName = _modInfo.EncounterNames[i];
                    }
                    if (_modInfo.ShowEncounters != null)
                    {
                        if (!_modInfo.ShowEncounters.Contains(fileName)) continue;
                    }
                    else if (_modInfo.HideEncounters != null)
                    {
                        if (_modInfo.HideEncounters.Contains(fileName)) continue;
                    }
                    encountersTemp.Add(showName);
                    encounterIndexesTemp.Add(i);
                }
                Encounters = encountersTemp.ToArray();
                EncounterIndexes = encounterIndexesTemp.ToArray();
            }

            if (Asterisk.displayModInfo)
            {
                if (!string.IsNullOrEmpty(_modInfo.SubtitleOverride)) SubTitle = _modInfo.SubtitleOverride;
            }

            if (SubTitle == null)
            {
                if (RealEncounters.Length == 1)
                {
                    SubTitle = Encounters[0];
                }
                else
                {
                    if (Encounters.Length <= 0)
                    {
                        SubTitle = (!GlobalControls.crate) ? "No  encounters" : "NO ENCUOTNERS";
                    }
                    else
                    {
                        SubTitle = "Has " + Encounters.Length + " encounter" + ((Encounters.Length > 1) ? "s" : "");
                        if (GlobalControls.crate)
                        {
                            SubTitle = "HSA " + Encounters.Length + " ENCUOTNER" + (Encounters.Length > 1 ? "S" : "");
                        }
                    }
                }
            }

            return true;
        }

        public void ReloadModInfo()
        {
            _modInfo = ModInfo.GetFromFile(RealName, false);
        }
    }
}
