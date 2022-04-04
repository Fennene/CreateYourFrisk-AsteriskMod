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


function RoomUpdate()
	if Input.GetKey("Tab") == 1 then
		--StartDialog("TEST", {"This is the test dialog."})
		Encount("TEST", true)
	end
end







function StateStarting(oldState)
	bg = CreateSprite("room/rooms/spr_pre_st_2", "RoomBackground")
	player_sprite = CreateSprite(PLAYER_SPRITE_DIR..player_anim_index, "Room")
	dialog_boxes[1] = NewSquare(321, 394, 578, 152, {1, 1, 1}, true, "RoomDialog")
	dialog_boxes[2] = NewSquare(321, 394, 566, 140, {0, 0, 0}, true, "RoomDialog")
	dialog_text = NewText(DIALOG_PREFIX, 62, 422, true, "RoomDialogText")
	encount_bubble = CreateSprite("room/players/spr_encount_1", "RoomEnc")
	encount_bubble.alpha = 0
	encount_mask = CreateSprite("px", "RoomEnc")
	encount_mask.alpha = 0
	encount_mask.color = {0, 0, 0}
	encount_mask.Scale(640, 480)
	encount_soul = CreateSprite("ut-heart", "RoomEncSoul")
	encount_soul.alpha = 0
	encount_soul.color = {1, 0, 0}
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
	local can_move_left = (now_position[1] > 100)
	local can_move_right = (now_position[1] < 580)
	local can_move_up = (now_position[2] < 336)
	local can_move_down = (now_position[2] > 192)
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
	DEBUG(now_position[1] .. ", " .. now_position[2])
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
		--[[
		TODO
		]]
		DEBUG("Encount Event Completed")
	end
end

function Update()
	InputUpdate()
	RoomUpdate()
	DialogUpdate()
	EncountUpdate()
end

function StateEnding(newState)
	encount_bubble.Remove()
	for i = 1, 2 do
		dialog_boxes[i].Remove()
	end
	dialog_boxes = {}
	bg.Remove()
	player_sprite.Remove()
	encount_mask.Remove()
	encount_soul.Remove()
end
