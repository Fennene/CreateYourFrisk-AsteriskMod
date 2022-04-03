local enemynames = {}

function StateStarting(oldState)
	enemynames = Encounter.Call("_GetEnemyNames")
	StateEditor.SetChoicesDialogText(enemynames, true)
	StateEditor.SetPlayerOnSelection(Encounter["enemyselect"], true)
end

function HandleCancel()
	Encounter["customstatename"] = "Recreates/ACTIONSELECT"
	State("CUSTOMSTATE")
end

function HandleArrows(left, right, up, down)
	--if #enemynames < 3 then return end
	if #enemynames < 2 then return end
	if not up and not down then return end
	Audio.PlaySound("menumove")
	Encounter["enemyselect"] = (Encounter["enemyselect"] % 2) + 1
	StateEditor.SetPlayerOnSelection(Encounter["enemyselect"], true)
end

function HandleAction()
	Audio.PlaySound("menuconfirm")
	Encounter["customstatename"] = "Recreates/ACTMENU"
	State("CUSTOMSTATE")
end

function StateEnding(newState)
end