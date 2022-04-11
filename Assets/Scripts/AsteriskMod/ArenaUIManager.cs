using AsteriskMod.Lua;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    // Be attached in Assets/Battle.Unity/Canvas/arena_container
    public class ArenaUIManager : MonoBehaviour
    {
        private GameObject border;
        private GameObject arena;
        private GameObject mainTextMan;

        internal static char asterisk_char = '*';
        public static ArenaUIManager Instance;

        public static void Initialize()
        {
            asterisk_char = '*';
        }

        private void Start()
        {
            border = transform.Find("arena_border_outer").gameObject;
            arena = border.transform.Find("arena").gameObject;
            mainTextMan = arena.transform.Find("TextManager").gameObject;
            Instance = this;
            ArenaUtil.textPosition = Vector2.zero;
        }

        public void SetBorderColor(Color color)
        {
            border.GetComponent<Image>().color = color;
        }

        public void SetInnerColor(Color color)
        {
            arena.GetComponent<Image>().color = color;
        }

        public void SetTextVolume(float value)
        {
            if (value < 0) value = 0;
            else if (value > 1) value = 1;
            mainTextMan.GetComponent<AudioSource>().volume = value;
        }

        public float GetTextVolume()
        {
            return mainTextMan.GetComponent<AudioSource>().volume;
        }

        public void SetTextMute(bool active)
        {
            mainTextMan.GetComponent<AudioSource>().enabled = !active;
        }

        public bool GetTextMute()
        {
            return !mainTextMan.GetComponent<AudioSource>().enabled;
        }
    }
}