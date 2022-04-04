-- 01 On Active
local mercy_info = {}

function StateStarting(oldState)
	mercy_info = Encounter.Call("_GetMercyMenuInfo")
	local flee = mercy_info[1]
	local yellow = mercy_info[2]
	local options = {}
	options[1] = "Spare"
	if yellow then
		options[1] = "[starcolor:ffff00][color:ffff00]" .. options[1] .. "[color:ffffff]"
	end
	if flee then
		options[2] = "Flee"
	else
		Encounter["_mercy_menu_index"] = 1
	end
	StateEditor.SetPlayerVisible(true)
	StateEditor.SetButtonActive(false, false, false, true)
	StateEditor.SetChoicesDialogText(options, true)
	StateEditor.SetPlayerOnSelection(Encounter["_mercy_menu_index"], true)
end

function HandleAction()
	if Encounter["_mercy_menu_index"] == 1 then
		Audio.PlaySound("menuconfirm")
		Encounter.Call("_Spare")
	elseif Encounter["_mercy_menu_index"] == 2 then
		Encounter.Call("_Flee")
	end
end

function HandleArrows(left, right, up, down)
	if not mercy_info[1] then return end
	if not up and not down then return end
	Encounter["_mercy_menu_index"] = (Encounter["_mercy_menu_index"] % 2) + 1
	StateEditor.SetPlayerOnSelection(Encounter["_mercy_menu_index"], true)
end

function HandleCancel()
	State("ACTIONSELECT")
end

function StateEnding(newState)
end
