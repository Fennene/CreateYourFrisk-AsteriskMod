spawntimer = 0
bullet = {}

function Update()
    spawntimer = spawntimer + 1

    if spawntimer == 1 then
        local posx = 30 - math.random(60)
        local posy = Arena.height/2 + 20
        bullet = CreateProjectile('bullet', posx, posy)
        bullet["initX"] = posx
        bullet["initY"] = posy
        bullet.SetVar('velx', 1 - 2*math.random())
        bullet.SetVar('vely', 0)
    end

    if spawntimer < 120 and spawntimer % 2 == 0 then
        bullet.MoveTo(bullet["initX"] + math.random(-2, 2), bullet["initY"] + math.random(-2, 2))
    end

    if spawntimer == 120 then
        bullet.MoveTo(bullet["initX"], bullet["initY"])
    end

    if spawntimer > 200 then
        local velx = bullet.GetVar('velx')
        local vely = bullet.GetVar('vely')
        local newposx = bullet.x + velx
        local newposy = bullet.y + vely
        if(bullet.x > -Arena.width/2 and bullet.x < Arena.width/2) then
            if(bullet.y < -Arena.height/2 + 8) then 
                newposy = -Arena.height/2 + 8
                vely = 4
            end
        end
        vely = vely - 0.04
        bullet.MoveTo(newposx, newposy)
        bullet.SetVar('vely', vely)
    end

    if spawntimer == 600 then
        EndWave()
    end
end