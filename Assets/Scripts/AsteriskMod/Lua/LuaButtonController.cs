using MoonSharp.Interpreter;
using UnityEngine.UI;

namespace AsteriskMod.Lua
{
    public class LuaButtonController
    {
        public static LuaButton FIGHT
        {
            get;
            private set;
        }

        public static LuaButton ACT
        {
            get;
            private set;
        }

        public static LuaButton ITEM
        {
            get;
            private set;
        }

        public static LuaButton MERCY
        {
            get;
            private set;
        }

        [MoonSharpHidden]
        public static void Initialize(Image[] buttons)
        {
            FIGHT = new LuaButton(0, buttons[0].gameObject);
            ACT = new LuaButton(1, buttons[1].gameObject);
            ITEM = new LuaButton(2, buttons[2].gameObject);
            MERCY = new LuaButton(3, buttons[3].gameObject);
        }

        [MoonSharpHidden]
        public static bool CanInactiveButton(int buttonID)
        {
            bool[] actives = new bool[4] {
                FIGHT.GetActive(),
                ACT.GetActive(),
                ITEM.GetActive(),
                MERCY.GetActive()
            };
            actives[buttonID] = false;
            return actives[0] || actives[1] || actives[2] || actives[3];
        }

        public static void SetActives(bool fight = true, bool act = true, bool item = true, bool mercy = true)
        {
            if (!(fight || act || item || mercy))
            {
                throw new CYFException("ButtonUtil.SetActives(): attempted to inactivate all button.");
            }
            FIGHT.SetActive(fight);
            ACT.SetActive(act);
            ITEM.SetActive(item);
            MERCY.SetActive(mercy);
        }

        public static void SetSprites(string dirPath)
        {
            FIGHT.SetSprite("fightbt_0", "fightbt_1", dirPath);
            ACT.SetSprite("actbt_0", "actbt_1", dirPath);
            ITEM.SetSprite("itembt_0", "itembt_1", dirPath);
            MERCY.SetSprite("mercybt_0", "mercybt_1", dirPath);
        }
    }
}
