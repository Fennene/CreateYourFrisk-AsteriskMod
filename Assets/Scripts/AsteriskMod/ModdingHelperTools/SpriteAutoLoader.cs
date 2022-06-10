using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    public class SpriteAutoLoader : MonoBehaviour
    {
        public Image img;
        public bool SetNativeSize;
        public string SpritePath;

        private void OnEnable()
        {
            if (SpritePath.IsNullOrWhiteSpace()) return;
            
            //Image                  img  = GetComponent<Image>();
            //SpriteRenderer         img2 = GetComponent<SpriteRenderer>();
            //ParticleSystemRenderer img3 = GetComponent<ParticleSystemRenderer>();

            //if (img != null)
            img.sprite = FakeSpriteRegistry.Get(SpritePath);
            if (img.sprite == null/* && currHandleDictErrors*/)
            {
                UnitaleUtil.DisplayLuaError("AutoloadSpritesFromRegistry", "You tried to load the sprite \"" + SpritePath + "\", but it doesn't exist.");
                return;
            }
            //if (img.sprite != null)
            //{
                //img.sprite.name = SpritePath.ToLower(); TODO: Find a way to store the sprite's path
                if (SetNativeSize)
                {
                    img.SetNativeSize();
                    img.rectTransform.localScale = new Vector3(1, 1, 1);
                    img.rectTransform.sizeDelta = new Vector2(img.sprite.texture.width, img.sprite.texture.height);
                }
            //}
        }
    }
}
