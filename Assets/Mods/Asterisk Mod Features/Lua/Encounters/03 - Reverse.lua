-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "Oh no!\n"
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
    "bullettest_randomcircle", "bullettest_cyanorange",
    "bluesoultest_smalljump", "bluesoultest_hittoplatform", "bluesoultest_movetoplatform"
}

function EncounterStarting()
    --[[
    if Misc.FileExists("Audio/Reverse_Ideology.ogg") then
        Audio.LoadFile("Reverse_Ideology")
    end
    ]]
    Player.name = "Nil256"

    local random = math.random(1, 3)
    if random == 1 then
        Misc.CameraHorizontalReverse()
        encountertext = encountertext .. "Camera is reversed horizontally."
    elseif random == 2 then
        Misc.CameraVerticalReverse()
        encountertext = encountertext .. "Camera is reversed vertically."
    else
        Misc.cameraRotation = 180
        encountertext = encountertext .. "Camera is reversed horizontally\rand vertically."
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