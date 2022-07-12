using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>
    /// Provides the method of modifying as GameObject in Lua Script.<br/>
    /// Be attached to <c>Canvas</c> in <c>Battle</c> Scene
    /// </summary>
    [ToDo("Recreate")]
    public class GameObjectModifyingSystem_ : MonoBehaviour
    {
        public UnityObject_ Find(string name)
        {
            GameObject gameObject = GameObject.Find(name);
            if (gameObject == null) throw new CYFException("GameObject \"" + name + "\" is not found.");
            return new UnityObject_(gameObject, true);
        }

        public UnityObject_ CreateObject(string name)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.parent = Instance.transform;
            gameObject.AddComponent<RectTransform>();
            return new UnityObject_(gameObject, false);
        }

        internal static GameObjectModifyingSystem_ Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
