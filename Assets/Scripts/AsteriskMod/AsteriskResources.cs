using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    internal static class AsteriskResources
    {
        internal static readonly Font PixelOperator;
        internal static readonly Font EightBitoperator;
        internal static readonly Font JFDotShinonome14;

        static AsteriskResources()
        {
            PixelOperator    = Resources.Load<Font>("Fonts/PixelOperator/PixelOperator-Bold");
            EightBitoperator = Resources.Load<Font>("Fonts/8bitoperator_JVE/8bitoperator_jve");
            JFDotShinonome14 = Resources.Load<Font>("Fonts/JF-Dot-Shinonome14/JF-Dot-Shinonome14");
        }
    }
}
