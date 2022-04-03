BUTTONS = {"FIGHT", "ACT", "ITEM", "MERCY"}
NEXT_STATES = {"ENEMYSELECT_FIGHT", "ENEMYSELECT_ACT", "ITEMMENU", "MERCYMENU"}

function StateStarting(oldState)
	StateEditor.SetPlayerVisible(true)
	StateEditor.SetDialogText(Encounter["encountertext"])
	StateEditor.SetCurrentAction(BUTTONS[Encounter["action"]], true)
end

function HandleCancel()
	StateEditor.SkipDialogText()
end

function HandleArrows(left, right, up, down)
	if not left and not right then return end
	local action = Encounter["action"]
	action = action - 1
	if left then
		action = action - 1
	end
	if right then
		action = action + 1
	end
	action = (action % 4) + 1
	Audio.PlaySound("menumove")
	StateEditor.SetCurrentAction(BUTTONS[action], true)
	Encounter["action"] = action
end

function HandleAction()
	Audio.PlaySound("menuconfirm")
	if Encounter["action"] == 3 then return end
	if Encounter["action"] == 1 then
		-- ENEMYSELECT(FIGHT) & ATTACKING is very tedious. I'm lazy.
		StateEditor.SetCurrentAction("FIGHT", false)
		State("ENEMYSELECT")
		return
	end
	Encounter["customstatename"] = "Recreates/" .. NEXT_STATES[Encounter["action"]]
	State("CUSTOMSTATE")
end

function StateEnding(newState)
end