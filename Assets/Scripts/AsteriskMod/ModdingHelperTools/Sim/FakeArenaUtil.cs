using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeArenaUtil : MonoBehaviour
    {
        internal static FakeArenaUtil Instance;

        private GameObject border;
        private GameObject arena;
        //* private static GameObject mainTextMan;
        //* private static Vector2 mainTextManPos;
        private GameObject Target;
        private GameObject TargetChoice;

        private Vector2 _arenaOffset;
        internal Vector2 ArenaOffset
        {
            get { return _arenaOffset; }
            set
            {
                Vector2 oldArenaOffset = _arenaOffset;
                _arenaOffset = value;
                UIController.UIState state = SimInstance.BattleSimulator.CurrentState;
                if (state == UIController.UIState.DEFENDING || state == UIController.UIState.CUSTOMSTATE)
                {
                    FakeArenaManager.instance.MoveImmediate(-oldArenaOffset.x + _arenaOffset.x, -oldArenaOffset.y + _arenaOffset.y, true);
                }
                else
                {
                    FakeArenaManager.instance.resetArena();
                }
            }
        }

        private Vector2 _arenaOffsetSize;
        internal Vector2 ArenaOffsetSize
        {
            get { return _arenaOffsetSize; }
            set { SetArenaOffsetSize(value.x, value.y, false); }
        }
        internal void SetArenaOffsetSize(float width, float height, bool immediate)
        {
            if (width < -ArenaManager.UIWidth) width = -ArenaManager.UIWidth;
            if (height < -ArenaManager.UIHeight) height = -ArenaManager.UIHeight;
            Vector2 oldArenaOffsetSize = _arenaOffsetSize;
            _arenaOffsetSize = new Vector2(width, height);
            UIController.UIState state = SimInstance.BattleSimulator.CurrentState;
            if (state == UIController.UIState.DEFENDING || state == UIController.UIState.CUSTOMSTATE)
            {
                float newWidth = FakeArenaManager.instance.desiredWidth - oldArenaOffsetSize.x + _arenaOffsetSize.x;
                float newHeight = FakeArenaManager.instance.desiredHeight - oldArenaOffsetSize.y + _arenaOffsetSize.y;
                FakeArenaManager.instance.ResizeImmediate(newWidth, newHeight);
            }
            else
            {
                FakeArenaManager.instance.resetArena(immediate);
            }
        }

        internal void Initialize()
        {
            _arenaOffset = Vector2.zero;
            _arenaOffsetSize = Vector2.zero;
        }

        private void Awake()
        {
            Initialize();

            border = transform.Find("arena_border_outer").gameObject;
            arena = border.transform.Find("arena").gameObject;
            //* mainTextMan = arena.transform.Find("TextManager").gameObject;
            //* mainTextManPos = Vector2.zero;
            Target = arena.transform.Find("FightUI").gameObject;
            TargetChoice = Target.transform.Find("FightUILine").gameObject;

            Instance = this;
        }

        public void SetBorderColor(Color color) { border.GetComponent<Image>().color = color; }

        public void SetInnerColor(Color color) { arena.GetComponent<Image>().color = color; }

        /*
        public static void SetMainTextPosition(int newX, int newY)
        {
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.transform == null) return;
            Vector3 oldPos = UIController.instance.mainTextManager.transform.position;
            Vector2 oldRelativePos = mainTextManPos;
            mainTextManPos = new Vector2(newX, newY);
            UIController.instance.mainTextManager.transform.position = new Vector3(
                oldPos.x - oldRelativePos.x + mainTextManPos.x,
                oldPos.y - oldRelativePos.y + mainTextManPos.y,
                oldPos.z
            );
        }

        public static Vector2 GetMainTextPosition()
        {
            return mainTextManPos;
        }

        public static Table GetMainTextLetters()
        {
            Table table = new Table(null);
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.letterReferences == null) return table;
            int key = 0;
            foreach (Image i in UIController.instance.mainTextManager.letterReferences)
            {
                if (i != null)
                {
                    key++;
                    LuaSpriteController letter = new LuaSpriteController(i) { tag = "letter" };
                    table.Set(key, UserData.Create(letter, LuaSpriteController.data));
                }
            }
            return table;
        }
        */

        public void SetTargetSprite(string path)
        {
            Target.GetComponent<Image>().sprite = SimInstance.FakeSpriteRegistry.Get(path);
            //Target.GetComponent<AutoloadResourcesFromRegistry>().SpritePath = path;
        }

        public void SetTargetChoiceSprite(string path)
        {
            TargetChoice.GetComponent<Image>().sprite = SimInstance.FakeSpriteRegistry.Get(path);
            //TargetChoice.GetComponent<AutoloadResourcesFromRegistry>().SpritePath = path;
        }

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
