-- Below code checks that users use Asterisk Mod.
--[[
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end
]]

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
    DEBUG("isModifiedCYF = " .. tostring(isModifiedCYF))
    DEBUG("Asterisk = " .. tostring(Asterisk))
    DEBUG("AsteriskVersion = " .. tostring(AsteriskVersion))
    DEBUG("AsteriskExperiment = " .. tostring(AsteriskExperiment))

    if Asterisk then
        Player.name = "Nil256"
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