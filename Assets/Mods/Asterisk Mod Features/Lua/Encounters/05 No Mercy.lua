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

function EncounterStarting()
    Player.name = string.upper("Nil256")

    -- Move all button to center
    ButtonUtil.FIGHT.MoveTo(234, 0)
    ButtonUtil.ACT.MoveTo(81, 0)
    ButtonUtil.ITEM.MoveTo(-79, 0)
    ButtonUtil.MERCY.MoveTo(-234, 0)

    -- Move
    ButtonUtil.FIGHT.Move(-187, 0)
    ButtonUtil.ITEM.Move(187, 0)

    -- Inactivate MERCY button.
    ButtonUtil.MERCY.SetActive(false)
    -- This is same to call below function
    --ButtonUtil.SetActives(true, true, true, false) -- FIGHT, ACT, ITEM, MERCY
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