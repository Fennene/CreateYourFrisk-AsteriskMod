using AsteriskMod;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour {
    public LuaSpriteController slice;
    public LifeBarController lifeBar;
    public TextManager damageText;
    private RectTransform damageTextRt;

    public bool shakeInProgress;
    private int[] shakeX = { 12, -12, 7, -7, 3, -3, 1, -1, 0 };
    //private int[] shakeX = new int[] { 24, 0, 0, 0, 0, -48, 0, 0, 0, 0, 38, 0, 0, 0, 0, -28, 0, 0, 0, 0, 20, 0, 0, 0, 0, -12, 0, 0, 0, 0, 8, 0, 0, 0, 0, -2, 0, 0, 0, 0};
    private int shakeIndex = -1;
    private int Damage = -478294;
    private float shakeTimer;
    private float totalShakeTime = 1.5f;
    public float sliceAnimFrequency = 1 / 6f;
    public EnemyController enemy;
    public Vector2 enePos, eneSize;
    private string[] sliceAnim = {
        "UI/Battle/spr_slice_o_0",
        "UI/Battle/spr_slice_o_1",
        "UI/Battle/spr_slice_o_2",
        "UI/Battle/spr_slice_o_3",
        "UI/Battle/spr_slice_o_4",
        "UI/Battle/spr_slice_o_5"
    };
    private bool wait1frame; //Hacky way to wait one frame before launching the lifebars anim
    private bool needAgain;
    private bool showedup;
    public bool stopped;
    public bool isCoroutine;
    public bool waitingToFade;
    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    private bool doShake;
    // --------------------------------------------------------------------------------

    public void Start() {
        foreach(RectTransform child in transform) {
            switch (child.name) {
                case "SliceAnim": slice = new LuaSpriteController(child.GetComponent<Image>()); break;
                case "HPBar":    lifeBar = child.GetComponent<LifeBarController>(); break;
                case "DamageNumber":
                    damageText = child.GetComponent<TextManager>();
                    damageTextRt = child.GetComponent<RectTransform>();
                    break;
            }
        }
        sliceAnim = UIController.instance.fightUI.sliceAnim;
        sliceAnimFrequency = UIController.instance.fightUI.sliceAnimFrequency;
        shakeX = UIController.instance.fightUI.shakeX;
        damageText.SetFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_DAMAGETEXT_NAME));
        damageText.SetMute(true);
    }

    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    internal void SetDamageText(string text)
    {
        if (shakeInProgress)
        {
            damageText.SetText(new TextMessage(text, false, true));
            Vector3 currentPosition = damageTextRt.localPosition;
            damageTextRt.localPosition = new Vector3(-UnitaleUtil.CalcTextWidth(damageText) / 2 + enemy.offsets[2].x, currentPosition.y);
        }
    }
    // --------------------------------------------------------------------------------

    public void Init(int enemyIndex) {
        Start();
        Damage = -478294;
        lifeBar.setVisible(false);
        lifeBar.whenDamage = true;
        enemy = UIController.instance.encounter.EnabledEnemies[enemyIndex];
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        enemy._SetDamageText = SetDamageText;
        // --------------------------------------------------------------------------------
        lifeBar.transform.SetParent(enemy.transform);
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        lifeBar.setBackgroundColor(enemy.BGBarColor);
        lifeBar.setFillColor(enemy.FillBarColor);
        // --------------------------------------------------------------------------------
        damageText.transform.SetParent(enemy.transform);
        slice.img.transform.SetParent(enemy.transform);
        enePos = enemy.GetComponent<RectTransform>().position;
        eneSize = enemy.GetComponent<RectTransform>().sizeDelta;
        shakeTimer = 0;
    }

    public void quickInit(int enemyIndex, EnemyController target, int damage = -478294) {
        Init(enemyIndex);
        enemy = target;
        if (damage != -478294)
            Damage = damage;
        shakeInProgress = false;
    }

    public bool Finished() {
        if (shakeTimer > 0)
            return shakeTimer >= totalShakeTime;
        return false;
    }

    public void ChangeTarget(EnemyController target) {
        enemy = target;
        if (Damage != -478294)
            Damage = 0;
        Damage = FightUIController.instance.getDamage(enemy, PlayerController.instance.lastHitMult);
        enePos = enemy.GetComponent<RectTransform>().position;
        eneSize = enemy.GetComponent<RectTransform>().sizeDelta;
        lifeBar.transform.SetParent(enemy.transform);
        damageText.transform.SetParent(enemy.transform);
        slice.img.transform.SetParent(enemy.transform);
        /*Vector3 slicePos = new Vector3(enemy.GetComponent<RectTransform>().position.x + enemy.offsets[0].x,
                                       enemy.GetComponent<RectTransform>().position.y + eneSize.y / 2 + enemy.offsets[0].y - 55, enemy.GetComponent<RectTransform>().position.z);*/
    }

    public void StopAction(float atkMult) {
        PlayerController.instance.lastHitMult = FightUIController.instance.getAtkMult();
        bool damagePredefined = Damage != -478294;
        stopped = true;
        enemy.TryCall("BeforeDamageCalculation");
        if (!damagePredefined)
            Damage = FightUIController.instance.getDamage(enemy, atkMult);
        UpdateSlicePos();
        //slice.StopAnimation();
        slice.SetAnimation(sliceAnim, sliceAnimFrequency);
        slice.loopmode = "ONESHOT";
    }

    // Update is called once per frame
    private void Update() {
        // do not update the attack UI if the ATTACKING state is frozen
        if (UIController.instance.frozenState != UIController.UIState.PAUSE)
            return;

        if (shakeInProgress) {
            int shakeidx = (int)Mathf.Floor(shakeTimer * shakeX.Length / totalShakeTime);
            if (Damage > 0 && shakeIndex != shakeidx) {
                if (shakeIndex != shakeidx && shakeIndex >= shakeX.Length)
                    shakeIndex = shakeX.Length - 1;
                shakeIndex = shakeidx;
                // --------------------------------------------------------------------------------
                //                          Asterisk Mod Modification
                // --------------------------------------------------------------------------------
                //Vector2 localEnePos = enemy.GetComponent<RectTransform>().anchoredPosition; // get local position to do the shake
                //enemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(localEnePos.x + shakeX[shakeIndex], localEnePos.y);
                if (doShake)
                {
                    Vector2 localEnePos = enemy.GetComponent<RectTransform>().anchoredPosition; // get local position to do the shake
                    enemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(localEnePos.x + shakeX[shakeIndex], localEnePos.y);
                }
                enemy.nowShakeX = shakeX[shakeIndex];
                // --------------------------------------------------------------------------------

                /*#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                    if (StaticInits.ENCOUNTER == "01 - Two monsters" && StaticInits.MODFOLDER == "Examples")
                        Misc.MoveWindow(shakeX[shakeIndex] * 2, 0);
                #endif*/
            }
            if (shakeTimer < 1.5f)
                damageTextRt.localPosition = new Vector2(damageTextRt.localPosition.x, enemy.offsets[2].y + 40 * (2 + Mathf.Sin(shakeTimer * Mathf.PI * 0.75f)));
            shakeTimer += Time.deltaTime;
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            //if (shakeTimer >= totalShakeTime)
            //    shakeInProgress = false;
            if (shakeTimer >= totalShakeTime)
            {
                enemy.nowShakeX = 0;
                shakeInProgress = false;
            }
            // --------------------------------------------------------------------------------
        }
        else if ((slice.animcomplete &&!slice.img.GetComponent<KeyframeCollection>().enabled && stopped &&!showedup) || needAgain) {
            needAgain = true;
            if (!wait1frame) {
                wait1frame = true;
                slice.StopAnimation();
                slice.Set("empty");
                enemy.TryCall("BeforeDamageValues", new[] { DynValue.NewNumber(Damage) });
                if (Damage > 0) {
                    AudioSource aSrc = GetComponent<AudioSource>();
                    aSrc.clip = AudioClipRegistry.GetSound("hitsound");
                    aSrc.Play();
                }
                // set damage numbers and positioning
                string damageTextStr;
                if (Damage == 0) {
                    if (enemy.DefenseMissText == null) damageTextStr = "[color:c0c0c0]MISS";
                    else                               damageTextStr = "[color:c0c0c0]" + enemy.DefenseMissText;
                }
                else if (Damage > 0) damageTextStr = "[color:ff0000]" + Damage;
                else damageTextStr = "[color:00ff00]" + Damage;
                // --------------------------------------------------------------------------------
                //                          Asterisk Mod Modification
                // --------------------------------------------------------------------------------
                if (enemy.ForcedDamageText != null) damageTextStr = enemy.ForcedDamageText;
                // --------------------------------------------------------------------------------
                damageTextRt.localPosition = new Vector3(0, 0, 0);
                damageText.SetText(new TextMessage(damageTextStr, false, true));
                damageTextRt.localPosition = new Vector3(-UnitaleUtil.CalcTextWidth(damageText)/2 + enemy.offsets[2].x, 40 + enemy.offsets[2].y);

                // initiate lifebar and set lerp to its new health value
                if (Damage != 0) {
                    int newHP = enemy.HP - Damage;
                    try {
                        lifeBar.GetComponent<RectTransform>().localPosition = new Vector2(enemy.offsets[2].x, 20 + enemy.offsets[2].y);
                        lifeBar.GetComponent<RectTransform>().sizeDelta = new Vector2(enemy.GetComponent<RectTransform>().rect.width, 13);
                        lifeBar.whenDamageValue = enemy.GetComponent<RectTransform>().rect.width;
                        lifeBar.setInstant(enemy.HP < 0 ? 0 : enemy.HP / (float)enemy.MaxHP);
                        lifeBar.setLerp(enemy.HP / (float)enemy.MaxHP, newHP / (float)enemy.MaxHP);
                        // --------------------------------------------------------------------------------
                        //                          Asterisk Mod Modification
                        // --------------------------------------------------------------------------------
                        //lifeBar.setVisible(true);
                        lifeBar.setVisible(enemy.ShowHPBar);
                        // --------------------------------------------------------------------------------
                        enemy.doDamage(Damage);
                    } catch { return; }
                }
                enemy.HandleAttack(Damage);
            } else {
                // finally, damage enemy and call its attack handler in case you wanna stop music on death or something
                shakeInProgress = true;
                waitingToFade = true;
                needAgain = false;
                totalShakeTime = shakeX.Length * (1.5f / 8.0f);
                showedup = true;
                // --------------------------------------------------------------------------------
                //                          Asterisk Mod Modification
                // --------------------------------------------------------------------------------
                doShake = enemy.Shake;
                // --------------------------------------------------------------------------------
            }
        } else if (!slice.animcomplete)
            slice.img.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(slice.img.GetComponent<Image>().sprite.rect.width, slice.img.GetComponent<Image>().sprite.rect.height);
    }

    Vector3 CalculateSlicePos() {
        return new Vector3(enemy.offsets[0].x, eneSize.y / 2 + enemy.offsets[0].y - 55, 0);
    }

    void UpdateSlicePos() {
        slice.img.GetComponent<RectTransform>().localPosition = CalculateSlicePos();
    }
}
