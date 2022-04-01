-- You should check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "You can press [Delete]\rto damage yourself."
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
"poseur_common"
}

enemypositions = {
{0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

LV = -4
HP = 0
MaxHP = 0

function EncounterStarting()
    Player.name = string.upper("Nil256")

    LV = math.random(1, 9) * 100 + math.random(1, 9) * 10 + math.random(1, 9)
    MaxHP = LV * 4 + 16
    HP = MaxHP
    PlayerUtil.HPUIMoveTo(10, 0)
    PlayerUtil.SetHPControlOverride(true)
    PlayerUtil.SetLV(tostring(LV))
    PlayerUtil.SetHPBarLength(MaxHP)
    PlayerUtil.SetHP(HP, MaxHP, true)

    Player.atk = LV * 2 + 8
end

function PlayerHurt(amount, adjust)
    if Player.ishurting then return end
    if adjust == nil or adjust then
        amount = amount * 6
    end
    HP = HP - amount
    Player.Hurt(0, 1.7, true, true)
    PlayerUtil.SetHP(HP, MaxHP, true)
end

function Update()
    if HP <= 0  then
        Player.hp = 0
    end
    if Input.GetKey("Delete") ~= 1 then return end
    HP = HP - math.floor(MaxHP / 10)
    Player.Hurt(0, 0, true, true)
    PlayerUtil.SetHP(HP, MaxHP, true)    
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