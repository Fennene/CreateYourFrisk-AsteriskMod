local title = nil
local example = nil
local press = nil
local frame_counter = 0

function StateStarting(oldState)
	Audio.PlaySound("intro_noise")
	title = CreateSprite("cyf-asteriskmod", "RoomForeground")
	example = CreateText("[font:uidialog][instant]Example", {-256, -256}, 65536, "RoomForeground")
	example.HideBubble()
	example.progressmode = "none"
	title.MoveToAbs(320, 260)
	example.MoveToAbs(320 - example.GetTextWidth() / 2, 180)
	press = CreateSprite("press", "RoomForeground")
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
	if frame_counter <= 7 then return end
	if GetAlMightyGlobal("*CYF-Example-OnActive-Name") == nil then
		Encounter["customstatename"] = "BEGIN"
	else
		Encounter["customstatename"] = "MENU"
	end
	Encounter["customstatename"] = "ROOM"
	State("CUSTOMSTATE")
end

function StateEnding(newState)
	--Audio.LoadFile("mus_menu")
	press.Remove()
	example.Remove()
	title.Remove()
end