local commands = {}
local selection = 1

function StateStarting(oldState)
	commands = Encounter.Call("_GetEnemyCommands")
	StateEditor.SetChoicesDialogText(commands, false)
	StateEditor.SetPlayerOnSelection(selection, false)
end

function HandleCancel()
	Encounter["customstatename"] = "Recreates/ENEMYSELECT_ACT"
	State("CUSTOMSTATE")
end

function SetSelection(newSelection)
	if newSelection < 1 or newSelection > #commands then return end
	selection = newSelection
	Audio.PlaySound("menumove")
	StateEditor.SetPlayerOnSelection(selection, false)
end

function HandleArrows(left, right, up, down)
	if left then
		SetSelection(selection - 1)
	end
	if right then
		SetSelection(selection + 1)
	end
	if up then
		SetSelection(selection - 2)
	end
	if down then
		SetSelection(selection + 2)
	end
end

function HandleAction()
	Audio.PlaySound("menuconfirm")
	Encounter.Call("_ChooseCommands", selection)
end

function StateEnding(newState)
end