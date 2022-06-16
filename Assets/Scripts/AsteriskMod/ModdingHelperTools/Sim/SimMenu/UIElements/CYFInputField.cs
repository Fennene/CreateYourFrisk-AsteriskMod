using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools.UI
{
    public class CYFInputField : MonoBehaviour
    {
        public InputField InputField;
        public Image OuterImage;
        public Image InnerImage;

        public void ResetOuterColor()
        {
            OuterImage.color = new Color32(64, 64, 64, 255);
        }
    }
}
