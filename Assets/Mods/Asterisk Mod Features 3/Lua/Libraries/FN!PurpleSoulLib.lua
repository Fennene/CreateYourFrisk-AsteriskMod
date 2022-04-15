--[[
------------------------------------------------------------------------------------------------------------------
                                       FN! Purple Soul Library
------------------------------------------------------------------------------------------------------------------
by Nil256 [Fennene]

This code is NOT from Undertale, It's Nil256's unique code.
このコードはUndertaleのものではなく、にるにころが作成した独自のコードです。

frame based (60 fps)
60fps基準

< functions >
- Configs --
PurpleSoul.ResetSpecialEffects()
	Resets all configs to default.
	全ての設定をデフォルトに戻します。
PurpleSoul.AllowSlowMove(active)
	Allows that the Player move slowly by keeping pressing X(Shift) Key.
	This is not effect for jumping to net.
	Defualt to true.
	X(Shift)キーでプレイヤーが遅く動くことを許可する。
	ネット移動には影響はない。
	デフォルトではtrue。
PurpleSoul.SetControlOverride(active)
	Set Non-Jumping Control Override.
	ネット移動以外の動きをOFFにする。
-- Main --
PurpleSoul.SetActive(|bool|active, |bool|changeColor = true, |bool|controlOverride = true)
	Sets active of blue soul.
	紫色ソウルのアクティブ化/非アクティブ化を行う。
	changeColor: Whether the color will be changed or not.
				 色を変更するかどうか。
	controlOverride: Whether Player.SetControlOverride() will be called or not. (only when active is false)
					 Player.SetControlOverride()を呼び出すかどうか (active が false の時のみ)
|bool| PurpleSoul.GetActive()
	Gets active of blue soul.
	紫色ソウルがアクティブかどうか返す。
PurpleSoul.CreateNet(|num|netNum, |bool|horizontal = true, |str|layer = "BelowBullet")
	Creates nets.
	If nets has existed already on calling this, RemoveNet() is called automatically and this creates new nets.
	ネットを生成する。
	既にネットが存在する場合、RemoveNet()が自動的に呼び出され、新しいネットが作られる。
	horizontal: whether create nets horizontally to arena or not.
				アリーナ(ダイアログボックス)に対して水平にネットを作成するかどうか。
PurpleSoul.RemoveNet()
	Removes nets.
	ネットを削除する。
PurpleSoul.JumpToNet(|num|netNum)
	Jump the Player to specified net.
	You need to call CreateNet() before calling this.
	プレイヤーを指定したネットに移動させる。
	これを呼び出す前に、CreateNet()を呼び出している必要がある。
-- System --
PurpleSoul.Update()
	Please call this function in Wave Scripts' Update().
	You need to call CreateNet() and JumpToNet() before calling this.
	Waveスクリプト内のUpdate()でこれを呼び出してください。
	これを呼び出す前に、CreateNet()とJumpToNet()を呼び出している必要がある。
PurpleSoul.Dispose()
	Please call this function in Wave Scripts' EndingWave().
	Waveスクリプト内のEndingWave()でこれを呼び出してください。
-- Misc --
PurpleSoul.GetColor()
]]

local PurpleSoul = {}


--< Variables >--
-- Main --
local _active = false
local _horizontal = true
local _nets = {}
local _current_net_num = 0


-- Config --
local allow_slow_move = true
local control_override = false



--< Functions >--
-- Exceptions --
local function CheckArgumentType(funcName, arg, argName, argNum, argType, nullable)
	local error_message = ""
	if arg == nil then
		error_message = "FN!PurpleSoulLib."..funcName.."()-NullReferenceException: "
	elseif type(arg) ~= argType then
		error_message = "FN!PurpleSoulLib."..funcName.."()-ArgumentException: "
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


-- Config --
function PurpleSoul.ResetSpecialEffects()
	allow_slow_move = true
	control_override = false
end

function PurpleSoul.AllowSlowMove(active)
	CheckArgumentType("AllowSlowMove", active, "active", 0, "boolean", false)
	allow_slow_move = active
end

function PurpleSoul.SetControlOverride(active)
	CheckArgumentType("SetControlOverride", active, "active", 0, "boolean", false)
	control_override = active
end


-- Util --
local function GetPositionHorizontalToNet(horizontalNet)
	if _horizontal then return ArenaUtil.centerabsx
	else                return ArenaUtil.centerabsy
	end
end

local function GetPositionVerticalToNet(horizontalNet)
	if _horizontal then return ArenaUtil.centerabsy
	else                return ArenaUtil.centerabsx
	end
end

local function GetArenaSizeHorizontalToNet(horizontalNet)
	if _horizontal then return Arena.width
	else                return Arena.height
	end
end

local function GetArenaSizeVerticalToNet(horizontalNet)
	if _horizontal then return Arena.height
	else                return Arena.width
	end
end

local function ConvertPosition(horizontalX, horizontalY)
	if _horizontal then return horizontalX, horizontalY
	else                return horizontalY, horizontalX
	end
end


-- Main --
function PurpleSoul.SetActive(active, changeColor, controlOverride)
	if changeColor == nil then     changeColor = true     end
	if controlOverride == nil then controlOverride = true end
	CheckArgumentType("SetActive", active,          "active", 1,          "boolean", false)
	CheckArgumentType("SetActive", changeColor,     "changeColor", 2,     "boolean", true)
	CheckArgumentType("SetActive", controlOverride, "controlOverride", 3, "boolean", true)
	_active = active
	if _active then
		Player.SetControlOverride(true)
		if changeColor then Player.sprite.color32 = {213,  53, 217} end
	else
		if controlOverride then Player.SetControlOverride(false) end
		if changeColor then Player.sprite.color = {1, 0, 0} end
	end
end
function PurpleSoul.GetActive() return _active end

function PurpleSoul.RemoveNet()
	if #_nets == 0 then return end
	for i = 1, #_nets do
		_nets[i].sprite.Remove()
	end
	_nets = {}
	_current_net_num = 0
end
function PurpleSoul.CreateNet(netNum, horizontal, layer)
	if horizontal == nil then horizontal = true     end
	if layer == nil then      layer = "BelowBullet" end
	CheckArgumentType("CreateNet", netNum,     "netNum", 1,     "number", false)
	CheckArgumentType("CreateNet", horizontal, "horizontal", 2, "boolean", true)
	CheckArgumentType("CreateNet", layer,      "layer", 3,      "string", true)
	if netNum <= 0 then                             error("FN!PurpleSoulLib.CreateNet()-ArgumentOutOfRangeException: argument#1(netNum) should be positive number.")        end
	if horizontal and netNum > Arena.height then    error("FN!PurpleSoulLib.CreateNet()-ArgumentOutOfRangeException: argument#1(netNum) should be less than Arena.height.") end
	if not horizontal and netNum > Arena.width then error("FN!PurpleSoulLib.CreateNet()-ArgumentOutOfRangeException: argument#1(netNum) should be less than Arena.width.")  end
	if #_nets > 0 then PurpleSoul.RemoveNet() end
	_horizontal = horizontal
	local arena_p_nth = GetPositionHorizontalToNet()
	local arena_p_ntv = GetPositionVerticalToNet()
	local arena_s_nth = GetArenaSizeHorizontalToNet()
	local arena_s_ntv = GetArenaSizeVerticalToNet()
	for i = 1, netNum do
		_nets[i] = {}
		_nets[i].type = 1
		_nets[i].sprite = CreateSprite("px", layer)
		_nets[i].sprite.color32 = {213, 53, 217}
		local v = arena_p_ntv - arena_s_ntv / 2 + arena_s_ntv * i / (netNum + 1)
		_nets[i].sprite.MoveToAbs(ConvertPosition(arena_p_nth, v))
		_nets[i].sprite.Scale(ConvertPosition(arena_s_nth, 1))
	end
	--[[-- Evaluation methods (Idea) -- if horizontal then
	if horizontal then
		for i = 1, netNum do
			_nets[i] = {}
			_nets[i].horizontal = true
			_nets[i].type = 1
			_nets[i].sprite = CreateSprite("px", layer)
			_nets[i].sprite.color32 = {213,  53, 217}
			local y = ArenaUtil.centerabsy - Arena.height / 2 + Arena.height * i / (netNum + 1)
			_nets[i].sprite.MoveToAbs(ArenaUtil.centerabsx, y)
			_nets[i].sprite.Scale(Arena.width, 1)
		end
	end
	]]
end

function PurpleSoul.JumpToNet(netNum)
	CheckArgumentType("JumpToNet", netNum, "netNum", 1, "number", false)
	if #_nets == 0 then error("FN!PurpleSoulLib.JumpToNet()-NetNotFoundException: you should call CreateNet() before calling JumpToNet().")             end
	if netNum <= 0 then error("FN!PurpleSoulLib.JumpToNet()-ArgumentOutOfRangeException: argument#1(netNum) should be positive number.")                end
	if netNum > #_nets then error("FN!PurpleSoulLib.JumpToNet()-ArgumentOutOfRangeException: argument#1(netNum) shouldn't be over the number of nets.") end
	if _horizontal then Player.MoveToAbs(Player.absx, _nets[netNum].sprite.absy)
	else                Player.MoveToAbs(_nets[netNum].sprite.absx, Player.absy)
	end
	_current_net_num = netNum
end


-- System --
local function GetJumpKey()
	if _horizontal then return Input.Down, Input.Up
	else                return Input.Left, Input.Right
	end
end

local function UpdateMove()
	if control_override then return end
	local speed = 2
	if allow_slow_move and Input.Cancel >= 1 then
		speed = 1
	end
	if _horizontal then
		if Input.Left  >= 1 then Player.Move(-speed, 0, false) end
		if Input.Right >= 1 then Player.Move( speed, 0, false) end
	else
		if Input.Down >= 1 then Player.Move(0, -speed, false) end
		if Input.Up   >= 1 then Player.Move(0,  speed, false) end
	end
end

local function UpdateJump()
	if #_nets == 0 then           error("FN!PurpleSoulLib.Update(): The nets should be created before calling Update()")            end
	if _current_net_num == 0 then error("FN!PurpleSoulLib.Update(): The Player should be moved to any net before calling Update()") end
	local negativeJumpKey, positiveJumpKey = GetJumpKey()
	if negativeJumpKey == 1 and _current_net_num > 1 then      PurpleSoul.JumpToNet(_current_net_num - 1) end
	if positiveJumpKey == 1 and _current_net_num < #_nets then PurpleSoul.JumpToNet(_current_net_num + 1) end
end

-- System(Main) --
function PurpleSoul.Update()
	if not _active then return end
	UpdateMove()
	UpdateJump()
end

function PurpleSoul.Dispose()
	PurpleSoul.RemoveNet()
end


-- Misc --
function PurpleSoul.GetColor()
	return {213, 53, 217}
end

return PurpleSoul