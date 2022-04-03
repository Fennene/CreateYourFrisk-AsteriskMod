-- You should check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 7.5
arenasize = {155, 130}
flee = false

enemies = {
"poseur_04"
}

enemypositions = {
{0, 0}
}


possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

local HP = 92
local Karma = 0
local MAX_KARMA = 40
local HurtDrain = 0
local KarmaDrain = 0
local KRLabel = CreateSprite("spr_krlabel_1", "BelowArena")
KRLabel.MoveToAbs(407, 70)
local FakeHPBar = CreateSprite("px", "BelowArena")
FakeHPBar.SetPivot(0, 0.5)
FakeHPBar.MoveToAbs(276, 70)
FakeHPBar.color = {1, 1, 0}
FakeHPBar.Scale(99 * 1.2, 20)

function EncounterStarting()
    Player.name = string.upper("Nil256")
    Player.lv = 19
    Player.hp = 92
end

-- This is for PlayerUtil. You can't use functions of PlayerUtil in EncounterStarting() because of technical issue.
function FirstFrameUpdate()
    PlayerUtil.HPTextMoveTo(30, 0)
    PlayerUtil.SetHPBarFillColor(1, 0, 1)
    PlayerUtil.SetHPControlOverride(true)
end

function PlayerHurt(karmaAmount)
    if HurtDrain ~= 0 then return end
    if karmaAmount <= 0 then return end
    Player.Hurt(0, 0.01, false, true)
    HP = HP - 1
    Karma = math.min(Karma + karmaAmount, MAX_KARMA)
    HurtDrain = HurtDrain + 1
end

function PlayerHeal(amount)
    HP = math.min(HP + math.max(0, amount), 99)
    Audio.PlaySound("healsound")
end

function Update()
    if HurtDrain > 0 then
        if HurtDrain >= 2 then
            HurtDrain = 0
        else
            HurtDrain = HurtDrain + 1
        end
    end
    if Karma > 0 then
        KarmaDrain = KarmaDrain + 1
        local reduce = false
        if Karma >= 40 then
            reduce = (KarmaDrain >= 2)
        elseif Karma >= 30 then
            reduce = (KarmaDrain >= 4)
        elseif Karma >= 20 then
            reduce = (KarmaDrain >= 10)
        elseif Karma >= 10 then
            reduce = (KarmaDrain >= 30)
        else
            reduce = (KarmaDrain >= 60)
        end
        if reduce then
            Karma = Karma - 1
            KarmaDrain = 0
            if HP == 1 then
                Karma = 0
                HP = 2
            end
            HP = HP - 1
        end
    end

    PlayerUtil.SetHP(HP, 92, true)
    local currentHP = math.max(1, HP - Karma)
    FakeHPBar.Scale(currentHP * 1.2, 20)

    if Karma <= 0 then
        PlayerUtil.SetHPTextColor(1, 1, 1)
    else
        PlayerUtil.SetHPTextColor(1, 0, 1)
    end

    if HP <= 0 then
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