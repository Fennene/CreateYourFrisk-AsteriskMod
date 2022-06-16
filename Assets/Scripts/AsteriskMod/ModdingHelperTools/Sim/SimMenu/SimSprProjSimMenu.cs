using AsteriskMod.ModdingHelperTools.UI;
using MoonSharp.Interpreter;
using System;
using System.IO;
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

            AwakeCreateObjects();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.IsRunningAnimation) return;
                SimMenuWindowManager.ChangePage(SimMenuWindowManager.DisplayingSimMenu.SprProjSim, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
            StartCreateObjects();
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

        internal static CYFInputField Create_SpriteName;
        internal static Dropdown Create_SpriteLayer;
        internal static Toggle Create_AsBullet;
        internal static Button Create_Run;
        internal static Image Create_Run_Image;
        internal static bool CanCreateSprite { get { return Sprites[MAX_SPRITE_OBJECT - 1] == null; } }
        internal static bool CanCreateBullet { get { return Bullets[MAX_BULLET_OBJECT - 1] == null; } }

        internal void AwakeCreateObjects()
        {
            Transform objManagerWindow = transform.Find("ObjectManagerWindow").Find("View");
            Transform temp = objManagerWindow.Find("ObjCreate");
            Create_SpriteName = temp.Find("SpriteName").GetComponent<CYFInputField>();
            Create_SpriteLayer = temp.Find("LayerName").GetComponent<Dropdown>();
            Create_AsBullet = temp.Find("ProjCheck").GetComponent<Toggle>();
            Create_Run = temp.Find("Create").GetComponent<Button>();
            Create_Run_Image = temp.Find("Create").GetComponent<Image>();
        }

        internal void StartCreateObjects()
        {
            Create_SpriteName.InputField.onValueChanged.RemoveAllListeners();
            Create_SpriteName.InputField.onValueChanged.AddListener((value) =>
            {
                FileInfo fi = new FileInfo(FakeFileLoader.pathToModFile("Sprites/" + value + ".png"));
                if (!fi.Exists) fi = new FileInfo(FakeFileLoader.pathToDefaultFile("Sprites/" + value + ".png"));
                if (!fi.Exists) Create_SpriteName.OuterImage.color = new Color32(255, 64, 64, 255);
                else            Create_SpriteName.ResetOuterColor();
                if (!Create_AsBullet.isOn && !CanCreateSprite) return;
                if (Create_AsBullet.isOn && !CanCreateBullet) return;
                if (Create_Run.enabled == fi.Exists) return;
                Create_Run.enabled = fi.Exists;
                Create_Run_Image.color = fi.Exists ? new Color32(242, 242, 242, 255) : new Color32(192, 192, 192, 255);
            });

            Create_AsBullet.onValueChanged.RemoveAllListeners();
            Create_AsBullet.onValueChanged.AddListener((value) =>
            {
                Create_SpriteLayer.value = value ? 5 : 2;
                bool cantCreate = ((!value && !CanCreateSprite) || (value && !CanCreateBullet));
                Create_Run.enabled = !cantCreate;
                Create_Run_Image.color = !cantCreate ? new Color32(242, 242, 242, 255) : new Color32(192, 192, 192, 255);
            });
        }
    }
}
