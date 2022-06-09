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

        public bool IsStringArray { get { return _isStringArray; } }
        public string String { get { return _stringArray[0]; } }
        public string[] StringArray { get { return _stringArray; } }
    }
}
