function OnHit(bullet)
	Player.Hurt(2)
end

Arena.Resize(320, 130)

local BlueSoul = require "FN!BlueSoulLib"
local PurpleSoul = require "FN!PurpleSoulLib"

BlueSoul.SetActive(true, true, true)
PurpleSoul.SetActive(true, false, false)
BlueSoul.SetControlOverride(true)
PurpleSoul.SetControlOverride(true)
PurpleSoul.CreateNet(7, false)
PurpleSoul.JumpToNet(4)

local half_soul = CreateSprite("FN!SoulModeLib/ut-heart-half", "BelowBullet")
half_soul.color32 = PurpleSoul.GetColor()
half_soul.MoveToAbs(Player.absx, Player.absy)

local box = CreateSprite("empty", "BelowBullet")
box.MoveToAbs(ArenaUtil.centerabsx, ArenaUtil.centerabsy)
box.Scale(Arena.width, Arena.height)
box.Mask("BOX")
local bottom_bullets = {}
local fall_bullets = {}
local frame_counter = 0

function UpdateBullet()
	frame_counter = frame_counter + 1
	if frame_counter % 40 == 0 then
		local bullet = CreateProjectile("bullet", 170, -Arena.height/2 + 8)
		bullet.sprite.SetParent(box)
		table.insert(bottom_bullets, bullet)
	end
	if frame_counter % 50 == 0 then
		local bullet = CreateProjectile("bullet", Player.x, Arena.height/2 + 8)
		bullet.sprite.SetParent(box)
		table.insert(fall_bullets, bullet)
	end
	for i = 1, #bottom_bullets do
		bottom_bullets[i].Move(-3, 0)
	end
	for i = 1, #fall_bullets do
		fall_bullets[i].Move(0, -1)
	end
end

function Update()
	BlueSoul.Update()
	PurpleSoul.Update()
	half_soul.MoveToAbs(Player.absx, Player.absy)
	half_soul.alpha = PlayerUtil.GetSoulAlpha()
	UpdateBullet()
end

function EndingWave()
	for i = 1, #bottom_bullets do
		bottom_bullets[i].Remove()
	end
	for i = 1, #fall_bullets do
		fall_bullets[i].Remove()
	end
	box.Remove()
	half_soul.Remove()
	PurpleSoul.SetActive(false, false, false)
	BlueSoul.SetActive(false, true, true)
	PurpleSoul.Dispose()
	BlueSoul.Dispose()
end