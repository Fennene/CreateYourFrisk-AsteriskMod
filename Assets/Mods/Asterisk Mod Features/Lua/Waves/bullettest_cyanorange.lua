local hasSpecialHurtMethod = (Encounter["PlayerHurt"] ~= nil)
local fellize = (Encounter["fell"] ~= nil)
function OnHit(bullet)
    local damage = (bullet["cyan"] and Player.ismoving) or (bullet["orange"] and not Player.ismoving)
    if damage then
        if hasSpecialHurtMethod then
            Encounter.Call("PlayerHurt", 4)
        else
            Player.Hurt(4)
        end
    end
end

local box = CreateSprite("empty", "BelowBullet")
box.MoveToAbs(ArenaUtil.centerabsx, ArenaUtil.centerabsy)
box.Scale(Arena.width, Arena.height)
box.Mask("BOX")
local vertical_bullets = {}
local horizontal_bullets = {}
local frame_counter = 0

function Update()
    if frame_counter % 70 == 20 then
        local startX = -Arena.width / 2 + 8
        local orange = math.random(1, 2) == 1
        for i = 1, math.floor(Arena.width / 16) + 1 do
            local bullet = CreateProjectile("bullet", startX + ((i - 1) * 16), Arena.height / 2)
            if orange then
                bullet.sprite.color32 = { 255, 166, 0 }
            else
                bullet.sprite.color32 = { 66, 226, 255 } -- or { 0, 162, 232 }
            end
            bullet["orange"] = orange
            bullet["cyan"] = not orange
            bullet.sprite.SetParent(box)
            table.insert(vertical_bullets, bullet)
        end
    elseif frame_counter % 70 == 55 then
        local startY = -Arena.height / 2 + 8
        orange = math.random(1, 2) == 1
        for i = 1, math.floor(Arena.height / 16) + 1 do
            local bullet = CreateProjectile("bullet", Arena.width / 2, startY + ((i - 1) * 16))
            if orange then
                bullet.sprite.color32 = { 255, 166, 0 }
            else
                bullet.sprite.color32 = { 66, 226, 255 } -- or { 0, 162, 232 }
            end
            bullet["orange"] = orange
            bullet["cyan"] = not orange
            bullet.sprite.SetParent(box)
            table.insert(horizontal_bullets, bullet)
        end
    end
    for i = 1, #vertical_bullets do
        vertical_bullets[i].Move(0, -2)
    end
    for i = 1, #horizontal_bullets do
        horizontal_bullets[i].Move(-2, 0)
    end
    frame_counter = frame_counter + 1
end

function EndingWave()
    -- Before removes parent object, you'd better to remove all projectile.
    --[[
    for i = 1, #vertical_bullets do
        vertical_bullets[i].Remove()
    end
    for i = 1, #horizontal_bullets do
        horizontal_bullets[i].Remove()
    end
    box.Remove()
    ]]
    -- However, there is another solution.
    box.Remove(true)
end