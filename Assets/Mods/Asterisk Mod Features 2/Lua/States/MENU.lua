local texts = {}
local menus = {}
local c_text = {}
local selecting = 1
function NewText(text, x, y, color)
	if color == nil then color = {1, 1, 1} end
	local text = CreateText(text, {x, y}, 65536, "RoomForeground")
	text.HideBubble()
	text.progressmode = "none"
	text.color = color
	return text
end

function StateStarting()
	local name = GetAlMightyGlobal("*CYF-Example-OnActive-Name")
	if name == nil then name = "" end
	local time = 0
	local room = -1
	local prefix = "[font:uidialog][instant]"
	local text = name
	local space_count = 10 - string.len(name)
	if space_count > 0 then
		for i = 1, space_count do
			text = text .. " "
		end
	end
	text = text .. "LV 1"
	texts[1] = NewText(prefix..text, 128, 330)
	texts[2] = NewText(prefix.."0:00", 456, 330)
	menus[1] = NewText(prefix.."Continue", 170, 245)
	menus[2] = NewText(prefix.."Reset", 390, 245)
	menus[3] = NewText(prefix.."Settings", 264, 204)
	menus[3].color = {0.5, 0.5, 0.5}
	if Encounter["__Menu_from_name"] ~= nil then
		selecting = 2
		Encounter["__Menu_from_name"] = nil
	end
	menus[selecting].color = {1, 1, 0}
	c_text[1] = NewText("[font:uibattlesmall][instant]CYF ASTERISK MOD V" .. AsteriskVersion .. "  NIL256  2022", 140, 20, {0.5, 0.5, 0.5})
	c_text[1].absx = 320 - (c_text[1].GetTextWidth() / 2 * 0.75)
	c_text[1].Scale(0.75, 0.75)
	c_text[2] = NewText("[font:uibattlesmall][instant]CYF V0.6.5  RHENAUD THE LUKARK   UNDERTALE  TOBY FOX", 140, 6, {0.5, 0.5, 0.5})
	c_text[2].absx = 320 - (c_text[2].GetTextWidth() / 2 * 0.75)
	c_text[2].Scale(0.75, 0.75)
end

function HandleArrows(left, right, up, down)
	if not left and not right then return end
	menus[selecting].color = {1, 1, 1}
	selecting = selecting % 2 + 1
	menus[selecting].color = {1, 1, 0}
end

function HandleAction()
	if selecting == 1 then
		Encounter["__continue"] = true
		Encounter.Call("PrepareItemFunEvent")
		Encounter.Call("PrepareRoom")
		return
	elseif selecting == 2 then
		Encounter["__Name_from_menu"] = true
		Encounter["customstatename"] = "NAME"
	end
	State("CUSTOMSTATE")
end

function StateEnding()
	for i = 1, 2 do
		texts[i].Remove()
		c_text[i].Remove()
	end
	for i = 1, 3 do
		menus[i].Remove()
	end
	texts = {}
	menus = {}
	c_text = {}
end
