using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod
{
    public class PlayerUtil
    {
        public static LimitedLuaStaticTextManager Name { get { return PlayerNameText.instance.NameTextMan; } }

        public static LimitedLuaStaticTextManager Love { get { return PlayerLoveText.instance.LoveTextMan; } }
    }
}
