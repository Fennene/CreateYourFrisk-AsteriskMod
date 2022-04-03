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
"poseur_13" -- this mod does NOT support over 2 enemies.
}

enemypositions = {
{0, 0}
}

_dialog = ""
action = 1
enemyselect = 1
mercyselect = 1

function _GetEnemyNames()
    local names = {}
    for i = 1, math.min(2, #enemies) do
        names[i] = enemies[i].GetVar("name")
    end
    return names
end

function _GetEnemyCommands()
    local commands = {}
    if enemies[enemyselect].GetVar("cancheck") then
        commands[1] = "Check"
    end
    local com = enemies[enemyselect].GetVar("commands")
    for i = 1, #com do
        commands[#commands + 1] = com[i]
    end
    return commands
end

function _CheckAnyCanSpare()
    for i = 1, #enemies do
        if enemies[1].GetVar("canspare") then return true end
    end
    return false
end

BattleDialog = function(text)
    _dialog = text
    customstatename = "Recreates/DIALOGRESULT"
    State("CUSTOMSTATE")
end

function _ChooseCommands(select)
    local commands = _GetEnemyCommands()
    if select == 1 and enemies[enemyselect].GetVar("cancheck") then
        BattleDialog(string.upper(enemies[enemyselect].GetVar("name")) .. " " .. enemies[enemyselect].GetVar("atk") .. " ATK " .. enemies[enemyselect].GetVar("def") .. " DEF\n" .. enemies[enemyselect].GetVar("check"))
        return
    end
    enemies[enemyselect].Call("HandleCustomCommand", string.upper(commands[select]))
end

function EncounterStarting()
    Player.name = string.upper("Nil256")
    customstatename = "Recreates/ACTIONSELECT"
    State("CUSTOMSTATE")
end

function EnteringState(newState, oldState)
    if newState == "ACTIONSELECT" then
        customstatename = "Recreates/ACTIONSELECT"
        State("CUSTOMSTATE")
    end
end

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

function Update()
    DEBUG(GetCurrentState())
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
    --[[
    customstatename = "Recreates/ENEMYDIALOGUE"
    State("CUSTOMSTATE")
    ]]
end

function HandleItem(ItemID)
    BattleDialog({"Selected item " .. ItemID .. "."})
end