namespace AsteriskMod.ModdingHelperTools
{
    internal static class BattleSimulator
    {
        private static UIController.UIState _state;

        internal static bool autoLineBreak;

        private static string _playerName;
        private static int _playerLV;
        private static int _playerHP;
        private static int _playerMaxHP;

        internal static void Initialize()
        {
            _state = UIController.UIState.ACTIONSELECT;
            autoLineBreak = false;
            _playerName = (Asterisk.language == Languages.Japanese) ? "にるにころ" : "Nil256";
            _playerLV = 1;
            _playerHP = 20;
            _playerMaxHP = 20;
        }

        internal static string PlayerName
        {
            get { return _playerName; }
            set
            {
                string shortName = value;
                if (shortName.Length > 9)
                    shortName = value.Substring(0, 9);
                _playerName = shortName;
                if (FakePlayerName.instance) FakePlayerName.instance.SetName(_playerName);
            }
        }

        internal static int PlayerLV
        {
            get { return _playerLV; }
            set
            {
                int _ = value;
                if (_ < 1) _ = 1;
                if (_ > ControlPanel.instance.LevelLimit) _ = ControlPanel.instance.LevelLimit;
                _playerLV = _;
                if (FakePlayerLV.instance) FakePlayerLV.instance.SetLove(_playerLV);
                PlayerMaxHP = 16 + _playerLV * 4;
            }
        }

        internal static int PlayerHP
        {
            get { return _playerHP; }
            set
            {
                int _ = value;
                if (_ < 1) _ = 1;
                if (_ > _playerMaxHP) _ = _playerMaxHP;
                _playerHP = _;
                FakePlayerHPStat.instance.SetHP();
            }
        }

        internal static int PlayerMaxHP
        {
            get { return _playerMaxHP; }
            set
            {
                int _ = value;
                if (_ < 1) _ = 1;
                if (_ > ControlPanel.instance.HPLimit) _ = ControlPanel.instance.HPLimit;
                _playerMaxHP = _;
                if (_playerHP > _playerMaxHP) _playerHP = _playerMaxHP;
                FakePlayerHPStat.instance.SetHP();
            }
        }

        internal static UIController.UIState CurrentState
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
