function OnHit(bullet)
    if bullet.sprite.color32[1] == 200 then
        Encounter.Call("PlayerHurt", 4)
    else
        Encounter.Call("PlayerHurt", {8, 1.0})
    end
end
spawntimer = 0
bullets = {}
function Update()
    spawntimer = spawntimer + 1
    if spawntimer%22 == 0 then
        local bullet = CreateProjectile('bullet', 30-math.random(60), Arena.height/2)
        bullet.sprite.color32 = {200, 127, 127}
        bullet.SetVar('velx', 1 - 2*math.random())
        bullet.SetVar('vely', 0)
        table.insert(bullets, bullet)
    end
    if spawntimer%24 == 0 then
        local bullet = CreateProjectile('bullet', 30-math.random(60), Arena.height/2)
        bullet.sprite.color32 = {240, 152, 152}
        bullet.SetVar('velx', 1 - 2*math.random())
        bullet.SetVar('vely', 0)
        table.insert(bullets, bullet)
    end
    for i=1,#bullets do
        local bullet = bullets[i]
        local vely = bullet.GetVar('vely')
        local newposy = bullet.y + vely
        if(bullet.x > -Arena.width/2 and bullet.x < Arena.width/2) then
            if(bullet.y < -Arena.height/2 + 8) then 
                newposy = -Arena.height/2 + 8
                vely = 4
            end
        end
        vely = vely - 0.04
        bullet.MoveTo(bullet.x+bullet.GetVar('velx'), newposy)
        bullet.SetVar('vely', vely)
    end
    if spawntimer == 400 then EndWave() end
end
function EndingWave()
    for i = 1, #bullets do
        bullets[i].Remove()
    end
end