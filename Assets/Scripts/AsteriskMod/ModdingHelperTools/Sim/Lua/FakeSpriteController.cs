using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeSpriteController
    {
        internal GameObject gameObject;
        internal Image img { get { return gameObject.GetComponent<Image>(); } }
        //* public LuaSpriteShader shader;
        private bool firstFrame = true;
        public Vector2 nativeSizeDelta;                   // The native size of the image
        private Vector3 internalRotation = Vector3.zero;  // The rotation of the sprite
        private float xScale = 1;                         // The X scale of the sprite
        private float yScale = 1;                         // The Y scale of the sprite
        private Sprite originalSprite;                    // The original sprite
        //*public KeyframeCollection keyframes;              // This variable is used to store an animation
        public string tag;                                // The tag of the sprite : "projectile", "enemy", "bubble", "letter", "ui"(Asterisk Mod Modification) or "other"
        //*private KeyframeCollection.LoopMode loop = KeyframeCollection.LoopMode.LOOP;
        //public static MoonSharp.Interpreter.Interop.IUserDataDescriptor data = UserData.GetDescriptorForType<LuaSpriteController>(true);

        private const string TAG_PROJECTILE = "projectile";
        private const string TAG_ENEMY = "enemy";
        private const string TAG_BUBBLE = "bubble";
        private const string TAG_LETTER = "letter";
        private const string TAG_OTHER = "other";
        private const string TAG_UI = "ui";

        private const string TAG_EVENT = "event";

        internal bool ignoreSet = false;

        //The name of the sprite
        public string spritename
        {
            get { return img ? img.sprite.name : gameObject.GetComponent<SpriteRenderer>().sprite.name; }
        }

        // The x position of the sprite, relative to the arena position and its anchor.
        public float x
        {
            get
            {
                float val = gameObject.GetComponent<RectTransform>().anchoredPosition.x;
                if (gameObject.transform.parent == null) return val;
                if (gameObject.transform.parent.name == "SpritePivot")
                    val += gameObject.transform.parent.localPosition.x;
                return val;
            }
            set
            {
                if (gameObject.transform.parent.name == "SpritePivot")
                    gameObject.transform.parent.localPosition = new Vector3(value, gameObject.transform.parent.localPosition.y, gameObject.transform.parent.localPosition.z) - (Vector3)gameObject.GetComponent<RectTransform>().anchoredPosition;
                else
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(value, gameObject.GetComponent<RectTransform>().anchoredPosition.y);
            }
        }

        // The y position of the sprite, relative to the arena position and its anchor.
        public float y
        {
            get
            {
                float val = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
                if (gameObject.transform.parent == null) return val;
                if (gameObject.transform.parent.name == "SpritePivot")
                    val += gameObject.transform.parent.localPosition.y;
                return val;
            }
            set
            {
                if (gameObject.transform.parent.name == "SpritePivot")
                    gameObject.transform.parent.localPosition = new Vector3(gameObject.transform.parent.localPosition.x, value, gameObject.transform.parent.localPosition.z) - (Vector3)gameObject.GetComponent<RectTransform>().anchoredPosition;
                else
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x, value);
            }
        }

        // The z position of the sprite, relative to the arena position and its anchor. (Only useful in the overworld)
        public float z
        {
            get { return GetTarget().localPosition.z; }
            set
            {
                Transform target = GetTarget();
                target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, value);
            }
        }

        // The x position of the sprite, relative to the bottom left corner of the screen.
        public float absx
        {
            get { return GetTarget().position.x; }
            set
            {
                Transform target = GetTarget();
                target.position = new Vector3(value, target.position.y, target.position.z);
            }
        }

        // The y position of the sprite, relative to the bottom left corner of the screen.
        public float absy
        {
            get { return GetTarget().position.y; }
            set
            {
                Transform target = GetTarget();
                target.position = new Vector3(target.position.x, value, target.position.z);
            }
        }

        // The z position of the sprite, relative to the bottom left corner of the screen. (Only useful in the overworld)
        public float absz
        {
            get { return GetTarget().position.z; }
            set
            {
                Transform target = GetTarget();
                target.position = new Vector3(target.position.x, target.position.y, value);
            }
        }

        // The x scale of the sprite. This variable is used for the same purpose as img, to be able to do other things when setting the variable
        public float xscale
        {
            get { return xScale; }
            set
            {
                xScale = value;
                Scale(xScale, yScale);
            }
        }

        // The y scale of the sprite.
        public float yscale
        {
            get { return yScale; }
            set
            {
                yScale = value;
                Scale(xScale, yScale);
            }
        }

        // Is the sprite active? True if the image of the sprite isn't null, false otherwise
        public bool isactive
        {
            get { return GlobalControls.retroMode ? gameObject == null : gameObject != null; }
        }

        // The original width of the sprite
        public float width
        {
            get
            {
                if (tag == TAG_LETTER) return img.sprite.rect.width;
                if (img) return img.mainTexture.width;
                return gameObject.GetComponent<SpriteRenderer>().sprite.texture.width;
            }
        }

        // The original height of the sprite
        public float height
        {
            get
            {
                if (tag == TAG_LETTER) return img.sprite.rect.height;
                if (img) return img.mainTexture.height;
                return gameObject.GetComponent<SpriteRenderer>().sprite.texture.height;
            }
        }

        // The x pivot of the sprite.
        public float xpivot
        {
            get { return gameObject.GetComponent<RectTransform>().pivot.x; }
            set { SetPivot(value, gameObject.GetComponent<RectTransform>().pivot.y); }
        }

        // The y pivot of the sprite.
        public float ypivot
        {
            get { return gameObject.GetComponent<RectTransform>().pivot.y; }
            set { SetPivot(gameObject.GetComponent<RectTransform>().pivot.x, value); }
        }

        /** animcomplete
        // Is the current animation finished? True if there is a finished animation, false otherwise
        public bool animcomplete
        {
            get
            {
                if (keyframes == null)
                    if (gameObject.GetComponent<KeyframeCollection>())
                        keyframes = gameObject.GetComponent<KeyframeCollection>();
                if (keyframes == null) return false;
                if (keyframes.enabled == false) return true;
                if (loop == KeyframeCollection.LoopMode.LOOP) return false;
                return keyframes.enabled && keyframes.animationComplete();
            }
        }
        */

        /** loopmode
        // The loop mode of the animation
        public string loopmode
        {
            get { return loop.ToString(); }
            set
            {
                try
                {
                    loop = (KeyframeCollection.LoopMode)Enum.Parse(typeof(KeyframeCollection.LoopMode), value.ToUpper(), true);
                    if (keyframes != null)
                        keyframes.SetLoop((KeyframeCollection.LoopMode)Enum.Parse(typeof(KeyframeCollection.LoopMode), value.ToUpper(), true));
                }
                catch { throw new CYFException("sprite.loopmode can only have either \"ONESHOT\", \"ONESHOTEMPTY\" or \"LOOP\", but you entered \"" + value.ToUpper() + "\"."); }
            }
        }
        */

        // The color of the sprite. It uses an array of three floats between 0 and 1
        public float[] color
        {
            get
            {
                if (img)
                {
                    Image imgtemp = img.GetComponent<Image>();
                    return new[] { img.color.r, img.color.g, img.color.b };
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    return new[] { imgtemp.color.r, imgtemp.color.g, imgtemp.color.b };
                }
            }
            set
            {
                if (value == null)
                    throw new CYFException("sprite.color can't be nil.");
                if (img)
                {
                    switch (value.Length)
                    {
                        // If we don't have three floats, we throw an error
                        case 3: img.color = new Color(value[0], value[1], value[2], alpha); break;
                        case 4: img.color = new Color(value[0], value[1], value[2], value[3]); break;
                        default: throw new CYFException("You need 3 or 4 numeric values when setting a sprite's color.");
                    }
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    switch (value.Length)
                    {
                        // If we don't have three floats, we throw an error
                        case 3: imgtemp.color = new Color(value[0], value[1], value[2], alpha); break;
                        case 4: imgtemp.color = new Color(value[0], value[1], value[2], value[3]); break;
                        default: throw new CYFException("You need 3 or 4 numeric values when setting a sprite's color.");
                    }
                }
            }
        }

        // The color of the sprite on a 32 bits format. It uses an array of three floats between 0 and 255
        public float[] color32
        {
            // We need first to convert the Color into a Color32, and then get the values.
            get
            {
                if (img)
                {
                    return new float[] { ((Color32)img.color).r, ((Color32)img.color).g, ((Color32)img.color).b };
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    return new float[] { ((Color32)imgtemp.color).r, ((Color32)imgtemp.color).g, ((Color32)imgtemp.color).b };
                }
            }
            set
            {
                if (value == null)
                    throw new CYFException("sprite.color can't be nil.");
                for (int i = 0; i < value.Length; i++)
                    if (value[i] < 0) value[i] = 0;
                    else if (value[i] > 255) value[i] = 255;
                if (img)
                {
                    // If we don't have three/four floats, we throw an error
                    if (value.Length == 3) img.color = new Color32((byte)value[0], (byte)value[1], (byte)value[2], (byte)alpha32);
                    else if (value.Length == 4) img.color = new Color32((byte)value[0], (byte)value[1], (byte)value[2], (byte)value[3]);
                    else throw new CYFException("You need 3 or 4 numeric values when setting a sprite's color.");
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    // If we don't have three/four floats, we throw an error
                    if (value.Length == 3) imgtemp.color = new Color32((byte)value[0], (byte)value[1], (byte)value[2], (byte)alpha32);
                    else if (value.Length == 4) imgtemp.color = new Color32((byte)value[0], (byte)value[1], (byte)value[2], (byte)value[3]);
                    else throw new CYFException("You need 3 or 4 numeric values when setting a sprite's color.");
                }
            }
        }

        // The alpha of the sprite. It is clamped between 0 and 1
        public float alpha
        {
            get
            {
                if (img)
                {
                    return img.color.a;
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    return imgtemp.color.a;
                }
            }
            set
            {
                if (img)
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Clamp01(value));
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    imgtemp.color = new Color(imgtemp.color.r, imgtemp.color.g, imgtemp.color.b, Mathf.Clamp01(value));
                }
            }
        }

        // The alpha of the sprite in a 32 bits format. It is clamped between 0 and 255
        public float alpha32
        {
            get
            {
                if (img)
                {
                    return ((Color32)img.color).a;
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    return ((Color32)imgtemp.color).a;
                }
            }
            // We need first to convert the Color into a Color32, and then get the values.
            // Color32s needs bytes, not ints, so we cast them
            set
            {
                if (img)
                {
                    img.color = new Color32(((Color32)img.color).r, ((Color32)img.color).g, ((Color32)img.color).b, (byte)value);
                }
                else
                {
                    SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                    imgtemp.color = new Color32(((Color32)imgtemp.color).r, ((Color32)imgtemp.color).g, ((Color32)imgtemp.color).b, (byte)value);
                }
            }
        }

        // The rotation of the sprite
        public float rotation
        {
            get { return internalRotation.z; }
            set
            {
                // We mod the value from 0 to 360 because angles are between 0 and 360 normally
                internalRotation.z = Math.Mod(value, 360);
                gameObject.GetComponent<RectTransform>().eulerAngles = internalRotation;
                if (gameObject.GetComponent<FakeProjectile>()/**&& gameObject.GetComponent<FakeProjectile>().isPP()*/)
                    gameObject.GetComponent<FakeProjectile>().needSizeRefresh = true;
            }
        }

        // The layer of the sprite
        public string layer
        {
            // You can't get or set the layer on an enemy sprite
            get
            {
                Transform target = GetTarget();
                if (tag == TAG_BUBBLE || tag == TAG_EVENT || tag == TAG_LETTER) return "none";
                if (tag == TAG_PROJECTILE && !target.parent.name.Contains("Layer")) return "BulletPool";
                if (tag == TAG_ENEMY && !target.parent.name.Contains("Layer")) return "specialEnemyLayer";
                if (tag == TAG_UI && !target.parent.name.Contains("Layer")) return "specialStatsLayer";
                return target.parent.name.Substring(0, target.parent.name.Length - 5);
            }
            set
            {
                switch (tag)
                {
                    case TAG_EVENT: throw new CYFException("sprite.layer: Overworld events' layers can't be changed.");
                    case TAG_BUBBLE: throw new CYFException("sprite.layer: Bubbles' layers can't be changed.");
                    case TAG_LETTER: throw new CYFException("sprite.layer: Letters' layers can't be changed.");
                    case TAG_UI: throw new CYFException("sprite.layer: UI' layers can't be changed.");
                }

                Transform target = GetTarget();
                Transform parent = target.parent;
                try
                {
                    target.SetParent(GameObject.Find(value + "Layer").transform);
                    gameObject.GetComponent<MaskImage>().inverted = false;
                }
                catch { target.SetParent(parent); }
            }
        }

        /*
        public bool filter {
            get { return img.sprite.texture.filterMode != FilterMode.Point; }
            set {
                if (value)  img.sprite.texture.filterMode = FilterMode.Trilinear;
                else        img.sprite.texture.filterMode = FilterMode.Point;
            }
        }
        */

        // The function that creates a sprite.
        public FakeSpriteController(Image i)
        {
            gameObject = i.gameObject;
            originalSprite = i.sprite;
            nativeSizeDelta = new Vector2(100, 100);
            if (gameObject.GetComponent<Projectile>()) tag = TAG_PROJECTILE;
            else if (gameObject.GetComponent<EnemyController>()) tag = TAG_ENEMY;
            else if (i.transform.parent != null)
                if (i.transform.parent.GetComponent<EnemyController>()) tag = TAG_BUBBLE;
                else tag = TAG_OTHER;
            if (gameObject.name == "*HPLabel" || gameObject.name == "*HPLabelCrate") tag = TAG_UI;
            //*shader = new LuaSpriteShader("sprite", gameObject);
        }

        public FakeSpriteController(SpriteRenderer i)
        {
            gameObject = i.gameObject;
            originalSprite = i.sprite;
            nativeSizeDelta = new Vector2(100, 100);
            tag = TAG_EVENT;
            //*shader = new LuaSpriteShader("event", gameObject);
        }

        // Changes the sprite of this instance
        public void Set(string name)
        {
            if (tag == TAG_UI && ignoreSet) return;
            // Change the sprite
            if (name == null)
                throw new CYFException("You can't set a sprite as nil!");
            if (img)
            {
                SimInstance.FakeSpriteRegistry.SwapSpriteFromFile(img, name);
                originalSprite = img.sprite;
                nativeSizeDelta = new Vector2(img.sprite.texture.width, img.sprite.texture.height);
            }
            else
            {
                SpriteRenderer imgtemp = img.GetComponent<SpriteRenderer>();
                SimInstance.FakeSpriteRegistry.SwapSpriteFromFile(imgtemp, name);
                originalSprite = imgtemp.sprite;
                nativeSizeDelta = new Vector2(imgtemp.sprite.texture.width, imgtemp.sprite.texture.height);
            }
            Scale(xScale, yScale);
            if (tag == TAG_PROJECTILE)
                gameObject.GetComponent<Projectile>().needUpdateTex = true;
        }

        // Sets the parent of a sprite.
        public void SetParent(FakeSpriteController parent)
        {
            if (parent == null) throw new CYFException("sprite.SetParent() can't set null as the sprite's parent.");
            if (tag == TAG_BUBBLE) throw new CYFException("sprite.SetParent() can not be used with bubbles.");
            if (tag == TAG_EVENT || parent != null && parent.tag == TAG_EVENT) throw new CYFException("sprite.SetParent() can not be used with an Overworld Event's sprite.");
            if (tag == TAG_LETTER ^ (parent != null && parent.tag == TAG_LETTER)) throw new CYFException("sprite.SetParent() can not be used between letter sprites and other sprites.");
            if (tag == TAG_UI) throw new CYFException("sprite.SetParent() can not be used with UI.");
            try
            {
                GetTarget().SetParent(parent.gameObject.transform);
                if (gameObject.GetComponent<MaskImage>())
                    gameObject.GetComponent<MaskImage>().inverted = parent._masked == LuaSpriteController.MaskMode.INVERTEDSPRITE || parent._masked == LuaSpriteController.MaskMode.INVERTEDSTENCIL;
            }
            catch { throw new CYFException("sprite.SetParent(): You tried to set a removed sprite/nil sprite as this sprite's parent."); }
        }

        // Sets the pivot of a sprite (its rotation point)
        public void SetPivot(float x, float y)
        {
            gameObject.GetComponent<RectTransform>().pivot = new Vector2(x, y);
            if (gameObject.transform.parent.name == "SpritePivot")
                gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }

        // Sets the anchor of a sprite
        public void SetAnchor(float x, float y)
        {
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(x, y);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(x, y);
        }

        public void Move(float x, float y)
        {
            if (gameObject.transform.parent.name == "SpritePivot")
                gameObject.transform.parent.localPosition = new Vector3(x + this.x, y + this.y, gameObject.transform.parent.localPosition.z) - (Vector3)gameObject.GetComponent<RectTransform>().anchoredPosition;
            else
                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x + this.x, y + this.y);
        }

        public void MoveTo(float x, float y)
        {
            if (gameObject.transform.parent.name == "SpritePivot")
                gameObject.transform.parent.localPosition = new Vector3(x, y, gameObject.transform.parent.localPosition.z) - (Vector3)gameObject.GetComponent<RectTransform>().anchoredPosition;
            else
                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }

        public void MoveToAbs(float x, float y)
        {
            GetTarget().position = new Vector3(x, y, GetTarget().position.z);
        }

        // Sets both xScale and yScale of a sprite
        public void Scale(float xs, float ys)
        {
            if (gameObject.GetComponent<Projectile>())
                gameObject.GetComponent<Projectile>().needSizeRefresh = true;
            xScale = xs;
            yScale = ys;
            if (tag == TAG_LETTER)
            {
                nativeSizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(nativeSizeDelta.x * Mathf.Abs(xScale), nativeSizeDelta.y * Mathf.Abs(yScale));
            }
            else if (img)
            { // In battle
                nativeSizeDelta = new Vector2(img.sprite.texture.width, img.sprite.texture.height);
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(nativeSizeDelta.x * Mathf.Abs(xScale), nativeSizeDelta.y * Mathf.Abs(yScale));
                // img.GetComponent<RectTransform>().localScale = new Vector3(xs < 0 ? -1 : 1, ys < 0 ? -1 : 1, 1);
            }
            else
            { // In overworld
                nativeSizeDelta = new Vector2(gameObject.GetComponent<SpriteRenderer>().sprite.texture.width, gameObject.GetComponent<SpriteRenderer>().sprite.texture.height);
                gameObject.GetComponent<RectTransform>().localScale = new Vector3(100 * Mathf.Abs(xScale), 100 * Mathf.Abs(yScale), 1);
            }
            internalRotation = new Vector3(ys < 0 ? 180 : 0, xs < 0 ? 180 : 0, internalRotation.z);
            gameObject.GetComponent<RectTransform>().eulerAngles = internalRotation;
        }

        /** SetAnimation
        // Sets an animation for this instance
        public void SetAnimation(string[] frames) { SetAnimation(frames, 1 / 30f); }

        // Sets an animation for this instance with a frame timer
        public void SetAnimation(string[] spriteNames, float frametime, string prefix = "")
        {
            if (spriteNames == null) throw new CYFException("sprite.SetAnimation: The first argument (list of images) is nil.\n\nSee the documentation for proper usage.");
            if (spriteNames.Length == 0) throw new CYFException("sprite.SetAnimation: No sequence of animations was provided (animation table is empty).");
            if (frametime < 0) throw new CYFException("sprite.SetAnimation: An animation can not have negative speed!");
            if (frametime == 0) throw new CYFException("sprite.SetAnimation: An animation can not play at 0 frames per second!");

            if (prefix != "")
            {
                while (prefix.StartsWith("/"))
                    prefix = prefix.Substring(1);

                if (!prefix.EndsWith("/"))
                    prefix += "/";

                for (int i = 0; i < spriteNames.Length; i++)
                    spriteNames[i] = prefix + spriteNames[i];
            }

            Vector2 pivot = gameObject.GetComponent<RectTransform>().pivot;
            Keyframe[] kfArray = new Keyframe[spriteNames.Length];
            for (int i = 0; i < spriteNames.Length; i++)
            {
                // at least one sprite in the sequence was unable to be loaded
                if (FakeSpriteRegistry.Get(spriteNames[i]) == null)
                    throw new CYFException("sprite.SetAnimation: Failed to load sprite with the name \"" + spriteNames[i] + "\". Are you sure it is spelled correctly?");

                kfArray[i] = new Keyframe(FakeSpriteRegistry.Get(spriteNames[i]), spriteNames[i].ToLower());
            }
            if (keyframes == null)
            {
                if (gameObject.GetComponent<KeyframeCollection>())
                {
                    keyframes = gameObject.GetComponent<KeyframeCollection>();
                    keyframes.enabled = true;
                }
                else
                {
                    keyframes = gameObject.AddComponent<KeyframeCollection>();
                    keyframes.spr = this;
                }
            }
            else
                keyframes.enabled = true;
            keyframes.loop = loop;
            keyframes.Set(kfArray, frametime);
            //* UpdateAnimation();
            gameObject.GetComponent<RectTransform>().pivot = pivot;
        }
        */

        /** StopAnimation()
        public void StopAnimation()
        {
            if (keyframes == null) return;
            Vector2 pivot = gameObject.GetComponent<RectTransform>().pivot;
            keyframes.enabled = false;
            if (img)
            {
                img.sprite = originalSprite;
            }
            else
            {
                SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                imgtemp.sprite = originalSprite;
            }
            gameObject.GetComponent<RectTransform>().pivot = pivot;
        }
        */

        /** animationpaused
        // Gets or sets the paused state of a sprite's animation.
        public DynValue animationpaused
        {
            get
            {
                if (gameObject && keyframes != null)
                    return DynValue.NewBoolean(keyframes.paused);
                return DynValue.NewNil();
            }
            set
            {
                if (!gameObject) return;
                if (value.Type != DataType.Boolean)
                    throw new CYFException("sprite.paused can only be set to a boolean value.");

                if (keyframes != null)
                    keyframes.paused = value.Boolean;
                else
                    throw new CYFException("Unable to pause/resume a sprite without an active animation.");
            }
        }
        */

        /** currentframe
        // Gets or sets the current frame of an animated sprite's animation.
        // Example: If a sprite's animation table is      {"sans_head_1", "sans_head_2", "sans_head_3", "sans_head"2},
        // then for each sprite in the table, this will be: ^ 1            ^ 2            ^ 3            ^ 4
        public int currentframe
        {
            set
            {
                if (!gameObject) return;
                if (keyframes != null && keyframes.enabled)
                {
                    if (value < 1 || value > keyframes.keyframes.Length)
                        throw new CYFException("sprite.currentframe: New value " + value + " is out of bounds.");
                    // Store the previous "progress" of the frame
                    float progress = (keyframes.currTime / keyframes.timePerFrame) % 1;
                    // Calls keyframes.currTime %= keyframes.totalTime
                    keyframes.SetLoop(keyframes.loop);
                    keyframes.currTime = ((value - 1) * keyframes.timePerFrame) + (progress * keyframes.timePerFrame);
                }
                else
                    throw new CYFException("sprite.currentframe: You can not set the current frame of a sprite without an active animation.");
            }
            get
            {
                if (gameObject && keyframes != null)
                    return keyframes.getIndex();
                return 0;
            }
        }
        */

        /** currenttime
        // Gets or sets the current "play position" of a sprite's animation, in seconds.
        public float currenttime
        {
            set
            {
                if (!gameObject) return;
                if (keyframes != null && keyframes.enabled)
                {
                    if (value < 0 || value > keyframes.totalTime)
                        throw new CYFException("sprite.currenttime: New value " + value + " is out of bounds.");
                    keyframes.currTime = value % keyframes.totalTime;
                }
                else
                    throw new CYFException("sprite.currenttime: You can not set the current time of a sprite without an active animation.");
            }
            get
            {
                if (!gameObject || keyframes == null) return 0;
                if (!keyframes.enabled) return keyframes.totalTime;
                if (!keyframes.animationComplete())
                    return keyframes.currTime % keyframes.totalTime;
                return keyframes.totalTime;
            }
        }
        */

        /** totaltime
        // Gets (read-only) the total time an animation will run for, in seconds.
        public float totaltime
        {
            get
            {
                if (gameObject && keyframes != null)
                    return keyframes.totalTime;
                return 0;
            }
        }
        */

        /** animationspeed
        // Gets or sets the speed of an animated sprite's animation.
        public float animationspeed
        {
            set
            {
                if (!gameObject) return;
                if (keyframes != null)
                {
                    if (value < 0) throw new CYFException("sprite.animationspeed: An animation can not have negative speed!");
                    if (value == 0) throw new CYFException("sprite.animationspeed: An animation can not play at 0 frames per second!");

                    float percentCompletion = keyframes.currTime / (keyframes.keyframes.Length * keyframes.timePerFrame);
                    // Calls keyframes.totalTime = keyframes.timePerFrame * keyframes.Length;
                    keyframes.Set(keyframes.keyframes, value);
                    keyframes.currTime = percentCompletion * (keyframes.keyframes.Length * keyframes.timePerFrame);
                    // Calls keyframes.currTime %= keyframes.totalTime
                    keyframes.SetLoop(keyframes.loop);
                }
                else
                    throw new CYFException("sprite.animationspeed: You can not change the speed of a sprite without an active animation.");
            }
            get
            {
                if (gameObject && keyframes != null)
                    return keyframes.timePerFrame;
                return 0;
            }
        }
        */

        public void SendToTop()
        {
            GetTarget().SetAsLastSibling();
        }

        public void SendToBottom()
        {
            GetTarget().SetAsFirstSibling();
        }

        public void MoveBelow(FakeSpriteController sprite)
        {
            if (sprite == null) throw new CYFException("sprite.MoveBelow: The sprite passed as an argument is nil.");
            if (sprite.GetTarget().parent != GetTarget().parent) UnitaleUtil.Warn("You can't change the order of two sprites without the same parent.");
            else GetTarget().SetSiblingIndex(sprite.GetTarget().GetSiblingIndex());
        }

        public void MoveAbove(FakeSpriteController sprite)
        {
            if (sprite == null) throw new CYFException("sprite.MoveAbove: The sprite passed as an argument is nil.");
            if (sprite.GetTarget().parent != GetTarget().parent) UnitaleUtil.Warn("You can't change the order of two sprites without the same parent.");
            else GetTarget().SetSiblingIndex(sprite.GetTarget().GetSiblingIndex() + 1);
        }

        public LuaSpriteController.MaskMode _masked;
        public void Mask(string mode)
        {
            switch (tag)
            {
                case TAG_EVENT: throw new CYFException("sprite.Mask: Can not be applied to Overworld Event sprites.");
                case TAG_LETTER: throw new CYFException("sprite.Mask: Can not be applied to Letter sprites.");
                default: if (mode == null) throw new CYFException("sprite.Mask: No argument provided."); break;

            }

            LuaSpriteController.MaskMode masked;
            try
            {
                masked = (LuaSpriteController.MaskMode)Enum.Parse(typeof(LuaSpriteController.MaskMode), mode, true);
            }
            catch
            {
                throw new CYFException("sprite.Mask: Invalid mask mode \"" + mode + "\".");
            }

            if (masked != _masked)
            {
                //If children need to have their "inverted" property updated, then do so
                if ((int)_masked < 4 && (int)masked > 3 || (int)_masked > 3 && (int)masked < 4)
                    foreach (Transform child in GetTarget())
                    {
                        MaskImage childmask = child.gameObject.GetComponent<MaskImage>();
                        if (childmask != null)
                            childmask.inverted = (int)masked > 3;
                    }
                RectMask2D box = gameObject.GetComponent<RectMask2D>();
                Mask spr = gameObject.GetComponent<Mask>();

                switch (masked)
                {
                    case LuaSpriteController.MaskMode.BOX:
                        //Remove sprite mask if applicable
                        spr.enabled = false;
                        box.enabled = true;
                        break;
                    case LuaSpriteController.MaskMode.OFF:
                        //Mask has been disabled
                        spr.enabled = false;
                        box.enabled = false;
                        break;
                    default:
                        //The mask mode now can't possibly be box, so remove box mask if applicable
                        spr.enabled = true;
                        box.enabled = false;
                        // Used to differentiate between "sprite" and "stencil"-like display modes
                        spr.showMaskGraphic = masked == LuaSpriteController.MaskMode.SPRITE || masked == LuaSpriteController.MaskMode.INVERTEDSPRITE;
                        break;
                }
            }

            _masked = masked;
        }

        public void Remove()
        {
            if (gameObject == null)
                return;
            if (!GlobalControls.retroMode && tag == TAG_PROJECTILE)
            {
                gameObject.GetComponent<Projectile>().ctrl.Remove();
                return;
            }

            bool throwError = false;
            if ((!GlobalControls.retroMode && gameObject.gameObject.name == "player") || (!GlobalControls.retroMode && tag == "projectile") || tag == TAG_ENEMY || tag == TAG_BUBBLE || tag == TAG_UI)
            {
                if (gameObject.gameObject.name == "player")
                    throw new CYFException("sprite.Remove(): You can't remove the Player's sprite!");
                if (tag == TAG_PROJECTILE)
                {
                    if (gameObject.GetComponent<Projectile>().ctrl != null)
                        if (gameObject.GetComponent<Projectile>().ctrl.isactive) throwError = true;
                }
                else throwError = true;
            }
            if (throwError)
                throw new CYFException("sprite.Remove(): You can't remove a " + tag + "'s sprite!");

            if (tag == TAG_PROJECTILE)
            {
                Projectile[] pcs = img.GetComponentsInChildren<Projectile>();
                for (int i = 1; i < pcs.Length; i++)
                    pcs[i].ctrl.Remove();
            }
            //* StopAnimation();
            Object.Destroy(GetTarget().gameObject);
            gameObject = null;
        }

        /** Dust()
        public void Dust(bool playDust = true, bool removeObject = false)
        {
            if (tag == TAG_ENEMY || tag == TAG_BUBBLE || tag == TAG_UI)
                throw new CYFException("sprite.Dust(): You can't dust a " + tag + "'s sprite!");

            GameObject go = Object.Instantiate(Resources.Load<GameObject>("Prefabs/MonsterDuster"));
            go.transform.SetParent(UIController.instance.psContainer.transform);
            if (playDust)
                UnitaleUtil.PlaySound("DustSound", AudioClipRegistry.GetSound("enemydust"));
            gameObject.GetComponent<ParticleDuplicator>().Activate(this);
            if (gameObject.gameObject.name == "player") return;
            gameObject.SetActive(false);
            if (removeObject)
                Remove();
        }
        */

        /** UpdateAnimation()
        internal void UpdateAnimation()
        {
            if (!gameObject)
                return;
            if (keyframes == null || keyframes.paused)
                return;
            Keyframe k = keyframes.getCurrent();
            Sprite s;
            if (k != null)
                s = k.sprite;
            else
            {
                StopAnimation();
                return;
            }

            if (k.sprite == null) return;
            Quaternion rot = gameObject.transform.rotation;
            Vector2 pivot = gameObject.GetComponent<RectTransform>().pivot;
            if (img)
            {
                if (img.sprite != s)
                {
                    img.sprite = s;
                    originalSprite = img.sprite;
                    nativeSizeDelta = new Vector2(img.sprite.texture.width, img.sprite.texture.height);
                    //*shader.UpdateTexture(imgtemp.sprite.texture);
                    Scale(xScale, yScale);
                    if (tag == TAG_PROJECTILE)
                        gameObject.GetComponent<Projectile>().needUpdateTex = true;
                    gameObject.transform.rotation = rot;
                }
            }
            else
            {
                SpriteRenderer imgtemp = gameObject.GetComponent<SpriteRenderer>();
                if (imgtemp.sprite != s)
                {
                    imgtemp.sprite = s;
                    originalSprite = imgtemp.sprite;
                    nativeSizeDelta = new Vector2(imgtemp.sprite.texture.width, imgtemp.sprite.texture.height);
                    //* shader.UpdateTexture(imgtemp.sprite.texture);
                    Scale(xScale, yScale);
                    gameObject.transform.rotation = rot;
                }
            }
            gameObject.GetComponent<RectTransform>().pivot = pivot;
        }
        */

        /*
        internal void UpdateAnimation() {
            if (keyframes == null)
                return;
            Keyframe k = keyframes.getCurrent();
            Sprite s = SpriteRegistry.GENERIC_SPRITE_PREFAB.sprite;

            if (k != null)
                s = k.sprite;

            if (gameObject.sprite != s)
                gameObject.sprite = s;
        }*/

        void Update()
        {
            //* UpdateAnimation();
            firstFrame = false;
        }

        /** Get/SetVar()
        public void SetVar(string name, DynValue value)
        {
            if (name == null)
                throw new CYFException("sprite.SetVar: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
            vars[name] = value;
        }

        public DynValue GetVar(string name)
        {
            if (name == null)
                throw new CYFException("sprite.GetVar: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
            DynValue retval;
            return vars.TryGetValue(name, out retval) ? retval : DynValue.NewNil();
        }

        public DynValue this[string key]
        {
            get { return GetVar(key); }
            set { SetVar(key, value); }
        }
        */

        private Transform GetTarget()
        {
            Transform target = gameObject.transform;
            if (gameObject.transform.parent.name == "SpritePivot")
                target = target.parent;
            return target;
        }
    }
}
