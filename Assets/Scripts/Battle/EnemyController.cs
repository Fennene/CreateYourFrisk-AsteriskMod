using AsteriskMod;
using System;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {
    internal Sprite textBubbleSprite;

    internal Vector2 textBubblePos;

    protected UIController ui;

    public Vector2 DialogBubblePosition {
        get {
            Sprite diagBubbleSpr = SpriteRegistry.Get(DialogBubble);
            RectTransform t = GetComponent<RectTransform>();
            if (diagBubbleSpr.name.StartsWith("right"))        textBubblePos = new Vector2(t.rect.width + 5, (-t.rect.height + diagBubbleSpr.rect.height) / 2);
            else if (diagBubbleSpr.name.StartsWith("left"))    textBubblePos = new Vector2(-diagBubbleSpr.rect.width - 5, (-t.rect.height + diagBubbleSpr.rect.height) / 2);
            else if (diagBubbleSpr.name.StartsWith("top"))     textBubblePos = new Vector2((t.rect.width - diagBubbleSpr.rect.width) / 2, diagBubbleSpr.rect.height + 5);
            else if (diagBubbleSpr.name.StartsWith("bottom"))  textBubblePos = new Vector2((t.rect.width - diagBubbleSpr.rect.width) / 2, -t.rect.height - 5);
            else                                               textBubblePos = new Vector2(t.rect.width + 5, (t.rect.height - diagBubbleSpr.rect.height) / 2); // rightside default
            return textBubblePos;
        }
    }

    public void Handle(string command) {
        string cmd = command.ToUpper().Trim();
        if (CanCheck && cmd.Equals("CHECK"))  HandleCheck();
        else                                  HandleCustomCommand(cmd);
    }

    public virtual void HandleCheck() { ui.ActionDialogResult(new RegularMessage(Name.ToUpper() + " " + Attack + " ATK " + Defense + " DEF\n" + CheckData), UIController.UIState.ENEMYDIALOGUE); }

    public void doDamage(int damage) {
        int newHP = HP - damage;
        HP = newHP;
    }

    internal string scriptName;
    internal ScriptWrapper script;
    internal bool inFight = true; // if false, enemy will no longer be considered as an option in menus and such
    private string lastBubbleName;
    public int presetDmg = -1826643; // You'll not be able to deal exactly -1 826 643 dmg with this technique.
    public float xFightAnimShift = 0;
    public LuaSpriteController sprite;
    public float bubbleWidth = 0;
    public int index = -1;
    public Vector2[] offsets = { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                               //SliceAnimOffset    BubbleOffset       DamageUIOffset

    internal bool spared;
    internal bool killed;
    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    internal bool canSetActive = true;
    // --------------------------------------------------------------------------------
    public bool canMove;

    public string Name {
        get { return script.GetVar("name").String; }
        set { script.SetVar("name", DynValue.NewString(value)); }
    }

    public string[] ActCommands {
        get {
            DynValue actCmds = script.GetVar("commands");
            string[] tempActCmds;
            int add = 0;
            if (!CanCheck)
                tempActCmds = new string[actCmds.Table.Length];
            else {
                tempActCmds = new string[actCmds.Table.Length + 1];
                tempActCmds[0] = "Check"; // HACK: remove hardcoding of Check, but otherwise gets converted to Tuple? idk
                add = 1;
            }
            for (int i = add; i < actCmds.Table.Length + add; i++)
                tempActCmds[i] = actCmds.Table.Get(i - add + 1).String;
            return tempActCmds;
        }
        set {
            DynValue[] values = new DynValue[value.Length];
            for (int i = 0; i < value.Length; i++)
                values[i] = DynValue.NewString(value[i]);
            script.SetVar("commands", DynValue.NewTuple(values));
        }
    }

    public string[] Comments {
        get {
            DynValue comments = script.GetVar("comments");
            string[] tempComments = new string[comments.Table.Length];
            for (int i = 0; i < comments.Table.Length; i++)
                tempComments[i] = comments.Table.Get(i + 1).String;
            return tempComments;
        }
        set {
            DynValue[] values = new DynValue[value.Length];
            for (int i = 0; i < value.Length; i++)
                values[i] = DynValue.NewString(value[i]);
            script.SetVar("comments", DynValue.NewTuple(values));
        }
    }

    public string[] Dialogue {
        get {
            DynValue randDialogue = script.GetVar("randomdialogue");
            if (randDialogue == null || randDialogue.Table == null)
                return null;
            string[] tempDialogue = new string[randDialogue.Table.Length];
            for (int i = 0; i < randDialogue.Table.Length; i++)
                tempDialogue[i] = randDialogue.Table.Get(i + 1).String;
            return tempDialogue;
        }
        set {
            DynValue[] values = new DynValue[value.Length];
            for (int i = 0; i < value.Length; i++)
                values[i] = DynValue.NewString(value[i]);
            script.SetVar("randomdialogue", DynValue.NewTuple(values));
        }
    }

    public string CheckData {
        get { return script.GetVar("check").String; }
        set { script.SetVar("check", DynValue.NewString(value)); }
    }

    public int HP {
        get {
            if (GlobalControls.retroMode && (int)script.GetVar("hp").Number > MaxHP)
                MaxHP = (int)script.GetVar("hp").Number;
            return (int)script.GetVar("hp").Number;
        }
        set { script.SetVar("hp", DynValue.NewNumber(value)); }
    }

    public int MaxHP {
        get { return (int)script.GetVar("maxhp").Number; }
        set { script.SetVar("maxhp", DynValue.NewNumber(value)); }
    }

    public int Attack {
        get { return (int)script.GetVar("atk").Number; }
        set { script.SetVar("atk", DynValue.NewNumber(value)); }
    }

    public int Defense {
        get { return (int)script.GetVar("def").Number; }
        set { script.SetVar("def", DynValue.NewNumber(value)); }
    }

    public int XP {
        get { return (int)script.GetVar("xp").Number; }
        set { script.SetVar("xp", DynValue.NewNumber(value)); }
    }

    public int Gold {
        get { return (int)script.GetVar("gold").Number; }
        set { script.SetVar("gold", DynValue.NewNumber(value)); }
    }

    public bool CanSpare {
        get {
            DynValue spareVal = script.GetVar("canspare");
            if (spareVal == null)
                return false;
            return spareVal.Boolean;
        }
        set { script.SetVar("canspare", DynValue.NewBoolean(value)); }
    }

    public bool CanCheck {
        get {
            DynValue checkVal = script.GetVar("cancheck");
            if (checkVal == null)
                return true;
            return checkVal.Boolean;
        }
        set { script.SetVar("cancheck", DynValue.NewBoolean(value)); }
    }

    public bool Unkillable {
        get {
            DynValue checkVal = script.GetVar("unkillable");
            if (checkVal == null)
                return false;
            return checkVal.Boolean;
        }
        set { script.SetVar("unkillable", DynValue.NewBoolean(value)); }
    }

    public string DialogBubble {
        get {
            if (script.GetVar("dialogbubble") == null)
                return "UI/SpeechBubbles/right";

            return "UI/SpeechBubbles/" + script.GetVar("dialogbubble").String;
        }
    }

    public string DialoguePrefix {
        get {
            DynValue dialoguePrefix = script.GetVar("dialogueprefix");
            if (dialoguePrefix == null || dialoguePrefix.Type == DataType.Nil)
                return "[effect:rotate]";
            return dialoguePrefix.String;
        }
        set { script.SetVar("dialogueprefix", DynValue.NewString(value)); }
    }

    public string Font {
        get {
            DynValue fontVal = script.GetVar("font");
            if (fontVal == null || fontVal.Type == DataType.Nil)
                return SpriteFontRegistry.UI_MONSTERTEXT_NAME;
            return fontVal.String;
        }
        set { script.SetVar("font", DynValue.NewString(value)); }
    }

    public string Voice {
        get {
            DynValue voiceVal = script.GetVar("voice");
            if (voiceVal == null || voiceVal.Type == DataType.Nil)
                return "";
            return voiceVal.String;
        }
        set { script.SetVar("voice", DynValue.NewString(value)); }
    }

    public string DefenseMissText {
        get { return script.GetVar("defensemisstext").String; }
        set { script.SetVar("defensemisstext", DynValue.NewString(value)); }
    }

    public string NoAttackMissText {
        get { return script.GetVar("noattackmisstext").String; }
        set { script.SetVar("noattackmisstext", DynValue.NewString(value)); }
    }

    public float PosX {
        get { return GetComponent<RectTransform>().position.x; }
    }

    public float PosY {
        get { return GetComponent<RectTransform>().position.y; }
    }

    private void Start() {
        try {
            string scriptText = ScriptRegistry.Get(ScriptRegistry.MONSTER_PREFIX + scriptName);
            if (scriptText == null) {
                UnitaleUtil.DisplayLuaError(StaticInits.ENCOUNTER, "Tried to load monster script " + scriptName + ".lua but it didn't exist. Is it misspelled?");
                return;
            }
            script.scriptname = scriptName;
            script.Bind("SetSprite", (Action<string>)SetSprite);
            script.Bind("SetActive", (Action<bool>)SetActive);
            script.Bind("isactive", DynValue.NewBoolean(true));
            script.Bind("Kill", (Action)DoKill);
            script.Bind("Spare", (Action)DoSpare);
            script.Bind("Move", (Action<float, float>)Move);
            script.Bind("MoveTo", (Action<float, float>)MoveTo);
            script.Bind("BindToArena", (Action<bool, bool>)BindToArena);
            script.Bind("SetDamage", (Action<int>)SetDamage);
            script.Bind("SetBubbleOffset", (Action<int, int>)SetBubbleOffset);
            script.Bind("SetDamageUIOffset", (Action<int, int>)SetDamageUIOffset);
            script.Bind("SetSliceAnimOffset", (Action<int, int>)SetSliceAnimOffset);
            script.Bind("State", (Action<Script, string>)UIController.SwitchStateOnString);
            // --------------------------------------------------------------------------------
            //                          Asterisk Mod Modification
            // --------------------------------------------------------------------------------
            script.Bind("GetShakeX", (Func<int>)(() => { return nowShakeX; }));
            script.Bind("SetDamageText", (Action<string>)SetDamageText);
            // --------------------------------------------------------------------------------
            script.SetVar("canmove", DynValue.NewBoolean(false));
            sprite = new LuaSpriteController(GetComponent<Image>());
            script.SetVar("monstersprite", UserData.Create(sprite, LuaSpriteController.data));
            script.DoString(scriptText);

            string spriteFile = script.GetVar("sprite").String;
            if (spriteFile != null)
                SetSprite(spriteFile);
            else
                throw new CYFException("missing sprite");

            ui = FindObjectOfType<UIController>();
            if (MaxHP == 0)
                MaxHP = HP;

            textBubbleSprite = Resources.Load<Sprite>("Sprites/UI/SpeechBubbles/right");

            /*if (script.GetVar("canspare") == null) CanSpare = false;
            if (script.GetVar("cancheck") == null) CanCheck = true;*/
        }
        catch (InterpreterException ex) { UnitaleUtil.DisplayLuaError(scriptName, ex.DecoratedMessage != null ? UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message : ex.Message); }
        catch (Exception ex)            { UnitaleUtil.DisplayLuaError(scriptName, "Unknown error. Usually means you're missing a sprite.\nSee documentation for details.\nStacktrace below in case you wanna notify a dev.\n\nError: " + ex.Message + "\n\n" + ex.StackTrace); }
    }

    public void HandleAttack(int hitStatus) { TryCall("HandleAttack", new[] { DynValue.NewNumber(hitStatus) }); }

    public string[] GetDefenseDialog() {
        DynValue dialogues = script.GetVar("currentdialogue");
        if (dialogues == null)
            return null;
        if (dialogues.Table == null)
            if (dialogues.String != null)  return new[] { dialogues.String };
            else if (Dialogue == null)     return null;
            else                           return new[] { Dialogue[Random.Range(0, Dialogue.Length)] };

        string[] dialogueStrings = new string[dialogues.Table.Length];
        for (int i = 0; i < dialogues.Table.Length; i++)
            dialogueStrings[i] = dialogues.Table.Get(i + 1).String;
        script.SetVar("currentdialogue", DynValue.NewNil());
        return dialogueStrings;
    }

    public bool TryCall(string func, DynValue[] param = null) {
        try {
            DynValue sval = script.GetVar(func);
            if (sval == null || sval.Type == DataType.Nil) return false;
            if (param != null)                             script.Call(func, param);
            else                                           script.Call(func);
            return true;
        }
        catch (InterpreterException ex) { UnitaleUtil.DisplayLuaError(scriptName, UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message); }
        return true;
    }

    protected void HandleCustomCommand(string command) {
        TryCall("HandleCustomCommand", new[] { DynValue.NewString(command) });
    }

    public void SetSprite(string filename) {
        if (filename == null)
            throw new CYFException("The enemy's sprite can't be nil!");
        SpriteUtil.SwapSpriteFromFile(this, filename);
    }

    /// <summary>
    /// Call function to grey out enemy and pop the smoke particles, and mark it as spared.
    /// </summary>
    public void DoSpare() {
        if (!inFight)
            return;
        UIController.instance.gold += Gold;
        // We have to code the particles separately because they don't work well in UI screenspace. Great stuff.
        ParticleSystem spareSmoke = Instantiate(Resources.Load<ParticleSystem>("Prefabs/MonsterSpareParticleSys"));
        spareSmoke.Emit(10);
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[10];
        spareSmoke.GetParticles(particles);
        Vector3 particlePos = RTUtil.AbsCenterOf(GetComponent<RectTransform>());
        particlePos.z = 5;
        for (int i = 0; i < particles.Length; i++)
            particles[i].position = particlePos + particles[i].velocity.normalized * 5;
        spareSmoke.SetParticles(particles, particles.Length);
        spareSmoke.gameObject.transform.SetParent(UIController.instance.psContainer.transform);

        // The actually relevant part of sparing code.
        GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
        UIController.PlaySoundSeparate(AudioClipRegistry.GetSound("enemydust"));
        SetActive(false);
        spared = true;

        UIController.instance.CheckAndTriggerVictory();
    }

    /// <summary>
    /// Call function to turn enemy to dust and mark it as killed.
    /// </summary>
    public void DoKill() {
        if (!inFight)
            return;
        UIController.instance.gold += Gold;
        UIController.instance.exp += XP;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/MonsterDuster"));
        go.transform.SetParent(UIController.instance.psContainer.transform);
        GetComponent<ParticleDuplicator>().Activate(sprite);
        SetActive(false);
        killed = true;
        UIController.PlaySoundSeparate(AudioClipRegistry.GetSound("enemydust"));

        UIController.instance.CheckAndTriggerVictory();
    }

    /// <summary>
    /// Set if we should consider this monster for menus e.g.
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active) {
        inFight = active;
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        if (Asterisk.RequireExperimentalFeature("OnActive", false))
        {
            if (!canSetActive)
            {
                UnitaleUtil.DisplayLuaError(scriptName, "Do NOT call SetActive() in OnActive!");
                return;
            }
            try
            {
                if (script.GetVar("OnActive") != null)
                {
                    canSetActive = false;
                    script.Call("OnActive", new[] { DynValue.NewBoolean(active) });
                    canSetActive = true;
                }
            }
            catch (InterpreterException ex)
            {
                UnitaleUtil.DisplayLuaError(scriptName, UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message);
            }
        }
        // --------------------------------------------------------------------------------
        script.SetVar("isactive", DynValue.NewBoolean(active));
    }

    public void Move(float x, float y) {
        if (!canMove)
            return;
        GetComponent<RectTransform>().position = new Vector2(GetComponent<RectTransform>().position.x + x, GetComponent<RectTransform>().position.y + y);
    }

    public void MoveTo(float x, float y) {
        if (!canMove)
            return;
        GetComponent<RectTransform>().position = new Vector2(x, y);
    }

    public void BindToArena(bool bind, bool isUnderArena = false) {
        int count = 0;
        if (bind) {
            count += FindObjectsOfType<EnemyController>().Count(luaec => luaec.transform.parent.name == "LuaEnemyEncounterGO" && luaec.index < index);
            transform.SetParent(GameObject.Find("LuaEnemyEncounterGO").transform, true);
        } else {
            count += FindObjectsOfType<EnemyController>().Count(luaec => luaec.transform.parent.name == "arena_container" && luaec.index <= index &&
                                                                            (isUnderArena && luaec.transform.GetSiblingIndex() < GameObject.Find("arena_border_outer").transform.GetSiblingIndex() || !isUnderArena));
            if (!isUnderArena) count++;
            transform.SetParent(GameObject.Find("arena_container").transform, true);
        }
        transform.SetSiblingIndex(count);
    }

    public void SetDamage(int dmg) { presetDmg = dmg; }

    public void Update() {
        try {
            script.SetVar("posx", DynValue.NewNumber(GetComponent<RectTransform>().position.x));
            script.SetVar("posy", DynValue.NewNumber(GetComponent<RectTransform>().position.y));
        } catch { /* ignored */ }

        if (ArenaManager.instance.firstTurn || canMove) return;
        canMove = true;
        script.SetVar("canmove", DynValue.NewBoolean(true));
    }

    public void SetSliceAnimOffset(int x, int y) { offsets[0] = new Vector2(x, y); }

    public void SetBubbleOffset(int x, int y) { offsets[1] = new Vector2(x, y); }

    public void SetDamageUIOffset(int x, int y) { offsets[2] = new Vector2(x, y); }

    // --------------------------------------------------------------------------------
    //                          Asterisk Mod Modification
    // --------------------------------------------------------------------------------
    //private const string VAR_NAME_DAMAGE_TEXT_VISIBLE = "showdamagetext";

    internal int nowShakeX = 0;
    internal Action<string> _SetDamageText = (_ => { });
    private void SetDamageText(string damageText) { _SetDamageText(damageText);  }

    private Color32 ConvertToColor(DynValue colorValue, DynValue alphaValue, byte init_r, byte init_g, byte init_b, byte init_a = 255)
    {
        int[] color = new int[4] { init_r, init_g, init_b, init_a };
        if (colorValue != null && (colorValue.Type == DataType.Table || colorValue.Type == DataType.Tuple) && colorValue.Table.Length >= 3)
        {
            for (var i = 0; i < 3; i++)
            {
                color[i] = (int)colorValue.Table.Get(i + 1).Number;
                if (color[i] < 0) color[i] = 0;
                if (color[i] > 255) color[i] = 255;
            }
        }
        if (alphaValue != null && alphaValue.Type == DataType.Number)
        {
            color[3] = (int)alphaValue.Number;
            if (color[3] < 0) color[3] = 0;
            if (color[3] > 255) color[3] = 255;
        }
        return new Color32((byte)color[0], (byte)color[1], (byte)color[2], (byte)color[3]);
    }

    public Color BGBarColorInFIGHT
    {
        get
        {
            DynValue dynColor = script.GetVar(MonsterVar.FIGHT_MENU_BG_BAR_COLOR);
            DynValue dynAlpha = script.GetVar(MonsterVar.FIGHT_MENU_BG_BAR_ALPHA);
            /*
            int[] color = new int[3] { 255, 0, 0 };
            int alpha = 255;
            if (dynColor != null && (dynColor.Type == DataType.Table || dynColor.Type == DataType.Tuple) && dynColor.Table.Length >= 3)
            {
                for (var i = 0; i < 3; i++)
                {
                    color[i] = (int)dynColor.Table.Get(i + 1).Number;
                    if (color[i] < 0) color[i] = 0;
                    if (color[i] > 255) color[i] = 255;
                }
            }
            if (dynAlpha != null && dynAlpha.Type == DataType.Number)
            {
                alpha = (int)dynAlpha.Number;
                if (alpha < 0) alpha = 0;
                if (alpha > 255) alpha = 255;
            }
            return new Color32((byte)color[0], (byte)color[1], (byte)color[2], (byte)alpha);
            */
            return ConvertToColor(dynColor, dynAlpha, 255, 0, 0);
        }
        set
        {
            Color32 _ = value;
            DynValue[] values = new DynValue[3] { DynValue.NewNumber(_.r), DynValue.NewNumber(_.g), DynValue.NewNumber(_.b) };
            script.SetVar(MonsterVar.FIGHT_MENU_BG_BAR_COLOR, DynValue.NewTuple(values));
            script.SetVar(MonsterVar.FIGHT_MENU_BG_BAR_ALPHA, DynValue.NewNumber(_.a));
        }
    }

    public Color FillBarColorInFIGHT
    {
        get
        {
            DynValue dynColor = script.GetVar(MonsterVar.FIGHT_MENU_FILL_BAR_COLOR);
            DynValue dynAlpha = script.GetVar(MonsterVar.FIGHT_MENU_FILL_BAR_ALPHA);
            return ConvertToColor(dynColor, dynAlpha, 0, 255, 0);
        }
        set
        {
            Color32 _ = value;
            DynValue[] values = new DynValue[3] { DynValue.NewNumber(_.r), DynValue.NewNumber(_.g), DynValue.NewNumber(_.b) };
            script.SetVar(MonsterVar.FIGHT_MENU_FILL_BAR_COLOR, DynValue.NewTuple(values));
            script.SetVar(MonsterVar.FIGHT_MENU_FILL_BAR_ALPHA, DynValue.NewNumber(_.a));
        }
    }

    public Color BGBarColor
    {
        get
        {
            DynValue dynColor = script.GetVar(MonsterVar.FIGHT_UI_BG_BAR_COLOR);
            return ConvertToColor(dynColor, null, 64, 64, 64);
        }
        set
        {
            Color32 _ = value;
            DynValue[] values = new DynValue[3] { DynValue.NewNumber(_.r), DynValue.NewNumber(_.g), DynValue.NewNumber(_.b) };
            script.SetVar(MonsterVar.FIGHT_UI_BG_BAR_COLOR, DynValue.NewTuple(values));
        }
    }

    public Color FillBarColor
    {
        get
        {
            DynValue dynColor = script.GetVar(MonsterVar.FIGHT_UI_FILL_BAR_COLOR);
            return ConvertToColor(dynColor, null, 0, 255, 0);
        }
        set
        {
            Color32 _ = value;
            DynValue[] values = new DynValue[3] { DynValue.NewNumber(_.r), DynValue.NewNumber(_.g), DynValue.NewNumber(_.b) };
            script.SetVar(MonsterVar.FIGHT_UI_FILL_BAR_COLOR, DynValue.NewTuple(values));
        }
    }

    public bool ShowHPBar
    {
        get
        {
            DynValue value = script.GetVar(MonsterVar.FIGHT_UI_BARS_VISIBILITY);
            return (value != null && value.Type == DataType.Boolean) ? value.Boolean : true;
        }
        set
        {
            script.SetVar(MonsterVar.FIGHT_UI_BARS_VISIBILITY, DynValue.NewBoolean(value));
        }
    }

    public string ForcedDamageText
    {
        get
        {
            DynValue value = script.GetVar(MonsterVar.FIGHT_UI_FORCIBLY_CHANGE_DAMAGE_TEXT);
            return (value != null && value.Type == DataType.String) ? value.String : null;
        }
        set
        {
            script.SetVar(MonsterVar.FIGHT_UI_FORCIBLY_CHANGE_DAMAGE_TEXT, value == null ? DynValue.NewNil() : DynValue.NewString(value));
        }
    }

    public bool Shake
    {
        get
        {
            DynValue value = script.GetVar(MonsterVar.FIGHT_UI_NO_SHAKE);
            if (value == null || value.Type != DataType.Boolean) return true;
            return !value.Boolean;
        }
        set
        {
            script.SetVar(MonsterVar.FIGHT_UI_NO_SHAKE, DynValue.NewBoolean(!value));
        }
    }
    // --------------------------------------------------------------------------------
}