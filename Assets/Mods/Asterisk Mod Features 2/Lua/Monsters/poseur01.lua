sprite = "empty"
name = "Poseur"

local enemysprite = CreateSprite("poseur", "BelowArena")
enemysprite.Move(0, enemysprite.height / 2)
enemysprite.Move(0, -9)

local damaged = false
local shakeInProgress = false
--[[
local shakeX = {
    [0] =  12,
    [1] = -12,
    [2] =  7,
    [3] = -7,
    [4] =  3,
    [5] = -3,
    [6] =  1,
    [7] = -1,
    [8] =  0
 }
 ]]
local shakeX = {
    [0] =  12,
    [1] = -24,
    [2] =  19,
    [3] = -14,
    [4] =  10,
    [5] = -6,
    [6] =  4,
    [7] = -2,
    [8] =  1
 }
local shakeX_Length = 9
local shakeIndex = -1
local shakeTimer = 0.0
local totalShakeTime = 1.5

function StartShake()
    if shakeInProgress then return end
    shakeInProgress = true
    totalShakeTime = shakeX_Length * (1.5 / 8.0)
end

function UpdateShake()
    if shakeInProgress then
        local shakeidx = math.floor(shakeTimer * shakeX_Length / totalShakeTime)
        if shakeIndex ~= shakeidx then
            if shakeIndex ~= shakeidx and shakeIndex >= shakeX_Length then
                shakeIndex = shakeX_Length - 1
            end
            shakeIndex = shakeidx
            local localEnePos = {x = enemysprite.absx, y = enemysprite.absy}
            enemysprite.MoveToAbs(localEnePos.x + shakeX[shakeIndex], localEnePos.y)
        end
        if shakeTimer < 1.5 then
        end
        shakeTimer = shakeTimer + Time.dt
        if shakeTimer >= totalShakeTime then
            shakeInProgress = false
            enemysprite.Dust()
        end
    end
end