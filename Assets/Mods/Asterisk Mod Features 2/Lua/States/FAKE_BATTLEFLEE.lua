local frame_counter = 0
local run_soul = nil
local run_soul_index = 0
local frame_counter_total = 60
function StateStarting()
	Audio.Stop()
	StateEditor.SetPlayerVisible(false)
	StateEditor.SetButtonActive()
	StateEditor.SetDialogText("Escaped...")
end

function StateEnding()
end