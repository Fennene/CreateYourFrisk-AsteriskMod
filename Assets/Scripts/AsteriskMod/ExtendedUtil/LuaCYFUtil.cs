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

        public static float CalculateKRLabelX(int playerMaxHP = -1)
        {
            if (playerMaxHP < 0) playerMaxHP = PlayerCharacter.instance.MaxHP;
            return 296.6f + (playerMaxHP * 1.2f);
        }
        public const float KR_LABEL_Y = 70;
    }
}
