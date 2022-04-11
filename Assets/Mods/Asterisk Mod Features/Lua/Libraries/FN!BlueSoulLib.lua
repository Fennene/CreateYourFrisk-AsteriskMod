--[[
------------------------------------------------------------------------------------------------------------------
                                        FN! Blue Soul Library
------------------------------------------------------------------------------------------------------------------
by Nil256 [Fennene]

This code is NOT from Undertale, It's Nil256's unique code.
このコードはUndertaleのものではなく、にるにころが作成した独自のコードです。

frame based (60 fps)
60fps基準

< functions >
- Configs --
BlueSoul.ResetSpecialEffects()
	Resets all configs to default.
	全ての設定をデフォルトに戻します。
BlueSoul.AllowSlowMove(active)
	Allows that the Player move slowly by keeping pressing X(Shift) Key.
	This is not effect for jumping.
	Defualt to true.
	X(Shift)キーでプレイヤーが遅く動くことを許可する。
	ジャンプには影響はない。
	デフォルトではtrue。
BlueSoul.SetControlOverride(active)
	Set Non-Jumping Control Override.
	ジャンプ以外の動きをOFFにする。
BlueSoul.SetInactivePlatformAlpha(alpha)
	Sets alpha value of inactive(not enabled) platfoms.
	Default to 0.5.
	有効ではないプラットフォームのアルファ値を設定する。
	デフォルトでは0.5
BlueSoul.AllowToJumpByKeepingPressingJumpKeyAfterPlayerIsHitToSurface(active)
	Allows to jump by keeping pressing jump key after the Player is hit to surface.
	Default to false.
	地面に叩きつけられた後でも、ジャンプキーを押したままでジャンプできるかどうか。
	デフォルトではfalse。
-- Main --
BlueSoul.SetActive(|bool|active, |bool|changeColor = true, |bool|controlOverride = true)
	Sets active of blue soul.
	青色ソウルのアクティブ化/非アクティブ化を行う。
	changeColor: Whether the color will be changed or not.
				 色を変更するかどうか。
	controlOverride: Whether Player.SetControlOverride() will be called or not. (only when active is false)
					 Player.SetControlOverride()を呼び出すかどうか (active が false の時のみ)
|bool| BlueSoul.GetActive()
	Gets active of blue soul.
	青色ソウルがアクティブかどうか返す。
BlueSoul.SetGravity(|str|gravityDirection = "down", |bool|resetSpeed = true)
	Changes gravity.
	You can assgin following value to gravityDirection: "down", "up", "left", "right"
	重力を変更する。
	gravityDirection には "down", "up", "left", "right" のいずれかを代入する。
	resetSpeed: Whether resets the player's velocity or not.
				プレイヤーの重力処理を初期化し直すかどうか。
|str| BlueSoul.GetGravity()
	Gets gravity.
	重力名を返す。
BlueSoul.HitToSurface(|int|shakeFrame = 3, |int|shakeStrength = 6)
	Hits the player to surface or platforms.
	プレイヤーを地面かプラットフォームに叩きつける。
-- Platforms --
I recommend you assgin platformID as string or number.
platformIDは文字列(string)か数値(number)をお勧めします。
|bool| BlueSoul.PlatfromExists(platformID)
	Returns whether specified platformID exists or not.
	指定したplatformIDが存在するかどうか返します。
BlueSoul.CreatePlatformAbs(platformID, |num|absx, |num|absy, |str|layer = "BelowBullet")
BlueSoul.CreatePlatform(platformID, |num|x, |num|y, |str|layer = "BelowBullet")
	Creates a platform.
	プラットフォームを作成します。
BlueSoul.RemovePlatform(platformID)
	Removes a platform.
	プラットフォームを削除します。
BlueSoul.RemoveAllPlatform()
	Removes all platform.
	全てのプラットフォームを削除します。
BlueSoul.PlatformMove(platformID, |num|x, |num|y, ignorePlayer = false, ignoreGravity = false)
	Moves a platform.
	プラットフォームを動かします。
|num| BlueSoul.GetPlatformAbsX(platformID)
	Returns the position's absx of a platform.
	プラットフォームのabsx座標を返します。
|num| BlueSoul.GetPlatformAbsY(platformID)
	プラットフォームのabsy座標を返します。
	Returns the position's absy of a platform.
BlueSoul.SetPlatformParent(platformID, |spr|sprite)
	Sets the parent of a platform's sprite.
	プラットフォームの親スプライトを設定します。
|spr| BlueSoul._GetPlatformSpriteObject(platformID)
|platform| BlueSoul._GetPlatformRawData(platformID)
-- System --
BlueSoul.Update()
	Please call this function in Wave Scripts' Update().
	Waveスクリプト内のUpdate()でこれを呼び出してください。
BlueSoul.Dispose()
	Please call this function in Wave Scripts' EndingWave().
	Waveスクリプト内のEndingWave()でこれを呼び出してください。
]]

local BlueSoul = {}


--< Variables >--
-- const --
local SPRITE_PLATFORM = "BlueSoul-Platform"
local SOUND_HIT_TO_SURFACE = "impact"
local PLAYER_SPRITE_HALF_SIZE = 8
local PLAYER_COLLISION_HALF_SIZE = 4


-- Main --
local GARVITY_DIRECTION = {}
GARVITY_DIRECTION.DOWN = 1
GARVITY_DIRECTION.UP = 2
GARVITY_DIRECTION.LEFT = 3
GARVITY_DIRECTION.RIGHT = 4

local JUMP_STAT = {}
JUMP_STAT.NONE = 0
JUMP_STAT.JUMP = 1
JUMP_STAT.STAY = 2
JUMP_STAT.FALL = 3

local _active = false
local _garvity = GARVITY_DIRECTION.DOWN
local _jump_stat = JUMP_STAT.NONE
local _jump_speed = 0
local _jump_frame_counter = 0
local _ridden_platform = nil
local _wait_re_press_key = false


-- Platforms --
local _horizontal_platforms = {}
local _vertical_platforms = {}
local _crushed_shards = {}


-- Config --
local allow_slow_move = true
local control_override = false
local allow_keep_pressing = false
local deactive_platform_alpha = 0.5



--< Functions >--
-- Exceptions --
local function CheckArgumentType(funcName, arg, argName, argNum, argType, nullable)
	local error_message = ""
	if arg == nil then
		error_message = "FN!BlueSoulLib."..funcName.."()-NullReferenceException: "
	elseif type(arg) ~= argType then
		error_message = "FN!BlueSoulLib."..funcName.."()-ArgumentException: "
	end
	if error_message == "" then return end
	error_message = error_message .. "argument"
	if argNum > 0 then
		error_message = error_message .. "#"..argNum
	end
	error_message = error_message .. "(" .. argName .. ") should be "
	if nullable == true then
		error_message = error_message .. "nilable-"
	end
	error_message = error_message .. argType
	error(error_message)
end

local function PlatformIDExists(funcName, ID, notFoundException)
	if ID == nil then
		error("FN!BlueSoulLib."..funcName.."()-NullReferenceException: argument#1(platformID) shouldn't be nil")
	end
	local horizontal = _horizontal_platforms[ID]
	local vertical = _vertical_platforms[ID]
	local exists = horizontal ~= nil or vertical ~= nil
	if (exists and notFoundException) or (not exists and not notFoundException) then
		if horizontal ~= nil then return _horizontal_platforms
		else                      return _vertical_platforms
		end
	end
	local error_message = "FN!BlueSoulLib."..funcName.."()-"
	if notFoundException then
		error_message = error_message .. "PlatformIDNotFoundException: platformID\""..ID.."\" is not found"
	else
		error_message = error_message .. "PlatformIDExistException: platformID\""..ID.."\" exists already"
	end
	error(error_message)
end

local function CheckLayer(funcName, layer)
	if LayerExists(layer) then return end
	error("FN!BlueSoulLib."..funcName.."()-NullReferenceException: Layer\""..layer.."\" is not found.")	
end

local function ThrowIllegalGravity(funcName)
	error("FN!BlueSoulLib-InternalError: Illegal Gravity Direction\nin FN!BlueSoulLib."..funcName.."() <Local>")
end


-- Config --
function BlueSoul.ResetSpecialEffects()
	allow_slow_move = true
	control_override = false
end

function BlueSoul.AllowSlowMove(active)
	CheckArgumentType("AllowSlowMove", active, "active", 0, "boolean", false)
	allow_slow_move = active
end

function BlueSoul.SetControlOverride(active)
	CheckArgumentType("SetControlOverride", active, "active", 0, "boolean", false)
	control_override = active
end

function BlueSoul.SetInactivePlatformAlpha(alpha)
	CheckArgumentType("SetInactivePlatformAlpha", alpha, "alpha", 0, "number", false)
	deactive_platform_alpha = math.max(0, math.min(alpha, 1))
end

function BlueSoul.AllowToJumpByKeepingPressingJumpKeyAfterPlayerIsHitToSurface(active)
	CheckArgumentType("AllowToJumpByKeepingPressingJumpKeyAfterPlayerIsHitToSurface", active, "active", 0, "boolean", false)
	allow_keep_pressing = active
end


-- Util --
local function IsVerticalGravity()
	return _garvity <= 2
end

local function IsPositiveGravity()
	return _garvity % 2 == 0
end

local function GetRotation()
	if _garvity == GARVITY_DIRECTION.DOWN then return 0
	elseif _garvity == GARVITY_DIRECTION.UP then return 180
	elseif _garvity == GARVITY_DIRECTION.LEFT then return 270
	elseif _garvity == GARVITY_DIRECTION.RIGHT then return 90
	end
	ThrowIllegalGravity("GetRotation")
end


-- Platforms --
local function SetPlatformAlpha()
	local hAlpha = 1
	local vAlpha = deactive_platform_alpha
	if not IsVerticalGravity() then
		hAlpha = deactive_platform_alpha
		vAlpha = 1
	end
	for key, parameter in pairs(_horizontal_platforms) do
		_horizontal_platforms[key].sprite.alpha = hAlpha
	end
	for key, parameter in pairs(_vertical_platforms) do
		_vertical_platforms[key].sprite.alpha = vAlpha
	end
end

function BlueSoul.PlatfromExists(platformID)
	if platformID == nil then
		error("FN!BlueSoulLib.PlatfromExists()-NullReferenceException: argument#1(platformID) shouldn't be nil")
	end
	return _horizontal_platforms[platformID] ~= nil or _vertical_platforms[platformID] ~= nil
end

function BlueSoul.CreatePlatformAbs(platformID, absx, absy, layer)
	if layer == nil then layer = "BelowBullet" end
	PlatformIDExists("CreatePlatform()/CreatePlatformAbs", platformID, false)
	CheckArgumentType("CreatePlatform()/CreatePlatformAbs", absx,  "absx", 2,  "number", false)
	CheckArgumentType("CreatePlatform()/CreatePlatformAbs", absy,  "absy", 3,  "number", false)
	CheckArgumentType("CreatePlatform()/CreatePlatformAbs", layer, "layer", 4, "string", true)
	CheckLayer("CreatePlatform()/CreatePlatformAbs", layer)
	local platformList = _horizontal_platforms
	if not IsVerticalGravity() then platformList = _vertical_platforms end
	platformList[platformID] = {}
	platformList[platformID].sprite = CreateSprite(SPRITE_PLATFORM, layer)
	platformList[platformID].sprite.MoveToAbs(absx, absy)
	platformList[platformID].sprite.rotation = GetRotation()
	platformList[platformID].half_width = platformList[platformID].sprite.width / 2
end
function BlueSoul.CreatePlatform(platformID, x, y, layer)
	CheckArgumentType("CreatePlatform()/CreatePlatformAbs", x, "x", 2, "number", false)
	CheckArgumentType("CreatePlatform()/CreatePlatformAbs", y, "y", 3, "number", false)
	BlueSoul.CreatePlatformAbs(platformID, x + ArenaUtil.centerabsx, y + ArenaUtil.centerabsy, layer)
end

function BlueSoul.RemovePlatform(platformID)
	local platformList = PlatformIDExists("RemovePlatform", platformID, true)
	platformList[platformID].sprite.Remove()
	platformList[platformID] = nil
end

function BlueSoul.RemoveAllPlatform()
	for key, parameter in pairs(_vertical_platforms) do
		_vertical_platforms[key].sprite.Remove()
		_vertical_platforms[key] = nil
	end
	for key, parameter in pairs(_horizontal_platforms) do
		_horizontal_platforms[key].sprite.Remove()
		_horizontal_platforms[key] = nil
	end
	_vertical_platforms = {}
	_horizontal_platforms = {}
end

function BlueSoul.PlatformMove(platformID, x, y, ignorePlayer, ignoreGravity)
	if ignorePlayer == nil then  ignorePlayer = false  end
	if ignoreGravity == nil then ignoreGravity = false end
	local platformList = PlatformIDExists("PlatformMove", platformID, true)
	CheckArgumentType("PlatformMove", x,            "x", 2,              "number", false)
	CheckArgumentType("PlatformMove", y,            "y", 3,              "number", false)
	CheckArgumentType("PlatformMove", ignorePlayer,  "ignorePlayer", 4,  "boolean", true)
	CheckArgumentType("PlatformMove", ignoreGravity, "ignoreGravity", 5, "boolean", true)
	platformList[platformID].sprite.Move(x, y)
	if platformID ~= _ridden_platform or ignorePlayer then return end
	if ignoreGravity then
		if _garvity == GARVITY_DIRECTION.DOWN then y = math.max(0, y)
		elseif _garvity == GARVITY_DIRECTION.UP then y = math.min(y, 0)
		elseif _garvity == GARVITY_DIRECTION.LEFT then x = math.max(0, x)
		elseif _garvity == GARVITY_DIRECTION.RIGHT then x = math.min(x, 0)
		else ThrowIllegalGravity("PlatformMove")
		end
	end
	Player.Move(x, y)
end

function BlueSoul.GetPlatformAbsX(platformID)
	local platformList = PlatformIDExists("GetPlatformAbsX", platformID, true)
	return platformList[platformID].sprite.absx
end

function BlueSoul.GetPlatformAbsY(platformID)
	local platformList = PlatformIDExists("GetPlatformAbsX", platformID, true)
	return platformList[platformID].sprite.absy
end

function BlueSoul.SetPlatformParent(platformID, sprite)
	local platformList = PlatformIDExists("SetPlatformParent", platformID, true)
	platformList[platformID].sprite.SetParent(sprite)
end

function BlueSoul._GetPlatformSpriteObject(platformID)
	local platformList = PlatformIDExists("_GetPlatformSpriteObject", platformID, true)
	return platformList[platformID].sprite
end

function BlueSoul._GetPlatformRawData(platformID)
	local platformList = PlatformIDExists("_GetPlatformRawData", platformID, true)
	return platformList[platformID]
end


-- Main --
local function EnableJump()
	_jump_stat = JUMP_STAT.NONE
	_jump_speed = 0
	_jump_frame_counter = 0
end

local function Fall()
	_jump_stat = JUMP_STAT.FALL
	_jump_speed = 0
	_jump_frame_counter = 0
end

function BlueSoul.SetGravity(gravityDirection, resetSpeed)
	if gravityDirection == nil then gravityDirection = "down" end
	if resetSpeed == nil then       resetSpeed = true end
	CheckArgumentType("SetGravity", gravityDirection, "gravityDirection", 0, "string", true)
	CheckArgumentType("SetGravity", resetSpeed,       "resetSpeed", 0,       "boolean", true)
	if resetSpeed then Fall() end
	gravityDirection = string.lower(gravityDirection)
	if gravityDirection == "down" or gravityDirection == "bottom" then
		_garvity = GARVITY_DIRECTION.DOWN
	elseif gravityDirection == "up" or gravityDirection == "top" then
		_garvity = GARVITY_DIRECTION.UP
	elseif gravityDirection == "left" then
		_garvity = GARVITY_DIRECTION.LEFT
	elseif gravityDirection == "right" then
		_garvity = GARVITY_DIRECTION.RIGHT
	else
		local error_message = "FN!BlueSoulLib.SetGravity()-ArgumentException: unkwnon direction\"" .. direction
		error_message = error_message .. "\".\ndirection should be \"down\", \"up\", \"left\" or \"right\""
		error(error_message)
	end
	Player.sprite.rotation = GetRotation()
	SetPlatformAlpha()
end
function BlueSoul.GetGravity()
	if _garvity == GARVITY_DIRECTION.DOWN then return "down"
	elseif _garvity == GARVITY_DIRECTION.UP then return "up"
	elseif _garvity == GARVITY_DIRECTION.LEFT then return "left"
	elseif _garvity == GARVITY_DIRECTION.RIGHT then return "right"
	end
	ThrowIllegalGravity("GetGravity")
end

function BlueSoul.SetActive(active, changeColor, controlOverride)
	if changeColor == nil then     changeColor = true     end
	if controlOverride == nil then controlOverride = true end
	CheckArgumentType("SetActive", active,          "active", 1,          "boolean", false)
	CheckArgumentType("SetActive", changeColor,     "changeColor", 2,     "boolean", true)
	CheckArgumentType("SetActive", controlOverride, "controlOverride", 3, "boolean", true)
	if _active == active then return end
	EnableJump()
	_active = active
	if _active then
		Player.SetControlOverride(true)
		if changeColor then Player.sprite.color32 = {0, 60, 255} end
		Player.sprite.rotation = GetRotation()
		Fall()
	else
		if controlOverride then Player.SetControlOverride(false) end
		if changeColor then Player.sprite.color = {1, 0, 0} end
		Player.sprite.rotation = 0
	end
end
function BlueSoul.GetActive() return _active end


-- System --
local function GetJumpKey()
	if _garvity == GARVITY_DIRECTION.DOWN then return Input.Up
	elseif _garvity == GARVITY_DIRECTION.UP then return Input.Down
	elseif _garvity == GARVITY_DIRECTION.LEFT then return Input.Right
	elseif _garvity == GARVITY_DIRECTION.RIGHT then return Input.Left
	end
	ThrowIllegalGravity("GetJumpKey")
end

local function UpdateMove()
	if control_override then return end
	local speed = 2
	if allow_slow_move and Input.Cancel >= 1 then
		speed = 1
	end
	if IsVerticalGravity() then
		if Input.Left  >= 1 then Player.Move(-speed, 0, false) end
		if Input.Right >= 1 then Player.Move( speed, 0, false) end
	else
		if Input.Down >= 1 then Player.Move(0, -speed, false) end
		if Input.Up   >= 1 then Player.Move(0,  speed, false) end
	end
end

local function Jump(speed)
	if _garvity == GARVITY_DIRECTION.DOWN then      Player.Move(0,  speed)
	elseif _garvity == GARVITY_DIRECTION.UP then    Player.Move(0, -speed)
	elseif _garvity == GARVITY_DIRECTION.LEFT then  Player.Move( speed, 0)
	elseif _garvity == GARVITY_DIRECTION.RIGHT then Player.Move(-speed, 0)
	else ThrowIllegalGravity("Jump")
	end	
end

local function CalculateJumpSpeed()
	local jumpKey = GetJumpKey()
	if _jump_stat == JUMP_STAT.NONE and jumpKey >= 1 then
		if jumpKey == 1 or not _wait_re_press_key then
			_jump_stat = JUMP_STAT.JUMP
			_jump_speed = 4
			_jump_frame_counter = 0
			_wait_re_press_key = false
		end
	end
	if _jump_stat == JUMP_STAT.JUMP then
		if jumpKey == 2 and _jump_speed > 0 then
			if _jump_frame_counter % 2 ^ (_jump_speed + 1) == 0 then
				_jump_speed = _jump_speed - 1
			end
			_jump_frame_counter = _jump_frame_counter + 1
		end
		if jumpKey == -1 or _jump_speed == 0 then
			_jump_stat = JUMP_STAT.STAY
			_jump_speed = 0
			_jump_frame_counter = 0
		end
	end
	if _jump_stat == JUMP_STAT.STAY then
		if     _jump_frame_counter < 3 then _jump_speed =  1
		elseif _jump_frame_counter > 9 then _jump_speed = -1
		else                                _jump_speed =  0
		end
		_jump_frame_counter = _jump_frame_counter + 1
		if _jump_frame_counter == 12 then
			_jump_stat = JUMP_STAT.FALL
			_jump_speed = 0
			_jump_frame_counter = 0
		end
	end
	if _jump_stat == JUMP_STAT.FALL then
		if _jump_frame_counter % 6 == 0 then
			_jump_speed = _jump_speed - 1
		end
		_jump_frame_counter = _jump_frame_counter + 1
	end
end

local function CheckSurface()
	if _garvity == GARVITY_DIRECTION.DOWN then      return Player.y - PLAYER_SPRITE_HALF_SIZE == -Arena.height / 2
	elseif _garvity == GARVITY_DIRECTION.UP then    return Player.y + PLAYER_SPRITE_HALF_SIZE ==  Arena.height / 2
	elseif _garvity == GARVITY_DIRECTION.LEFT then  return Player.x - PLAYER_SPRITE_HALF_SIZE == -Arena.width  / 2
	elseif _garvity == GARVITY_DIRECTION.RIGHT then return Player.x + PLAYER_SPRITE_HALF_SIZE ==  Arena.width  / 2
	end
	ThrowIllegalGravity("CheckSurface")
end

local function GetPlatformList(platformID)
	if platformID == nil then
		if IsVerticalGravity() then return _horizontal_platforms
		else                        return _vertical_platforms
		end
	end
	if _horizontal_platforms[platformID] ~= nil then   return _horizontal_platforms
	elseif _vertical_platforms[platformID] ~= nil then return _vertical_platforms
	end
end

local function GetPositionVerticalToGravity(anyClass)
	if IsVerticalGravity() then return anyClass.absy
	else                        return anyClass.absx
	end
end

local function GetPositionHorizontalToGravity(anyClass)
	if IsVerticalGravity() then return anyClass.absx
	else                        return anyClass.absy
	end
end

local function ConvertPosition(horizontal, vertical)
	if IsVerticalGravity() then return horizontal, vertical
	else                        return vertical, horizontal
	end
end

local function CheckNeedReversingSign()
	if IsPositiveGravity() then return -1
	else                        return  1
	end
end

local function CheckPlatform(speed)
	if speed > 0 then return nil end
	local riddenPlatformID = nil
	local sign = CheckNeedReversingSign()
	local playerVerticalPosition = GetPositionVerticalToGravity(Player)
	local playerHorizontalPosition = GetPositionHorizontalToGravity(Player)
	local playerNextFalling = playerVerticalPosition + speed * sign
	local playerWidthMin = playerHorizontalPosition - PLAYER_COLLISION_HALF_SIZE
	local playerWidthMax = playerHorizontalPosition + PLAYER_COLLISION_HALF_SIZE
	local playerLandingPoint = playerVerticalPosition - PLAYER_SPRITE_HALF_SIZE * sign
	local platformLists = GetPlatformList()
	for key, parameter in pairs(platformLists) do
		local platform = platformLists[key]
		local platformVerticalPosition = GetPositionVerticalToGravity(platform.sprite)
		local platformHorizontalPosition = GetPositionHorizontalToGravity(platform.sprite)
		local platformWidthMin = platformHorizontalPosition - platform.half_width
		local platformWidthMax = platformHorizontalPosition + platform.half_width
		if platformVerticalPosition * sign <= playerLandingPoint * sign then
			if platformWidthMin <= playerWidthMax and playerWidthMin <= platformWidthMax then
				local simulatedNextFalling = playerLandingPoint + speed * sign
				if simulatedNextFalling * sign <= platformVerticalPosition * sign then
				if playerNextFalling * sign <= (platformVerticalPosition + PLAYER_SPRITE_HALF_SIZE * sign) * sign then
					playerNextFalling = platformVerticalPosition + PLAYER_SPRITE_HALF_SIZE * sign
					riddenPlatformID = key
				end
				end
			end
		end
	end
	--[[ -- Evaluation methods (Idea) -- if _garvity == GARVITY_DIRECTION.DOWN then
	local playerNextAbsy = Player.absy + speed
	local playerRightX = Player.absx + PLAYER_COLLISION_HALF_SIZE
	local playerLeftX = Player.absx - PLAYER_COLLISION_HALF_SIZE
	local playerBottomY = Player.absy - PLAYER_SPRITE_HALF_SIZE
	local platformLists = GetPlatformList()
	for key, parameter in pairs(platformLists) do
		local platform = platformLists[key]
		if platform.sprite.absy <= playerBottomY then
			if platform.sprite.absx - platform.half_width <= playerRightX then
			if playerLeftX <= platform.sprite.absx + platform.half_width then
				if playerBottomY + speed <= platform.sprite.absy then
				if playerNextAbsy <= platform.sprite.absy + PLAYER_SPRITE_HALF_SIZE then
					playerNextAbsy = platform.sprite.absy + PLAYER_SPRITE_HALF_SIZE
					riddenPlatformID = key
				end
				end
			end
			end
		end
	end
	_jump_speed = playerNextAbsy - Player.absy
	return riddenPlatformID
	]]
	_jump_speed = (playerNextFalling - playerVerticalPosition) * sign
	return riddenPlatformID
end

local function UpdateJump()
	if CheckSurface() or _ridden_platform ~= nil then
		EnableJump()
	elseif _jump_stat == JUMP_STAT.NONE then
		Fall()
	end
	CalculateJumpSpeed()
	_ridden_platform = CheckPlatform(_jump_speed, false)
	Jump(_jump_speed)
end


-- System(Main) --
function BlueSoul.HitToSurface(shakeFrame, shakeStrength)
	if shakeFrame == nil then    shakeFrame = 3    end
	if shakeStrength == nil then shakeStrength = 6 end
	CheckArgumentType("HitToSurface", shakeFrame,    "shakeFrame", 1,    "number", true)
	CheckArgumentType("HitToSurface", shakeStrength, "shakeStrength", 2, "number", true)
	local speed = -Arena.height
	if not IsVerticalGravity() then speed = -Arena.width end
	_ridden_platform = CheckPlatform(speed)
	Jump(_jump_speed)
	_wait_re_press_key = not allow_keep_pressing
	Misc.ShakeScreen(shakeFrame, shakeStrength, false)
	if SOUND_HIT_TO_SURFACE == nil then return end
	Audio.PlaySound(SOUND_HIT_TO_SURFACE)
end

function BlueSoul.Update()
	if not _active then return end
	UpdateMove()
	UpdateJump()
end

function BlueSoul.Dispose()
	BlueSoul.RemoveAllPlatform()
end


return BlueSoul