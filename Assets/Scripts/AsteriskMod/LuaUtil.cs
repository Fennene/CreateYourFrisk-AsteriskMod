namespace AsteriskMod
{
    public class LuaUtil
    {
        public static float CalculateKRLabelX(int playerMaxHP = -1)
        {
            if (playerMaxHP < 0) playerMaxHP = PlayerCharacter.instance.MaxHP;
            return 296.6f + (playerMaxHP * 1.2f);
        }
        public const float KR_LABEL_Y = 70;

        public static string[] StringSplit(string text, params char[] separator) { return text.Split(separator); }
        public static bool StringStartsWith(string text, string value) { return text.StartsWith(value); }
        public static bool StringEndsWith(string text, string value) { return text.EndsWith(value); }

        public const int PLAYER_ABSX_FIGHT = 48;
        public const int PLAYER_ABSX_ACT = 202;
        public const int PLAYER_ABSX_ITEM = 361;
        public const int PLAYER_ABSX_MERCY = 515;
        public const int PLAYER_ABSY_BUTTON = 25;
    }
}
