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

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

function EncounterStarting()
    -- If player launch this mod with official CYF, show error
    if isModifiedCYF == nil then
        error("This game can not be launched on official CYF.\nUse to lanuch CYF-AsteriskMod.")
    end
    if Asterisk == nil then
        error("This game can be launched on only CYF-AsteriskMod.")
    end

    Player.name = "FN"

    -- Set activate of buttons
    ButtonUtil.SetActives(true, true, true, false)
    -- Move all buttoms to center
    ButtonUtil.FIGHT.MoveTo(234, 0)
    ButtonUtil.ACT.MoveTo(81, 0)
    ButtonUtil.ITEM.MoveTo(-79, 0)
    --ButtonUtil.MERCY.MoveTo(-234, 0)
    -- Move
    ButtonUtil.FIGHT.Move(-187, 0)
    ButtonUtil.ITEM.Move(187, 0)
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