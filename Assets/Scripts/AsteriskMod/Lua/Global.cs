using MoonSharp.Interpreter;

namespace AsteriskMod.Lua
{
    public class Global
    {
        public DynValue this[string key]
        {
            get { return LuaScriptBinder.GetBattle(null, key); }
            set { LuaScriptBinder.SetBattle(null, key, value); }
        }
    }
}
