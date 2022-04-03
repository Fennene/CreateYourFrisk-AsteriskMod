-- You should check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end
-- checks that the player enable Exprimental Features option.
if not AsteriskExperiment then
    error("You should enable Experimental Features in option.")
end

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}
flee = false
unescape = true

enemies = {
    "Null", "poseur_01"
}

enemypositions = {
    {0, 0}, {0, 0}
}

CreateLayer("ScreenMask", "Top", true)
_mask = CreateSprite("px", "ScreenMask")
_mask.color = {0, 0, 0}
_mask.Scale(640, 480)

function Audio_TryPlay(audioName, loop)
    if not Misc.FileExists("Audio/" .. audioName .. ".ogg")
       and audioName ~= "mus_battle1"
       and audioName ~= "mus_battle1 fj7x"
       and audioName ~= "mus_gameover" then
        return false
    end
    if loop == nil then loop = true end
    NewAudio.PlayMusic("src", audioName, loop)
end

_initialized = false
_battles = {}
_savedatas = {}
_nowbattle = false

function Load()
    _savedatas = {}
    for i = 1, #_battles do
        _savedatas[i] = 0
    end
    if not Misc.FileExists("save") then return end
    local filebytes = Misc.OpenFile("save", "r").ReadBytes()
    for i = 1, math.min(#_savedatas, #filebytes) do
        _savedatas[i] = filebytes
    end
end

function Save()
    Misc.OpenFile("save", "w").WriteBytes(_savedatas)
end

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

function EncounterStarting()
    Player.name = string.upper("Nil256")
    Audio.Stop()
    CreateLayer("Menu", "ScreenMask", false)

    for i = 2, #enemies do
        enemies[i].Call("SetActive", false)
    end

    _battles = Misc.ListDir("Sprites/Icons", true)
    Load()

    _initialized = true
    customstatename = "TITLE"
    State("CUSTOMSTATE")
end

turn = 0
BattleData = {}
hasUpdate = false

function _LoadBattle(selection)
    encountertext = "Poseur strikes a pose!"
    nextwaves = {"bullettest_chaserorb"}
    wavetimer = 4.0
    arenasize = {155, 130}
    turn = 0
    BattleData = require("Battles/" .. _battles[selection])
    hasUpdate = BattleData.Update ~= nil
    if BattleData.TargetEnemy == 1 then
        error("TargetEnemy shouldn't be 1")
    end
    enemies[BattleData.TargetEnemy].Call("SetActive", true)
    enemies[1].Call("SetActive", false)
    if BattleData.Audio ~= nil then
        Audio_TryPlay(BattleData.Audio, true)
    end
    _mask.alpha = 0
    _nowbattle = true
    BattleData.EncounterStarting()
end

function _CloseBattle()
    Audio.Stop()
    _mask.alpha = 1
    enemies[1].Call("SetActive", true)
    enemies[BattleData.TargetEnemy].Call("SetActive", false)
    _nowbattle = false
    customstatename = "BATTLESELECT"
    State("CUSTOMSTATE")
end

function Update()
    if hasUpdate then
        BattleData.Update()
    end
    if Input.GetKey("Escape") == 1 then
        if _nowbattle then
            _CloseBattle()
        else
            State("DONE")
        end
    end
end

function EnteringState(newState, oldState)
    if oldState == "MERCYMENU" then
        State("ACTIONSELECT")
    elseif oldState == "DIALOGRESULT" then
        State("ACTIONSELECT")
    else
        turn = turn + 1
    end
end

function EnemyDialogueStarting()
    BattleData.EnemyDialogueStarting()
end

function EnemyDialogueEnding()
    BattleData.EnemyDialogueEnding()
end

function DefenseEnding()
    BattleData.DefenseEnding()
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    BattleDialog({"Selected item " .. ItemID .. "."})
end