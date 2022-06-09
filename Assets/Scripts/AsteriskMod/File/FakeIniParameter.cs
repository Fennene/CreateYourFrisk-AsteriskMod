namespace AsteriskMod
{
    internal class FakeIniParameter
    {
        private bool _isStringArray;
        private string[] _stringArray;

        public FakeIniParameter()
        {
            _isStringArray = false;
            _stringArray = new string[1] { string.Empty };
        }

        public FakeIniParameter(string parameter)
        {
            _isStringArray = false;
            _stringArray = new string[1] { parameter };
        }

        public FakeIniParameter(string[] parameter, bool copy = false)
        {
            _isStringArray = true;
            if (!copy)
            {
                _stringArray = parameter;
                return;
            }
            _stringArray = new string[parameter.Length];
            for (var i = 0; i < parameter.Length; i++) _stringArray[i] = parameter[i];
        }

        public bool IsArray { get { return _isStringArray; } }
        public string String { get { return _stringArray[0]; } }
        public string[] Array { get { return _stringArray; } }

        public override string ToString()
        {
            if (!_isStringArray) return "\"" + String + "\"";
            if (_stringArray.Length == 0) return "{ }";
            string _ = "{\"" + String + "\"";
            for (var i = 1; i < _stringArray.Length; i++)
            {
                _ += ", \"" + _stringArray[i] + "\"";
            }
            return _ + "}";
        }
    }
}
