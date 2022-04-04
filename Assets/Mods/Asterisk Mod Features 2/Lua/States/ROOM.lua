--[[
You wanna create overworld?

um, You should not refer below code, because this overworld feature can be create only 640x480 sized room and can be not saved.

Moreover, this code is a part of overworld tool's code that have more features than below code.
It is "FN!OverworldCreator", but I have not released.
Look forward!

...wait. Let me take that back.
I forgot to tell you that "FN!OverworldCreator" is supported only Japanese.
]]
local bg = nil
local PLAYER_SPRITE_DIR = "room/players/frisk/"
local player_sprite = nil
local player_direction = 0
local player_anim_index = 1
local player_walk_timer = 0
local event = ""
local dialog_boxes = {}
local dialog_text = nil
local DIALOG_PREFIX = "[font:uidialog][voice:monsterfont][linespacing:4]"
local dialog_event = false
local dialog_event_id = 1
local dialog_event_texts = {}
local encount_bubble = nil
local encount_mask = nil
local encount_soul = nil
local encount_framecounter = 0
local encount_event = false
local encount_event_battleID = ""
local encount_velocity = {}
local room_mask_remove = -1
function NewSquare(x, y, w, h, color, hide, layer)
	if color == nil then color = {1, 1, 1} end
	if hide == nil then hide = true end
	if layer == nil then layer = "RoomDialog" end
	local px = CreateSprite("px", layer)
	px.MoveToAbs(x, y)
	px.color = color
	if hide then px.alpha = 0 end
	px.Scale(w, h)
	return px
end
function NewText(text, x, y, hide, layer)
	if hide == nil then hide = true end
	if layer == nil then layer = "RoomDialogText" end
	local text = CreateText(text, {x, y}, 65536, layer)
	text.HideBubble()
	text.progressmode = "none"
	if hide then text.alpha = 0 end
	return text
end

-- funcs

function StartDialog(eventID, texts, prefix)
	if event ~= "" then return end
	if prefix == nil then prefix = "" end
	if type(texts) ~= "table" then texts = {texts} end
	for i = 1, #texts do
		texts[i] = DIALOG_PREFIX .. prefix .. "* " .. string.gsub(string.gsub(texts[i], "%\n", "\n* "), "%\r", "\n  ")
	end
	dialog_event_texts = texts
	if Encounter["_player_position"][2] < 240 then
		dialog_boxes[1].MoveToAbs(321, 394)
		dialog_boxes[2].MoveToAbs(321, 394)
		dialog_text.MoveToAbs(62, 422)
	else
		dialog_boxes[1].MoveToAbs(321, 84)
		dialog_boxes[2].MoveToAbs(321, 84)
		dialog_text.MoveToAbs(62, 114)
	end
	dialog_event_id = 1
	for i = 1, 2 do
		dialog_boxes[i].alpha = 1
	end
	dialog_text.SetText(dialog_event_texts[dialog_event_id])
	dialog_text.alpha = 1
	dialog_event = true
	PlayerSpriteUpdate(0, 0, 0, 0)
	event = eventID
end
function Encount(targetBattleID, immediate, moveToFightButton)
	if event ~= "" then return end
	if room_mask_remove > -1 then return end
	if immediate == nil then immediate = false end
	if moveToFightButton == nil then moveToFightButton = true end
	encount_bubble.MoveToAbs(Encounter["_player_position"][1], Encounter["_player_position"][2] + 43)
	encount_soul.MoveToAbs(Encounter["_player_position"][1], Encounter["_player_position"][2] - 10)
	if immediate then
		encount_framecounter = 37
	else
		encount_framecounter = 0
	end
	if moveToFightButton then
		encount_velocity[1] = (48 - encount_soul.absx) / 42
		encount_velocity[2] = (25 - encount_soul.absy) / 42
	else
		encount_velocity[1] = (320 - encount_soul.absx) / 42
		encount_velocity[2] = (225 - encount_soul.absy) / 42
	end
	encount_event_battleID = targetBattleID
	encount_event = true
	PlayerSpriteUpdate(0, 0, 0, 0)
	event = "ENCOUNT"
end

-- Custom area

local walk_frame_counter = 0
local encount_frame = 360
if not Encounter["first_encount"] then
	encount_frame = 300
end
if Player.lv > 5 then
	encount_frame = encount_frame + (Player.lv - 4) * 45
end
encount_frame = encount_frame + math.random(-50, 50)

function CheckEncount(left, right, up, down, canMoveLeft, canMoveRight, canMoveUp, canMoveDown)
	-- gate
	if Encounter["_player_position"][2] > 346 and up >= 1 and not canMoveUp then
		StartDialog("Dark", {"It's very dark.", "You shouldn't go ahead."})
	end
	-- down & left
	local flag1 = (Encounter["_player_position"][2] < 182 and down >= 1 and not canMoveDown)
	local flag2 = (Encounter["_player_position"][1] < 91 and left >= 1 and not canMoveLeft)
	if flag1 or flag2 then
		StartDialog("OutOfWorld", {"You don't need to know\rabout out of world."})
	end
	-- Encount
	flag1 = (left ~= 0 and right == 0 and canMoveLeft)
	flag2 = (right ~= 0 and left == 0 and canMoveRight)
	local flag3 = (up ~= 0 and down == 0 and canMoveUp)
	local flag4 = (down ~= 0 and up == 0 and canMoveDown)
	if flag1 or flag2 or flag3 or flag4 then
		walk_frame_counter = walk_frame_counter + 1
	end
	if event == "" and walk_frame_counter >= encount_frame then
		Encount("Poseur")
	end
end

function RoomUpdate()
	if Input.GetKey("Tab") == 1 then
		--StartDialog("TEST", {"This is the test dialog."})
		Encount("Poseur", true)
	end
end

-- SYSTEM

function StateStarting(oldState)
	bg = CreateSprite("room/rooms/spr_pre_st_2", "RoomBackground")
	player_sprite = CreateSprite(PLAYER_SPRITE_DIR..player_anim_index, "Room")
	dialog_boxes[1] = NewSquare(321, 394, 578, 152, {1, 1, 1}, true, "RoomDialog")
	dialog_boxes[2] = NewSquare(321, 394, 566, 140, {0, 0, 0}, true, "RoomDialog")
	dialog_text = NewText(DIALOG_PREFIX, 62, 422, true, "RoomDialogText")
	encount_bubble = CreateSprite("room/players/spr_encount_1", "RoomEnc")
	encount_bubble.alpha = 0
	encount_mask = CreateSprite("px", "RoomEnc")
	if Encounter["__continue"] ~= nil then
		encount_mask.alpha = 0
		Encounter["__continue"] = nil
	else
		room_mask_remove = 0
	end
	encount_mask.color = {0, 0, 0}
	encount_mask.Scale(640, 480)
	encount_soul = CreateSprite("ut-heart", "RoomEncSoul")
	encount_soul.alpha = 0
	encount_soul.color = {1, 0, 0}
	Player.name = GetAlMightyGlobal("*CYF-Example-OnActive-Name")
end

function RemoveMaskUpdate()
	if room_mask_remove == -1 then return end
	room_mask_remove = room_mask_remove + 1
	encount_mask.alpha = 1 - (room_mask_remove / 16)
	if room_mask_remove == 16 then
		encount_mask.alpha = 0
		room_mask_remove = -1
	end
end

function PlayerSpriteUpdate(left, right, up, down)
	local old_direction = player_direction

	if down == 0 and player_direction == 0 then player_direction = -1 end
	if left == 0 and player_direction == 1 then player_direction = -1 end
	if right == 0 and player_direction == 2 then player_direction = -1 end
	if up == 0 and player_direction == 3 then player_direction = -1 end

	if up ~= 0 and player_direction == -1 then player_direction = 3 end
	if down ~= 0 and player_direction == -1 then player_direction = 0 end
	if right ~= 0 and player_direction == -1 then player_direction = 2 end
	if left ~= 0 and player_direction == -1 then player_direction = 1 end

	if player_direction ~= -1 then
		player_anim_index = math.floor(player_walk_timer / 10)
		player_anim_index = player_anim_index % 4
		player_anim_index = (player_direction * 4 + player_anim_index)
		player_sprite.Set(PLAYER_SPRITE_DIR..player_anim_index)
		player_walk_timer = player_walk_timer + 1
		Encounter["_player_direction"] = player_direction
	else
		if old_direction ~= -1 then
			player_anim_index = (old_direction * 4 + 1)
			player_sprite.Set(PLAYER_SPRITE_DIR..player_anim_index)
		end
		player_walk_timer = 0
	end
end

function PlayerPositionUpdate(left, right, up, down)
	local now_position = Encounter["_player_position"]
	local can_move_left = (now_position[1] > 101)
	local can_move_right = (now_position[1] < 580)
	local can_move_up = (now_position[2] < 336)
	local can_move_down = (now_position[2] > 192)
	-- gate
	if Encounter["_player_position"][1] >= 274 and Encounter["_player_position"][1] <= 284 then
		can_move_up = (now_position[2] < 350)
	end
	if Encounter["_player_position"][2] > 336 then
		can_move_left = (now_position[1] > 274)
		can_move_right = (now_position[1] < 284)
	end
	-- down
	if Encounter["_player_position"][1] >= 500 and Encounter["_player_position"][1] <= 539 then
		can_move_down = (now_position[2] > 30)
	end
	if Encounter["_player_position"][2] < 192 then
		can_move_left = (now_position[1] > 500)
		can_move_right = (now_position[1] < 539)
	end
	-- left
	if Encounter["_player_position"][2] >= 225 and Encounter["_player_position"][2] <= 297 then
		can_move_left = (now_position[1] > 30)
	end
	if Encounter["_player_position"][1] < 101 then
		can_move_up = (now_position[2] < 297)
		can_move_down = (now_position[2] > 225)
	end
	--
	if left >= 1 and can_move_left then
		now_position[1] = now_position[1] - 3
	end
	if right >= 1 and can_move_right then
		now_position[1] = now_position[1] + 3
	end
	if up >= 1 and can_move_up then
		now_position[2] = now_position[2] + 3
	end
	if down >= 1 and can_move_down then
		now_position[2] = now_position[2] - 3
	end
	player_sprite.MoveToAbs(now_position[1], now_position[2])
	Encounter["_player_position"] = now_position
	CheckEncount(left, right, up, down, can_move_left, can_move_right, can_move_up, can_move_down)
	--DEBUG(now_position[1] .. ", " .. now_position[2])
end

function InputUpdate()
	local left = Input.Left
	local right = Input.Right
	local up = Input.Up
	local down = Input.Down
	if event == "" then
		PlayerSpriteUpdate(left, right, up, down)
		PlayerPositionUpdate(left, right, up, down)
	end
	if Input.Menu == 1 and event == "" then
		StartDialog("NoMenu", "Apparently, AsteriskMod's Dev\rdidn't make Menu.[w:4]\nHe may be too lazy.")
	end
end

function EndDialog()
	dialog_text.alpha = 0
	for i = 1, 2 do
		dialog_boxes[i].alpha = 0
	end
	dialog_event = false
	event = ""
end

function DialogUpdate()
	if not dialog_event then return end
	if not dialog_text.lineComplete then return end
	if Input.Confirm ~= 1 then return end
	dialog_event_id = dialog_event_id + 1
	if dialog_event_texts[dialog_event_id] == nil then
		EndDialog()
	else
		dialog_text.SetText(dialog_event_texts[dialog_event_id])
	end
end

function EncountUpdate()
	if not encount_event then return end
	if encount_framecounter == 0 then
		encount_bubble.alpha = 1
	end
	encount_framecounter = encount_framecounter + 1
	if encount_framecounter == 6 then
		Audio.PlaySound("BeginBattle1")
	end
	if encount_framecounter == 38 then
		encount_bubble.alpha = 0
		player_sprite.layer = "RoomEnc"
		player_sprite.SendToTop()
		encount_mask.alpha = 1
	end
	if encount_framecounter >= 40 and encount_framecounter <= 56 and encount_framecounter % 4 == 0 then
		if encount_framecounter % 8 == 0 then
			Audio.PlaySound("BeginBattle2")
		end
		encount_soul.alpha = (encount_soul.alpha + 1) % 2
	end
	if encount_framecounter == 60 then
		Audio.PlaySound("BeginBattle3")
		player_sprite.layer = "Room"
	end
	if encount_framecounter > 60 and encount_framecounter <= 102 then
		encount_soul.Move(encount_velocity[1], encount_velocity[2])
	end
	if encount_framecounter == 102 then
		Encounter.Call("PrepareBattle", encount_event_battleID)
	end
end

function Update()
	RemoveMaskUpdate()
	InputUpdate()
	RoomUpdate()
	DialogUpdate()
	EncountUpdate()
end

function StateEnding(newState)
	encount_bubble.Remove()
	dialog_text.Remove()
	for i = 1, 2 do
		dialog_boxes[i].Remove()
	end
	dialog_boxes = {}
	bg.Remove()
	player_sprite.Remove()
	encount_mask.Remove()
	encount_soul.Remove()
	StateEditor.SetCurrentAction("FIGHT", false)
end
