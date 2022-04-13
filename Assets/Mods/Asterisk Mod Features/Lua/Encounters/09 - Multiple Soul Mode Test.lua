-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "Check out the wave (script)."
nextwaves = {"multiplesoultest_blueandpurple"} -- check it out...
wavetimer = 6.0
arenasize = {155, 130}

enemies = {
    "poseur"
}

enemypositions = {
    {0, 0}
}

function EncounterStarting()
    Player.name = "Nil256"
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
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