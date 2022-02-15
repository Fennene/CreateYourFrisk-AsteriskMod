
local selection = 1

function StateStarting(newState)
	StateEditor.SetChoicesDialogText({"DogTest1", "DogTest2", "DogTest3"}, true)
	StateEditor.SetPlayerOnSelection(selection, true)
end

function HandleAction()
	DEBUG("Confirmed")
end

function HandleArrows(left, right, up, down)
	DEBUG("AnyArrow")
	if not up and not down then
		return
	end
	if up and selection > 1 then
		selection = selection - 1
	end
	if down and selection < 3 then
		selection = selection + 1
	end
	StateEditor.SetPlayerOnSelection(selection, true)
end

function HandleCancel()
	DEBUG("Canceled")
	State("ACTIONSELECT")
end

function Update()
end

function StateEnding(newState)
end
