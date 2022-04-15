local slice = nil
local frame_counter = 0
local mask = nil

function StateStarting(oldState)
	Arena.Resize(155, 130)
	mask = CreateSprite("px", "Top")
	mask.alpha = 0
	mask.color = {0, 0, 0}
	mask.Scale(640, 480)
end

function Update()
	frame_counter = frame_counter + 1
	if frame_counter == 60 then
		slice = CreateSprite("UI/Battle/spr_slice_o_0", "BelowPlayer")
		slice.MoveToAbs(320, 360)
		Audio.PlaySound("slice")
	end
	if frame_counter < 60 then return end
	if frame_counter > 60 and frame_counter <= 110 and frame_counter % 10 == 0 then
		slice.Set("UI/Battle/spr_slice_o_" .. ((frame_counter - 60) / 10))
	end
	if frame_counter == 120 then
		slice.Remove()
	end
	if frame_counter == 130 then
		Audio.PlaySound("hitsound")
		Encounter.Call("StartShake")
	end
	if frame_counter < 300 then return end
	local lerp = math.min((frame_counter - 300) / 180, 1)
	mask.alpha = lerp
	NewAudio.SetVolume("src", 1 - lerp)
	if frame_counter == 500 then
		State("DONE")
	end
end

function StateEnding(newState)
end