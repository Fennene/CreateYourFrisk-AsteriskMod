comments = {"Smells like the work\rof an enemy stand.", "Poseur is posing like his\rlife depends on it.", "Poseur's limbs shouldn't be\rmoving in this way."}
commands = {"Toggle Flee"}
randomdialogue = {"Random\nDialogue\n1.", "Random\nDialogue\n2.", "Random\nDialogue\n3."}

sprite = "poseur"
name = "Poseur"
hp = 100
atk = 1
def = 1
check = "Check message goes here."
dialogbubble = "right"
canspare = false
cancheck = true

function HandleAttack(attackstatus)
    if attackstatus == -1 then
    else
    end
end
 
function HandleCustomCommand(command)
    if command == "TOGGLE FLEE" then
        activeFlee = Encounter.Call("ToggleFlee")
        local text = "Flee option got disabled."
        if activeFlee then
            text = "Flee option got enabled!"
        end
        BattleDialog({text})
    end
end