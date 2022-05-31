using MoonSharp.Interpreter;
//using UnityEngine.UI;

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
        public static void Initialize(/*Image[] buttons*/)
        {
            FIGHT = new LuaButton(0);
            ACT = new LuaButton(1);
            ITEM = new LuaButton(2);
            MERCY = new LuaButton(3);
        }

        public static void SetActives(bool fight = true, bool act = true, bool item = true, bool mercy = true)
        {
            UIController.ActionButtonManager.SetActives(fight, act, item, mercy);
            /*
            if (!(fight || act || item || mercy))
            {
                throw new CYFException("ButtonUtil.SetActives(): attempted to inactivate all button.");
            }
            FIGHT.SetActive(fight);
            ACT.SetActive(act);
            ITEM.SetActive(item);
            MERCY.SetActive(mercy);
            */
        }

        public static void SetSprites(string dirPath)
        {
            UIController.ActionButtonManager.SetSprites(dirPath, true);
            /*
            FIGHT.SetSprite("fightbt_0", "fightbt_1", dirPath);
            ACT.SetSprite("actbt_0", "actbt_1", dirPath);
            ITEM.SetSprite("itembt_0", "itembt_1", dirPath);
            MERCY.SetSprite("mercybt_0", "mercybt_1", dirPath);
            */
        }

        public static void RevertAll()
        {
            UIController.ActionButtonManager.Revert(true);
            /*
            FIGHT.Revert();
            ACT.Revert();
            ITEM.Revert();
            MERCY.Revert();
            */
        }
    }
}
