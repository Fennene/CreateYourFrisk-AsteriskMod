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

        public bool done;
        private bool doneFromLoadedScene;
        private bool handleDictErrors;

        private void OnEnable()
        {
            SimInstance.FakeStaticInits.Loaded += LateStart;
            if (FindObjectsOfType<SpriteAutoLoader>().Any(a => a.done || a.doneFromLoadedScene))
                LateStart();
        }

        private void OnDisable()
        {
            SimInstance.FakeStaticInits.Loaded -= LateStart;
        }

        private void LateStart()
        {
            if (this == null) return;
            if ((done || !handleDictErrors) && (doneFromLoadedScene || handleDictErrors)) return;
            if (!done && handleDictErrors)
                done = true;
            else
                doneFromLoadedScene = true;
            bool currHandleDictErrors = handleDictErrors;

            if (!SpritePath.IsNullOrWhiteSpace())
            {
                if (img != null)
                {
                    img.sprite = SimInstance.FakeSpriteRegistry.Get(SpritePath);
                    if (img.sprite == null && currHandleDictErrors)
                    {
                        UnitaleUtil.DisplayLuaError("AutoloadSpritesFromRegistry", "You tried to load the sprite \"" + SpritePath + "\", but it doesn't exist.");
                        return;
                    }
                    if (img.sprite != null)
                    {
                        if (SetNativeSize)
                        {
                            img.SetNativeSize();
                            img.rectTransform.localScale = new Vector3(1, 1, 1);
                            img.rectTransform.sizeDelta = new Vector2(img.sprite.texture.width, img.sprite.texture.height);
                        }
                    }
                }
                else
                {
                    throw new CYFException("The GameObject " + gameObject.name + " doesn't have an Image, Sprite Renderer or Particle System component.");
                }
            }
            handleDictErrors = true;
        }
    }
}
