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

        public static ArenaUIManager Instance;

        private void Start()
        {
            border = transform.Find("arena_border_outer").gameObject;
            Instance = this;
            ArenaUtil.textPosition = Vector2.zero;
        }

        public void SetBorderColor(Color color)
        {
            border.GetComponent<Image>().color = color;
        }
    }
}