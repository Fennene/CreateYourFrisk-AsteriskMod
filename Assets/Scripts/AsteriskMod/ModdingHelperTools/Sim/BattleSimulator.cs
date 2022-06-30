using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class BattleSimulator
    {
        private UIController.UIState _state;

        internal bool autoLineBreak;
        internal Vector2 arenaSize;

        private string _playerName;
        private int _playerLV;
        private int _playerHP;
        private int _playerMaxHP;

        internal void Initialize()
        {
            _state = UIController.UIState.ACTIONSELECT;
            autoLineBreak = false;
            arenaSize = new Vector2(155, 130);
            _playerName = (Asterisk.language == Languages.Japanese && Asterisk.changeUIwithLanguage) ? "にるにころ" : "Nil256";
            _playerLV = 1;
            _playerHP = 20;
            _playerMaxHP = 20;

            MenuOpened = false;
            LeftMenu = true;
        }

        internal string PlayerName
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

        internal int PlayerLV
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

        internal int PlayerHP
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

        internal int PlayerMaxHP
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

        internal UIController.UIState CurrentState
        {
            get { return _state; }
            set { _state = value; }
        }

        internal bool MenuOpened = false;
        internal bool LeftMenu = true;
    }
}
