local hasSpecialHurtMethod = (Encounter["PlayerHurt"] ~= nil)
local fellize = (Encounter["fell"] ~= nil)
function OnHit(bullet)
    if hasSpecialHurtMethod then
        Encounter.Call("PlayerHurt", 7)
    else
        Player.Hurt(7)
    end
end

Arena.Resize(320, 150)

local BlueSoul = require "FN!BlueSoulLib"
local frame_counter = 0
local box = nil
local bullets = {}
local after_bullets = {}
local right_to_left = math.random(1, 2) == 1

function Initialize()
    box = CreateSprite("empty", "BelowBullet")
    box.MoveToAbs(ArenaUtil.centerabsx, ArenaUtil.centerabsy)
    box.Scale(Arena.width, Arena.height)
    box.Mask("BOX")
    for j = 1, 3 do
        for i = 1, 20 do
            local bullet = CreateProjectile("bullet", -168 + i * 16, -83 + j * 16)
            if fellize then bullet.sprite.color = {1, 0, 0} end
            bullet.sprite.SetParent(box)
            if (right_to_left and i <= 10) or (not right_to_left and i > 10) then
                table.insert(bullets, bullet)
            else
                bullet.Move(0, -48)
                table.insert(after_bullets, bullet)
            end
        end
    end
    if right_to_left then
        Player.MoveTo(80, Player.y)
        BlueSoul.CreatePlatform("platform", -80, -21)
    else
        Player.MoveTo(-80, Player.y)
        BlueSoul.CreatePlatform("platform", 80, -21)
    end
    BlueSoul.SetActive(true)
end

function Update()
    BlueSoul.Update()
    if frame_counter == 0 then
        Initialize()
    end
    frame_counter = frame_counter + 1
    if frame_counter >= 122 and frame_counter <= 170 then
        for i = 1, #after_bullets do
            after_bullets[i].Move(0, 1)
        end
    end
    if frame_counter == 260 then
        EndWave()
    end
end

function EndingWave()
    -- Before removes parent object, you'd better to remove all projectile.
    for i = 1, #after_bullets do
        after_bullets[i].Remove()
    end
    for i = 1, #bullets do
        bullets[i].Remove()
    end
    box.Remove()
    BlueSoul.SetActive(false)
    BlueSoul.Dispose()
end