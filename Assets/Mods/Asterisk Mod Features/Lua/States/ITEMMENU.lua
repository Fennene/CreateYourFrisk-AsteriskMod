local selection = 1
local displayed_selection = 1
local items = {}
local frame_counter = 0
local arrows = {}
local squares = {}

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
		else
			break
		end
	end
	StateEditor.SetChoicesDialogText(display_items, true) -- if Argument#2 is false, selections are lined up like ACTMENU.
end

function StateStarting(oldState) -- requited
	items = Encounter["item_inventory"]
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
	UpdateItemDisplay()
	StateEditor.SetPlayerOnSelection(displayed_selection, true)
end

function Update() -- optional
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

function HandleArrows(left, right, up, down) -- optional
	if not up and not down then -- all vars (left, right, up & down) are boolean
		return
	end
	squares[selection].Scale(1, 1)
	if up and selection > 1 then -- This is simillary to "if Input.Up == 1 and selection > 1 then"
		selection = selection - 1
		if displayed_selection > 1 then
			displayed_selection = displayed_selection - 1
		end
		Audio.PlaySound("menumove")
	end
	if down and selection < #items then -- This is simillary to "if Input.Down == 1 and selection > 1 then"
		selection = selection + 1
		if displayed_selection < 3 then
			displayed_selection = displayed_selection + 1
		end
		Audio.PlaySound("menumove")
	end
	UpdateItemDisplay()
	StateEditor.SetPlayerOnSelection(displayed_selection, true)
end

function HandleAction() -- optional
	local target_name = Encounter["item_inventory"][selection]
	table.remove(Encounter["item_inventory"], selection)
	Audio.PlaySound("menuconfirm")
	Encounter.Call("HandleItem", {target_name, selection})
end

function StateEnding(newState) -- requited
	for i = 1, #squares do
		squares[i].Remove()
	end
	arrows[1].Remove()
	arrows[2].Remove()
end
