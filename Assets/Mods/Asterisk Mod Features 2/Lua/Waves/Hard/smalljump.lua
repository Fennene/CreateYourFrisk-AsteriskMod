function OnHit(bullet) Encounter.Call("PlayerHurt", 5) end
Arena.Resize(320, 150)
local BlueSoul = require "FN!BlueSoulLib"
local frame_counter = 0
local box = nil
local toleft_bullets = {}
local toright_bullets = {}
function CreatePole()
	local startY = -Arena.height / 2 + 8
	for i = 1, (Arena.height / 16) do
		local bullet = nil
		local bullet2 = nil
		if i == 1 then
			bullet = CreateProjectile("bullet", 180, startY)
			bullet2 = CreateProjectile("bullet", -180, startY)
			startY = startY + 4
		elseif i ~= 2 then
			bullet = CreateProjectile("bullet", 180, startY + ((i - 1) * 16))
			bullet2 = CreateProjectile("bullet", -180, startY + ((i - 1) * 16))
		end
		if bullet ~= nil then
			bullet.sprite.SetParent(box)
			bullet2.sprite.SetParent(box)
			bullet.sprite.color32 = {200, 127, 127}
			bullet2.sprite.color32 = {200, 127, 127}
			table.insert(toleft_bullets, bullet)
			table.insert(toright_bullets, bullet2)
		end
	end
end
function Initialize()
	box = CreateSprite("empty", "BelowBullet")
	box.MoveToAbs(ArenaUtil.centerabsx, ArenaUtil.centerabsy)
	box.Scale(Arena.width, Arena.height + 10)
	box.Mask("BOX")
	BlueSoul.SetActive(true)
end
function Update()
	BlueSoul.Update()
	if frame_counter == 0 then Initialize() end
	frame_counter = frame_counter + 1
	if frame_counter % 30 == 0 then CreatePole() end
	for i = 1, #toleft_bullets do toleft_bullets[i].Move(-5, 0) end
	for i = 1, #toright_bullets do toright_bullets[i].Move(5, 0) end
	if frame_counter == 380 then EndWave() end
end
function EndingWave()
	for i = 1, #toleft_bullets do toleft_bullets[i].Remove() end
	for i = 1, #toright_bullets do toright_bullets[i].Remove() end
	box.Remove()
	BlueSoul.SetActive(false)
	BlueSoul.Dispose()
end