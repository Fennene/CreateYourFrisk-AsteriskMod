namespace AsteriskMod.ExtendedUtil
{
    public class LuaCYFUtil
    {
        public const int WIDTH = 640;
        public const int HEIGHT = 480;
        public const int WIDTH_HALF = 320;
        public const int HEIGHT_HALF = 240;

        public const int PLAYER_ABSX_FIGHT = 48;
        public const int PLAYER_ABSX_ACT = 202;
        public const int PLAYER_ABSX_ITEM = 361;
        public const int PLAYER_ABSX_MERCY = 515;
        public const int PLAYER_ABSY_BUTTON = 25;

        public static int GetMaxHP(int lv) { return 16 + lv * 4; }
        public static int GetATK(int lv) { return 8 + lv * 2; }
        public static int GetDEF(int lv) { return 10 + (lv - 1) / 4; }

        private static readonly int[] _totalEXPs = new int[20]
        {
                0,    10,    30,    70,   120,
              200,   300,   500,   800,  1200,
             1700,  2500,  3500,  5000,  7000,
            10000, 15000, 25000, 50000, 99999
        };
        public static int GetTotalEXP(int lv)
        {
            if (lv < 1) return 0;
            if (lv > 20) return 99999;
            return _totalEXPs[lv + 1];
        }

        public static float CalculateKRLabelX(int playerMaxHP = -1)
        {
            if (playerMaxHP < 0) playerMaxHP = PlayerCharacter.instance.MaxHP;
            return 296.6f + (playerMaxHP * 1.2f);
        }
        public const float KR_LABEL_Y = 70;
    }
}
