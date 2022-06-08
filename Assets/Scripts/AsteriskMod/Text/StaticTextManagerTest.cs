using UnityEngine;

namespace AsteriskMod
{
    [ToDo("Delete")]
    public class StaticTextManagerTest : MonoBehaviour
    {
        private StaticTextManager textMan;

        private void Start()
        {
            textMan = GetComponent<StaticTextManager>();
            //textMan.enabled = true;
            textMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_DEFAULT_NAME));
            textMan.SetText(new InstantTextMessage("This is test text."));
        }
    }
}
