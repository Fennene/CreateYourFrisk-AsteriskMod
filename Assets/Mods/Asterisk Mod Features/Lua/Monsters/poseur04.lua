comments = {"Smells like the work\rof an enemy stand.", "Poseur is posing like his\rlife depends on it.", "Poseur's limbs shouldn't be\rmoving in this way."}
commands = {}
randomdialogue = {"Random\nDialogue\n1.", "Random\nDialogue\n2.", "Random\nDialogue\n3."}

sprite = "poseur"
name = "Poseur"
hp = 500
atk = 1
def = 1
xp = 99999
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
end

function OnDeath()
    require("_").Check(4)
    Kill()
end
