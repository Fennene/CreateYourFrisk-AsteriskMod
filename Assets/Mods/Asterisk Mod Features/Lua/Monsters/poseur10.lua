commands = {"Check"}
randomdialogue = {"Random\nDialogue\n1.", "Random\nDialogue\n2.", "Random\nDialogue\n3."}

sprite = "poseur"
name = "[starcolor:ffffff][color:ffffff]Poseur"
hp = 100
atk = 1
def = 1
check = "No comment"
dialogbubble = "right"
canspare = false
cancheck = false
 
function HandleAttack(attackstatus)
    if attackstatus == -1 then
    else
    end
end
 
function HandleCustomCommand(command)
    BattleDialog({"POSEUR " .. atk .. " ATK " .. def .. " DEF"})
end

function OnSpare()
    require("_").Check(10)
    Spare()
end
