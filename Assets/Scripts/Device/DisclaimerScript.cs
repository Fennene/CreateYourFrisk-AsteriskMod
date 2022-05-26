using AsteriskMod;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Attached to the disclaimer screen so you can skip it.
/// </summary>
public class DisclaimerScript : MonoBehaviour {
    public GameObject Logo, LogoCrate, Desc1, Desc2, Desc3, Desc4, Desc5, Version;
    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    public GameObject DescA1, DescA2, DescA3, DescA4, DescA5;
    public GameObject Load1, Load2;
    // --------------------------------------------------------------------------------

    private void Start() {
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        int _ = Random.Range(0, 1000);
        // --------------------------------------------------------------------------------
        if (GlobalControls.crate) {
            Logo.GetComponent<Image>().enabled = false;
            LogoCrate.GetComponent<Image>().enabled = true;
            Desc1.GetComponent<Text>().text = "GO TO /R/UNITLAE. FOR UPDTAES!!!!!";
            Desc2.GetComponent<Text>().text = "NO RELESLING HERE!!! IT'S RFEE!!! OR TUBY FEX WILL BE ANGER!!! U'LL HVAE A BED TMIE!!!";
            Desc3.GetComponent<Text>().text = "SPACE OR KLIK TO\n<color='#ff0000'>PALY MODS!!!!!</color>";
            Desc4.GetComponent<Text>().text = "PRSES O TO\n<color='#ffff00'>OOVERWURL!!!!!</color>";
            Desc5.GetComponent<Text>().text = "<b><color='red'>KNOW YUOR CODE</color> R U'LL HVAE A BED TMIE!!!</b>";
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            //Version.GetComponent<Text>().text = "v" + Random.Range(0, 9) + "." + Random.Range(0, 9) + "." + Random.Range(0, 9);
            Version.GetComponent<Text>().text = "v" + Random.Range(0, 9) + "." + Random.Range(0, 9) + "." + Random.Range(0, 9) + " *v" + Random.Range(0, 9) + "." + Random.Range(0, 9) + "." + Random.Range(0, 9) + "." + Random.Range(0, 9);
            DescA3.GetComponent<Text>().text = "<b><color='red'>KNOW YUOR CODE</color> R U'LL HVAE A BED TMIE!!!</b>";
            DescA4.GetComponent<Text>().text = "GO TO /R/UNITLAE. FOR UPDTAES!!!!!";
            DescA5.GetComponent<Text>().text = "NO RELESLING HERE!!! IT'S RFEE!!! OR TUBY FEX WILL BE ANGER!!! U'LL HVAE A BED TMIE!!!";
            // --------------------------------------------------------------------------------
        }
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        //else if (Random.Range(0, 1000) == 021) {
        else if (_ == 021) {
        // --------------------------------------------------------------------------------
            Logo.GetComponent<Image>().enabled              = false;
            Version.GetComponent<Transform>().localPosition = new Vector3(0f, 160f, 0f);
            Version.GetComponent<Text>().color              = new Color(1f, 1f, 1f, 1f);
            Version.GetComponent<Text>().text               = "Not Unitale v0.2.1a";
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        } else if (_ == 256) {
            Logo.GetComponent<Image>().enabled              = false;
            Version.GetComponent<Transform>().localPosition = new Vector3(0f, 160f, 0f);
            Version.GetComponent<Text>().color              = new Color(1f, 1f, 1f, 1f);
            Version.GetComponent<Text>().text               = "Not Official CYF";
        // --------------------------------------------------------------------------------
        } else {
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            //Version.GetComponent<Text>().text = "v" + GlobalControls.CYFversion;
            Version.GetComponent<Text>().text = "v" + GlobalControls.CYFversion + " *v" + Asterisk.ModVersion;
            // --------------------------------------------------------------------------------
        }
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        AsteriskEngine.SafeInitialize();
        // --------------------------------------------------------------------------------
    }

    /// <summary>
    /// Checks if you pressed one of the things the disclaimer tells you to. It's pretty straightforward.
    /// </summary>
    private void Update() {
        // Try to hook on to the game window when the user interacts
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.F4)
             || Input.GetKeyDown(KeyCode.Return) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))  // LAlt/RAlt + Enter
                Misc.RetargetWindow();
        #endif

        if (!ScreenResolution.hasInitialized) return;
        if (Input.GetKeyDown(KeyCode.O)) {
            StaticInits.MODFOLDER   = StaticInits.EDITOR_MODFOLDER;
            StaticInits.Initialized = false;
            StaticInits.InitAll();
            GlobalControls.modDev = false;
            SceneManager.LoadScene("Intro");
            Destroy(this);
        } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            StartCoroutine(ModSelect());
    }

    // The mod select screen can take some extra time to load,
    // because it now searches for encounter files on top of mods.
    // To compensate, this function will add "Loading" text to the Disclaimer screen
    // whenever it's time to go to the mod select menu.
    private IEnumerator ModSelect() {
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        //Desc5.GetComponent<Text>().text = GlobalControls.crate ? "LAODING MODS!!!!!" : "Loading mods...";
        if (GlobalControls.crate)
        {
            Desc5.GetComponent<Text>().text = GlobalControls.crate ? "LAODING MODS!!!!!" : "Loading mods...";
            DescA1.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Random.Range(200, 620), Random.Range(4, 40) + Random.Range(0, 20) + Random.Range(-10, 10));
            DescA2.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Random.Range(200, 620), Random.Range(4, 40) + Random.Range(0, 20) + Random.Range(-10, 10));
        }
        else
        {
            DescA1.GetComponent<Image>().enabled = false;
            DescA2.GetComponent<Image>().enabled = false;
            Load1.GetComponent<Image>().enabled = true;
        }
        // --------------------------------------------------------------------------------
        yield return new WaitForEndOfFrame();
        GlobalControls.modDev = true;
        DiscordControls.StartModSelect(false);
        SceneManager.LoadScene("ModSelect");
    }
}