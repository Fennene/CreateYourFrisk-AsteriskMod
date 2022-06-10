using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeArenaUtil : MonoBehaviour
    {
        private static GameObject border;
        private static GameObject arena;
        private static GameObject mainTextMan;
        private static Vector2 mainTextManPos;
        private static GameObject Target;
        private static GameObject TargetChoice;

        private static Vector2 _arenaOffset;
        internal static Vector2 ArenaOffset
        {
            get { return _arenaOffset; }
            set
            {
                Vector2 oldArenaOffset = _arenaOffset;
                _arenaOffset = value;
                UIController.UIState state = BattleSimulator.CurrentState;

                return;

                if (state == UIController.UIState.DEFENDING || state == UIController.UIState.CUSTOMSTATE)
                {
                    ArenaManager.instance.MoveImmediate(-oldArenaOffset.x + _arenaOffset.x, -oldArenaOffset.y + _arenaOffset.y, true);
                }
                else
                {
                    ArenaManager.instance.resetArena();
                    if (state == UIController.UIState.ENEMYSELECT || state == UIController.UIState.ACTMENU || state == UIController.UIState.ITEMMENU || state == UIController.UIState.MERCYMENU)
                    {
                        float xPos = PlayerController.instance.self.anchoredPosition.x - oldArenaOffset.x + _arenaOffset.x;
                        float yPos = PlayerController.instance.self.anchoredPosition.y - oldArenaOffset.y + _arenaOffset.y;
                        PlayerController.instance.SetPosition(xPos, yPos, false);
                    }
                }
            }
        }

        internal static void Initialize()
        {
            _arenaOffset = Vector2.zero;
        }

        private void Awake()
        {
            border = transform.Find("arena_border_outer").gameObject;
            arena = border.transform.Find("arena").gameObject;
            mainTextMan = arena.transform.Find("TextManager").gameObject;
            mainTextManPos = Vector2.zero;
            Target = arena.transform.Find("FightUI").gameObject;
            TargetChoice = Target.transform.Find("FightUILine").gameObject;
        }

        public static void SetBorderColor(Color color) { border.GetComponent<Image>().color = color; }

        public static void SetInnerColor(Color color) { arena.GetComponent<Image>().color = color; }

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

        public static void SetTargetSprite(string path)
        {
            Target.GetComponent<Image>().sprite = FakeSpriteRegistry.Get(path);
            //Target.GetComponent<AutoloadResourcesFromRegistry>().SpritePath = path;
        }

        public static void SetTargetChoiceSprite(string path)
        {
            TargetChoice.GetComponent<Image>().sprite = FakeSpriteRegistry.Get(path);
            //TargetChoice.GetComponent<AutoloadResourcesFromRegistry>().SpritePath = path;
        }
    }
}
