-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\n[このModはCYF-AsteriskModでのみ起動できます。]"
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "[starcolor:ffffff][color:ff0000]R [color:fca600]A [color:ffff00]I [color:00c000]N [color:42fcff]B [color:003cff]O [color:d535d9]W [color:ffffff]!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 7.0
arenasize = {155, 130}
playerskipdocommand = true
flee = false

enemies = {
    "poseur10"
}

enemypositions = {
    {0, 0}
}

possible_attacks = {
    "bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou",
    "bullettest_randomcircle"
}

rainbow = 1
r = 255
g = 0
b = 0
defending = false
afterimage = nil
speed = 4

function EncounterStarting()
    Player.name = "Nil256"

    Inventory.AddCustomItems({"[starcolor:ffffff][color:ff0000]R [color:fca600]A [color:ffff00]I [color:00c000]N [color:42fcff]B [color:003cff]O [color:d535d9]W [color:ffffff]"}, {3})
    Inventory.SetInventory({"[starcolor:ffffff][color:ff0000]R [color:fca600]A [color:ffff00]I [color:00c000]N [color:42fcff]B [color:003cff]O [color:d535d9]W [color:ffffff]"})

    ButtonUtil.FIGHT.SetSprite("fightbt_0", "fightbt_1", "UI/NonColoredButtons")
    ButtonUtil.ACT.SetSprite("actbt_0", "actbt_1", "UI/NonColoredButtons")
    ButtonUtil.ITEM.SetSprite("itembt_0", "itembt_1", "UI/NonColoredButtons")
    ButtonUtil.MERCY.SetSprite("mercybt_0", "mercybt_1", "UI/NonColoredButtons")
    -- The above code is same to below code.
    -- ButtonUtil.SetSprites("UI/NonColoredButtons")

    local BGMask = CreateSprite("px", "BelowUI")
    BGMask.color = {0, 0, 0}
    BGMask.Scale(640, 480)

    afterimage = CreateSprite("ut-heart", "BelowPlayer")
    afterimage.alpha = 0
end

function UpdatePlayerSoul()
    if not defending then return end
    afterimage.MoveToAbs(Player.absx, Player.absy)
    Player.sprite.color32 = {r, g, b}
    afterimage.color32 = {r, g, b}
    Player.SetControlOverride(true)
    if Input.Left >= 1 then
        Player.Move(-speed, 0)
    end
    if Input.Right >= 1 then
        Player.Move(speed, 0)
    end
    if Input.Down >= 1 then
        Player.Move(0, -speed)
    end
    if Input.Up >= 1 then
        Player.Move(0, speed)
    end
end

function Update()
    if rainbow == 1 then
        g = math.min(g + 3, 255)
        if g == 255 then rainbow = 2 end
    elseif rainbow == 2 then
        r = math.max(0, r - 3)
        if r == 0 then rainbow = 3 end
    elseif rainbow == 3 then
        b = math.min(b + 3, 255)
        if b == 255 then rainbow = 4 end
    elseif rainbow == 4 then
        g = math.max(0, g - 3)
        if g == 0 then rainbow = 5 end
    elseif rainbow == 5 then
        r = math.min(r + 3, 255)
        if r == 255 then rainbow = 6 end
    elseif rainbow == 6 then
        b = math.max(0, b - 3)
        if b == 0 then rainbow = 1 end
    end
    PlayerUtil.HPBar.fillcolor32 = {r, g, b}
    ButtonUtil.FIGHT.color32 = {r, g, b}
    ButtonUtil.ACT.color32 = {r, g, b}
    ButtonUtil.ITEM.color32 = {r, g, b}
    ButtonUtil.MERCY.color32 = {r, g, b}
    ArenaUtil.SetBorderColor32(r, g, b)
    enemies[1].SetVar("fillbarcolor", {r, g, b})
    UpdatePlayerSoul()
end

function EnteringState(oldState, newState)
    if newState == "ACTIONSELECT" then
        local can_spare = (speed >= 10)
        enemies[1].SetVar("canspare", can_spare)
        if can_spare then
            sparetext = "[starcolor:ffffff][color:ff0000]S[color:fca600]p[color:ffff00]a[color:00c000]r[color:42fcff]e[color:003cff]![color:d535d9]![color:ffffff]"
        else
            sparetext = "[starcolor:969696][color:969696]Spare"
        end
    end
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
    nextwaves = { possible_attacks[math.random(#possible_attacks)] }
    defending = true
    afterimage.MoveToAbs(Player.absx, Player.absy)
    afterimage.alpha = 0.75
end

function DefenseEnding()
    defending = false
    afterimage.alpha = 0
    Player.sprite.color = {1, 0, 0}
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    speed = speed + 2
    BattleDialog({"[starcolor:ffffff][color:ff0000]R [color:fca600]A [color:ffff00]I [color:00c000]N [color:42fcff]B [color:003cff]O [color:d535d9]W [color:ffffff]![health:99]"})
end

function BeforeDeath()
    Player.sprite.color = {1, 0, 0}
end