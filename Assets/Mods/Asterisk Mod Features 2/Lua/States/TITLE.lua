local title = nil
local press = nil
local frame_counter = 0

function StateStarting(oldState)
	Encounter.Call("Audio_TryPlay", {"mus_intronoise", false})
	title = CreateSprite("cyf-asteriskmod", "Menu")
	press = CreateSprite("press", "Menu")
	press.alpha = 0
	press.MoveToAbs(320, 120)
end

function Update()
	if frame_counter == 200 then
		press.alpha = 1
	end
	frame_counter = frame_counter + 1
end

function HandleAction()
	Encounter["customstatename"] = "BATTLESELECT"
	State("CUSTOMSTATE")
end

function StateEnding(newState)
	press.Remove()
	title.Remove()
end