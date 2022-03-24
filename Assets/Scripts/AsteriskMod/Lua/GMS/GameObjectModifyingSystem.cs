using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.Lua.GMS
{
    public class GameObjectModifyingSystem
    {
        public static UnityObject Find(string name)
        {
            GameObject obj = GameObject.Find(name);
            return (obj != null) ? new UnityObject(obj, true) : null;
        }
    }
}
