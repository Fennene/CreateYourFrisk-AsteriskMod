using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.GameobjectModifyingSystem
{
    public class GameobjectModifyingSystemMain : MonoBehaviour
    {
        public static GameobjectModifyingSystemMain Instance;

        internal GameObject Canvas;

        private void Awake()
        {
            Canvas = gameObject;
            Instance = this;
        }
    }
}
