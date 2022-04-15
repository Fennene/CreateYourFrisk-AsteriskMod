--[[

-- The StateEditor Object --

In State Scripts, we can use the special object that can not use in other scripts.
It's The StateEditor Object.

StateEditor object is created from a part of CYF's codes and adjusted for CustomState.

By the way, do you know how to be written each states in CYF?
Each states are some differences, but they are written to focus on below points basically.
	1. Whether shows or hides the Player's soul.
	2. (In 1, if shows the soul:) Where is the soul placed?
	3. (In 1, if shows the soul:) Whether the Player can control the soul freely or not.
	4. Whether the text in arena (dialog box) show any texts or hide.
	5. (In 4, if shows any texts:) How and What does the text show. (How: like DIALOGRESULT? ACTMENU? ITEMMENU?)
	6. Whether each buttons is active (colored yellow, normally) or inactive (colored orange, normally).
	7. Whether arena is resized to arenasize (Encounter Sctipts' variable) or not.
In CYF, there is the function you can do things #1, #2 #3 and #7.
However, there is not functions of controlling the text in CYF.
That the functions is The StateEditor Object in AsteriskMod.
Let's check that so let's go to StateStarting()
]]

-- Variables --
local current_inventory = Encounter["inventory"]
local selection = 1 -- the Player's select 1 ~ #inventory
local display_selection = 1 -- the soul's position that is depended on selection.
-- Variables(UI) --
local boxes = {}
local arrows = {}
local frame_counter = 0

local function GetItemTexts()
	local firstInventoryIndex = selection - (display_selection - 1)
	local displayItems = {}
	for i = firstInventoryIndex, math.min(firstInventoryIndex + 2, #current_inventory) do
		table.insert(displayItems, current_inventory[i])
	end
	return displayItems
end

-- This runs once on entering this state. | This is called after calling EnteringState().
function StateStarting(oldState) -- required
	-- In ITEMMENU, the Player's soul is shown, the Player can not control freely, and move on the options (selections)
	StateEditor.SetPlayerVisible(true) -- The CYF's way the soul is shown is not using Player.alpha. -- #1
	Player.SetControlOverride(true) -- #3
	-- We have not write codes about the soul's position, but that is recommended after writing codes about the texts.

	-- Next, we need to show items as like ACTMENU.
	-- If you wanna shows the text lined up, you can do by using StateEditor.SetChoicesDialogText().
	StateEditor.SetChoicesDialogText(GetItemTexts(), true) -- #4 & #5
	-- If StateEditor.SetChoicesDialogText()'s argument#2 is false, texts is lined up as like normal ITEMMENU.

	-- We'll need to make only ITEM button active (will color yellow).
	StateEditor.SetButtonActive(false, false, true, false) -- FIGHT, ACT, ITEM, MERCY -- #6
	-- However, we don't have to write StateEditor.SetButtonActive()
	--    'cause this is not written in all states and called only if we wanna inactive all buttons basically in CYF.

	-- Finally, sets the position of the soul.
	StateEditor.SetPlayerOnSelection(display_selection, true) -- #2
	-- If StateEditor.SetChoicesDialogText()'s argument#2 is false,
	--     we should assgin StateEditor.SetPlayerOnSelection()'s argument#2 to false.

	-- Actually, you don't have to call that all function.
	-- At this time, we have set the timing of entering this state.
	-- That timing is if the Player try to enter normal ITEMMENU.
	-- Therefore really needed codes are below.
	--[[
	StateEditor.SetChoicesDialogText(GetItemTexts(), true)
	StateEditor.SetPlayerOnSelection(display_selection, true)
	]]
	-- ...if objects we have to prepare are only texts and soul.

	-- If you wanna modify arena, use The Arena Object.
	-- Moreover, you should call StateEditor.ResetArena() in StateEnding() if you do.

	-- There are boxes and arrows that shows about items in Japanese-styled ITEMMENU.
	-- Below codes don't have any secret or special, so I don't describe them.
	for i = 1, #current_inventory do
		boxes[i] = CreateSprite("spr_itemmenu_square", "BelowBullet")
		local absy = 160 + (((#current_inventory - 1) / 2) * 10) - ((i - 1) * 10)	
		boxes[i].MoveToAbs(590, absy)
	end
	boxes[1].Scale(2, 2)
	for i = 1, 2 do
		arrows[i] = CreateSprite("spr_itemmenu_arrow", "BelowBullet")
		local absy = 10 + (((#current_inventory - 1) / 2)) * 10
		absy = 160 + absy * math.pow(-1, i - 1)
		arrows[i].MoveToAbs(590, absy)
		arrows[i]["init_absy"] = absy
		arrows[i].rotation = 180 * (i - 1)
	end
	arrows[1].alpha = 0
	if #current_inventory <= 1 then
		arrows[2].alpha = 0
	end
	-- We'll done!
	-- That's all. Enjoy.
end

-- This runs for a frame the Player presses Z or Enter key.
function HandleAction()
	local target_name = Encounter["inventory"][selection]
	table.remove(Encounter["inventory"], selection)
	Audio.PlaySound("menuconfirm")
	Encounter.Call("HandleItem", {target_name, selection})
end

-- This runs for a frame the Player presses any arrows key.
function HandleArrows(left, right, up, down)
	if not up and not down then return end
	boxes[selection].Scale(1, 1)
	if up and selection > 1 then
		selection = selection - 1
		display_selection = math.max(1, display_selection - 1)
		Audio.PlaySound("menumove")
	end
	if down and selection < #current_inventory then
		selection = selection + 1
		display_selection = math.min(display_selection + 1, 3)
		Audio.PlaySound("menumove")
	end
	if selection == 1 then arrows[1].alpha = 0
	else                   arrows[1].alpha = 1 end
	if selection == #current_inventory then arrows[2].alpha = 0
	else                                    arrows[2].alpha = 1 end
	boxes[selection].Scale(2, 2)
	StateEditor.SetPlayerOnSelection(display_selection, true)
	StateEditor.SetChoicesDialogText(GetItemTexts(), true)
end

-- This runs for a frame the Player presses X or Shift key.
function HandleCancel()
	State("ACTIONSELECT")
end

-- This runs for every frame on this state.
function Update()
	frame_counter = (frame_counter + 1) % 60
	local add_y = 0
	if frame_counter > 25 then
		add_y = 3
	elseif frame_counter > 15 then
		add_y = 2
	elseif frame_counter > 5 then
		add_y = 1
	end
	arrows[1].absy = arrows[1]["init_absy"] + add_y
	arrows[2].absy = arrows[2]["init_absy"] - add_y
end

-- This is called after calling EnteringState() when State() is called in this script.
function StateEnding(newState) -- required
	arrows[1].Remove()
	arrows[2].Remove()
	for i = 1, #boxes do
		boxes[i].Remove()
	end
end