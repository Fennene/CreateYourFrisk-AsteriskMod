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

encountertext = ""
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}
playerskipdocommand = true

enemies = {
    "Null", "poseur_01"
}

enemypositions = {
    {0, 0}, {0, 0}
}

-- SYSTEM

_initialized = false

CreateLayer("ScreenMask", "Top", true)
_mask = CreateSprite("px", "ScreenMask")
_mask.color = {0, 0, 0}
_mask.Scale(640, 480)
_mask_anim_frame = 0
_mask_anim_frame_total = 0

_game_mode = 0
GameModeID = {
    Menu = 0,
    Room = 1,
    GameOver = 2
}

_player_position = {320, 240}
_player_direction = 0
_player_hp = 20
_player_exp = 0

function EncounterStarting()
    Audio.Stop()
    Player.name = "You"
    for i = 2, #enemies do
        enemies[i].Call("SetActive", false)
    end
    CreateLayer("RoomBottom", "ScreenMask", false)
    CreateLayer("RoomBackground", "RoomBottom", false)
    CreateLayer("Room", "RoomBackground", false)
    CreateLayer("RoomForeground", "Room", false)
    CreateLayer("RoomDialog", "RoomForeground", false)
    CreateLayer("RoomDialogText", "RoomDialog", false)
    CreateLayer("RoomEnc", "RoomDialogText", false)
    CreateLayer("RoomEncSoul", "RoomEnc", false)
    CreateLayer("RoomTop", "RoomEncSoul", false)
    SetFrameBasedMovement(true)
    PlayerUtil.SetHPControlOverride(true)
    _initialized = true
    customstatename = "TITLE"
    State("CUSTOMSTATE")
end

function SetMaskFadeOut(frame)
    _mask_anim_frame = 0
    _mask_anim_frame_total = -frame
end
function SetMaskFadeIn(frame)
    _mask_anim_frame = 0
    _mask_anim_frame_total = frame
end
function _UpdateCYFMask()
    if _mask_anim_frame_total == 0 then return end
    _mask_anim_frame = _mask_anim_frame + 1
    if _mask_anim_frame_total > 0 then
        _mask.alpha = _mask_anim_frame / _mask_anim_frame_total
    else
        _mask.alpha = 1 - (_mask_anim_frame / -_mask_anim_frame_total)
    end
    if _mask_anim_frame >= math.abs(_mask_anim_frame_total) then
        if _mask_anim_frame_total > 0 then
            _mask.alpha = 1
        else
            _mask.alpha = 0
        end
        _mask_anim_frame_total = 0
    end
end

function _Update()
    _UpdateCYFMask()
    if _game_mode ~= GameModeID.Battle then return end
    for i = 2, #enemies do
        if enemies[i].GetVar("isactive") then
            enemies[i].Call("Update")
        end
    end
end

function PrepareRoom()
    NewAudio.StopAll()
    _game_mode = GameModeID.Room
    customstatename = "ROOM"
    State("CUSTOMSTATE")
end

BattleID = ""
_mercy_menu_index = 1

function HandleItem(ItemID)
    --BattleDialog({"Selected item " .. ItemID .. "."})
end

function _GetMercyMenuInfo()
    local any_can_spare = false
    for i = 2, #enemies do
        if enemies[i].GetVar("isactive") and enemies[i].GetVar("canspare") then
            any_can_spare = true
        end
    end
    local flee_exists = flee
    if flee == nil then flee_exists = true end
    return {flee_exists, any_can_spare}
end

function _Win()
    enemies[1].Call("SetActive", true)
    customstatename = "FAKE_BATTLEWIN"
    State("CUSTOMSTATE")
end

function _CheckVictory()
    for i = 2, #enemies do
        if enemies[i].GetVar("isactive") then
            return
        end
    end
    _Win()
end

function _Spare()
    local any_enemies_exist = false
    for i = 2, #enemies do
        if enemies[i].GetVar("isactive") then
            if enemies[i].GetVar("canspare") then
                enemies[i].Call("OnSpare")
            else
                any_enemies_exist = true
            end
        end
    end
    if any_enemies_exist then return end
    _Win()
end

function _Flee()
    local num = 50
    if fleesuccess == nil then
    elseif type(fleesuccess) == "boolean" then
        if fleesuccess then
            num = 100
        else
            num = 0
        end
    elseif type(fleesuccess) == "number" then
        num = math.max(0, math.min(fleesuccess, 100))
    end
    local result = math.random(1, 100) <= num
    if result then
        customstatename = "FAKE_BATTLEFLEE"
        State("CUSTOMSTATE")
    else
        Audio.PlaySound("menuconfirm")
        HandleFlee()
    end
end

function _CheckEarned()
    local exp = 0
    local gold = 0
    for i = 2, #enemies do
        if enemies[i].GetVar("_Active") then
            if enemies[i].GetVar("_Killed") then
                exp = exp + enemies[i].GetVar("KilledEXP")
                gold = gold + enemies[i].GetVar("KilledGold")
            end
            if enemies[i].GetVar("_Spared") then
                gold = gold + enemies[i].GetVar("SparedGold")
            end
        end
    end
    return {exp, gold}
end

function _EnteringState(newState, oldState)
    if oldState == "ATTACKING" then
        _CheckVictory()
    end
    if newState == "ITEMMENU" then
    end
    if newState == "MERCYMENU" then
        customstatename = "FAKE_MERCYMENU"
        State("CUSTOMSTATE")
        return true
    end
    return false
end

function PrepareBattle(battleID)
    _mercy_menu_index = 1
    for i = 2, #enemies do
        enemies[i].Call("Deactivate")
    end
    Player.ResetStats()
    BattleID = battleID
    _game_mode = GameModeID.Battle
    BattleStarting()
    enemies[1].Call("SetActive", false)
    SetMaskFadeOut(16)
    State("ACTIONSELECT")
end

-- battles

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

function BattleStarting()
    if BattleID == "Poseur" then
        Audio.LoadFile("mus_battle1")
        encountertext = "Poseur strikes a pose!"
        nextwaves = {"bullettest_chaserorb"}
        wavetimer = 4.0
        arenasize = {155, 130}
        enemies[2].Call("SetActive", true)
        flee = true
        fleesuccess = nil
    end
end

function Update()
    _Update() -- don't delete
    if _game_mode ~= GameModeID.Battle then return end -- don't delete
    if Input.GetKey("F6") == 1 then
        Player.atk = 999
    end
end

function EnteringState(newState, oldState)
    if _EnteringState(newState, oldState) then return end -- don't delete
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
    if BattleID == "Poseur" then
        nextwaves = { possible_attacks[math.random(#possible_attacks)] }
    end
end

function DefenseEnding()
    if BattleID == "Poseur" then
        encountertext = RandomEncounterText()
    end
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleFlee()
    State("ENEMYDIALOGUE")
end
