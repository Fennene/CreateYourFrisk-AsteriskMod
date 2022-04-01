-- You should check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

music = "mus_battle1 fj7x"
encountertext = "[color:ff0000]Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}
sparetext = "[color:ff0000]Spare"
fleetext = "[color:ff0000]Flee"

enemies = {
"poseur_fell"
}

enemypositions = {
{0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}
fell = true
isActionSelect = true
action = 0

function EncounterStarting()
    Player.name = string.upper("Nil256")

    PlayerUtil.SetNameColor(1, 0, 0)
    PlayerUtil.SetLVColor(1, 0, 0)
    PlayerUtil.SetHPLabelColor(1, 0, 0)
    PlayerUtil.SetHPTextColor(1, 0, 0)

    ButtonUtil.FIGHT.SetColor(1, 0.5, 0)
    ButtonUtil.ACT.SetColor(1, 0, 0)
    ButtonUtil.ITEM.SetColor(1, 0, 0)
    ButtonUtil.MERCY.SetColor(1, 0, 0)
    ArenaUtil.SetBorderColor(1, 0, 0)

    -- Hide BG (Below code don't relate FELLize)
    local BGMask = CreateSprite("px", "BelowUI")
    BGMask.color = {0, 0, 0}
    BGMask.Scale(640, 480)
end

-- This is for PlayerUtil. You can't use functions of PlayerUtil in EncounterStarting() because of technical issue.
function FirstFrameUpdate()
end

function SetButtonColor(index, r, g, b)
    if index == 0 then
        ButtonUtil.FIGHT.SetColor(r, g, b)
    elseif index == 1 then
        ButtonUtil.ACT.SetColor(r, g, b)
    elseif index == 2 then
        ButtonUtil.ITEM.SetColor(r, g, b)
    elseif index == 3 then
        ButtonUtil.MERCY.SetColor(r, g, b)
    end
end

function Update()
    if not AsteriskExperiment then
        PlayerUtil.SetHPTextColor(1, 0, 0)
    end
    if not isActionSelect then return end
    if Input.Right == 1 then
        SetButtonColor(action, 1, 0, 0)
        action = (action + 1) % 4
        SetButtonColor(action, 1, 0.5, 0)
    end
    if Input.Left == 1 then
        SetButtonColor(action, 1, 0, 0)
        action = (action - 1) % 4
        SetButtonColor(action, 1, 0.5, 0)
    end
end

function OnHPChanged() -- This is the one of Experimental Features added in v0.5.2.9.
    PlayerUtil.SetHPTextColor(1, 0, 0)
    -- This is better than calling it in Update() because that function is called when it needed only.
end

function EnteringState(newState, oldState)
    isActionSelect = (newState == "ACTIONSELECT")
end

function EnemyDialogueStarting()
    SetButtonColor(action, 1, 0, 0)
end

function EnemyDialogueEnding()
    nextwaves = { possible_attacks[math.random(#possible_attacks)] }
end

function DefenseEnding()
    SetButtonColor(action, 1, 0.5, 0)
    encountertext = "[color:ff0000]" .. RandomEncounterText()
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    BattleDialog({"Selected item " .. ItemID .. "."})
end