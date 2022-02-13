-- A basic monster script skeleton you can copy and modify for your own creations.
comments = {"Smells like the work\rof an enemy stand.", "Poseur is posing like his\rlife depends on it.", "Poseur's limbs shouldn't be\rmoving in this way."}
commands = {"JP Button", "Act 1", "Act 2", "Act 3"}
randomdialogue = {"Random\nDialogue\n1.", "Random\nDialogue\n2.", "Random\nDialogue\n3."}

sprite = "poseur" --Always PNG. Extension is added automatically.
name = "Poseur"
hp = 100
atk = 1
def = 1
check = "Check message goes here."
dialogbubble = "right" -- See documentation for what bubbles you have available.
canspare = false
cancheck = true
minibgcolor = {255, 0, 0}
bgcolor = {255, 0, 0}
fillcolor = {0, 255, 255}

-- Happens after the slash animation but before 
function HandleAttack(attackstatus)
    if attackstatus == -1 then
        -- player pressed fight but didn't press Z afterwards
    else
        -- player did actually attack
    end
end
 
-- This handles the commands; all-caps versions of the commands list you have above.
function HandleCustomCommand(command)
    if command == "JP BUTTON" then
        ButtonUtil.FIGHT.SetSprite("UI/Buttons/fightbt_2", "UI/Buttons/fightbt_3")
        ButtonUtil.ACT.SetSprite("UI/Buttons/actbt_2", "UI/Buttons/actbt_3")
        ButtonUtil.ITEM.SetSprite("UI/Buttons/itembt_2", "UI/Buttons/itembt_3")
        ButtonUtil.MERCY.SetSprite("UI/Buttons/mercybt_2", "UI/Buttons/mercybt_3")
        commands[1] = "EN Button"
        BattleDialog({"Japanese Buttons."})
    elseif command == "EN BUTTON" then
        ButtonUtil.FIGHT.SetSprite("UI/Buttons/fightbt_0", "UI/Buttons/fightbt_1")
        ButtonUtil.ACT.SetSprite("UI/Buttons/actbt_0", "UI/Buttons/actbt_1")
        ButtonUtil.ITEM.SetSprite("UI/Buttons/itembt_0", "UI/Buttons/itembt_1")
        ButtonUtil.MERCY.SetSprite("UI/Buttons/mercybt_0", "UI/Buttons/mercybt_1")
        commands[1] = "JP Button"
        BattleDialog({"English Buttons."})
    end
    if command == "ACT 1" then
        currentdialogue = {"Selected\nAct 1."}
        BattleDialog({"You selected " .. command .. "."})
    elseif command == "ACT 2" then
        currentdialogue = {"Selected\nAct 2."}
        BattleDialog({"You selected " .. command .. "."})
    elseif command == "ACT 3" then
        currentdialogue = {"Selected\nAct 3."}
        BattleDialog({"You selected " .. command .. "."})
    end
end