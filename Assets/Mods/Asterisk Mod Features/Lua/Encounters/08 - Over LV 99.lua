-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
    "poseur"
}

enemypositions = {
    {0, 0}
}

possible_attacks = {
    "bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou",
    "bullettest_randomcircle", "bullettest_cyanorange",
    "bluesoultest_smalljump", "bluesoultest_hittoplatform", "bluesoultest_movetoplatform"
}

fake_LV = 1
fake_HP = 20
fake_MaxHP = 20

function EncounterStarting()
    Player.name = "Nil256"

    -- Sets LOVE
    fake_LV = math.random(1, 9) * 100 + math.random(0, 9) * 10 + math.random(0, 9)
    -- Calculates
    fake_MaxHP = 16 + fake_LV * 4
    fake_HP = fake_MaxHP
    Player.atk = 8 + 2 * fake_LV

    PlayerUtil.SetLV(fake_LV)
    PlayerUtil.SetHPControlOverride(true)
    PlayerUtil.SetHPBarLength(fake_MaxHP)

    encountertext = "Determination."
end

-- defines alternative hurt function. It is called from Wave Scripts.
function PlayerHurt(amount)
    --if type(amount) ~= "number" then error("EncounterScript: PlayerHurt() - argument#1(amount) should be number") end
    if Player.ishurting then return end -- checks that the player is hurting.
    if amount <= 0 then return end -- checks that argument is positive value
    Audio.PlaySound("hurtsound")
    fake_HP = math.max(0, fake_HP - amount)
    Player.Hurt(0, 1.7, false, false)
end

-- defines alternative heal function.
function PlayerHeal(amount)
    --if type(amount) ~= "number" then error("EncounterScript: PlayerHeal() - argument#1(amount) should be number") end
    if amount <= 0 then return end -- checks that argument is positive value
    Audio.PlaySound("healsound")
    fake_HP = math.min(fake_HP + amount, fake_MaxHP)
end

function Update()
    PlayerUtil.SetHP(fake_HP, fake_MaxHP, true)
    -- Gameover
    if fake_HP <= 0 then
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
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    BattleDialog({"Selected item " .. ItemID .. "."})
end