local selection = 1
local icons = {}
local animation = 0
local animation_frame = 0
local text = nil
local squares = {}

function StateStarting(oldState)
	StateEditor.HideDialogText()
	local battles = Encounter["_battles"]
	selection = 1
	for i = 1, #battles do
		local path_root = "Icons/" .. battles[i] .. "/"
		icons[i] = {}
		local icon_path = "icon_silhouette"
		if Encounter["_savedatas"][i] > 0 then
			icon_path = "icon"
		end
		icons[i].icon = CreateSprite(path_root .. icon_path, "Menu")
		icons[i].frame = CreateSprite(path_root .. "frame", "Menu")
		icons[i].frame.color = {0.5, 0.5, 0.5}
		icons[i].icon.MoveTo(320 + ((i - 1) * 300), 240)
		icons[i].frame.MoveTo(320 + ((i - 1) * 300), 240)
		squares[i] = CreateSprite("px", "Menu")
		local absx = 320 - (((#battles - 1) / 2) * 10) + ((i - 1) * 10)
		squares[i].MoveToAbs(absx, 40)
		squares[i].Scale(4, 4)
	end
	icons[selection].icon.Scale(2, 2)
	icons[selection].frame.Scale(2, 2)
	icons[selection].frame.color = {1, 1, 1}
	squares[selection].Scale(8, 8)
	text = CreateText("[font:uidialog][instant]"..battles[selection], {-256, 430}, 65536, "Menu")
	text.HideBubble()
	text.progressmode = "none"
	text.Scale(2, 2)
	text.absx = 320 - text.GetTextWidth()
end

function HandleArrows(left, right, up, down)
	if animation ~= 0 then return end
	if not left and not right then return end
	Audio.PlaySound("menumove")
	if left and selection > 1 then
		animation = -1
		animation_frame = 0
	end
	if right and selection < #Encounter["_battles"] then
		animation = 1
		animation_frame = 0
	end
end

function Update()
	if animation == 0 then return end
	animation_frame = animation_frame + 1
	if animation_frame == 1 then
		icons[selection].frame.color = {0.5, 0.5, 0.5}
		squares[selection].Scale(4, 4)
		text.alpha = 0
	end
	local scale = 2 - (1 * (animation_frame / 10))
	icons[selection].icon.Scale(scale, scale)
	icons[selection].frame.Scale(scale, scale)
	if animation == -1 then
		for i = 1, #icons do
			icons[i].icon.Move(30, 0)
			icons[i].frame.Move(30, 0)
		end
		scale = 1 + (animation_frame / 10)
		icons[selection - 1].icon.Scale(scale, scale)
		icons[selection - 1].frame.Scale(scale, scale)
	elseif animation == 1 then
		for i = 1, #icons do
			icons[i].icon.Move(-30, 0)
			icons[i].frame.Move(-30, 0)
		end
		scale = 1 + (animation_frame / 10)
		icons[selection + 1].icon.Scale(scale, scale)
		icons[selection + 1].frame.Scale(scale, scale)
	end
	if animation_frame == 10 then
		if animation == -1 then
			selection = selection - 1
		elseif animation == 1 then
			selection = selection + 1
		end
		icons[selection].icon.Scale(2, 2)
		icons[selection].frame.Scale(2, 2)
		icons[selection].frame.color = {1, 1, 1}
		squares[selection].Scale(8, 8)
		text.SetText("[font:uidialog][instant]"..Encounter["_battles"][selection])
		text.absx = 320 - text.GetTextWidth()
		text.alpha = 1
		animation_frame = 0
		if animation == -1 and Input.Left == 2 and selection > 1 then return end
		if animation == 1 and Input.Right == 2 and selection < #Encounter["_battles"] then return end
		animation = 0
	end
end

function HandleAction()
	if animation ~= 0 then return end
	Audio.PlaySound("menuconfirm")
	Encounter.Call("_LoadBattle", selection)
end

function StateEnding(newState)
	for i = #icons, 1, -1 do
		icons[i].icon.Remove()
		icons[i].frame.Remove()
		squares[i].Remove()
		icons[i] = nil
	end
	text.Remove()
end