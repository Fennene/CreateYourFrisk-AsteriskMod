using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.CYFExtender
{
    public class ImageObject : CommonObject
    {
        public Color GetColor()
        {
            return GetComponent<Image>().color;
        }

        public void SetColor(Color color)
        {
            GetComponent<Image>().color = color;
        }

        public void SetSprite(string fileNamePathRelativeModSprites)
        {
            SpriteUtil.SwapSpriteFromFile(GetComponent<Image>(), fileNamePathRelativeModSprites);
        }
    }
}
