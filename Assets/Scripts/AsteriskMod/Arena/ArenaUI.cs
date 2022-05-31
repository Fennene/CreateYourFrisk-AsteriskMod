using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    // Be attached in Assets/Battle.Unity/Canvas/arena_container
    public class ArenaUI : MonoBehaviour
    {
        private static GameObject border;
        private static GameObject arena;
        private static GameObject mainTextMan;
        private static Vector2 mainTextManPos;

        private void Awake()
        {
            border = transform.Find("arena_border_outer").gameObject;
            arena = border.transform.Find("arena").gameObject;
            mainTextMan = arena.transform.Find("TextManager").gameObject;
            mainTextManPos = Vector2.zero;
        }

        public static void SetBorderColor(Color color)
        {
            border.GetComponent<Image>().color = color;
        }

        public static void SetInnerColor(Color color)
        {
            arena.GetComponent<Image>().color = color;
        }

        public static void SetMainTextPosition(int newX, int newY)
        {
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.transform == null)
            {
                throw new CYFException("The UIController.instance.mainTextManager object has not been initialized yet.\n\nPlease wait to run this code.");
            }
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
            if (UIController.instance == null || UIController.instance.mainTextManager == null || UIController.instance.mainTextManager.letterReferences == null)
            {
                throw new CYFException("The UIController.instance.mainTextManager object has not been initialized yet.\n\nPlease wait to run this code.");
            }
            Table table = new Table(null);
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

        public static void SetTextVolume(float value)
        {
            if (value < 0) value = 0;
            else if (value > 1) value = 1;
            //mainTextMan.GetComponent<AudioSource>().volume = value;
            mainTextMan.GetComponent<TextManager>().letterSound.volume = value;
        }

        public static float GetTextVolume()
        {
            return mainTextMan.GetComponent<TextManager>().letterSound.volume;
        }
    }
}
