local soul = nil
local velocity = {}
local frame_counter = 0

function StateStarting(oldState)
	if not oldState == "*MENU" then
		error("")
	end
	soul = Encounter["soul"]
	Encounter["soul"] = nil
	Arena.ResizeImmediate(Encounter["arenasize"][1], Encounter["arenasize"][2])
	Player.MoveTo(0, 0)
end

function Update()
	frame_counter = frame_counter + 1
	if frame_counter >= 4 and frame_counter <= 24 and frame_counter % 4 == 0 then
		if frame_counter % 8 == 0 then
			Audio.PlaySound("BeginBattle2")
			soul.alpha = 1
		else
			soul.alpha = 0
		end
	end
	if frame_counter == 30 then
		Audio.PlaySound("BeginBattle3")
		velocity = {
			x = (ArenaUtil.centerabsx - soul.absx) / 42,
			y = (ArenaUtil.centerabsy - soul.absy) / 42
		}
	end
	if frame_counter > 30 and frame_counter <= 72 then
		soul.Move(velocity.x, velocity.y)
	end
	if frame_counter > 72 and frame_counter <= 88 then
		Encounter["_mask"].alpha = 1 - (frame_counter - 72) / 16
	end
	if frame_counter == 90 then
		State("ACTIONSELECT")
	end
end

function StateEnding(newState)
	soul.Remove()
end