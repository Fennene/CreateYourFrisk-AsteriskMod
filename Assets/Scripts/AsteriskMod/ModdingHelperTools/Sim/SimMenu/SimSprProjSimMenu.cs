using MoonSharp.Interpreter;
using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimSprProjSimMenu : MonoBehaviour
    {
        //* private static bool _uniqueCheck;

        internal static Button BackButton;
        internal static Transform bulletLayer;

        private void Awake()
        {
            //* if (_uniqueCheck) throw new Exception("SimSprProjSimMenuが複数存在します。");
            //* _uniqueCheck = true;

            BackButton = transform.Find("MenuNameLabel").Find("BackButton").GetComponent<Button>();
            bulletLayer = GameObject.Find("BulletPool").transform;

            Sprites = new FakeSpriteController[MAX_SPRITE_OBJECT];
            Bullets = new FakeProjectileController[MAX_BULLET_OBJECT];
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.IsRunningAnimation) return;
                SimMenuWindowManager.ChangePage(SimMenuWindowManager.DisplayingSimMenu.SprProjSim, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
        }

        internal static FakeSpriteController CreateSprite(string filename, string tag = "BelowArena", int childNumber = -1)
        {
            string canvas = "Canvas/";
            if (ParseUtil.TestInt(tag) && childNumber == -1)
            {
                childNumber = ParseUtil.GetInt(tag);
                tag = "BelowArena";
            }
            Image i = Object.Instantiate(FakeSpriteRegistry.GENERIC_SPRITE_PREFAB);
            if (!string.IsNullOrEmpty(filename))
                FakeSpriteRegistry.SwapSpriteFromFile(i, filename);
            else
                throw new CYFException("You can't create a sprite object with a nil sprite!");
            if (!GameObject.Find(tag + "Layer") && tag != "none")
                if (tag == "BelowArena")
                    i.transform.SetParent(GameObject.Find(canvas).transform);
                else
                    UnitaleUtil.DisplayLuaError("Creating a sprite", "The sprite layer " + tag + " doesn't exist.");
            else
            {
                i.transform.SetParent(GameObject.Find(tag == "none" ? canvas : tag + "Layer").transform, true);
                if (childNumber != -1)
                    i.transform.SetSiblingIndex(childNumber - 1);
            }
            //* return UserData.Create(new FakeSpriteController(i), LuaSpriteController.data);
            return new FakeSpriteController(i);
        }

        internal static FakeProjectileController CreateProjectileAbs(string sprite, float xpos, float ypos, string layerName = "")
        {
            FakeLuaProjectile projectile = Instantiate(Resources.Load<FakeLuaProjectile>("Prefabs/AsteriskMod/FakeLUAProjectile 1"));
            projectile.transform.SetParent(bulletLayer);
            projectile.GetComponent<RectTransform>().position = new Vector2(-999, -999);
            projectile.gameObject.SetActive(false);
            projectile.renewController();
            //* LuaProjectile projectile = (LuaProjectile)BulletPool.instance.Retrieve();
            if (sprite == null)
                throw new CYFException("You can't create a projectile with a nil sprite!");
            FakeSpriteRegistry.SwapSpriteFromFile(projectile, sprite);
            projectile.name = sprite;
            //* projectile.owner = s;
            projectile.gameObject.SetActive(true);
            projectile.ctrl.MoveToAbs(xpos, ypos);
            //projectile.ctrl.z = Projectile.Z_INDEX_NEXT; //doesn't work yet, thanks unity UI
            projectile.transform.SetAsLastSibling();
            //projectile.ctrl.UpdatePosition();
            projectile.ctrl.sprite.Set(sprite);
            if (layerName != "")
                try { projectile.transform.SetParent(GameObject.Find(layerName + "Bullet").transform); }
                catch
                {
                    try { projectile.transform.SetParent(GameObject.Find(layerName + "Layer").transform); }
                    catch { /* ignored */ }
                }
            //* DynValue projectileController = UserData.Create(projectile.ctrl);
            //Texture2D tex = (Texture2D)projectile.GetComponent<Image>().mainTexture;
            //projectile.selfAbs = UnitaleUtil.GetFurthestCoordinates(tex.GetPixels32(), tex.height, projectile.self);

            //* return projectileController;
            return projectile.ctrl;
        }

        internal static FakeProjectileController CreateProjectile(string sprite, float xpos, float ypos, string layerName = "")
        {
            return CreateProjectileAbs(sprite, FakeArenaManager.arenaCenter.x + xpos, FakeArenaManager.arenaCenter.y + ypos, layerName);
        }

        internal static readonly byte MAX_SPRITE_OBJECT = 16;
        internal static readonly byte MAX_BULLET_OBJECT = 16;

        internal static FakeSpriteController[] Sprites;
        internal static FakeProjectileController[] Bullets;
    }
}
