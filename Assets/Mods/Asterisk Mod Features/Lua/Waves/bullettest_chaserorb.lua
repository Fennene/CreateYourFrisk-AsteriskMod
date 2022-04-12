local hasSpecialHurtMethod = (Encounter["PlayerHurt"] ~= nil)
local fellize = (Encounter["fell"] ~= nil)
function OnHit(bullet)
    if hasSpecialHurtMethod then
        Encounter.Call("PlayerHurt", 10)
    else
        Player.Hurt(10)
    end
end
-- The chasing attack from the documentation example.
chasingbullet = CreateProjectile('bullet', Arena.width/2, Arena.height/2)
if fellize then chasingbullet.sprite.color = {1, 0, 0} end
chasingbullet.SetVar('xspeed', 0)
chasingbullet.SetVar('yspeed', 0)

function Update()
    local xdifference = Player.x - chasingbullet.x
    local ydifference = Player.y - chasingbullet.y
    local xspeed = chasingbullet.GetVar('xspeed') / 2 + xdifference / 100
    local yspeed = chasingbullet.GetVar('yspeed') / 2 + ydifference / 100
    chasingbullet.Move(xspeed, yspeed)
    chasingbullet.SetVar('xspeed', xspeed)
    chasingbullet.SetVar('yspeed', yspeed)
end

function EndingWave()
    chasingbullet.Remove()
end