using UnityEngine;

namespace AsteriskMod.ModdingHelperTools.UI
{
    public abstract class ExComponent : MonoBehaviour
    {
        public virtual void Enable() { }
        public virtual void Disable() { }
    }
}
