using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>
    /// Initializes about parameter of the engine (ex: frame rate) or Just creates contractor to the class that is needed to Lua Script<br/>
    /// Be attached to <c>Canvas</c> in <c>Battle</c> Scene<br/>
    /// Script Execution Order: 105
    /// </summary>
    public class FirstInitialize : MonoBehaviour
    {
        private void Awake()
        {
            AsteriskEngine.AwakeMod();
        }
    }
}
