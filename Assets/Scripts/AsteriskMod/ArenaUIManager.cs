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
        private GameObject mainTextMan;

        public static ArenaUIManager Instance;

        private void Start()
        {
            border = transform.Find("arena_border_outer").gameObject;
            Transform arena = border.transform.Find("arena");
            mainTextMan = arena.transform.Find("TextManager").gameObject;
            Instance = this;
            ArenaUtil.textPosition = Vector2.zero;
        }

        public void SetBorderColor(Color color)
        {
            border.GetComponent<Image>().color = color;
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
    }
}