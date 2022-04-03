function StateStarting(oldState)
	local canspare = Encounter.Call("_CheckAnyCanSpare")
	local menus = {}
	menus[1] = "Spare"
	if canspare then
		menus[1] = "[starcolor:ffff00][color:ffff00]" .. menus[1] .. "[color:ffffff]"
	end
	if Encounter["flee"] == nil or Encounter["flee"] then
		menus[2] = "Flee"
	end
	StateEditor.SetChoicesDialogText(menus, true)
	StateEditor.SetPlayerOnSelection(Encounter["mercyselect"], true)
end

function HandleCancel()
	Encounter["customstatename"] = "Recreates/ACTIONSELECT"
	State("CUSTOMSTATE")
end

function HandleArrows(left, right, up, down)
	if not up and not down then return end
	if Encounter["flee"] ~= nil and not Encounter["flee"] then return end
	Audio.PlaySound("menumove")
	Encounter["mercyselect"] = (Encounter["mercyselect"] % 2) + 1
	StateEditor.SetPlayerOnSelection(Encounter["mercyselect"], true)
end

function HandleAction()
	Audio.PlaySound("menuconfirm")
	Encounter.Call("HandleSpare")
end

function StateEnding(newState)
end