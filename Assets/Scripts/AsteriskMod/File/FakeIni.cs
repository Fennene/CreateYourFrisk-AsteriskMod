using System.Collections.Generic;

namespace AsteriskMod.FakeIniLoader
{
    internal class FakeIni
    {
        private FakeIniSection _main;
        private Dictionary<string, FakeIniSection> _sections;

        internal FakeIni()
        {
            _main = new FakeIniSection();
            _sections = new Dictionary<string, FakeIniSection>();
        }

        public bool SectionExists(string sectionName)
        {
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return false;
            return _sections.ContainsKey(sectionName);
        }

        public FakeIniSection GetSection(string sectionName)
        {
            if (sectionName.IsNullOrWhiteSpace()) return new FakeIniSection();
            if (FakeIniUtil.IsMainSection(sectionName)) return _main;
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return new FakeIniSection();
            if (!_sections.ContainsKey(sectionName)) return new FakeIniSection();
            return _sections[sectionName];
        }

        public void SetSection(string sectionName, FakeIniSection section)
        {
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return;
            _sections[sectionName] = section;
        }

        public void RemoveSection(string sectionName)
        {
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return;
            if (!_sections.ContainsKey(sectionName)) return;
            _sections.Remove(sectionName);
        }

        public bool ParameterExists(string sectionName, string parameterName)
        {
            if (sectionName.IsNullOrWhiteSpace()) return false;
            if (FakeIniUtil.IsMainSection(sectionName)) return _main.ParameterExists(parameterName);
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return false;
            return _sections[sectionName].ParameterExists(parameterName);
        }

        public FakeIniParameter GetParameter(string sectionName, string parameterName)
        {
            if (sectionName.IsNullOrWhiteSpace()) return new FakeIniParameter();
            if (FakeIniUtil.IsMainSection(sectionName)) return _main.GetParameter(parameterName);
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return new FakeIniParameter();
            if (!_sections.ContainsKey(sectionName)) return new FakeIniParameter();
            return _sections[sectionName].GetParameter(parameterName);
        }

        public void SetParameter(string sectionName, string parameterName, FakeIniParameter parameter)
        {
            if (sectionName.IsNullOrWhiteSpace()) return;
            if (FakeIniUtil.IsMainSection(sectionName)) _main.SetParameter(parameterName, parameter);
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return;
            if (!_sections.ContainsKey(sectionName)) _sections[sectionName] = new FakeIniSection();
            _sections[sectionName].SetParameter(parameterName, parameter);
        }

        public void RemoveParameter(string sectionName, string parameterName)
        {
            if (sectionName.IsNullOrWhiteSpace()) return;
            if (FakeIniUtil.IsMainSection(sectionName)) _main.RemoveParameter(parameterName);
            if (!FakeIniUtil.IsValidSectionName(sectionName)) return;
            if (!_sections.ContainsKey(sectionName)) return;
            _sections[sectionName].RemoveParameter(parameterName);
        }

        public FakeIniSection this[string sectionName]
        {
            get { return GetSection(sectionName); }
            internal set { SetSection(sectionName, value); }
        }

        public FakeIniSection Main { get { return _main; } }

        public IEnumerable<string> SectionNames { get { return _sections.Keys; } }
        public IEnumerable<FakeIniSection> Sections { get { return _sections.Values; } }

        public override string ToString()
        {
            string _ = _main.ToString();
            foreach (string sectionName in _sections.Keys)
            {
                if (!_.IsNullOrWhiteSpace()) _ += "\n";
                _ += "[" + sectionName + "]\n";
                _ += _sections[sectionName].ToString();
            }
            return _;
        }

        public FakeIni Clone()
        {
            FakeIni ini = new FakeIni();
            foreach (string parameterName in this.Main.ParameterNames)
            {
                ini.Main[parameterName] = this.Main[parameterName].Clone();
            }
            foreach (string sectionName in this.SectionNames)
            {
                ini[sectionName] = new FakeIniSection();
                foreach (string parameterName in this[sectionName].ParameterNames)
                {
                    ini[sectionName][parameterName] = this[sectionName][parameterName].Clone();
                }
            }
            return ini;
        }

        public void Join(FakeIni other, bool overriding = true)
        {
            foreach (string parameterName in other.Main.ParameterNames)
            {
                if (overriding || !this.Main.ParameterExists(parameterName))
                {
                    this.Main[parameterName] = other.Main[parameterName].Clone();
                }
            }
            foreach (string sectionName in other.SectionNames)
            {
                if (!this.SectionExists(sectionName))
                {
                    this[sectionName] = new FakeIniSection();
                }
                foreach (string parameterName in other[sectionName].ParameterNames)
                {
                    if (overriding || !this[sectionName].ParameterExists(parameterName))
                    {
                        this[sectionName][parameterName] = other[sectionName][parameterName].Clone();
                    }
                }
            }
        }
    }
}
