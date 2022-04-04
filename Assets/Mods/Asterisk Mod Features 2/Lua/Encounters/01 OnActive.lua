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

function PlayerHurt(amount, inv_time)
    if Player.ishurting then return end
    if amount < 0 then return end
    if inv_time == nil then inv_time = 1.7 end
    Audio.PlaySound("hurtsound")
    Player.Hurt(0, inv_time, true, false)
    _player_hp = math.max(0, _player_hp - amount)
end

function PlayerHeal(amount)
    if amount < 0 then return end
    Audio.PlaySound("healsound")
    _player_hp = math.min(_player_hp + amount, Player.maxhp)
end

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
    Inventory.AddCustomItems({"[color:ff0000]Error!"}, {3})
    Inventory.SetInventory({"[color:ff0000]Error!"})
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
    PlayerUtil.SetHP(_player_hp, Player.maxhp, true)
    for i = 2, #enemies do
        if enemies[i].GetVar("isactive") then
            enemies[i].Call("Update")
        end
    end
    if _player_hp == 0 then
        _mask.alpha = 1
        _game_mode = GameModeID.GameOver
        customstatename = "FAKE_GAMEOVER"
        State("CUSTOMSTATE")
    end
end

function PrepareRoom()
    NewAudio.StopAll()
    _game_mode = GameModeID.Room
    customstatename = "ROOM"
    State("CUSTOMSTATE")
end

BattleID = ""
item_inventory = {"Hot Ice", "G.Apple", "G.Apple", "V.Candy", "V.Candy", "B.Pie", "Love"}
_mercy_menu_index = 1
_item_healvalue = {
    ["Hot Ice"] = 16,
    ["G.Apple"] = 28,
    ["V.Candy"] = 48,
    ["B.Pie"] = -1,
    ["Love"] = -2,
    ["W.D."] = -2,
    ["W.Dog"] = -2
}

function HandleItem(itemName)
    if itemName == "Love" then
        if Player.lv == 1 then
            BattleDialog("You cannot use yet.")
        else
            BattleDialog("[func:_UseLoveItem]Determination.")
        end
        return
    end
    if itemName == "W.D." or itemName == "W.Dog" then
        for i = 1, 8 do
            item_inventory[i] = "W.Dog"
        end
        BattleDialog({"[noskip]You use the White Dog.", "[noskip][sound:dogsecret]Your inventory is filled with\rWhite Dog!!!!", "[noskip]Your HP...[w:10][func:PlayerHeal,30]"})
        return
    end
    local heal_amount = _item_healvalue[itemName]
    local texts = {""}
    if itemName == "Hot Ice" then
        texts = {"You eat the Hot Ice.", "I didn't understand\rit is hot or cold."}
    elseif itemName == "G.Apple" then
        texts = {"You eat the Green Apple."}
    elseif itemName == "V.Candy" then
        texts = {"You eat the Void Candy.\nDark Darker...[w:5]"}
    elseif itemName == "B.Pie" then
        texts = {"You eat the B.Pie.", "It was just apple pie.\nWhy does it name \"B.Pie\"?"}
    end
    if heal_amount == -1 then
        texts[1] = texts[1] .. "\n[func:PlayerHeal," .. Player.maxhp .. "]Your HP was maxed out."
    elseif _player_hp + heal_amount >= Player.maxhp then
        texts[1] = texts[1] .. "\n[func:PlayerHeal," .. heal_amount .. "]Your HP was maxed out."
    else
        texts[1] = texts[1] .. "\n[func:PlayerHeal," .. heal_amount .. "]You recovered " .. heal_amount .. " HP!"
    end
    BattleDialog(texts)
end

local totalEXP = {
    0, 10, 30, 70, 120,
    200, 300, 500, 800, 1200,
    1700, 2500, 3500, 5000, 7000,
    10000, 15000, 25000, 50000, 99999
}

function _UseLoveItem()
    Audio.PlaySound("saved")
    Player.lv = Player.lv - 1
    _player_hp = Player.maxhp
    _player_exp = totalEXP[Player.lv]
end

function PrepareItemFunEvent()
    local fun = GetAlMightyGlobal("*CYF-Example-OnActive-fun")
    if fun >= 60 and fun <= 69 then
        item_inventory[8] = "W.D."
    end
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
    if newState == "ACTIONSELECT" then
        if #item_inventory == 0 then
            Inventory.SetInventory({})
        end
    end
    if oldState == "ATTACKING" then
        _CheckVictory()
    end
    if newState == "ITEMMENU" then
        customstatename = "FAKE_ITEMMENU"
        State("CUSTOMSTATE")
        return true
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
    PlayerUtil.SetHPBarLength(Player.maxhp)
    BattleID = battleID
    _game_mode = GameModeID.Battle
    BattleStarting()
    enemies[1].Call("SetActive", false)
    SetMaskFadeOut(16)
    State("ACTIONSELECT")
end

-- battles (custom)
first_encount = false

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

function BattleStarting()
    first_encount = true
    if BattleID == "Poseur" then
        Audio.LoadFile("mus_battle1")
        encountertext = "Poseur strikes a pose!"
        nextwaves = {"bullettest_chaserorb"}
        wavetimer = 4.0
        arenasize = {155, 130}
        enemies[2].Call("SetActive", true)
        flee = true
        fleesuccess = nil
        deathtext = nil
        deathmusic = nil--"mus_gameover"
    end
end

function Update()
    _Update() -- don't delete
    if _game_mode ~= GameModeID.Battle then return end -- don't delete
    if Input.GetKey("F6") == 1 then
        Player.atk = 999
    end
    if Input.GetKey("End") == 1 then
        PlayerHurt(4, 0.5)
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
