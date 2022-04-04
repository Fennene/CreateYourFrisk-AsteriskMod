local texts = {}

function NewText(text, x, y, color32)
	if color32 == nil then color32 = {150, 150, 150} end
	local text = CreateText("[font:uidialog][instant]"..text, {x, y}, 65536, "RoomDialogText")
	text.HideBubble()
	text.progressmode = "none"
	text.color32 = color32
	return text
end

function StateStarting(oldState)
	texts[1] = NewText("---Instruction---", 178, 414)
	texts[2] = NewText("[Z or ENTER] - Confirm", 160, 356)
	texts[3] = NewText("[X or SHIFT] - Cancel", 160, 318)
	--[[
	texts[4] = NewText("[C or CTRL] - Menu(In-Game)", 160, 282)
	texts[5] = NewText("[F4] - Fullscreen", 160, 246)
	texts[6] = NewText("[HOLD ESC] - Quit", 160, 210)
	texts[7] = NewText("When HP is 0, you lose.", 160, 172)
	--]]
	texts[4] = NewText("[F4] - Fullscreen", 160, 282)
	texts[5] = NewText("[HOLD ESC] - Quit", 160, 246)
	texts[6] = NewText("When HP is 0, you lose.", 160, 210)
	texts[7] = NewText("Begin game", 170, 110, {255, 255, 0})
	texts[8] = NewText("Settings", 170, 68, {127, 127, 127})
	texts[9] = NewText("[font:uibattlesmall]CYF ASTERISK MOD V" .. AsteriskVersion .. "  NIL256  2022", 140, 20, {127, 127, 127})
	texts[9].absx = 320 - (texts[9].GetTextWidth() / 2 * 0.75)
	texts[9].Scale(0.75, 0.75)
	texts[10] = NewText("[font:uibattlesmall]CYF V0.6.5  RHENAUD THE LUKARK   UNDERTALE  TOBY FOX", 140, 6, {127, 127, 127})
	texts[10].absx = 320 - (texts[10].GetTextWidth() / 2 * 0.75)
	texts[10].Scale(0.75, 0.75)
end

function HandleAction()
	Encounter["customstatename"] = "NAME"
	--Encounter["__Name_from_menu"] = false
	State("CUSTOMSTATE")
end

function StateEnding(newState)
	for i = 1, #texts do
		texts[i].Remove()
	end
end
