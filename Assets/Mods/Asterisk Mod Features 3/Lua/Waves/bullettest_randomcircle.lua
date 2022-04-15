local hasSpecialHurtMethod = (Encounter["PlayerHurt"] ~= nil)
local fellize = (Encounter["fell"] ~= nil)
function OnHit(bullet)
    if hasSpecialHurtMethod then
        Encounter.Call("PlayerHurt", 4)
    else
        Player.Hurt(4)
    end
end

local main_bullet = CreateProjectileAbs("bullet", -256, -256)
if fellize then main_bullet.sprite.color = {1, 0, 0} end
local bullets = {}
local frame_counter = 0

function Update()
	if frame_counter % 50 == 0 then
		main_bullet.MoveTo(math.random(-310, 310), math.random(100, 230))
	elseif frame_counter % 50 == 49 then
		local centerx = main_bullet.x
		local centery = main_bullet.y
		local speed = 3
		local bullet_amount = math.random(8, 32)
		local an_angle = 360 / bullet_amount
		for i = 1, bullet_amount do
			local bullet = CreateProjectile("bullet", centerx, centery)
			local radian = math.rad(an_angle * i)
			bullet["velocity"] = {speed * math.cos(radian), speed * math.sin(radian)}
			if fellize then bullet.sprite.color = {1, 0, 0} end
			table.insert(bullets, bullet)
		end
	end
	for i = 1, #bullets do
		local bullet = bullets[i]
		bullet.Move(bullet["velocity"][1], bullet["velocity"][2])
	end
	frame_counter = frame_counter + 1
end

function EndingWave()
	for i = 1, #bullets do
		bullets[i].Remove()
	end
	main_bullet.Remove()
end