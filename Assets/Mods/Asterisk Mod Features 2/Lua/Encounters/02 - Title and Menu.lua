-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

--[[

-- CustomState --

First of all, you need to read "CustomState" sections in the documention.

Here is the simplest example of Custom State, "Fake Scene".
Let's check out defining variables, EncounterStarting() and Lua/States/MENU.lua.

]]
music = "mus_battle1 fj7x"
encountertext = "[color:c87f7f]Poseur blocks the way!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = math.huge
arenasize = {155, 130}
enemies = {"poseur02"}
enemypositions = {{0, 0}}
playerskipdocommand = true
flee = false

-- Enum --
GAMEMODE = {}
GAMEMODE.TITLE = 0
GAMEMODE.MENU = 1
GAMEMODE.GAME = 2
GAMEMODE.GAMEOVER = 3 -- You need if you create Fake GameOver by CustomState.

DIFFICULTY = {}
DIFFICULTY.EASY = 0
DIFFICULTY.MEDIUM = 1
DIFFICULTY.HARD = 2
DIFFICULTY.LUNATIC = 3

-- Varibles --
_game_mode = GAMEMODE.TITLE
_difficulty = DIFFICULTY.MEDIUM
_no_hit_mode = false
_mask = CreateSprite("px", "Top")
_mask.color = {0, 0, 0}
_mask.Scale(640, 480)
MAIN_COLOR32 = {r = 200, g = 127, b = 127}
MAIN_COLOR_HEX = "c87f7f"

function EncounterStarting()
    Audio.Stop()

    Player.name = "  You"
    Player.lv = 2
    Player.hp = 24
    PlayerUtil.SetHPControlOverride(true)
    PlayerUtil.SetHPBarBGColor(0, 0, 0, 0)
    PlayerUtil.SetHPBarFillColor(0, 0, 0, 0)

    PlayerUtil.SetNameColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    PlayerUtil.SetLVColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    PlayerUtil.SetHPLabelColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    PlayerUtil.SetHPTextColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    ButtonUtil.FIGHT.SetColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    ButtonUtil.ACT.SetColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    ButtonUtil.ITEM.SetColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    ButtonUtil.MERCY.SetColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    ArenaUtil.SetBorderColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
    sparetext = "[color:" .. MAIN_COLOR_HEX .. "]Spare"

    local items = {"MnstrCndy", "MnstrCndy", "SpidrDont", "SpidrDont", "SpidrCidr", "TestDog"}
    for i = 1, #items do
        items[i] = "[color:" .. MAIN_COLOR_HEX .. "]" .. items[i]
        Inventory.AddCustomItems({items[i]}, {0})
    end
    Inventory.SetInventory(items)

    State("*MENU")
    -- The above code is same to below code.
    -- customstatename = "TITLE"
    -- State("CUSTOMSTATE")
end

-- Let's go to Lua/States/MENU.lua.
---------------------------------------------------------------------------------------------------------------
-- from Lua/States/MENU.lua

function EnteringState(newState, oldState)
    -- Have you take enough break?
    -- OK, Let's resume.
    -- Since AsteriskMod v0.5.2.9,
    -- GetCurrentState()'s return and EnteringState()'s arguments are assigned CustomState's name.
    -- (It means not assigned "CUSTOMSTATE")
    -- You can check CustomState by below code.
    if oldState == "*MENU" and newState == "*START_ENCOUNT" then
        -- Old CustomState is NOT unloaded in this timing, so you can get variables from it.
        soul = CustomState["soul"]
        AcceptConfig()
    end
    BattleEnteringState(newState, oldState)
end

-- That is one of the purpose of the CustomState.
-- It may look alternative of Wave Script.
-- However State Script is better than Wave Script because
--      there is StateEnding() and you don't get confused not to need to go and come DEFENDING.
-- Of course, that is not at all of CustomState.
-- Finally, I recommend you check out Encounter "03 - Alt ITEM Menu"
---------------------------------------------------------------------------------------------------------------

wave_folder = "Hard/"
current_hp = 24
current_maxhp = 24
current_hp_addition = 0
current_maxhp_addition = 0
hp_bar = nil
additional_hp_bar = nil
additional_hp_text = nil
kr = false
karma = 0
MAX_KARMA = 40
karma_framecounter = 0
fake_fill_bar = nil
frame_counter = 0

function AcceptConfig()
    if _difficulty >= DIFFICULTY.HARD then
        wave_folder = "Hard/Ex/"
    end
    if not _no_hit_mode then
        if _difficulty == DIFFICULTY.EASY then
            current_maxhp_addition = 6
            additional_hp_text = CreateText("[font:uibattlesmall][instant] + 6", {357, 63}, 1023, "BelowArena")
            additional_hp_text.HideBubble()
            additional_hp_text.progressmode = "none"
            additional_hp_text.color32 = {255, 115, 0}
        elseif _difficulty == DIFFICULTY.LUNATIC then
            kr = true
            PlayerUtil.HPTextMoveTo(30, 0)
            local label = CreateSprite("spr_krlabel_0", "BelowArena")
            label.MoveToAbs(325.4, 70)
            label.color32 = {MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b}
            fake_fill_bar = CreateSprite("px", "BelowArena")
            fake_fill_bar.SetPivot(0, 0.5)
            fake_fill_bar.MoveToAbs(276, 70)
            fake_fill_bar.color = {1, 1, 0}
            fake_fill_bar.Scale((current_hp - karma) * 1.2, 20)
        end
    else
        current_maxhp = 1
        current_maxhp_addition = 0
        PlayerUtil.SetNameColor32(0, 0, 0, 0)
        PlayerUtil.SetLVColor32(0, 0, 0, 0)
        PlayerUtil.SetHPText("NO HIT!")
        ButtonUtil.SetActives(true, false, false, false)
        ButtonUtil.FIGHT.MoveTo(234, 0)
    end
    additional_hp_bar = PlayerUtil.CreateLifeBar(true)
    additional_hp_bar.SetBackgroundColor32(127, 57, 0)
    additional_hp_bar.SetFillColor32(255, 115, 0)
    hp_bar = PlayerUtil.CreateLifeBar(--[[false]])
    PlayerUtil.SetHPBarLength(current_maxhp + current_maxhp_addition)
    additional_hp_bar.length = current_maxhp + current_maxhp_addition
    hp_bar.length = current_maxhp
    current_hp = current_maxhp
    current_hp_addition = current_maxhp_addition
    if kr then
        hp_bar.SetFillColor(1, 0, 1)
    end
    _game_mode = GAMEMODE.GAME
end

function UpdateKarma()
    if not kr then return end
    fake_fill_bar.Scale(math.max(1, current_hp - karma) * 1.2, 20)
    if karma == 0 then return end
    karma_framecounter = karma_framecounter + 1
    local decrease_flag = false
    if karma >= 40 then
        decrease_flag = (karma_framecounter >= 2)
    elseif karma >= 30 then
        decrease_flag = (karma_framecounter >= 4)
    elseif karma >= 20 then
        decrease_flag = (karma_framecounter >= 10)
    elseif karma >= 10 then
        decrease_flag = (karma_framecounter >= 30)
    else
        decrease_flag = (karma_framecounter >= 60)
    end
    if decrease_flag then
        karma = karma - 1
        karma_framecounter = 0
        if current_hp == 1 then
            karma = 0
        else
            current_hp = current_hp - 1
        end
        if karma == 0 then
            PlayerUtil.SetHPTextColor32(MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b)
        end
    end
end

function UpdateLifeStat()
    if _no_hit_mode then return end
    additional_hp_bar.value = current_maxhp + current_hp_addition
    hp_bar.value = current_hp
    local text = current_hp
    if current_hp < 10 then
        text = "0" .. text
    end
    if current_maxhp_addition ~= 0 then
        text = text .. "    "
        additional_hp_text.SetText("[font:uibattlesmall][instant] + " .. current_hp_addition)
    end
    text = text .. " / " .. current_maxhp
    PlayerUtil.SetHPText(text)
end

__debug = false

function UpdateDebug()
    if Input.GetKey("C") == 1 and Input.GetKey("Y") == 1 and Input.GetKey("F") == 1 then
        __debug = not __debug
    end
    if not __debug then return end
    for i = 1, 9 do
        if Input.GetKey("Alpha" .. i) == 1 then
            if Input.Cancel <= 0 then
                PlayerHeal(i)
            else
                PlayerHurt(i)
            end
        end
    end
    if Input.GetKey("Alpha0") == 1 then
        if Input.Cancel <= 0 then
            PlayerHeal(99)
        else
            PlayerHurt(99)
        end
    end
    if Input.GetKey("Tab") == 1 then
        if GetCurrentState() == "ACTIONSELECT" then
            turn = turn + 1
            if attacks[turn + 1] == nil then
                DEBUG("nil")
            else
                DEBUG(attacks[turn + 1])
            end
        else
            DefenseEnding()
            State("ACTIONSELECT")
        end
    end
    if Input.GetKey("F6") == 1 then
        Player.atk = 1023
    end
end

function Update()
    if _game_mode ~= GAMEMODE.GAME then return end
    UpdateDebug()
    UpdateKarma()
    UpdateLifeStat()
    if current_hp == 0 then
        Player.hp = 0
    end
end

function PlayerHurt(amount, inv_time)
    if inv_time == nil then inv_time = 0.8 end
    if _no_hit_mode then
        Audio.PlaySound("hurtsound")
        Player.hp = 0
        return
    end
    if kr then
        if frame_counter % 2 == 1 then return end
        Audio.PlaySound("hurtsound")
        karma = math.min(karma + amount, MAX_KARMA)
        current_hp = current_hp - 1
        PlayerUtil.SetHPTextColor32(MAIN_COLOR32.r, 0, MAIN_COLOR32.b)
        return
    end
    if Player.ishurting then return end
    Audio.PlaySound("hurtsound")
    Player.Hurt(0, inv_time, false, false)
    if current_hp_addition > 0 then
        current_hp_addition = current_hp_addition - amount
        if current_hp_addition >= 0 then return end
        amount = math.abs(current_hp_addition)
        current_hp_addition = 0
    end
    current_hp = math.max(0, current_hp - amount)
end

function PlayerHeal(amount)
    Audio.PlaySound("healsound")
    current_hp = math.min(current_hp + amount, current_maxhp)
end

turn = 0
prevent_next_turn = false

function BattleEnteringState(newState, oldState)
    if _game_mode ~= GAMEMODE.GAME then return end
    if oldState == "*START_ENCOUNT" then
        Audio.Play()
    end
    if newState == "ENEMYDIALOGUE" and prevent_next_turn then
        State("ACTIONSELECT")
        prevent_next_turn = false
    end
end

function HandleItem(ItemID)
    local amount = 0
    local mess = {}
    ItemID = string.gsub(ItemID, string.upper("%[color:"..MAIN_COLOR_HEX.."]") , "")
    if ItemID == string.upper("MnstrCndy") then
        amount = 10
        mess = {"You ate the Monster Candy.[w:10]\rVery un-licorice-like.[w:10]"}
    elseif ItemID == string.upper("SpidrDont") then
        amount = 12
        mess = {"Don't worry,[w:5]spider didn't.[w:10]"}
    elseif ItemID == string.upper("SpidrCidr") then
        amount = 24
        mess = {"You drank the Spider Cider.[w:10]"}
    elseif ItemID == string.upper("TestDog") then
        amount = 32
        mess = {"Tastes like nostalgic.[w:10]"}
    end
    if amount <= 0 then return end
    if current_hp + amount >= current_maxhp then
        mess[#mess] = mess[#mess] .. "\nYour HP was maxed out."
    else
        mess[#mess] = mess[#mess] .. "\nYou recovered "..amount.." HP!"
    end
    for i = 1, #mess do
        mess[i] = "[color:" .. MAIN_COLOR_HEX .. "]" .. mess[i]
    end
    PlayerHeal(amount)
    BattleDialogue(mess)
end

function HandleSpare()
    State("ACTIONSELECT")
end

function EnemyDialogueStarting()
end

attacks = {"bouncy", "circle", "smalljump", "bullettest_touhou", "movetoplatform"}
all_attacks = {"bouncy2", "circle", "smalljump", "bullettest_touhou", "movetoplatform", "smalljumpup"}

function EnemyDialogueEnding()
    turn = turn + 1
    if attacks[turn] == nil then
        nextwaves = { wave_folder..all_attacks[math.random(#all_attacks)] }
    else
        nextwaves = { wave_folder..attacks[turn] }
    end
end

function DefenseEnding()
    if current_maxhp_addition > 0 and current_hp_addition < current_maxhp_addition then
        Audio.PlaySound("healsound")
        current_hp_addition = current_maxhp_addition
    end
    encountertext = "[color:" .. MAIN_COLOR_HEX .. "]" .. RandomEncounterText()
end
