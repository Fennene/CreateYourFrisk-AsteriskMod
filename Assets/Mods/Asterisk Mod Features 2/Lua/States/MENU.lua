--[[

-- State Scripts --

This is State Script. You can also use The Arena Object and The StateEditor Object.
In this time, we'll create Title Screen, Menu Screen, and Fake GameOver Screen.

StateStarting() and StateEnding() is required in State Script, so you should write them.
You can add Update(), OnHit(), HandleAction(), HandleArrows(), and HandleCancel() events.
An overview of each function can be found in the comments above the function.

If you want to take a closer look, use the following code to stop the execution.
-- if true then return end

At first, check out StateStarting().
]]

-- Variables(Objects) --
local title = nil
local sub_title = nil
local menu_texts = {}
soul = nil

-- Variables(Control) --
local frame_counter = 0
local selection = 1


-- Enum --
-- We can NOT get Table value from other scripts... (excepted SetGlobal()/GetGlobal maybe)
GAMEMODE = {}
GAMEMODE.TITLE = 0
GAMEMODE.MENU = 1
GAMEMODE.GAME = 2
DIFFICULTY = {}
DIFFICULTY.EASY = 0
DIFFICULTY.MEDIUM = 1
DIFFICULTY.HARD = 2
DIFFICULTY.LUNATIC = 3
MAIN_COLOR32 = {r = 200, g = 127, b = 127}
MAIN_COLOR_HEX = "c87f7f"

-- This runs once on entering this state. | This is called after calling EnteringState().
function StateStarting(--[[oldState]]) -- required
	-- This is useful to initialize variables and create new object.
	-- Let's create title.
	title = CreateSprite("cyf_title", "Top")
	-- There is no problem and it is very simple, isn't it?
	-- Let's add other objects and modify them.
	sub_title = CreateStaticText("uidialog", "Hard Mode? Poseur Fight", {0, -40}, 1023, "Top")
	sub_title.MoveToAbs(320 - sub_title.width / 2, 240) -- sub_title.MoveToAbs(320 - sub_title.GetTextWidth() / 2, 240)
	sub_title.color32 = {MAIN_COLOR32.r, MAIN_COLOR32.g, MAIN_COLOR32.b}
	title.Move(0, 30)
	sub_title.Move(0, -30) -- Yeah! You can text.Move() on Asterisk Mod!!!!!!!!
	-- Good, we have done preparing objects of title.
	-- Generally, you create title and menu separately but we create title and menu at same time in this script.
	-- Let's prepare objects for menu.
	-- title.Move(0, 150) -- for checking the positions of menu objects
	-- sub_title.Move(0, 150) -- for checking the positions of menu objects
	local MENU_TEXTS = {"Start", "Difficulty: Medium", "NoHit: Off", "Quit"}
	for i = 1, #MENU_TEXTS do
		menu_texts[i] = CreateStaticText("uidialog", MENU_TEXTS[i], {0, -40}, 1023, "Top")
	end
	for i = 1, #MENU_TEXTS do
		-- Sets the position based on the longest object.
		-- There are some number values without no description, but these are just values to adjust the positions.
		menu_texts[i].MoveToAbs(320 - menu_texts[2].width / 2 + 20, 240 - 16 - ((i - 1) * 40))
		-- hide
		menu_texts[i].Move(0, -300)
	end
	soul = CreateSprite("ut-heart", "Top")
	soul.alpha = 0
	soul.MoveToAbs(180, 234)
	soul.color = {1, 0, 0}
	-- Finally, sets sound.
	Audio.PlaySound("intro_noise")
	-- We've done!
	-- Next, Let's set animation of moving objects and shows menu so check out Update(). 
end


-- This runs for every frame on this state.
function Update()
	-- Next step, what we need to do is moveing objects (animation).
	-- First, we should check to whether animation is finished.
	-- Defining variables to check completing animation is not wrong,
	--     but I have already prepared the variable to check game's state
	--     so we'll use it.
	if Encounter["_game_mode"] ~= GAMEMODE.TITLE then return end -- Early Return
	frame_counter = frame_counter + 1
	if frame_counter > 60 then
		title.Move(0, 3)
		sub_title.Move(0, 3)
		for i = 1, #menu_texts do
			menu_texts[i].Move(0, 6)
		end
	end
	if frame_counter == 110 then -- 60 + 50 (start frame of animation + total time of animation)
		-- shows soul
		soul.alpha = 1
		menu_texts[1].color = {1, 1, 0}
		Audio.PlaySound("menumove")
		Encounter["_game_mode"] = GAMEMODE.MENU
	end
	-- We'll done. I recommend you play and check this animation.
	-- Honestly this animation is little long, is it?
	-- Let's make it allow to skip, so we need to HandleCancel().
end


-- This runs for a frame the Player presses X or Shift key.
function HandleCancel()
	-- All "Handle" functions are called for one frame that the Player press each key.
	-- If that key is Z or Enter, it runs HandleAction(),
	-- If that key is X or Shift, it runs HandleCancel(),
	-- If that key is any arrow, it runs HandleArrows().
	-- Let's write codes to skip
	if Encounter["_game_mode"] ~= GAMEMODE.TITLE then return end -- Early Return
	-- stops audio noise
	Audio.StopAll()
	-- sets the position of objects
	title.absy = 420
	sub_title.absy = 360
	for i = 1, #menu_texts do
		menu_texts[i].absy = 264 - i * 40
	end
	-- code from Update()
	soul.alpha = 1
	menu_texts[1].color = {1, 1, 0}
	Audio.PlaySound("menumove")
	Encounter["_game_mode"] = GAMEMODE.MENU	
	-- Hm? "We can do same thing by using The Input Object and Update(), cannot we?"
	-- Ah, that's right.
	-- you can do it if you don't like this method or you {like/wanna} {writing/write} codes complexly.
	-- Next, check out HandleArrows()
end


local function ChangeDifficulty(easier)
	local diff = Encounter["_difficulty"]
	if not easier then
		if diff == DIFFICULTY.LUNATIC then diff = DIFFICULTY.EASY
		else                               diff = diff + 1   end
	else
		if diff == DIFFICULTY.EASY then diff = DIFFICULTY.LUNATIC
		else                            diff = diff - 1       end
	end
	Encounter["_difficulty"] = diff
	local text = "UNKNWON (ERROR)"
	if     diff == DIFFICULTY.EASY    then text = "Easy"
	elseif diff == DIFFICULTY.MEDIUM  then text = "Medium"
	elseif diff == DIFFICULTY.HARD    then text = "Hard"
	elseif diff == DIFFICULTY.LUNATIC then text = "Lunatic"
	end
	menu_texts[2].SetText("Difficulty: " .. text)
end

local function ChangeNoHitMode()
	Encounter["_no_hit_mode"] = not Encounter["_no_hit_mode"]
	local text = "Off"
	if Encounter["_no_hit_mode"] then text = "On" end
	menu_texts[3].SetText("NoHit: " .. text)
end


-- This runs for a frame the Player presses any arrows key.
function HandleArrows(left, right, up, down) -- Note that arguments are boolean
	-- We finished to modify most objects.
	-- However we have not create important thing, it's control of menu.
	-- There is nothing special here excepted that arguments are boolean.
	-- I don't write specific description of  the codes here.
	if Encounter["_game_mode"] ~= GAMEMODE.MENU then return end -- Early Return is {good/nice/cool} in this code.
	-- change value -- (This makes codes a little complex.)
	if selection == 2 and (left or right) then
		ChangeDifficulty(left)
		Audio.PlaySound("menumove")
	elseif selection == 3 and (left or right) then
		ChangeNoHitMode()
		Audio.PlaySound("menumove")
	end
	-- main --
	if not up and not down then return end -- This code is based on CYF code, but you don't have to follow when you don't like it.
	menu_texts[selection].color = {1, 1, 1}
	if up then -- This is same to "if Input.Up == 1" in Update().
		if selection > 1 then
			selection = selection - 1
			Audio.PlaySound("menumove")
		end
	end
	if down then -- This is same to "if Input.Down == 1" in Update().
		if selection < #menu_texts then
			selection = selection + 1
			Audio.PlaySound("menumove")
		end
	end
	menu_texts[selection].color = {1, 1, 0}
	soul.MoveToAbs(180, 274 - selection * 40)
	-- Let's go next step, we'll check HandleAction()
end


-- This runs for a frame the Player presses Z or Enter key.
function HandleAction()
	-- All step is left is 2 steps contained this. (on this state)
	if Encounter["_game_mode"] ~= GAMEMODE.MENU then return end -- Early Return
	if selection == 1 then -- "Start"
		-- Wait wait wait. There are left some steps on this State Script.
		State("*START_ENCOUNT")
	elseif selection == 2 then -- "Difficulty"
		ChangeDifficulty(false)
	elseif selection == 3 then -- "NoHit"
		ChangeNoHitMode()
	elseif selection == 4 then -- "Quit"
		-- We can end the mod(game) by calling State("DONE") also in state script.
		State("DONE")
	end
	Audio.PlaySound("menuconfirm")
	-- Leftover step on this State Script is only StateEnding().
	-- Here we go.
end

-- This is called after calling EnteringState() when State() is called in this script.
function StateEnding(--[[newState]]) -- required
	-- You should write code removes objects that is created on this state.
	for i = 1, #menu_texts do
		menu_texts[i].Remove()
	end
	title.Remove()
	sub_title.Remove()
	-- There is the object that have not removed, isn't it?
	-- Yes, it's soul.
	-- However, that will be used on next state "START_ENCOUNT"
	-- so we leave it on purpose.
	-- Anyways, We'll done!
	-- welp... we'll take a break?
	-- If you are ready, we need to see the codes in EnteringState() so we'll go back Encounter Script!
end