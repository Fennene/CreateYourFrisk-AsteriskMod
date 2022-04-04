local now_name = {}
local selecting = 1
local check_screen = false
local go = false
local white_mask = nil
local frame_counter = 0
local texts = {}
local letters = {}
local options = {}
local check_screen_objects = {}
local text_frame_counter = -1
CHARS = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

function NewText(text, x, y, hide, layer)
	if hide == nil then hide = false end
	if layer == nil then layer = "RoomForeground" end
	local text = CreateText(text, {x, y}, 65536, layer)
	text.HideBubble()
	text.progressmode = "none"
	if hide then text.alpha = 0 end
	return text
end
function NewSquare(x, y, w, h, color, hide, layer)
	if color == nil then color = {1, 1, 1} end
	if hide == nil then hide = false end
	if layer == nil then layer = "RoomDialog" end
	local px = CreateSprite("px", layer)
	px.MoveToAbs(x, y)
	px.color = color
	if hide then
		px.alpha = 0
	end
	px.Scale(w, h)
	return px
end

function StateStarting(oldState)
	local prefix = "[font:uidialog][instant]"
	if Encounter["__Name_from_menu"] == nil then
		texts[1] = NewText(prefix.."What's your name?", 208, 396)
	else
		texts[1] = NewText(prefix.."You wanna change fun value, don't you?", 208, 396)
		local name = GetAlMightyGlobal("*CYF-Example-OnActive-Name")
		if name ~= nil then
			for i = 1, math.min(#name, 6) do
				local char = name:sub(i, i)
				now_name[i] = char
			end
		end
	end
	texts[1].MoveToAbs(320 - texts[1].GetTextWidth() / 2, 396)
	texts[2] = NewText(prefix.."AAAAAA", -64, -64)
	texts[2].MoveToAbs(320 - texts[2].GetTextWidth() / 2, 354)
	texts[2].SetText(prefix)
	prefix = "[font:uidialog][novoice][effect:shake,0.7]"
	for i = 1, 26 do
		local x = 121 + (64 * ((i - 1) % 7))
		local y = 306 - (math.floor((i - 1) / 7) * 29)
		letters[i] = NewText(prefix..CHARS[i], x, y)
	end
	for i = 1, 26 do
		local x = 121 + (64 * ((i - 1) % 7))
		local y = 183 - (math.floor((i - 1) / 7) * 29)
		letters[i + 26] = NewText(prefix..string.lower(CHARS[i]), x, y)
	end
	prefix = "[font:uidialog][instant]"
	options[1] = NewText(prefix.."Quit", 120, 52)
	options[2] = NewText(prefix.."Backspace", 240, 52)
	options[3] = NewText(prefix.."Done", 440, 52)
	letters[1].color = {1, 1, 0}
	check_screen_objects[1] = NewSquare(320, 240, 640, 480, {0, 0, 0}, true)
	check_screen_objects[2] = NewText(prefix.."Are you ready?", 170, 394, true, "RoomDialogText")
	prefix = "[font:uidialog][instant][effect:none]"
	check_screen_objects[3] = NewText(prefix.."Nil256", 150, 360, true, "RoomDialogText")
	check_screen_objects[3].MoveToAbs(320 - check_screen_objects[3].GetTextWidth() / 2, check_screen_objects[3].absy)
	prefix = "[font:uidialog][instant]"
	check_screen_objects[4] = NewText(prefix.."No", 147, 54, true, "RoomDialogText")
	check_screen_objects[5] = NewText(prefix.."Yes", 460, 54, true, "RoomDialogText")
	white_mask = NewSquare(320, 240, 640, 480, {1, 1, 1}, true, "RoomTop")
	UpdateDisplayName()
end

function GetName()
	local name = ""
	for i = 1, #now_name do
		name = name .. now_name[i]
	end
	return name
end

function UpdateDisplayName()
	texts[2].SetText("[font:uidialog][instant]"..GetName())
end

function SwitchCheckScreen()
	check_screen = not check_screen
	if not check_screen then
		for i = 1, 5 do
			check_screen_objects[i].alpha = 0
		end
		selecting = 55
		return
	end
	local name = GetName()
	if string.upper(name) == "FATAL" then
		NewAudio.StopAll()
		Encounter["customstatename"] = "TITLE"
		State("CUSTOMSTATE")
		return
	end
	check_screen_objects[3].SetText("[font:uidialog][instant][effect:none]"..name)
	check_screen_objects[3].Scale(1, 1)
	check_screen_objects[3].MoveToAbs(320 - check_screen_objects[3].GetTextWidth() / 2, 360)
	selecting = 1
	check_screen_objects[4].color = {1, 1, 0}
	for i = 1, 5 do
		check_screen_objects[i].alpha = 1
	end
	text_frame_counter = 0
end

function HandleCancel()
	if check_screen or #now_name == 0 then
		return
	end
	now_name[#now_name] = nil
	UpdateDisplayName()
end

function HandleArrows_Naming(left, right, up, down)
	local old_selecting = selecting
	if left then
		if selecting == 53 then
			selecting = 55
		elseif selecting ~= 1 then
			selecting = selecting - 1
		end
	end
	if right then
		if selecting == 55 then
			selecting = 53
		elseif selecting ~= 52 then
			selecting = selecting + 1
		end
	end
	if up then
		if selecting >= 1 and selecting <= 2 then
			selecting = 53
		elseif selecting >= 3 and selecting <= 5 then
			selecting = 54
		elseif selecting >= 6 and selecting <= 7 then
			selecting = 55
		elseif selecting >= 27 and selecting <= 31 then
			selecting = selecting - 5
		elseif selecting >= 32 and selecting <= 33 then
			selecting = selecting - 12
		elseif selecting == 53 then
			selecting = 48
		elseif selecting == 54 then
			selecting = 50
		elseif selecting == 55 then
			selecting = 46
		else
			selecting = selecting - 7
		end
	end
	if down then
		if selecting >= 20 and selecting <= 21 then
			selecting = selecting + 12
		elseif selecting >= 22 and selecting <= 26 then
			selecting = selecting + 5
		elseif selecting >= 46 and selecting <= 47 then
			selecting = 55
		elseif selecting >= 48 and selecting <= 49 then
			selecting = 53
		elseif selecting >= 50 and selecting <= 52 then
			selecting = 54
		elseif selecting == 53 then
			selecting = 1
		elseif selecting == 54 then
			selecting = 3
		elseif selecting == 55 then
			selecting = 6
		else
			selecting = selecting + 7
		end
	end
	if old_selecting ~= selecting then
		if old_selecting <= 52 then
			letters[old_selecting].color = {1, 1, 1}
		else
			options[old_selecting - 52].color = {1, 1, 1}
		end
		if selecting <= 52 then
			letters[selecting].color = {1, 1, 0}
		else
			options[selecting - 52].color = {1, 1, 0}
		end
	end
end

function HandleArrows_Checking(left, right, up, down)
	if not left and not right then
		return
	end
	if left then
		selecting = 1
		check_screen_objects[4].color = {1, 1, 0}
		check_screen_objects[5].color = {1, 1, 1}
	end
	if right and check_screen_objects[5].alpha == 1 then
		selecting = 2
		check_screen_objects[4].color = {1, 1, 1}
		check_screen_objects[5].color = {1, 1, 0}
	end
end

function HandleArrows(left, right, up, down)
	if go then
		return
	end
	if check_screen then
		HandleArrows_Checking(left, right, up, down)
	else
		HandleArrows_Naming(left, right, up, down)
	end
end

function HandleAction_Naming()
	if selecting <= 26 then
		if string.upper(GetName()) == "ECLIPS" and selecting == 5 then
			NewAudio.StopAll()
			Encounter["customstatename"] = "TITLE"
			State("CUSTOMSTATE")
			return
		end
		now_name[math.min(#now_name + 1, 6)] = CHARS[selecting]
	elseif selecting <= 52 then
		if string.upper(GetName()) == "ECLIPS" and selecting - 26 == 5 then
			NewAudio.StopAll()
			Encounter["customstatename"] = "TITLE"
			State("CUSTOMSTATE")
			return
		end
		now_name[math.min(#now_name + 1, 6)] = CHARS[selecting]
		now_name[math.min(#now_name + 1, 6)] = string.lower(CHARS[selecting - 26])
	elseif selecting == 53 then
		if Encounter["__Name_from_menu"] == nil then
			Encounter["customstatename"] = "BEGIN"
		else
			Encounter["customstatename"] = "MENU"
		end
		Encounter["__Menu_from_name"] = true
		State("CUSTOMSTATE")
	elseif selecting == 54 then
		HandleCancel()
	end
	if selecting ~= 53 and selecting ~= 55 then
		UpdateDisplayName()
	end
	if selecting == 55 and #now_name ~= 0 then
		SwitchCheckScreen()
	end
end

function HandleAction_Checking()
	if selecting == 1 then
		SwitchCheckScreen()
	else
		check_screen_objects[2].alpha = 0
		check_screen_objects[4].alpha = 0
		check_screen_objects[5].alpha = 0
		NewAudio.StopAll()
		Audio.PlaySound("intro_holdup")
		go = true
	end
end

function HandleAction()
	if go then
		return
	end
	if check_screen then
		HandleAction_Checking()
	else
		HandleAction_Naming()
	end
end

local letters_init_pos = {}

function UpdateNameText()
	if text_frame_counter < 0 then
		return
	end
	text_frame_counter = text_frame_counter + 1
	if text_frame_counter % 2 == 0 then
		local letters = check_screen_objects[3].GetLetters()
		if text_frame_counter == 2 then
			letters_init_pos = {}
			for i = 1, #letters do
				letters_init_pos[i] = letters[i].y
			end
		end
		local deg = math.random(-7, 7) / 6
		for i = 1, #letters do
			local rad = math.rad(deg)
			local r = (i - 1) * 16
			letters[i].MoveTo(math.cos(rad) * r, letters_init_pos[i] + math.sin(rad) * r)
			letters[i].rotation = deg
		end
	end
	if text_frame_counter >= 240 then
		return
	end
	local scale = 1 + (3 * text_frame_counter / 240)
	check_screen_objects[3].MoveToAbs(320 - (check_screen_objects[3].GetTextWidth() / 2 * scale), 360 - (200 * text_frame_counter / 240))
	check_screen_objects[3].Scale(scale, scale)	
end

function Update()
	UpdateNameText()
	if not go then
		return
	end
	frame_counter = frame_counter + 1
	if frame_counter < 340 then
		white_mask.alpha = frame_counter / 340
	end
	if frame_counter == 340 then
		white_mask.alpha = 1
	end
	if frame_counter == 368 then
		--[[
		SetAlMightyGlobal("*CYF-Example-OnActive-Name", GetName())
		SetAlMightyGlobal("*CYF-Example-OnActive-fun", math.random(1, 100))
		]]
		DEBUG("A")
		--Encounter.Call("GoToRoomState", 1)
	end
end

function StateEnding(newState)
	for i = 1, 2 do
		texts[i].Remove()
	end
	texts = {}
	for i = 1, 52 do
		letters[i].Remove()
	end
	letters = {}
	for i = 1, 3 do
		options[i].Remove()
	end
	options = {}
	for i = 1, 5 do
		check_screen_objects[i].Remove()
	end
	check_screen_objects = {}
	white_mask.Remove()
end
