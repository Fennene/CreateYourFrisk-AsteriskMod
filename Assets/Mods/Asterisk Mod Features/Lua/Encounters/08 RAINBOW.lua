-- You should check that the player use AsteriskMod.
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
"poseur_common"
}

enemypositions = {
{0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

rainbow = 1
r = 255
g = 0
b = 0

function EncounterStarting()
    Player.name = string.upper("Nil256")

    -- Set Non Colored Buttons
    ButtonUtil.FIGHT.SetSprite("UI/NonColoredButtons/fightbt_0", "UI/NonColoredButtons/fightbt_1")
    ButtonUtil.ACT.SetSprite("UI/NonColoredButtons/actbt_0", "UI/NonColoredButtons/actbt_1")
    ButtonUtil.ITEM.SetSprite("UI/NonColoredButtons/itembt_0", "UI/NonColoredButtons/itembt_1")
    ButtonUtil.MERCY.SetSprite("UI/NonColoredButtons/mercybt_0", "UI/NonColoredButtons/mercybt_1")

    -- Hide BG (Below code don't relate FELLize)
    local BGMask = CreateSprite("px", "BelowUI")
    BGMask.color = {0, 0, 0}
    BGMask.Scale(640, 480)
end

function Update()
    if rainbow == 1 then
        g = math.min(g + 3, 255)
        if g == 255 then rainbow = 2 end
    elseif rainbow == 2 then
        r = math.max(0, r - 3)
        if r == 0 then rainbow = 3 end
    elseif rainbow == 3 then
        b = math.min(b + 3, 255)
        if b == 255 then rainbow = 4 end
    elseif rainbow == 4 then
        g = math.max(0, g - 3)
        if g == 0 then rainbow = 5 end
    elseif rainbow == 5 then
        r = math.min(r + 3, 255)
        if r == 255 then rainbow = 6 end
    elseif rainbow == 6 then
        b = math.max(0, b - 3)
        if b == 0 then rainbow = 1 end
    end
    PlayerUtil.SetHPBarFillColor32(r, g, b)
    ButtonUtil.FIGHT.SetColor32(r, g, b)
    ButtonUtil.ACT.SetColor32(r, g, b)
    ButtonUtil.ITEM.SetColor32(r, g, b)
    ButtonUtil.MERCY.SetColor32(r, g, b)
    ArenaUtil.SetBorderColor32(r, g, b)
    enemies[1].SetVar("fillcolor", {r, g, b})
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