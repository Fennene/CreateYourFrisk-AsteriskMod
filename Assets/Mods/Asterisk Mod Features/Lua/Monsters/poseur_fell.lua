comments = {"Smells like the work\rof an enemy stand.", "Poseur is posing like his\rlife depends on it.", "Poseur's limbs shouldn't be\rmoving in this way."}
commands = {"[color:ff0000]Check", "[color:ff0000]Act 1", "[color:ff0000]Act 2", "[color:ff0000]Act 3"}
randomdialogue = {"Random\nDialogue\n1.", "Random\nDialogue\n2.", "Random\nDialogue\n3."}

sprite = "poseur_fell"
name = "[color:ff0000]Poseur"
hp = 100
atk = 1
def = 1
check = "Check message goes here."
dialogbubble = "right"
canspare = false
cancheck = false
dialogueprefix = "[color:ff0000][effect:rotate]"

function HandleAttack(attackstatus)
    if attackstatus == -1 then
    else
    end
end
 
function HandleCustomCommand(command)
    if command == "[COLOR:FF0000]CHECK" then
        BattleDialog({"[color:ff0000]POSEUR " .. atk .. " ATK " .. def .. " DEF\n" .. check})
        return
    end
    if command == "ACT 1" then
        currentdialogue = {"Selected\nAct 1."}
    elseif command == "ACT 2" then
        currentdialogue = {"Selected\nAct 2."}
    elseif command == "ACT 3" then
        currentdialogue = {"Selected\nAct 3."}
    end
    BattleDialog({"[color:ff0000]You selected " .. command .. "."})
end

function OnDeath()
    require("_").Check(6)
    Kill()
end
