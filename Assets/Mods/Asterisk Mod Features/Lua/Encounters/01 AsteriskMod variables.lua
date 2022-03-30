-- This encounter can be launch on any CYF!
encountertext = "Check out debugger!"
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
    DEBUG("isModifiedCYF = " .. tostring(isModifiedCYF))
    DEBUG("Asterisk = " .. tostring(Asterisk))
    DEBUG("AsteriskVersion = " .. tostring(AsteriskVersion))
    DEBUG("AsteriskCustomStateUpdate = " .. tostring(AsteriskCustomStateUpdate))
    DEBUG("AsteriskExperiment = " .. tostring(AsteriskExperiment))
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