using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsteriskMod
{
    public class UTini
    {
        private string _path;
        private Dictionary<string, Dictionary<string, string>> _ini;

        public UTini(string fullPath)
        {
            _path = fullPath;
            Load();
        }

        private static readonly char[] ValidChars = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z'
        };
        private static bool StartsWithNumber(char value)
        {
            return value == '0' || value == '1' || value == '2' || value == '3' || value == '4' || value == '5' || value == '6' || value == '7' || value == '8' || value == '9';
        }

        private bool SectionParse(string value, out string sectionName)
        {
            sectionName = null;
            bool isInBrackets = false;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] == '[')
                {
                    if (isInBrackets) return false;
                    isInBrackets = true;
                }
                else if (value[i] == ']')
                {
                    if (!isInBrackets) return false;
                    isInBrackets = false;
                }
                else
                {
                    if (value[i] == ';') break;
                    if (isInBrackets)
                    {
                        if (sectionName == null)
                        {
                            if (StartsWithNumber(value[i])) return false;
                            sectionName = "";
                        }
                        if (!ValidChars.Contains(value[i])) return false;
                        sectionName += value[i];
                    }
                    else
                    {
                        if (value[i] != ' ' && value[i] != '\r' && value[i] != '\n' && value[i] != '\t') return false;
                    }
                }
            }
            if (isInBrackets) return false;
            if (sectionName.IsNullOrWhiteSpace()) return false;
            return true;
        }

        private bool ParameterParse(string value, out string parameterName, out string parameter)
        {
            parameterName = null;
            parameter = null;

            if (!value.Contains('=')) return false;
            string[] _ = value.Split(new char[1] { '=' }, 2);
            string parameterNameValue = _[0];
            string parameterValue = _[1];

            parameterNameValue = parameterNameValue.Trim();
            for (var i = 0; i < parameterNameValue.Length; i++)
            {
                if (!ValidChars.Contains(parameterNameValue[i])) return false;
                if (parameterName == null)
                {
                    if (StartsWithNumber(parameterNameValue[i])) return false;
                    parameterName = "";
                }
                parameterName += parameterNameValue[i];
            }

            bool isInStringLiteral = false;
            char literal = ' ';
            parameterValue = parameterValue.Trim();
            for (var i = 0; i < parameterValue.Length; i++)
            {
                if (!isInStringLiteral)
                {
                    if (parameterValue[i] == '\"' || parameterValue[i] == '\'')
                    {
                        if (parameter != null) break;
                        literal = parameterValue[i];
                        parameter = "";
                        isInStringLiteral = true;
                    }
                    else if (parameterValue[i] == ';')
                    {
                        break;
                    }
                    else if (parameterValue[i] != ' ')
                    {
                        return false;
                    }
                }
                else
                {
                    if (parameterValue[i] == literal)
                    {
                        isInStringLiteral = false;
                    }
                    parameter += parameterValue[i];
                }
            }
            if (isInStringLiteral) return false;
            if (parameterName.IsNullOrWhiteSpace()) return false;
            if (parameter == null) return false;
            return true;
        }

        public void Load()
        {
            _ini = new Dictionary<string, Dictionary<string, string>>();
            if (!File.Exists(_path)) return;
            string[] fileData = new string[0];
            try   { fileData = File.ReadAllLines(_path); }
            catch { return; }

            string currentSectionName = null;

            for (var i = 0; i < fileData.Length; i++)
            {
                string line = fileData[i];
                if (line.IsNullOrWhiteSpace()) continue;
                if (line.StartsWith(";")) continue;
                line = line.Trim();
                if (line.StartsWith("["))
                {
                    string tempSectionName = null;
                    if (SectionParse(line, out tempSectionName))
                    {
                        currentSectionName = tempSectionName;
                        if (!_ini.ContainsKey(currentSectionName))
                        {
                            _ini.Add(currentSectionName, new Dictionary<string, string>());
                        }
                        else
                        {
                            _ini[currentSectionName] = new Dictionary<string, string>();
                        }
                    }
                    else
                    {
                        currentSectionName = null;
                    }
                }
                else if (currentSectionName != null)
                {
                    string tempParameterName = null;
                    string tempParameter = null;
                    if (ParameterParse(line, out tempParameterName, out tempParameter))
                    {
                        if (!_ini[currentSectionName].ContainsKey(tempParameterName))
                        {
                            _ini[currentSectionName].Add(tempParameterName, tempParameter);
                        }
                        else
                        {
                            _ini[currentSectionName][tempParameterName] = tempParameter;
                        }
                    }
                }
            }
        }

        public void Save()
        {
            string text = "";
            foreach (string sectionName in _ini.Keys)
            {
                text += "[" + sectionName + "]\n";
                foreach (string parameterName in _ini[sectionName].Keys)
                {
                    text += parameterName + "=\"" + _ini[sectionName][parameterName] + "\"\n";
                }
            }
            try   { File.WriteAllText(_path, text); }
            catch { /* ignore */ }
        }

        public bool FileExists() { return File.Exists(_path); }
        public bool SectionExists(string sectionName) { return _ini.ContainsKey(sectionName); }
        public bool ParameterExists(string sectionName, string parameterName) { return SectionExists(sectionName) && _ini[sectionName].ContainsKey(parameterName); }

        private bool IsValidName(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            if (StartsWithNumber(value[0])) return false;
            for (var i = 0; i < value.Length; i++)
            {
                if (!ValidChars.Contains(value[i])) return false;
            }
            return true;
        }

        public string GetString(string sectionName, string parameterName)
        {
            if (!ParameterExists(sectionName, parameterName)) return "";
            return _ini[sectionName][parameterName];
        }
        public float GetNumber(string sectionName, string parameterName)
        {
            if (!ParameterExists(sectionName, parameterName)) return 0f;
            float _;
            if (!float.TryParse(_ini[sectionName][parameterName], out _)) return 0f;
            return _;
        }

        public void SetString(string sectionName, string parameterName, string parameter)
        {
            if (!IsValidName(sectionName) || !IsValidName(parameterName)) return;
            if (!SectionExists(sectionName)) _ini.Add(sectionName, new Dictionary<string, string>());
            if (_ini[sectionName].ContainsKey(parameterName))
            {
                _ini[sectionName][parameterName] = parameter;
            }
            else
            {
                _ini[sectionName].Add(parameterName, parameter);
            }
        }
        public void SetNumber(string sectionName, string parameterName, float parameter)
        {
            string _ = parameter.ToString();
            int decimalPoint = _.IndexOf('.');
            if (decimalPoint == -1)
            {
                _ += ".000000";
            }
            else
            {
                if (decimalPoint == 0) // maybe impossible
                {
                    _ = "0" + _;
                    decimalPoint = 1;
                }
                if (_.Length <= decimalPoint + 6)
                {
                    for (var i = 0; i < decimalPoint + 6 - _.Length; i++)
                    {
                        _ += "0";
                    }
                }
                _ = _.Substring(0, decimalPoint + 6);
            }
            SetString(sectionName, parameterName, _);
        }

        public void RemoveParameter(string sectionName, string parameterName)
        {
            if (!ParameterExists(sectionName, parameterName)) return;
            _ini[sectionName].Remove(parameterName);
        }
        public void RemoveSection(string sectionName)
        {
            if (!SectionExists(sectionName)) return;
            _ini.Remove(sectionName);
        }
    }
}
