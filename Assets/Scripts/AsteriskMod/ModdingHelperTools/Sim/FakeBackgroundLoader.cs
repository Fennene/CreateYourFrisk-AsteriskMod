using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    public class FakeBackgroundLoader : MonoBehaviour
    {
        private Image bgImage;
        private void Start()
        {
            bgImage = GetComponent<Image>();
            try
            {
                Sprite bg = SimInstance.FakeSpriteRegistry.FromFile(SimInstance.FakeFileLoader.pathToModFile("Sprites/bg.png"));
                if (bg == null) return;
                bg.texture.filterMode = FilterMode.Point;
                bgImage.sprite = bg;
                bgImage.color = Color.white;
            }
            catch { /* ignore */ }
        }
    }
}
