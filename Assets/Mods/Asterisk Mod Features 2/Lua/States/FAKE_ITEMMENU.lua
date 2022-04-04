local selection = 1
local displayed_selection = 1
local items = {}
local itemheals = {}
local frame_counter = 0
local arrows = {}
local squares = {}
local heal_info = {}

function UpdateItemDisplay()
	squares[selection].Scale(2, 2)
	if selection == 1 then
		arrows[1].alpha = 0
	else
		arrows[1].alpha = 1
	end
	if selection == #items then
		arrows[2].alpha = 0
	else
		arrows[2].alpha = 1
	end
	local first_item_index = selection - (displayed_selection - 1)
	local display_items = {}
	for i = first_item_index, first_item_index + 2 do
		if items[i] ~= nil then
			display_items[#display_items + 1] = items[i]
			local heal_text = itemheals[items[i]]
			if heal_text == -1 then
				heal_text = "MAX"
				heal_info[i - first_item_index + 1].color = {0.1, 1, 0.1}
			elseif heal_text <= -2 then
				heal_text = "???"
				heal_info[i - first_item_index + 1].color = {0.5, 0.5, 0.5}
			else
				heal_info[i - first_item_index + 1].color = {0.1, 0.8, 0.1}
			end
			heal_info[i - first_item_index + 1].SetText("[font:uibattlesmall][instant]"..heal_text)
		else
			heal_info[i - first_item_index + 1].color = {0.5, 0.5, 0.5}
			heal_info[i - first_item_index + 1].SetText("[font:uibattlesmall][instant]")
			break
		end
	end
	StateEditor.SetChoicesDialogText(display_items, true)
end

function StateStarting(oldState)
	items = Encounter["item_inventory"]
	itemheals = Encounter["_item_healvalue"]
	for i = 1, #items do
		squares[i] = CreateSprite("spr_itemmenu_square", "BelowBullet")
		local absy = 160 + (((#items - 1) / 2) * 10) - ((i - 1) * 10)
		squares[i].MoveToAbs(590, absy)
	end
	arrows[1] = CreateSprite("spr_itemmenu_arrow", "BelowBullet")
	arrows[1].MoveToAbs(590, 160 + 10 + (((#items - 1) / 2)) * 10)
	arrows[1]["init_y"] = arrows[1].absy
	arrows[1].alpha = 0
	arrows[2] = CreateSprite("spr_itemmenu_arrow", "BelowBullet")
	arrows[2].MoveToAbs(590, 160 - 10 - (((#items - 1) / 2)) * 10)
	arrows[2].rotation = 180
	arrows[2]["init_y"] = arrows[2].absy
	arrows[2].alpha = 0
	for i = 1, 3 do
		local y = ArenaUtil.centerabsy - 8
		y = y + (2 - i) * 32
		heal_info[i] = CreateText("[font:uibattlesmall][instant]+??", {320, y}, 256, "RoomDialogText")
		heal_info[i].HideBubble()
		heal_info[i].progressmode = "none"
		heal_info[i].color = {0.1, 0.8, 0.1}
	end
	UpdateItemDisplay()
	StateEditor.SetPlayerOnSelection(displayed_selection, true)
end

function Update()
	frame_counter = frame_counter + 1
	frame_counter = frame_counter % 60
	local add_y = 0
	if frame_counter <= 5 then
		add_y = 0
	elseif frame_counter <= 15 then
		add_y = 1
	elseif frame_counter <= 25 then
		add_y = 2
	elseif frame_counter <= 60 then
		add_y = 3
	end
	arrows[1].MoveToAbs(arrows[1].absx, arrows[1]["init_y"] + add_y)
	arrows[2].MoveToAbs(arrows[2].absx, arrows[2]["init_y"] - add_y)
end

function HandleCancel() -- optional
	State("ACTIONSELECT")
end

function HandleArrows(left, right, up, down)
	if not up and not down then
		return
	end
	squares[selection].Scale(1, 1)
	if up and selection > 1 then
		selection = selection - 1
		if displayed_selection > 1 then
			displayed_selection = displayed_selection - 1
		end
		Audio.PlaySound("menumove")
	end
	if down and selection < #items then
		selection = selection + 1
		if displayed_selection < 3 then
			displayed_selection = displayed_selection + 1
		end
		Audio.PlaySound("menumove")
	end
	UpdateItemDisplay()
	StateEditor.SetPlayerOnSelection(displayed_selection, true)
end

function HandleAction()
	local target_name = Encounter["item_inventory"][selection]
	if target_name ~= "Love" then
		table.remove(Encounter["item_inventory"], selection)
	end
	Audio.PlaySound("menuconfirm")
	Encounter.Call("HandleItem", {target_name, selection})
end

function StateEnding(newState)
	for i = 1, 3 do
		heal_info[i].Remove()
	end
	for i = 1, #squares do
		squares[i].Remove()
	end
	arrows[1].Remove()
	arrows[2].Remove()
end

--[[
function PlayerHeal(amount, inv_time)
	Encounter.Call("PlayerHeal", {amount, inv_time})
end

function _UseLoveItem()
	Encounter.Call("_UseLoveItem")
end]]