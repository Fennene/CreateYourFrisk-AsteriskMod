function OnHit(bullet)
    if bullet.sprite.color32[1] == 200 then
        Encounter.Call("PlayerHurt", 3)
    else
        Encounter.Call("PlayerHurt", {6, 1.4})
    end
end
spawntimer = 0
bullets = {}
bullets2 = {}
mult = 0.5
function Update()
    spawntimer = spawntimer + 1
    if(spawntimer % 30 == 0) then
        local numbullets = 10
        for i=1,numbullets+1 do
            local bullet = CreateProjectile('bullet', 0, 180)
            bullet.sprite.color32 = {200, 127, 127}
            bullet.SetVar('timer', 0)
            bullet.SetVar('offset', math.pi * 2 * i / numbullets)
            bullet.SetVar('negmult', mult)
            bullet.SetVar('lerp', 0)
            table.insert(bullets, bullet)
        end
        mult = mult + 0.05
    end
    if spawntimer % 60 == 0 then
        local bullet = CreateProjectile('bullet', -320, Player.y)
        bullet.sprite.color32 = {240, 152, 152}
        table.insert(bullets2, bullet)
    end
    for i=1,#bullets do
        local bullet = bullets[i]
        local timer = bullet.GetVar('timer')
        local offset = bullet.GetVar('offset')
        local lerp = bullet.GetVar('lerp')
        local posx = (70*lerp)*math.sin(timer*bullet.GetVar('negmult') + offset)
        local posy = (70*lerp)*math.cos(timer + offset) + 180 - lerp*50
        bullet.MoveTo(posx, posy)
        bullet.SetVar('timer', timer + 1/40)
        lerp = lerp + 1 / 90
        if lerp > 4.0 then lerp = 4.0 end
        bullet.SetVar('lerp', lerp)
    end
    for i = 1, #bullets2 do bullets2[i].Move(7, 0) end
    if spawntimer == 450 then EndWave() end
end
function EndingWave()
    for i = 1, #bullets2 do bullets2[i].Remove() end
    for i = 1, #bullets do bullets[i].Remove() end
end