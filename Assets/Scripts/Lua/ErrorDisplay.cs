using AsteriskMod;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to contain a Lua error message, which is displayed as soon as the Error scene loads.
/// </summary>
public class ErrorDisplay : MonoBehaviour {
    public static string Message;

    private void Start() {
        if (GameObject.Find("Main Camera OW")) {
            Destroy(GameObject.Find("Main Camera OW"));
            Destroy(GameObject.Find("Canvas OW"));
            Destroy(GameObject.Find("Canvas Two"));
            Destroy(GameObject.Find("Player"));
        }
        UnitaleUtil.firstErrorShown = false;
        string mess = !GlobalControls.modDev ? "restart CYF" : "reload";
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        //GetComponent<Text>().text = Message + "\n\nPress ESC to " + mess;
        if (Asterisk.language == Languages.Japanese)
        {
            mess = !GlobalControls.modDev ? "CYFの再起動" : "リロード";
            GetComponent<Text>().text = Message + "\n\nESCキーで" + mess + "、\nCキーで上記のエラーメッセージをクリップボードにコピーします。";
        }
        else
        {
            GetComponent<Text>().text = Message + "\n\nPress ESC to " + mess + "\nPress C to copy above error messages to clipboard.";
        }
        // --------------------------------------------------------------------------------
    }

    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GUIUtility.systemCopyBuffer = Message;
        }
    }
    // --------------------------------------------------------------------------------
}
