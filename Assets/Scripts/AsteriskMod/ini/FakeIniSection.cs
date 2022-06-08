using System.Collections.Generic;

namespace AsteriskMod
{
    internal class FakeIniSection
    {
        private Dictionary<string, FakeIniParameter> _parameters;

        public FakeIniSection()
        {
            _parameters = new Dictionary<string, FakeIniParameter>();
        }

        public bool ParameterExists(string parameterName)
        {
            if (!FakeIniUtil.IsValidParameterName(parameterName)) return false;
            return _parameters.ContainsKey(parameterName);
        }

        //public void AddParameter(string parameterName, string parameter) { }

        public FakeIniParameter GetParameter(string parameterName)
        {
            if (!FakeIniUtil.IsValidParameterName(parameterName)) return new FakeIniParameter();
            if (!_parameters.ContainsKey(parameterName)) return new FakeIniParameter();
            return _parameters[parameterName];
        }

        public void SetParameter(string parameterName, FakeIniParameter parameter)
        {
            if (!FakeIniUtil.IsValidParameterName(parameterName)) return;
            _parameters[parameterName] = parameter;
        }

        public void RemoveParameter(string parameterName)
        {
            if (!FakeIniUtil.IsValidParameterName(parameterName)) return;
            if (!_parameters.ContainsKey(parameterName)) return;
            _parameters.Remove(parameterName);
        }

        public FakeIniParameter this[string parameterName]
        {
            get { return GetParameter(parameterName); }
            set { SetParameter(parameterName, value); }
        }
    }
}
