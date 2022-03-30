using AsteriskMod;
using MoonSharp.Interpreter;
using UnityEngine;

public class UIStats : MonoBehaviour {
    public static UIStats instance;

    private GameObject nameLevelTextManParent;
    private TextManager nameLevelTextMan;
    private GameObject hpTextManParent;
    private TextManager hpTextMan;
    private LifeBarController lifebar;
    private RectTransform lifebarRt;
    private GameObject hpRect;

    private bool initialized;

    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    private bool encounterHasOnHPChanged;
    // --------------------------------------------------------------------------------

    private void Awake() { instance = this; }

    private void Start() {
        lifebar = gameObject.GetComponentInChildren<LifeBarController>();
        lifebarRt = lifebar.GetComponent<RectTransform>();

        nameLevelTextManParent = GameObject.Find("NameLv");
        nameLevelTextManParent.transform.position = new Vector3(nameLevelTextManParent.transform.position.x, nameLevelTextManParent.transform.position.y - 1, nameLevelTextManParent.transform.position.z);
        hpTextManParent = GameObject.Find("HPTextParent");
        hpTextManParent.transform.position = new Vector3(hpTextManParent.transform.position.x, hpTextManParent.transform.position.y - 1, hpTextManParent.transform.position.z);

        nameLevelTextMan = nameLevelTextManParent.AddComponent<TextManager>();
        hpTextMan = hpTextManParent.AddComponent<TextManager>();
        hpRect = GameObject.Find("HPRect");

        hpTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
        initialized = true;
        setMaxHP();
        setPlayerInfo(PlayerCharacter.instance.Name, PlayerCharacter.instance.LV);

        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        encounterHasOnHPChanged = EnemyEncounter.script.GetVar("OnHPChanged") != null;
        // --------------------------------------------------------------------------------
    }

    public void setPlayerInfo(string newName, int newLv) {
        if (!initialized) return;
        nameLevelTextMan.enabled = true;
        nameLevelTextMan.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_SMALLTEXT_NAME));
        nameLevelTextMan.SetText(new TextMessage(newName.ToUpper() + "  LV " + newLv, false, true));
        hpRect.transform.position = new Vector3(hpRect.transform.parent.position.x + (PlayerCharacter.instance.Name.Length > 6 ? 286.1f : 215.1f), hpRect.transform.position.y, hpRect.transform.position.z);

        nameLevelTextMan.enabled = false;
    }

    public void setHP(float hpCurrent) {
        if (!initialized) return;
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        if (PlayerUIManager.Instance.hpbarControlOverride) return;
        // --------------------------------------------------------------------------------
        float hpMax  = PlayerCharacter.instance.MaxHP,
              hpFrac = hpCurrent / hpMax;
        lifebar.setInstant(hpFrac);
        int    count      = UnitaleUtil.DecimalCount(hpCurrent);
        string sHpCurrent = hpCurrent < 10 ? "0" + hpCurrent.ToString("F" + count) : hpCurrent.ToString("F" + count);
        string sHpMax     = hpMax     < 10 ? "0" + hpMax : "" + hpMax;
        hpTextMan.SetText(new TextMessage(sHpCurrent + " / " + sHpMax, false, true));

        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        if (encounterHasOnHPChanged && Asterisk.experimentMode)
            UIController.instance.encounter.TryCall("OnHPChanged");
        // --------------------------------------------------------------------------------
    }

    public void setMaxHP() {
        if (!initialized) return;
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        if (PlayerUIManager.Instance.hpbarControlOverride) return;
        // --------------------------------------------------------------------------------
        lifebarRt.sizeDelta = new Vector2(Mathf.Min(120, PlayerCharacter.instance.MaxHP * 1.2f), lifebarRt.sizeDelta.y);
        setHP(PlayerCharacter.instance.HP);
    }

    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    private uint requests = 0;
    private const byte REQUEST_SET_HP_TEXT = 1;
    private string request_set_hp_text_value;

    internal void setHPOverride(float hp, int maxhp, bool updateHPText)
    {
        if (!initialized) return;
        float hpMax = maxhp,
              hpFrac = hp / hpMax;
        lifebar.setInstantOverride(hpFrac, maxhp);
        if (!updateHPText) return;
        int count = UnitaleUtil.DecimalCount(hp);
        string sHpCurrent = hp < 10 ? "0" + hp.ToString("F" + count) : hp.ToString("F" + count);
        string sHpMax = hpMax < 10 ? "0" + hpMax : "" + hpMax;
        hpTextMan.SetText(new TextMessage(sHpCurrent + " / " + sHpMax, false, true));
    }

    internal void setMaxHPOverride(int maxhp)
    {
        if (!initialized) return;
        lifebarRt.sizeDelta = new Vector2(Mathf.Min(120, maxhp * 1.2f), lifebarRt.sizeDelta.y);
    }

    internal void setHPTextOverride(string hpText)
    {
        if (!initialized)
        {
            requests += REQUEST_SET_HP_TEXT;
            request_set_hp_text_value = hpText;
            return;
        }
        hpTextMan.SetText(new TextMessage(hpText, false, true));
    }

    /// <summary>
    /// Call methods called before initialize objects again.<br />
    /// 初期化前に呼ばれた関数を再度呼び出します。
    /// </summary>
    internal void Request()
    {
        if (requests == 0) return;
        if (requests % 2 == 1)
        {
            hpTextMan.SetText(new TextMessage(request_set_hp_text_value, false, true));
        }
    }
    // --------------------------------------------------------------------------------
}