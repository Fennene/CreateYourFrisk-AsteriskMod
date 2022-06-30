using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AsteriskMod.UnityUI
{
    public static class SetListeners
    {
        public static void SetListener(this UnityEvent unityEvent, UnityAction call)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(call);
        }

        public static void SetListener<T0>(this UnityEvent<T0> unityEvent, UnityAction<T0> call)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(call);
        }

        public static void SetListener<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(call);
        }

        public static void SetListener<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> call)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(call);
        }

        public static void SetListener<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent, UnityAction<T0, T1, T2, T3> call)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(call);
        }
    }
}
