using AsteriskMod;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Linq;

public class Misc {
    public string MachineName { get { return System.Environment.UserName; } }

    public void ShakeScreen(float duration, float intensity = 3, bool isIntensityDecreasing = true) {
        Camera.main.GetComponent<GlobalControls>().ShakeScreen(duration, intensity, isIntensityDecreasing);
    }

    public void StopShake() { GlobalControls.stopScreenShake = true; }

    public bool FullScreen {
        get { return Screen.fullScreen; }
        set {
            Screen.fullScreen = value;
            ScreenResolution.SetFullScreen(value, 2);
        }
    }

    public static int WindowWidth {
        get { return Screen.fullScreen && ScreenResolution.wideFullscreen ? Screen.currentResolution.width : (int)ScreenResolution.displayedSize.x; }
    }

    public static int WindowHeight {
        get { return Screen.fullScreen && ScreenResolution.wideFullscreen ? Screen.currentResolution.height : (int)ScreenResolution.displayedSize.y; }
    }

    public static int ScreenWidth {
        get { return Screen.fullScreen && !ScreenResolution.wideFullscreen ? (int)ScreenResolution.displayedSize.x : Screen.currentResolution.width; }
    }

    public static int ScreenHeight {
        get { return Screen.currentResolution.height; }
    }

    public static int MonitorWidth {
        get { return ScreenResolution.lastMonitorWidth; }
    }

    public static int MonitorHeight {
        get { return ScreenResolution.lastMonitorHeight; }
    }

    public void SetWideFullscreen(bool borderless) {
        if (!GlobalControls.isInFight)
            throw new CYFException("SetWideFullscreen is only usable from within battles.");
        ScreenResolution.wideFullscreen = borderless;
        if (Screen.fullScreen)
            ScreenResolution.SetFullScreen(true, 0);
    }

    public static float cameraX {
        get { return Camera.main.transform.position.x - 320; }
        set {
            if (UnitaleUtil.IsOverworld && !GlobalControls.isInShop)
                PlayerOverworld.instance.cameraShift.x += value - (Camera.main.transform.position.x - 320);
            else {
                Camera.main.transform.position = new Vector3(value + 320, Camera.main.transform.position.y, Camera.main.transform.position.z);
                if (UserDebugger.instance)
                    UserDebugger.instance.transform.position = new Vector3(value + 620, UserDebugger.instance.transform.position.y, UserDebugger.instance.transform.position.z);
            }
        }
    }

    public static float cameraY {
        get { return Camera.main.transform.position.y - 240; }
        set {
            if (UnitaleUtil.IsOverworld && !GlobalControls.isInShop)
                PlayerOverworld.instance.cameraShift.y += value - (Camera.main.transform.position.y - 240);
            else {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, value + 240, Camera.main.transform.position.z);
                if (UserDebugger.instance)
                    UserDebugger.instance.transform.position = new Vector3(UserDebugger.instance.transform.position.x, value + 480, UserDebugger.instance.transform.position.z);
            }
        }
    }

    public static void MoveCamera(float x, float y) {
        cameraX += x;
        cameraY += y;
    }

    public static void MoveCameraTo(float x, float y) {
        cameraX = x;
        cameraY = y;
    }

    public static void ResetCamera() {
        if (UnitaleUtil.IsOverworld && !GlobalControls.isInShop)
            PlayerOverworld.instance.cameraShift = Vector2.zero;
        else
            MoveCameraTo(0f, 0f);
    }

    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    public static float cameraRotation
    {
        get { return Camera.main.transform.eulerAngles.z; }
        set { Camera.main.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Math.Mod(value, 360)); }
    }

    public static void CameraHorizontalReverse()
    {
        Camera.main.transform.eulerAngles = new Vector3(0, 180, 0);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 500);
    }

    public static void CameraVerticalReverse()
    {
        Camera.main.transform.eulerAngles = new Vector3(180, 0, 0);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 500);
    }

    public static void ResetCameraReverse()
    {
        Camera.main.transform.eulerAngles = Vector3.zero;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10);
    }
    // --------------------------------------------------------------------------------

    public LuaSpriteShader ScreenShader {
        get { return CameraShader.luashader; }
    }

    public static void DestroyWindow() { Application.Quit(); }

    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    // TODO: When OW is reworked, add 3rd argument to open a file in any of "mod", "map" or "default" locations
    //public static LuaFile OpenFile(string path, string mode = "rw") { return new LuaFile(path, mode); }

    // TODO: When OW is reworked, add 4th argument to open a file in any of "mod", "map" or "default" locations
    public static LuaFile OpenFile(string path, string mode = "rw") { return new LuaFile(path, mode, false); }
    // --------------------------------------------------------------------------------

    public bool FileExists(string path) {
        if (path.Contains(".."))
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            throw new CYFException(EngineLang.Get("Exception", "MiscCheckFileOutside"));
            // --------------------------------------------------------------------------------
        return File.Exists((FileLoader.ModDataPath + "/" + path).Replace('\\', '/'));
    }

    public bool DirExists(string path) {
        if (path.Contains(".."))
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            throw new CYFException(EngineLang.Get("Exception", "MiscCheckDirOutside"));
            // --------------------------------------------------------------------------------
        return Directory.Exists((FileLoader.ModDataPath + "/" + path).Replace('\\', '/'));
    }

    public bool CreateDir(string path) {
        if (path.Contains(".."))
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            throw new CYFException(EngineLang.Get("Exception", "MiscCreateDirOutside"));
            // --------------------------------------------------------------------------------

        if (Directory.Exists((FileLoader.ModDataPath + "/" + path).Replace('\\', '/'))) return false;
        Directory.CreateDirectory(FileLoader.ModDataPath + "/" + path);
        return true;
    }

    private static bool PathValid(string path) { return path != " " && path != "" && path != "/" && path != "\\" && path != "." && path != "./" && path != ".\\"; }

    public bool MoveDir(string path, string newPath) {
        if (path.Contains("..") || newPath.Contains(".."))
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            throw new CYFException(EngineLang.Get("Exception", "MiscMoveDirOutside"));
            // --------------------------------------------------------------------------------

        if (!DirExists(path) || DirExists(newPath) || !PathValid(path)) return false;
        Directory.Move(FileLoader.ModDataPath + "/" + path, FileLoader.ModDataPath + "/" + newPath);
        return true;
    }

    public bool RemoveDir(string path, bool force = false) {
        if (path.Contains(".."))
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            throw new CYFException(EngineLang.Get("Exception", "MiscRemoveDirOutside"));
            // --------------------------------------------------------------------------------

        if (!Directory.Exists((FileLoader.ModDataPath + "/" + path).Replace('\\', '/'))) return false;
        try { Directory.Delete(FileLoader.ModDataPath + "/" + path, force); }
        catch { /* ignored */ }

        return false;
    }

    public string[] ListDir(string path, bool getFolders = false) {
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        if (path == null)        throw new CYFException(EngineLang.Get("Exception", "MiscNullListDir"));
        if (path.Contains("..")) throw new CYFException(EngineLang.Get("Exception", "MiscListDirOutside"));
        // --------------------------------------------------------------------------------

        path = (FileLoader.ModDataPath + "/" + path).Replace('\\', '/');
        if (!Directory.Exists(path))
            throw new CYFException("Invalid path:\n\n\"" + path + "\"");

        DirectoryInfo d = new DirectoryInfo(path);
        System.Collections.Generic.List<string> retval = new System.Collections.Generic.List<string>();
        retval.AddRange(!getFolders ? d.GetFiles().Select(fi => Path.GetFileName(fi.ToString()))
                                    : d.GetDirectories().Select(di => di.Name));
        return retval.ToArray();
    }

    public static string OSType {
        get {
            switch (Application.platform) {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer: return "Windows";
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:   return "Linux";
                default:                            return "Mac";
            }
        }
    }

    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [DllImport("user32.dll")]
        private static extern int GetActiveWindow();
        public static int window = GetActiveWindow();

        public static void RetargetWindow() { window = GetActiveWindow(); }

        [DllImport("user32.dll")]
        public static extern int FindWindow(string className, string windowName);
        [DllImport("user32.dll")]
        private static extern int MoveWindow(int hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);
        [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(int hwnd, StringBuilder lpWindowText, int nMaxCount);
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        //[DllImport("user32.dll", EntryPoint = "SetWindowText")]
        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Auto)]
        // --------------------------------------------------------------------------------
        public static extern bool SetWindowText(int hwnd, string text);
        //* [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
        //* public static extern bool SetWindowTextUnicode(int hwnd, string text);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(int hWnd, out RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static string WindowName {
            get {
                StringBuilder strbTitle = new StringBuilder(9999);
                GetWindowText(window, strbTitle, strbTitle.Capacity + 1);
                return strbTitle.ToString();
            }
            set { SetWindowText(window, value); }
        }

        public static int WindowX {
                get {
                    Rect size = GetWindowRect();
                    return (int)size.x;
                }
                set {
                     Rect size = GetWindowRect();
                     MoveWindow(window, value, (int)size.y, (int)size.width, (int)size.height, 1);
                }
            }

        public static int WindowY {
            get {
                Rect size = GetWindowRect();
                return Screen.currentResolution.height - (int)size.y - (int)size.height;
            }
            set {
                 Rect size = GetWindowRect();
                 MoveWindow(window, (int)size.x, Screen.currentResolution.height - value - (int)size.height, (int)size.width, (int)size.height, 1);
            }
        }

        public static void MoveWindow(int X, int Y) {
            Rect size = GetWindowRect();
            if (!Screen.fullScreen)
                MoveWindow(window, (int)size.x + X, (int)size.y - Y, (int)size.width, (int)size.height, 1);
        }

        public static void MoveWindowTo(int X, int Y) {
            Rect size = GetWindowRect();
            if (!Screen.fullScreen)
                MoveWindow(window, X, Screen.currentResolution.height - Y - (int)size.height, (int)size.width, (int)size.height, 1);
        }

        private static Rect GetWindowRect() {
            RECT r;
            GetWindowRect(window, out r);
            return new Rect(r.Left, r.Top, Mathf.Abs(r.Right - r.Left), Mathf.Abs(r.Top - r.Bottom));
        }

        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        [DllImport("user32.dll", EntryPoint = "MessageBox", CharSet = CharSet.Auto)]
        private static extern int MsgBox(int hWnd, string lpText, string lpCaption, uint uType);
        private const uint MB_OK = 0x00000000;
        private const uint MB_OKCANCEL = 0x00000001;
        private const uint MB_ABORTRETRYIGNORE = 0x00000002;
        private const uint MB_YESNOCANCEL = 0x00000003;
        private const uint MB_YESNO = 0x00000004;
        private const uint MB_RETRYCANCEL = 0x00000005;
        private const uint MB_CANCELTRYCONTINUE = 0x00000006;
        private const uint MB_ICONNONE = 0x00000000;
        private const uint MB_ICONERROR = 0x00000010;
        private const uint MB_ICONQUESTION = 0x00000020;
        private const uint MB_ICONWARNING = 0x00000030;
        private const uint MB_ICONINFORMATION = 0x00000040;

        private static uint ConvertButtonType(int buttonType)
        {
            switch (buttonType)
            {
                case 1:
                    return MB_OKCANCEL;
                case 2:
                    return MB_ABORTRETRYIGNORE;
                case 3:
                    return MB_YESNOCANCEL;
                case 4:
                    return MB_YESNO;
                case 5:
                    return MB_RETRYCANCEL;
                case 6:
                    return MB_CANCELTRYCONTINUE;
            }
            return MB_OK;
        }

        private static uint ConvertIconType(int iconType)
        {
            switch (iconType)
            {
                case 1:
                    return MB_ICONERROR;
                case 2:
                    return MB_ICONQUESTION;
                case 3:
                    return MB_ICONWARNING;
                case 4:
                    return MB_ICONINFORMATION;
            }
            return MB_ICONNONE;
        }

        public static int MessageBox(string text, string title = "", int iconType = 0, int buttonType = 0)
        {
            if (!Asterisk.RequireExperimentalFeature("Misc.MessageBox")) return 0;
            return MsgBox(window, text, title, ConvertIconType(iconType) + ConvertButtonType(buttonType));
        }
        // --------------------------------------------------------------------------------

    #else
        public static string WindowName {
            get {
                UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here.");
                return "";
            }
            set { UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here."); }
        }

        public static int WindowX {
            get {
                UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here.");
                return 0;
            }
            set { UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here."); }
        }

        public static int WindowY {
            get {
                UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here.");
                return 0;
            }
            set { UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here."); }
        }

        public static void MoveWindowTo(int X, int Y) {
            UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here.");
            return;
        }

        public static void MoveWindow(int X, int Y) {
            UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here.");
            return;
        }

        public static Rect GetWindowRect() {
            UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here.");
            return new Rect();
        }

        public static int MessageBox(string text, string title = "", int iconType = 0, int buttonType = 0)
        {
            UnitaleUtil.DisplayLuaError("Windows-only function", "This feature is Windows-only! Sorry, but you can't use it here.");
            return 0;
        }
    #endif
}