--[[
------------------------------------------------------------------------------------------------------------------
                                         FN! Purple Soul Library
------------------------------------------------------------------------------------------------------------------
by Nil256 [Fennene]

This code is NOT from Undertale, It's Nil256's unique code.
frame based (60 fps)

< functions >
PurpleSoul.SetPurple(|bool|active, |bool?|changeColor = true, |bool?|controlOverride = true)
	Changes Soul Mode
	active: false/true
	changeColor: nil/false/true - Color the Player soul or don't
	controlOverride: nil/false/true - Call Player.ControlOverride() or not (only if active is false)
PurpleSoul.CreateNet(|num|netAmount, |bool?|horizontal = true)
	Creates Nets
	netAmount: 0 < |int|
	horizontal: nil/false/true
PurpleSoul.RemoveNet()
	Removes Nets
MoveToNet(|num|netNumber)
	Moves the Player to net
	netNumber: 0 < |int| <= the number of nets
PurpleSoul.SetControlOverride(|bool|controlOverride)
	controlOverride: false/true
PurpleSoul.Update()
	You should write this in Wave Script's Update().
]]
local PurpleSoul = {}

local purple = false
local net_prepared = false
local net_sprites = {}
local net_horizon = true
local net_num = 0
local net_direction_control_override

function PurpleSoul.SetPurple(active, changeColor, controlOverride)
	if active == nil then
		error("FN!PurpleSoluLib.SetPurple()-NullReferenceException: argument#1(active) should be boolean.")
	end
	if changeColor == nil then changeColor = true end
	if controlOverride == nil then controlOverride = true end
	if type(active) ~= "boolean" then
		error("FN!PurpleSoluLib.SetPurple()-ArgumentException: argument#1(active) should be boolean.")
	end
	if type(changeColor) ~= "boolean" then
		error("FN!PurpleSoluLib.SetPurple()-ArgumentException: argument#2(changeColor) should be nilable-boolean.")
	end
	if type(controlOverride) ~= "boolean" then
		error("FN!PurpleSoluLib.SetPurple()-ArgumentException: argument#3(controlOverride) should be nilable-boolean.")
	end
	if purple == active then return end
	if active then
		if changeColor then Player.sprite.color32 = {213, 53, 217} end
		Player.SetControlOverride(true)
	else
		if controlOverride then Player.SetControlOverride(false) end
		if changeColor then Player.sprite.color = {1, 0, 0} end
	end
	purple = active
end

function PurpleSoul.CreateNet(netAmount, horizontal)
	if netAmount == nil then
		error("FN!PurpleSoluLib.CreateNet()-NullReferenceException: argument#1(netAmount) should be number.")
	end
	if horizontal == nil then horizontal = true end
	if type(netAmount) ~= "number" then
		error("FN!PurpleSoluLib.CreateNet()-ArgumentException: argument#1(netAmount) should be number.")
	end
	if type(horizontal) ~= "boolean" then
		error("FN!PurpleSoluLib.CreateNet()-ArgumentException: argument#2(horizontal) should be boolean.")
	end
	if netAmount <= 0 then
		error("FN!PurpleSoluLib.CreateNet()-ArgumentException: argument#1(netAmount) shouldn't be 0 or negative.")
	end
	if net_prepared then return end
	net_horizon = horizontal
	if horizontal then
		for i = 1, netAmount do
			local y = 240 - 15
			y = y - Arena.height * (i / (netAmount + 1))
			net_sprites[i] = CreateSprite("px", "BelowPlayer")
			net_sprites[i].color32 = {213, 53, 217}
			net_sprites[i].MoveToAbs(320, y)
			net_sprites[i].Scale(Arena.width, 2)
		end
	else
		local y = 240 - 15
		y = y - Arena.height / 2
		if Asterisk ~= nil and AsteriskGMSUpdate ~= nil then
			y = ArenaUtil.centerabsy
		end
		for i = 1, netAmount do
			local x = 320 - Arena.width / 2
			x = x + Arena.width * (i / (netAmount + 1))
			net_sprites[i] = CreateSprite("px", "BelowPlayer")
			net_sprites[i].color32 = {213, 53, 217}
			net_sprites[i].MoveToAbs(x, y)
			net_sprites[i].Scale(2, Arena.height)
		end
	end
	net_prepared = true
end

function PurpleSoul.RemoveNet()
	if not net_prepared then return end
	for i = 1, #net_sprites do
		net_sprites[i].Remove()
	end
	net_sprites = {}
	net_prepared = true
end

function PurpleSoul.MoveToNet(netNumber)
	if netNumber == nil then
		error("FN!PurpleSoluLib.MoveToNet()-NullReferenceException: argument(netNumber) should be number.")
	end
	if type(netNumber) ~= "number" then
		error("FN!PurpleSoluLib.MoveToNet()-ArgumentException: argument(netNumber) should be number.")
	end
	if not net_prepared then return end
	if netNumber <= 0 then
		error("FN!PurpleSoluLib.MoveToNet()-ArgumentException: argument(netNumber) shouldn't be 0 or negative.")
	end
	if netNumber > #net_sprites then
		error("FN!PurpleSoluLib.MoveToNet()-ArgumentOutOfRangeException: argument(netNumber) shouldn't be over the number of nets.")
	end
	if net_horizon then
		local y = Arena.height / 2
		y = y - Arena.height * (netNumber / (#net_sprites + 1))
		Player.MoveTo(Player.x, y)
	else
		local x = -Arena.width / 2
		x = x + Arena.width * (netNumber / (#net_sprites + 1))
		Player.MoveTo(x, Player.y)
	end
	net_num = netNumber
end

function PurpleSoul.SetControlOverride(controlOverride)
	if controlOverride == nil then
		error("FN!PurpleSoluLib.SetControlOverride()-NullReferenceException: argument(controlOverride) should be boolean.")
	end
	if type(controlOverride) ~= "boolean" then
		error("FN!PurpleSoluLib.SetControlOverride()-NullReferenceException: argument(controlOverride) should be boolean.")
	end
	net_direction_control_override = controlOverride
end

function PurpleSoul.Update()
	if net_num == 0 then error("FN!PurpleSoluLib.Update()-Exception: You should call MoveToNet() before calling Update()") end
	if not purple and not net_prepared then return end
	local speed = 2
	if Input.Cancel >= 1 then speed = 1 end
	if net_horizon then
		if Input.Up == 1 then
			net_num = math.max(1, net_num - 1)
			PurpleSoul.MoveToNet(net_num)
		end
		if Input.Down == 1 then
			net_num = math.min(net_num + 1, #net_sprites)
			PurpleSoul.MoveToNet(net_num)
		end
		if net_direction_control_override then return end
		if Input.Left >= 1 then
			Player.Move(-speed, 0)
		end
		if Input.Right >= 1 then
			Player.Move(speed, 0)
		end
	else
		if Input.Left == 1 then
			net_num = math.max(1, net_num - 1)
			PurpleSoul.MoveToNet(net_num)
		end
		if Input.Right == 1 then
			net_num = math.min(net_num + 1, #net_sprites)
			PurpleSoul.MoveToNet(net_num)
		end
		if net_direction_control_override then return end
		if Input.Down >= 1 then
			Player.Move(0, -speed)
		end
		if Input.Up >= 1 then
			Player.Move(0, speed)
		end
	end
end

return PurpleSoul