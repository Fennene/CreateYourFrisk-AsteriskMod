using System;

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

        internal bool _controlOverride;

        public void SetControlOverride(bool active)
        {
            _controlOverride = active;
        }
    }
}
