using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeLateUpdater : MonoBehaviour
    {
        public static List<Action> lateInit = new List<Action>();
        public static List<Action> lateActions = new List<Action>();
        private int frametimer;

        public static void Init() { InvokeList(lateInit); }

        private void Update()
        {
            if (frametimer > 0)
            {
                InvokeList(lateActions);
                Destroy(this);
            }
            frametimer++;
        }

        private static void InvokeList(ICollection<Action> l)
        {
            foreach (Action a in l)
                a.Invoke();
            l.Clear();
        }
    }
}
