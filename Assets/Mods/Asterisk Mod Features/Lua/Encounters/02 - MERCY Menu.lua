-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "Check out MERCY Menu!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
    "poseur02"
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

    flee = false
    fleesuccess = true

    -- changes texts of MERCY menu.
    sparetext = "Skip Turn" -- Normally, "Spare"
    fleetext = "[starcolor:ffff00][color:ffff00]Flee[starcolor:ffffff][color:ffffff]" -- Normally, "Flee"
end

function ToggleFlee()
    flee = not flee
    if flee then
        sparetext = "Spare"
    else
        sparetext = "Skip Turn"
    end
    return flee
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