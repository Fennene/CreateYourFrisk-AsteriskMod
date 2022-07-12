using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.GameobjectModifyingSystem
{
    public class GameobjectModifyingSystemMain : MonoBehaviour
    {
        [MoonSharpHidden] public static GameobjectModifyingSystemMain Instance;

        internal GameObject Canvas;

        private void Awake()
        {
            Canvas = gameObject;
            Instance = this;
        }

        public UnityObject Find(string name)
        {
            return new UnityObject(transform.Find(name).gameObject);
        }
    }
}
