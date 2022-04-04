comments = {"Smells like the work\rof an enemy stand.", "Poseur is posing like his\rlife depends on it.", "Poseur's limbs shouldn't be\rmoving in this way."}
commands = {"Test"}
randomdialogue = {"Random\nDialogue\n1.", "Random\nDialogue\n2.", "Random\nDialogue\n3."}

sprite = "poseur_empty"
name = "Poseur"
hp = 80
atk = 1
def = 1
check = "Check message goes here."
dialogbubble = "right"
canspare = true
cancheck = true

KilledEXP = 47
KilledGold = 36
SparedGold = 15

function HandleCustomCommand(command)
    if command == "TEST" then
        BattleDialog("This is test.[func:Test]")
    end
end

function Test()
    DEBUG("This is test.")
end

-- Even if it is downed, you can fight it again.

Sprite = nil
_Active = false
_Killed = false
_Spared = false

function OnActive(active) -- You MUST NOT use SetActive() in this function.
    if not Encounter["_initialized"] then return end
    if active then
        hp = 70 + (Player.lv * 10)
        if Player.lv >= 5 then
            hp = hp + math.random(-15, 120)
        end
        maxhp = hp
        KilledEXP = 43 + (Player.lv * 4) + math.random(-5, 30)
        if Sprite ~= nil then
            Sprite.Remove()
        end
        Sprite = CreateSprite("poseur", "BelowArena")
        Sprite.Move(0, Sprite.height / 2)
        Sprite.Move(0, -9)
        _Active = true
    else
        Encounter.Call("CheckWon")
    end
end

_Spare_dusts = {}

function OnSpare()
    _Spared = true
    for i = 1, 20 do
        _Spare_dusts[i] = CreateSprite("UI/Battle/spr_dustcloud_0", "BelowArena")
        _Spare_dusts[i].MoveToAbs(Sprite.absx, Sprite.absy)
        local d = math.random(4, 80)
        local rad = math.random(360)
        rad = rad * 180 / math.pi
        _Spare_dusts[i].Move(d * math.cos(rad), d * math.sin(rad))
        if math.random(4) == 1 then
            local scale = 1.0 + (math.random(0, 7) / 20)
            _Spare_dusts[i].Scale(scale, scale)
        end
        _Spare_dusts[i].SetAnimation({"spr_dustcloud_0", "spr_dustcloud_1", "spr_dustcloud_2"}, 0.13, "UI/Battle")
        _Spare_dusts[i].loopmode = "ONESHOTEMPTY"
    end
    Audio.PlaySound("enemydust")
    Sprite.alpha = 0.5
    SetActive(false)
end

function OnDeath()
    _Killed = true
    Sprite.Dust(true, true)
    SetActive(false)
end

function Deactivate()
    if #_Spare_dusts > 0 then
        for i = 1, #_Spare_dusts do
            _Spare_dusts[i].Remove()
        end
    end
    _Spare_dusts = {}
    _Active = false
end

-- From CYF

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
local wait1frame = false

function Update()
    if wait1frame and not shakeInProgress then
        shakeInProgress = true
        totalShakeTime = shakeX_Length * (1.5 / 8.0)
        wait1frame = false
        return
    end
    if shakeInProgress then
        local shakeidx = math.floor(shakeTimer * shakeX_Length / totalShakeTime)
        if damaged and shakeIndex ~= shakeidx then
            if shakeIndex ~= shakeidx and shakeIndex >= shakeX_Length then
                shakeIndex = shakeX_Length - 1
            end
            shakeIndex = shakeidx
            -- sprite shaker ---
            local localEnePos = {x = Sprite.absx, y = Sprite.absy}
            Sprite.MoveToAbs(localEnePos.x + shakeX[shakeIndex], localEnePos.y)
            --------------------
        end
        if shakeTimer < 1.5 then
        end
        shakeTimer = shakeTimer + Time.dt
        if shakeTimer >= totalShakeTime then
            shakeInProgress = false
        end
    end
end

function BeforeDamageValues(damage)
    damaged = false
    --shakeInProgress = false
    shakeIndex = -1
    shakeTimer = 0.0
    wait1frame = false
    if damage > 0 then
        damaged = true
    end
end

function HandleAttack(attackstatus)
    if attackstatus ~= -1 then
        if damaged then
            wait1frame = true
        end
    end
end
