namespace AsteriskMod.ExtendedUtil
{
    public class LuaCYFUtil
    {
        public LuaCYFUtil()
        {
            UseAltCyan = false;
        }

        public int WIDTH { get { return 640; } }
        public int HEIGHT { get { return 480; } }
        public int WIDTH_HALF { get { return 320; } }
        public int HEIGHT_HALF { get { return 240; } }

        public int PLAYER_ABSX_FIGHT { get { return 48; } }
        public int PLAYER_ABSX_ACT { get { return 202; } }
        public int PLAYER_ABSX_ITEM { get { return 361; } }
        public int PLAYER_ABSX_MERCY { get { return 515; } }
        public int PLAYER_ABSY_BUTTON { get { return 25; } }

        public int GetMaxHP(int lv) { return 16 + lv * 4; }
        public int GetATK(int lv) { return 8 + lv * 2; }
        public int GetDEF(int lv) { return 10 + (lv - 1) / 4; }

        private readonly int[] _totalEXPs = new int[20]
        {
                0,    10,    30,    70,   120,
              200,   300,   500,   800,  1200,
             1700,  2500,  3500,  5000,  7000,
            10000, 15000, 25000, 50000, 99999
        };
        public int GetTotalEXP(int lv)
        {
            if (lv < 1) return 0;
            if (lv > 20) return 99999;
            return _totalEXPs[lv + 1];
        }

        public float CalculateKRLabelX(int playerMaxHP = -1)
        {
            if (playerMaxHP < 0) playerMaxHP = PlayerCharacter.instance.MaxHP;
            return 296.6f + (playerMaxHP * 1.2f);
        }
        public float KR_LABEL_Y { get { return 70; } }

        public readonly byte[] RED      = new byte[3] { 255,   0,   0 };
        public readonly byte[] ORANGE   = new byte[3] { 255, 166,   0 };
        public readonly byte[] YELLOW   = new byte[3] { 255, 255,   0 };
        public readonly byte[] GREEN    = new byte[3] {   0, 192,   0 };
        public readonly byte[] CYAN     = new byte[3] {  66, 226, 255 };
        public readonly byte[] CYAN_ALT = new byte[3] {   0, 162, 232 };
        public readonly byte[] BLUE     = new byte[3] {   0,  60, 255 };
        public readonly byte[] PURPLE   = new byte[3] { 213,  53, 217 };

        public bool UseAltCyan;

        public byte[] GetColor32(string colorName)
        {
            colorName = colorName.ToLower();
            if (colorName == "red") return RED;
            if (colorName == "orange") return ORANGE;
            if (colorName == "yellow") return YELLOW;
            if (colorName == "green") return GREEN;
            if (colorName == "cyan") return UseAltCyan ? CYAN_ALT : CYAN;
            if (colorName == "blue") return BLUE;
            if (colorName == "purple") return PURPLE;
            if (colorName == "white") return new byte[3] { 255, 255, 255 };
            if (colorName == "black") return new byte[3] { 0, 0, 0 };
            if (colorName == "gray") return new byte[3] { 127, 127, 127 };
            return new byte[3] { 0, 0, 0 };
        }
    }
}
