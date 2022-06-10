using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    public static class FakeStaticInits
    {
        public static string MODFOLDER;
        public static string ENCOUNTER = "";
        private static bool firstInit;

        public static bool Initialized { get; set; }

        public static void Start()
        {
            if (!firstInit)
            {
                FakeSpriteRegistry.Start();
                FakeSpriteFontRegistry.Start();
                firstInit = true;
            }
            if (string.IsNullOrEmpty(MODFOLDER))
                MODFOLDER = "@Title";
            InitAll();
            Initialized = true;
            /*
            if (!firstInit)
            {
                firstInit = true;
                SpriteRegistry.Start();
                AudioClipRegistry.Start();
                SpriteFontRegistry.Start();
                ShaderRegistry.Start();
            }
            if (string.IsNullOrEmpty(MODFOLDER))
                MODFOLDER = EDITOR_MODFOLDER;
            //if (CurrMODFOLDER != MODFOLDER || CurrENCOUNTER != ENCOUNTER)
            InitAll();
            Initialized = true;
            */
        }

        public static void InitAll(/*bool shaders = false*/)
        {
            if (!Initialized)
            {
                FakeSpriteRegistry.Init();
                FakeSpriteFontRegistry.Init();
            }
            //LateUpdater.Init(); // ?
            MusicManager.src = Camera.main.GetComponent<AudioSource>(); // ?
            Initialized = true;
            /*
            if (!Initialized && (!GlobalControls.isInFight || GlobalControls.modDev))
            {
                //UnitaleUtil.createFile();
                Stopwatch sw = new Stopwatch(); //benchmarking terrible loading times
                sw.Start();
                ScriptRegistry.Init();
                sw.Stop();
                UnityEngine.Debug.Log("Script registry loading time: " + sw.ElapsedMilliseconds + "ms");
                sw.Reset();

                sw.Start();
                SpriteRegistry.Init();
                sw.Stop();
                UnityEngine.Debug.Log("Sprite registry loading time: " + sw.ElapsedMilliseconds + "ms");
                sw.Reset();

                sw.Start();
                AudioClipRegistry.Init();
                sw.Stop();
                UnityEngine.Debug.Log("Audio clip registry loading time: " + sw.ElapsedMilliseconds + "ms");
                sw.Reset();

                sw.Start();
                SpriteFontRegistry.Init();
                sw.Stop();
                UnityEngine.Debug.Log("Sprite font registry loading time: " + sw.ElapsedMilliseconds + "ms");
                sw.Reset();

                if (shaders)
                {
                    sw.Start();
                    ShaderRegistry.Init();
                    sw.Stop();
                    UnityEngine.Debug.Log("Shader registry loading time: " + sw.ElapsedMilliseconds + "ms");
                    sw.Reset();
                }
            }
            else
                Initialized = true;
            LateUpdater.Init(); // must be last; lateupdater's initialization is for classes that depend on the above registries
            MusicManager.src = Camera.main.GetComponent<AudioSource>();
            SendLoaded();
            //CurrENCOUNTER = ENCOUNTER;
            //CurrMODFOLDER = MODFOLDER;
            */
        }
    }
}
