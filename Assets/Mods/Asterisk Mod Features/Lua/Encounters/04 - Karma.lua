-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "You feel like you're going to\rhave a bad time."
nextwaves = {"bullettest_chaserorb"}
wavetimer = 7.5
arenasize = {155, 130}
flee = false

enemies = {
    "poseur04"
}

enemypositions = {
    {0, 0}
}

possible_attacks = {
    "bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou",
    "bullettest_randomcircle", "bluesoultest_smalljump"
}

-- variables to process karma mode --
-- Below code is NOT from Undertale. I don't know the actual code that handles Karma.
-- I referenced following site: https://undertale.fandom.com/wiki/Attack_Types#KARMA
current_hp = 20 -- fake HP's variable
karma = 0 -- amount of karma
MAX_KARMA = 40 -- constant. maximum value of karma
karma_framecounter = 0 -- if karma > 0, it increases
karma_label = CreateSprite("spr_krlabel_0", "BelowArena") -- KR
fake_fill_bar = CreateSprite("px", "BelowArena") -- Yellow bar
-- Undertale works 30fps, but CYF works 60fps. if you hurt the player in 1 frame on CYF, it does not work like Undertale.
framecounter = 0

function EncounterStarting()
    --[[
    if Misc.FileExists("Audio/Megalovania.ogg") then
        Audio.LoadFile("Megalovania")
    end
    ]]
    Player.name = "Nil256"
    Player.lv = 19

    local mask = CreateSprite("px", "BelowUI")
    mask.color = {0, 0, 0}
    mask.Scale(640, 480)

    current_hp = Player.maxhp
    -- palces KR label
    PlayerUtil.HPTextMoveTo(30, 0) -- makes space to place karma label
    karma_label.MoveToAbs(296.6 + (Player.maxhp * 1.2), 70) -- places the label
    -- places fake yellow bar
    fake_fill_bar.SetPivot(0, 0.5)
    fake_fill_bar.MoveToAbs(276, 70)
    fake_fill_bar.Scale((current_hp - karma) * 1.2, 20)
    -- changes color
    PlayerUtil.SetHPBarFillColor(1, 0, 1) -- purple
    fake_fill_bar.color = {1, 1, 0} -- yellow
    -- stops original process of the player's HP
    PlayerUtil.SetHPControlOverride(true)
    -- sets length of HP Bars
    PlayerUtil.SetHPBarLength(Player.maxhp)
    PlayerUtil.SetHP(current_hp, Player.maxhp, true)
end

-- defines alternative hurt function. It is called from Wave Scripts.
function PlayerHurt(kr_amount--[[, force]])
    --if type(kr_amount) ~= "number" then error("EncounterScript: PlayerHurt() - argument#1(kr_amount) should be number") end
    --if force == nil then force = false end
    --if type(force) ~= "boolean" then error("EncounterScript: PlayerHurt() - argument#2(force) should be nilable-boolean") end
    if kr_amount <= 0 then return end -- checks that argument is positive value
    if framecounter % 2 == 1 then return end -- 60fps -> 30fps
    Audio.PlaySound("hurtsound")
    karma = math.min(karma + kr_amount, MAX_KARMA)
    if GetCurrentState() == "DEFENDING" then -- It prevents that the player get gameover when current state is not DEFENDING.
        current_hp = current_hp - 1
    end
end

-- defines alternative heal function.
function PlayerHeal(amount)
    --if type(amount) ~= "number" then error("EncounterScript: PlayerHeal() - argument#1(amount) should be number") end
    if amount <= 0 then return end -- checks that argument is positive value
    Audio.PlaySound("healsound")
    current_hp = math.min(current_hp + amount, Player.maxhp)
end

function Update()
    -- for checking 60fps -> 30 fps
    framecounter = framecounter + 1

    -- debug
    --[[
    if Input.GetKey("Tab") >= 1 then
        PlayerHurt(6) -- 6 is karma amount of normal attacks. By the way, that amount of blaster is 10.
    end
    if Input.Menu == 1 then
        PlayerHeal(Player.maxhp)
    end
    ]]

    -- processing of karma
    if karma > 0 then
        karma_framecounter = karma_framecounter + 1
        local decrease_flag = false
        if karma >= 40 then -- 40
            decrease_flag = (karma_framecounter >= 2) -- On 30fps, 1 frame
        elseif karma >= 30 then -- 30 ~ 39
            decrease_flag = (karma_framecounter >= 4) -- On 30fps, 2 frame
        elseif karma >= 20 then -- 20 ~ 29
            decrease_flag = (karma_framecounter >= 10) -- On 30fps, 5 frame
        elseif karma >= 10 then -- 10 ~ 19
            decrease_flag = (karma_framecounter >= 30) -- On 30fps, 15 frame
        else -- 1 ~ 9
            decrease_flag = (karma_framecounter >= 60) -- On 30fps, 30 frame (= 1s)
        end
        if decrease_flag then
            karma = karma - 1
            karma_framecounter = 0
            if current_hp == 1 then -- Natural decreasing karma cannot kill the player.
                karma = 0
            else
                current_hp = current_hp - 1 -- decrease HP and karma at the same time.
            end
        end
        PlayerUtil.SetHPTextColor(1, 0, 1) -- sets the color of the text to purple
    else
        PlayerUtil.SetHPTextColor(1, 1, 1) -- reverts the color of the text
    end
    -- displaying
    PlayerUtil.SetHP(current_hp, Player.maxhp, true)
    fake_fill_bar.Scale(math.max(1, current_hp - karma) * 1.2, 20)

    -- checks gameover
    if current_hp <= 0 then
        Player.hp = 0
    end
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
    nextwaves = { possible_attacks[math.random(#possible_attacks)] }
end

function DefenseEnding()
    encountertext = RandomEncounterText()
    --[[
    if karma >= 40 then     encountertext = ...
    elseif karma >= 30 then encountertext = ...
    elseif karma >= 20 then encountertext = ...
    elseif karma >= 10 then encountertext = ...
    else                    encountertext = ...
    end
    ]]
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    BattleDialog({"Selected item " .. ItemID .. "."})
end