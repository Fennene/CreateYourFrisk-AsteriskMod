using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>
    /// Provides the method of modifying as GameObject in Lua Script.<br/>
    /// Be attached to <c>Canvas</c> in <c>Battle</c> Scene
    /// </summary>
    public class GameObjectModifyingSystem : MonoBehaviour
    {
        public static UnityObject Find(string name)
        {
            GameObject gameObject = GameObject.Find(name);
            if (gameObject == null) throw new CYFException("GameObject \"" + name + "\" is not found.");
            return new UnityObject(gameObject, true);
        }

        public static UnityObject CreateObject(string name)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.parent = Instance.transform;
            gameObject.AddComponent<RectTransform>();
            return new UnityObject(gameObject, false);
        }

        internal static GameObjectModifyingSystem Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
