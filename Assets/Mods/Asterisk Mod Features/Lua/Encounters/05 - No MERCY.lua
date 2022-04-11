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
    "bullettest_randomcircle", "bluesoultest_smalljump"
}

function EncounterStarting()
    Player.name = "Nil256"

    -- Inactivate MERCY button.
    ButtonUtil.MERCY.SetActive(false)
    -- You can do same thing by below code.
    -- ButtonUtil.SetActives(true, true, true, false) -- Note that you can't inactivate all buttons.

    -- Move all buttons to Center (horizontal position only)
    ButtonUtil.FIGHT.MoveTo(234, 0)
    ButtonUtil.ACT.MoveTo(81, 0)
    ButtonUtil.ITEM.MoveTo(-79, 0)
    ButtonUtil.MERCY.MoveTo(-234, 0)
    -- and...
    ButtonUtil.FIGHT.Move(-187, 0)
    ButtonUtil.ITEM.Move(187, 0)

    -- Use "SetColor()" if you wanna just hide buttons.
    -- ButtonUtil.ITEM.SetColor(1, 1, 1, 0)

    -- Don't hide them to place another buttons! You can change sprite with calling "button.SetSprite()"!
    -- ButtonUtil.FIGHT.SetSprite("UI/NonColoredButtons/fightbt_0", "UI/NonColoredButtons/fightbt_1")
    -- or
    -- ButtonUtil.FIGHT.SetSprite("fightbt_0", "fightbt_1", "UI/NonColoredButtons")
    -- or
    -- ButtonUtil.SetSprites("UI/NonColoredButtons")
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