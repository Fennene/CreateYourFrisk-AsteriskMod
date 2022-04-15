-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

-- um? Do you wanna know about CustomState?
-- Ahh... You should check "02 - Title and Menu" at first.
-- This is not good example.

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 8.0
arenasize = {155, 130}

enemies = {
    "poseur01"
}

enemypositions = {
    {0, 0}
}

attacks = {
    "bullettest_chaserorb", "bullettest_bouncy", "bullettest_cyanorange", "bullettest_touhou", "bullettest_randomcircle",
    "bluesoultest_smalljump", "bluesoultest_movetoplatform", "bluesoultest_hittoplatform"
}
attackID = 1

playerabsx = ArenaUtil.centerabsx
playerabsy = ArenaUtil.centerabsy
wait_a_frame = nil

enemyshake = false

function EncounterStarting()
    Player.name = "      "

    -- You can hide buttons by calling SetColor() and setting 0 to argument#4
    ButtonUtil.FIGHT.SetColor(0, 0, 0, 0)
    ButtonUtil.ACT.SetColor(0, 0, 0, 0)
    ButtonUtil.ITEM.SetColor(0, 0, 0, 0)
    ButtonUtil.MERCY.SetColor(0, 0, 0, 0)

    nextwaves[1] = attacks[1]
    State("DEFENDING")
end

function EnteringState(newState, oldState)
    if oldState == "DEFENDING" then
        playerabsx = Player.absx
        playerabsy = Player.absy
        attackID = attackID + 1
        if attacks[attackID] ~= nil then
            Wave[1].Call("EndingWave")
            nextwaves[1] = attacks[attackID]
            if nextwaves[1] ~= "bluesoultest_movetoplatform" and nextwaves[1] ~= "bluesoultest_hittoplatform" then
                wait_a_frame = true
            end
            State("DEFENDING")
        else
            State("*FAKE_ATTACKING")
        end
    end
end

function StartShake()
    enemies[1].Call("StartShake")
    enemyshake = true
end

function Update()
    if enemyshake then
        enemies[1].Call("UpdateShake")
    end
    if wait_a_frame == nil then return end
    if wait_a_frame then
        wait_a_frame = false
    else
        Player.MoveToAbs(playerabsx, playerabsy)
        wait_a_frame = nil
    end
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
end

function DefenseEnding()
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    BattleDialog({"Selected item " .. ItemID .. "."})
end