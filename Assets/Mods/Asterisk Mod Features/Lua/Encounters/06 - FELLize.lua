-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\n[このModはCYF-AsteriskModでのみ起動できます。]"
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

music = "mus_battle1 fj7x"
encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
    "poseur_fell"
}

enemypositions = {
    {0, 0}
}

possible_attacks = {
    "bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou",
    "bullettest_randomcircle", "bullettest_cyanorange",
    "bluesoultest_smalljump", "bluesoultest_hittoplatform", "bluesoultest_movetoplatform"
}

fell = true -- It's used this variables in wave scripts.

function EncounterStarting()
    Player.name = "Nil256"

    -- Set the texts' color of player's status to Red
    PlayerUtil.Name.color = {1, 0, 0}
    PlayerUtil.Love.color = {1, 0, 0}
    PlayerUtil.HPLabel.color = {1, 0, 0}
    PlayerUtil.HPText.color = {1, 0, 0}

    -- Set the color of all button to Red(?)
    ButtonUtil.FIGHT.color = {1, 0.5, 0}
    ButtonUtil.ACT.color = {1, 0, 0}
    ButtonUtil.ITEM.color = {1, 0, 0}
    ButtonUtil.MERCY.color = {1, 0, 0}

    -- Set the color of arena's border
    ArenaUtil.SetBorderColor(1, 0, 0)
    -- ArenaUtil.SetInnerColor(1, 0, 0) -- WHAT.

    -- Don't forget to set color in MERCY menu's options
    sparetext = "[color:ff0000]Spare"
    fleetext = "[color:ff0000]Flee"

    -- Also don't forget to set color encountertext. this is not features of Asterisk Mod
    encountertext = "[color:ff0000]Poseur strikes a pose!"

    -- Hides BG (Below code don't relate to FELLize)
    local BGMask = CreateSprite("px", "BelowUI")
    BGMask.color = {0, 0, 0}
    BGMask.Scale(640, 480)
end

function Update()
    if GetCurrentState() == "ACTIONSELECT" then
        local button = GetCurrentAction()
        ButtonUtil.FIGHT.color = {1, 0, 0}
        ButtonUtil.ACT.color = {1, 0, 0}
        ButtonUtil.ITEM.color = {1, 0, 0}
        ButtonUtil.MERCY.color = {1, 0, 0}
        if button == "FIGHT" then
            ButtonUtil.FIGHT.color = {1, 0.5, 0}
        elseif button == "ACT" then
            ButtonUtil.ACT.color = {1, 0.5, 0}
        elseif button == "ITEM" then
            ButtonUtil.ITEM.color = {1, 0.5, 0}
        elseif button == "MERCY" then
            ButtonUtil.MERCY.color = {1, 0.5, 0}
        end
    end
end

function EnteringState(newState, oldState)
    if newState == "ENEMYDIALOGUE" or newState == "DEFENDING" then
        ButtonUtil.FIGHT.color = {1, 0, 0}
        ButtonUtil.ACT.color = {1, 0, 0}
        ButtonUtil.ITEM.color = {1, 0, 0}
        ButtonUtil.MERCY.color = {1, 0, 0}
    end
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
    nextwaves = { possible_attacks[math.random(#possible_attacks)] }
end

function DefenseEnding()
    encountertext = "[color:ff0000]" .. RandomEncounterText()
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    BattleDialog({"[color:ff0000]Selected item " .. ItemID .. "."})
end

function BeforeDeath()
    Player.sprite.color = {1, 0, 0}
end