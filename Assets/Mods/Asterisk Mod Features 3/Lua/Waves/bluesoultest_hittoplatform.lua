local hasSpecialHurtMethod = (Encounter["PlayerHurt"] ~= nil)
local fellize = (Encounter["fell"] ~= nil)
function OnHit(bullet)
    if hasSpecialHurtMethod then
        Encounter.Call("PlayerHurt", 5)
    else
        Player.Hurt(5)
    end
end

Arena.Resize(320, 150)

local BlueSoul = require "FN!BlueSoulLib"
local frame_counter = 0
local bullets = {}
local platformX = math.random(80, 160) * math.pow(-1, math.random(1, 2))
local impact_frame = math.floor(math.abs(platformX) / 2) + 40
local end_frame = impact_frame + 100
local time_text = CreateText("[font:uidialog]", {ArenaUtil.centerabsx, ArenaUtil.centerabsy - 20}, 1023, "BelowPlayer")
time_text.HideBubble()
time_text.progressmode = "none"
time_text.alpha = 0.6

function Initialize()
    for j = 1, 4 do
        for i = 1, 20 do
            local bullet = CreateProjectile("bullet", -168 + i * 16, 83 - j * 16)
            if fellize then bullet.sprite.color = {1, 0, 0} end
            table.insert(bullets, bullet)
        end
    end
    BlueSoul.SetGravity("up")
    BlueSoul.CreatePlatform(1, platformX, 5)
    BlueSoul.SetGravity()
    Player.MoveTo(0, 0)
    BlueSoul.SetActive(true)
end

function Update()
    BlueSoul.Update()
    if frame_counter == 0 then
        Initialize()
    end
    frame_counter = frame_counter + 1
    if frame_counter <= impact_frame then
        time_text.SetText("[font:uidialog][instant]" .. (impact_frame - frame_counter))
        time_text.absx = ArenaUtil.centerabsx - time_text.GetTextWidth() / 2
    end
    if frame_counter == impact_frame then
        BlueSoul.SetGravity("up")
        BlueSoul.HitToSurface()
    end
    if frame_counter >= impact_frame + 20 then
        BlueSoul.PlatformMove(1, 0, -1)
    end
    if frame_counter >= end_frame then
        EndWave()
    end
end

function EndingWave()
    for i = 1, #bullets do
        bullets[i].Remove()
    end
    time_text.Remove()
    BlueSoul.SetActive(false)
    BlueSoul.Dispose()
end