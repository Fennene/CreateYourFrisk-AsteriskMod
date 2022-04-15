comments = {"Smells like the work\rof an enemy stand.", "Poseur is posing like his\rlife depends on it.", "Poseur's limbs shouldn't be\rmoving in this way.", "Smells like CYF.[w:4].[w:4].[w:4]?"}
commands = {"[color:c87f7f]Check"}
randomdialogue = {"[next]"}

sprite = "poseur_hard"
name = "[color:c87f7f]Poseur"
hp = 170
atk = 5
def = 4
check = "Check message goes here."
dialogbubble = "right"
canspare = false
cancheck = false
xp = 17
gold = 180

function HandleAttack(attackstatus)
    if attackstatus == -1 then
        if Encounter["turn"] == 0 then
            Encounter["prevent_next_turn"] = true
        end
    else
    end
end
 
function HandleCustomCommand(command)
    Encounter["prevent_next_turn"] = true
    BattleDialog({"[color:c87f7f]Poseur - 5 ATK 4 DEF\nJust a Poseur.\nNothing more, nothing less."})
end