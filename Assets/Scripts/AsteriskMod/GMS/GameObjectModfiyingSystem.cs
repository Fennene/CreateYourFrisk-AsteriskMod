using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>
    /// Be attached to <c>Canvas</c> in <c>Battle</c> Scene
    /// </summary>
    public class GameObjectModfiyingSystem : MonoBehaviour
    {
        public static UnityObject Find(string name)
        {
            GameObject gameObject = GameObject.Find(name);
            if (gameObject == null) throw new CYFException("GameObject \"" + name + "\" is not found.");
            return new UnityObject(gameObject);
        }

        public static UnityObject CreateObject(string name)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.parent = Instance.transform;
            return new UnityObject(gameObject);
        }

        private static GameObjectModfiyingSystem Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
