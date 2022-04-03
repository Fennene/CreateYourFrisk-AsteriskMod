local multi_dialog = type(Encounter["_dialog"]) == "table"
local now_dialog_ID = 1

function StateStarting(oldState)
	StateEditor.SetPlayerVisible(false)
	if multi_dialog then
		StateEditor.SetDialogText(Encounter["_dialog"][1])
	else
		StateEditor.SetDialogText(Encounter["_dialog"])
	end
end

function HandleCancel()
	StateEditor.SkipDialogText()
end

function HandleAction()
	if not StateEditor.GetLineCompleteDialogText() then return end
	if multi_dialog then
		now_dialog_ID = now_dialog_ID + 1
		if Encounter["_dialog"][now_dialog_ID] ~= nil then
			StateEditor.SetDialogText(Encounter["_dialog"][now_dialog_ID])
			return
		end
	end
	State("ENEMYDIALOGUE")
end

function StateEnding(newState)
end