using UnityEngine;

namespace AsteriskMod
{
    public class StaticTextManagerTest : MonoBehaviour
    {
        private StaticTextManager textMan;

        private void Start()
        {
            textMan = GetComponent<StaticTextManager>();
            //textMan.enabled = true;
            textMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_DEFAULT_NAME));
            //textMan.SetTextTest(new InstantTextMessage("This is test text."));
            textMan.SetText(new InstantTextMessage("[instant]This is test text."));
        }
    }
}
