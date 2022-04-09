-- Variables --
local selection = 1 -- selecting item's index  between 1 ~ #inventory
local displayed_selection = 1 -- between 1 ~ 3
local items = {} -- items' names
-- UI --
local arrows = {}
local squares = {}
local framecounter = 0
-- Functions --
local function UpdateDisplayingItems()
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

-- System(Main) --
-- This is called on entering this state.
function StateStarting(oldState) -- requited
	items = Encounter["inventory"]
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
	UpdateDisplayingItems()
	StateEditor.SetPlayerOnSelection(displayed_selection, true)
end

-- This is called on the first frame the confirm([Z] or [Enter]) key is pressed by the Player.
function HandleAction() -- optional
	local target_name = Encounter["inventory"][selection]
	table.remove(Encounter["inventory"], selection)
	Audio.PlaySound("menuconfirm")
	Encounter.Call("HandleItem", {target_name, selection})
end

-- This is called on the first frame the any arrows key is pressed by the Player.
function HandleArrows(left, right, up, down) -- optional
	if not up and not down then -- Note that all args (left, right, up & down) are boolean.
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
	UpdateDisplayingItems()
	StateEditor.SetPlayerOnSelection(displayed_selection, true)
end

-- This is called on the first frame the cancel([X] or [Shift]) key is pressed by the Player.
function HandleCancel() -- optional
	State("ACTIONSELECT") -- Go back
end

-- Update function.
function Update() -- optional
	framecounter = framecounter + 1
	framecounter = framecounter % 60
	local add_y = 0
	if framecounter <= 5 then
		add_y = 0
	elseif framecounter <= 15 then
		add_y = 1
	elseif framecounter <= 25 then
		add_y = 2
	elseif framecounter <= 60 then
		add_y = 3
	end
	arrows[1].MoveToAbs(arrows[1].absx, arrows[1]["init_y"] + add_y)
	arrows[2].MoveToAbs(arrows[2].absx, arrows[2]["init_y"] - add_y)
end

-- This is called on exiting this state. (For example, on calling State())
function StateEnding(newState)
	for i = 1, #squares do
		squares[i].Remove()
	end
	arrows[2].Remove()
	arrows[1].Remove()
end