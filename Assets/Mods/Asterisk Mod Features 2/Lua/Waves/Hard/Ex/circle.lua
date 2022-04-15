function OnHit(bullet) Encounter.Call("PlayerHurt", 6) end
local main_bullet = CreateProjectileAbs("bullet", -256, -256)
main_bullet.sprite.color32 = {200, 127, 127}
local bullets = {}
local frame_counter = 0
function Update()
	if frame_counter % 30 == 0 then
		main_bullet.MoveTo(math.random(-310, 310), math.random(120, 230))
	elseif frame_counter % 30 == 29 then
		local speed = 2.5 + math.random(0, 20) / 15
		local bullet_amount = math.random(22, 36)
		local an_angle = 360 / bullet_amount
		for i = 1, bullet_amount do
			local bullet = CreateProjectile("bullet", main_bullet.x, main_bullet.y)
			bullet.sprite.color32 = {200, 127, 127}
			local radian = math.rad(an_angle * i)
			bullet["velocity"] = {speed * math.cos(radian), speed * math.sin(radian)}
			table.insert(bullets, bullet)
		end
	end
	for i = 1, #bullets do bullets[i].Move(bullets[i]["velocity"][1], bullets[i]["velocity"][2]) end
	if frame_counter == 450 then EndWave() end
	frame_counter = frame_counter + 1
end
function EndingWave()
	for i = 1, #bullets do bullets[i].Remove() end
	main_bullet.Remove()
end