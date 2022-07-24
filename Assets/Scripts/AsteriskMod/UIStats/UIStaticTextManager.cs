using System;
using UnityEngine;

namespace AsteriskMod
{
    public class UIStaticTextManager : LuaStaticTextManager
    {
        public override void DestroyText()
        {
            throw new CYFException("Attempt to remove UI.");
        }

        internal Action<string> _SetText = (_ => { });

        public override void SetText(string text)
        {
            base.SetText(text);
            _SetText(text);
        }

        internal bool _positionOverride;
        internal bool _textOverride;

        public void SetPositionOverride(bool active)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion <= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("UIStaticTextManager", "SetPositionOverride");
            _positionOverride = active;
        }
        public void SetTextOverride(bool active)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion <= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("UIStaticTextManager", "SetTextOverride");
            _textOverride = active;
        }
        public void SetControlOverride(bool active)
        {
            _positionOverride = active;
            _textOverride = active;
        }

        internal Vector2 _relativePosition;
        private void Start() { _relativePosition = Vector2.zero; }
        public int offsetx
        {
            get
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion <= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("UIStaticTextManager", "relativex");
                CheckExists();
                return Mathf.RoundToInt(_relativePosition.x);
            }
            set
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion <= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("UIStaticTextManager", "relativex");
                SetOffset(value, offsety);
            }
        }
        public int offsety
        {
            get
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion <= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("UIStaticTextManager", "relativey");
                CheckExists();
                return Mathf.RoundToInt(_relativePosition.y);
            }
            set
            {
                if (AsteriskEngine.ModTarget_AsteriskVersion <= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("UIStaticTextManager", "relativey");
                SetOffset(offsetx, value);
            }
        }
        public void SetOffset(int newX, int newY)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion <= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("UIStaticTextManager", "SetRelativePosition");
            CheckExists();
            _relativePosition = new Vector2(newX, newY);
            _UpdatePosition(false);
        }

        internal Action<bool> _UpdatePosition = (_ => { });

        public void UpdatePosition()
        {
            _UpdatePosition(true);
        }
    }
}
