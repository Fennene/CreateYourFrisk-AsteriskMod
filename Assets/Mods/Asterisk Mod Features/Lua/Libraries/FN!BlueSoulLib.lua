--[[
------------------------------------------------------------------------------------------------------------------
                                          FN! Blue Soul Library
------------------------------------------------------------------------------------------------------------------
by Nil256 [Fennene]

This code is NOT from Undertale, It's Nil256's unique code.
frame based (60 fps)

< functions >
BlueSoul.SetBlue(|bool|active, |bool?|changeColor = true, |bool?|controlOverride = true)
	Changes Soul Mode
	active: false/true
	changeColor: nil/false/true - Color the Player soul or don't
	controlOverride: nil/false/true - Call Player.ControlOverride() or not (only if active is false)
BlueSoul.SetGravity(|string?|direction = "down")
	Changes direction of gravity
	direction: nil/"down"/"up"/"left"/"right"
BlueSoul.SetControlOverride(|bool|controlOverride)
	controlOverride: false/true
BlueSoul.Update()
	You should write this in Wave Script's Update().
]]
local BlueSoul = {}

--local targte_sprite = Player.sprite
local blue = false
local jump = false
local jump_speed = 0
local jump_frame = 0
local stay = false
local stay_frame = 0
local fall = false
local fall_frame = 0
local fall_speed = 0
local gravity_direction = "down"
local non_jump_control_override = false

--[[
function BlueSoul.SetTargetSprite(spriteObject)
	if spriteObject == nil then
		targte_sprite = Player.sprite
		return
	end
	if type(spriteObject) ~= "LuaSpriteController" then
		error("FN!BlueSoluLib.SetTargetSprite()-ArgumentException: argument(spriteObject) should be sprite object.")
	end
	if not spriteObject.isactive then
		error("FN!BlueSoluLib.SetTargetSprite()-NullReferenceException: attempt to set removed sprite object.")
	end
	targte_sprite = spriteObject
end
]]

function BlueSoul.SetBlue(active, changeColor, controlOverride)
	if active == nil then
		error("FN!BlueSoluLib.SetBlue()-NullReferenceException: argument#1(active) should be boolean.")
	end
	if changeColor == nil then changeColor = true end
	if controlOverride == nil then controlOverride = true end
	if type(active) ~= "boolean" then
		error("FN!BlueSoluLib.SetBlue()-ArgumentException: argument#1(active) should be boolean.")
	end
	if type(changeColor) ~= "boolean" then
		error("FN!BlueSoluLib.SetBlue()-ArgumentException: argument#2(changeColor) should be nilable-boolean.")
	end
	if type(controlOverride) ~= "boolean" then
		error("FN!BlueSoluLib.SetBlue()-ArgumentException: argument#3(controlOverride) should be nilable-boolean.")
	end
	if blue == active then return end
	if active then
		fall = true
		fallSpeed = 0
		fallFrame = 0
		if changeColor then Player.sprite.color32 = {0, 60, 255} end
		Player.SetControlOverride(true)
	else
		jump = false
		jump_speed = 0
		jump_frame = 0
		stay = false
		stay_frame = 0
		fall = false
		fall_frame = 0
		fall_speed = 0
		Player.sprite.rotation = 0
		if controlOverride then Player.SetControlOverride(false) end
		if changeColor then Player.sprite.color = {1, 0, 0} end
	end
	blue = active
end

function BlueSoul.SetGravity(direction)
	if direction == nil then direction = "down" end
	if type(direction) ~= "string" then
		error("FN!BlueSoluLib.SetGravity()-ArgumentException: argument(direction) should be nilable-string.")
	end
	direction = string.lower(direction)
	if gravity_direction == direction or not blue then return end
	if direction == "down" or direction == "bottom" then
		gravity_direction = "down"
		Player.sprite.rotation = 0
	elseif direction == "up" or direction == "top" then
		gravity_direction = "up"
		Player.sprite.rotation = 180
	elseif direction == "left" then
		gravity_direction = "left"
		Player.sprite.rotation = 270
	elseif direction == "right" then
		gravity_direction = "right"
		Player.sprite.rotation = 90
	else
		local error_message = "FN!BlueSoluLib.SetGravity()-ArgumentException: unkwnon direction\"" .. direction
		error_message = error_message .. "\".\ndirection should be \"down\", \"up\", \"left\" or \"right\""
		error(error_message)
	end
end

function BlueSoul.SetControlOverride(controlOverride)
	if controlOverride == nil then
		error("FN!BlueSoluLib.SetControlOverride()-NullReferenceException: argument(controlOverride) should be boolean.")
	end
	if type(controlOverride) ~= "boolean" then
		error("FN!BlueSoluLib.SetControlOverride()-NullReferenceException: argument(controlOverride) should be boolean.")
	end
	non_jump_control_override = controlOverride
end

local function GetJumpKey()
	if gravity_direction == "down" then
		return Input.Up
	elseif gravity_direction == "up" then
		return Input.Down
	elseif gravity_direction == "left" then
		return Input.Right
	elseif gravity_direction == "right" then
		return Input.Left
	end
	error("FN!BlueSoulLib-InternalError: Illegal Gravity Direction")
end

local function UpdateNonJumpKey()
	if non_jump_control_override then return end
	local speed = 2
	if Input.Cancel >= 1 then speed = 1 end
	if gravity_direction == "down" or gravity_direction == "up" then
		if Input.Left >= 1 then
			Player.Move(-speed, 0)
		end
		if Input.Right >= 1 then
			Player.Move(speed, 0)
		end
	elseif gravity_direction == "left" or gravity_direction == "right" then
		if Input.Down >= 1 then
			Player.Move(0, -speed)
		end
		if Input.Up >= 1 then
			Player.Move(0, speed)
		end
	end
end

local function CheckSurface()
	local is_surface = false
	if gravity_direction == "down" then
		is_surface = (Player.y - Player.sprite.height / 2 == - Arena.height / 2)
	elseif gravity_direction == "up" then
		is_surface = (Player.y + Player.sprite.height / 2 == Arena.height / 2)
	elseif gravity_direction == "left" then
		is_surface = (Player.x - Player.sprite.width / 2 == - Arena.width / 2)
	elseif gravity_direction == "right" then
		is_surface = (Player.x + Player.sprite.width / 2 == Arena.width / 2)
	else
		error("FN!BlueSoulLib-InternalError: Illegal Gravity Direction")
	end
	if is_surface then
		fall = false
		fall_speed = 0
	end
end

local function PlayerJump(displacement)
	if gravity_direction == "down" then
		Player.Move(0, displacement)
	elseif gravity_direction == "up" then
		Player.Move(0, -displacement)
	elseif gravity_direction == "left" then
		Player.Move(displacement, 0)
	elseif gravity_direction == "right" then
		Player.Move(-displacement, 0)
	else
		error("FN!BlueSoulLib-InternalError: Illegal Gravity Direction")
	end
end

function BlueSoul.Update()
	if not blue then return end
	UpdateNonJumpKey()
	local jump_key = GetJumpKey()
	if jump_key >= 1 and not jump and not stay and not fall then
		jump = true
		jump_frame = 0
		jump_speed = 4
	end
	if jump_key == 2 and jump_speed > 0 then
		if jump_frame % 2 ^ (jump_speed + 1) == 0 then
			jump_speed = jump_speed - 1
		end
		jump_frame = jump_frame + 1
	end
	if (jump_key == -1 or jump_speed == 0) and jump then
		jump = false
		jump_speed = 0
		stay = true
		stay_frame = 0
	end
	if stay then
		if stay_frame < 3 then
			PlayerJump(1)
		end
		if stay_frame > 9 then
			PlayerJump(-1)
		end
		if stay_frame == 11 then
			stay = false
			fall = true
			fall_speed = 0
			fall_frame = 0
		end
		stay_frame = stay_frame + 1
	end
	if fall then
		if fall_frame % 6 == 0 then
			fall_speed = fall_speed + 1
		end
		fall_frame = fall_frame + 1
	end
	CheckSurface()
	PlayerJump(jump_speed)
	PlayerJump(-fall_speed)
end

return BlueSoul