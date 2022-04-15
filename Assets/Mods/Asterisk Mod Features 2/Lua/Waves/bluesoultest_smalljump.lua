local hasSpecialHurtMethod = (Encounter["PlayerHurt"] ~= nil)
local fellize = (Encounter["fell"] ~= nil)
function OnHit(bullet)
    if hasSpecialHurtMethod then
        Encounter.Call("PlayerHurt", 3)
    else
        Player.Hurt(3)
    end
end

Arena.Resize(320, 150)

local BlueSoul = require "FN!BlueSoulLib"
local frame_counter = 0
local box = nil
local toleft_bullets = {}
local toright_bullets = {}

function CreatePole()
	local startY = -Arena.height / 2 + 8
	local bullet_num = Arena.height / 16
	for i = 1, bullet_num do
		local bullet = nil
		local bullet2 = nil
		if i == 1 then
			bullet = CreateProjectile("bullet", 180, startY)
			bullet2 = CreateProjectile("bullet", -180, startY)
			if fellize then
				bullet.sprite.color = {1, 0, 0}
				bullet2.sprite.color = {1, 0, 0}
			end
			startY = startY + 4
		elseif i ~= 2 then
			bullet = CreateProjectile("bullet", 180, startY + ((i - 1) * 16))
			bullet2 = CreateProjectile("bullet", -180, startY + ((i - 1) * 16))
			if fellize then
				bullet.sprite.color = {1, 0, 0}
				bullet2.sprite.color = {1, 0, 0}
			end
		end
		if bullet ~= nil then
			bullet.sprite.SetParent(box)
			bullet2.sprite.SetParent(box)
			table.insert(toleft_bullets, bullet)
			table.insert(toright_bullets, bullet2)
		end
	end
end

function Initialize()
	box = CreateSprite("empty", "BelowBullet")
	box.MoveToAbs(ArenaUtil.centerabsx, ArenaUtil.centerabsy)
	box.Scale(Arena.width, Arena.height)
	box.Mask("BOX")
	BlueSoul.SetActive(true)
end

function Update()
	BlueSoul.Update()
	if frame_counter == 0 then
		Initialize()
	end
	frame_counter = frame_counter + 1
	if frame_counter % 45 == 0 then
		CreatePole()
	end
	for i = 1, #toleft_bullets do
		toleft_bullets[i].Move(-4, 0)
	end
	for i = 1, #toright_bullets do
		toright_bullets[i].Move(4, 0)
	end
end

function EndingWave()
	-- Before removes parent object, you'd better to remove all projectile.
	for i = 1, #toleft_bullets do
		toleft_bullets[i].Remove()
	end
	for i = 1, #toright_bullets do
		toright_bullets[i].Remove()
	end
	box.Remove()
	BlueSoul.SetActive(false)
	BlueSoul.Dispose()
end