function OnHit(bullet)
    if bullet.sprite.color32[1] == 200 then
        Encounter.Call("PlayerHurt", 6)
    else
        Encounter.Call("PlayerHurt", {10, 1.2})
    end
end
Arena.Resize(320, 150)
local BlueSoul = require "FN!BlueSoulLib"
local frame_counter = 0
local box = nil
local bullets = {}
local bullets2 = {}
local after_bullets = {}
local right_to_left = math.random(1, 2) == 1
function Initialize()
    box = CreateSprite("empty", "BelowBullet")
    box.MoveToAbs(ArenaUtil.centerabsx, ArenaUtil.centerabsy + 10)
    box.Scale(Arena.width, Arena.height)
    box.Mask("BOX")
    for j = 1, 3 do
        for i = 1, 20 do
            local bullet = CreateProjectile("bullet", -168 + i * 16, -83 + j * 16)
            bullet.sprite.color32 = {240, 152, 152}
            bullet.sprite.SetParent(box)
            if right_to_left == (i <= 10) then
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
    if frame_counter == 0 then Initialize() end
    frame_counter = frame_counter + 1
    if frame_counter >= 122 and frame_counter <= 170 then
        for i = 1, #after_bullets do
            after_bullets[i].Move(0, 1)
        end
    end
    if frame_counter < 120 then return end
    if frame_counter % 30 == 20 then
        local x = 160
        if not right_to_left then x = -160 end
        local bullet = CreateProjectile("bullet", x, -20)
        bullet.sprite.color32 = {200, 127, 127}
        bullet.sprite.SetParent(box)
        table.insert(bullets2, bullet)
    end
    for i = 1, #bullets2 do
        local x = -1
        if not right_to_left then x = 1 end
        bullets2[i].Move(x * 3, 0)
    end
    if frame_counter < 200 then return end
    if frame_counter >= 220 then
        local x = 1
        if not right_to_left then x = -1 end
        BlueSoul.PlatformMove("platform", x, 0)
    end
    if frame_counter == 510 then EndWave() end
end
function EndingWave()
    for i = 1, #after_bullets do after_bullets[i].Remove() end
    for i = 1, #bullets2 do bullets2[i].Remove() end
    for i = 1, #bullets do bullets[i].Remove() end
    box.Remove()
    BlueSoul.SetActive(false)
    BlueSoul.Dispose()
end