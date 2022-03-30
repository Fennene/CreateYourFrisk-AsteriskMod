encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
"poseur (old)"
}

enemypositions = {
{0, 0}
}

flee = false
sparetext = "Skip Turn"

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}
final_attack = false

function EncounterStarting()
    -- If player launch this mod with official CYF, show error
    if isModifiedCYF == nil then
        error("This game can not be launched on official CYF.\nUse to lanuch CYF-AsteriskMod.")
    end
    if Asterisk == nil then
        error("This game can be launched on only CYF-AsteriskMod.")
    end

    Player.name = "Fennene"
    Player.lv = 20
    Player.hp = 99

    -- Set activate of buttons
    ButtonUtil.SetActives(true, true, false, true)
    -- Move all buttoms to center
    ButtonUtil.FIGHT.MoveTo(234, 0)
    ButtonUtil.ACT.MoveTo(81, 0)
    --ButtonUtil.ITEM.MoveTo(-79, 0)
    ButtonUtil.MERCY.MoveTo(-234, 0)
    -- Move
    ButtonUtil.FIGHT.Move(-187, 0)
    ButtonUtil.MERCY.Move(187, 0)

    ButtonUtil.FIGHT.SetColor(0.9, 0.7, 0)

    --ArenaUtil.SetDialogTextPosition(0, 0)

    -- Camera's Reverse
    --Misc.CameraVerticalReverse()
    --Misc.CameraHorizontalReverse()
    --Misc.ResetCameraReverse()

    -- Below methods of PlayerUtil works in EncounterStarting().
    -- However, recommend to use methods of PlayerUtil in FirstFrameUpdate().
    --PlayerUtil.SetHPLabelColor(1, 1, 0)
    --PlayerUtil.SetHPBarFillColor32(0, 255, 255)
    --PlayerUtil.HPTextMoveTo(10, 0)
end

function SetFell(active)
    local c = 1
    local prefix = "[effect:rotate]"
    if active then
        c = 0
        prefix = "[color:ff0000]" .. prefix
    end
    ArenaUtil.SetBorderColor(1, c, c)
    enemies[1].GetVar("monstersprite").color = {1, c, c}
    enemies[1].SetVar("dialogueprefix", prefix)
end

function FirstFrameUpdate()
    PlayerUtil.SetLV("??")
    PlayerUtil.HPUIMoveTo(-40, 0)
    PlayerUtil.SetHPLabelColor(1, 1, 0)
    PlayerUtil.SetHPBarFillColor32(0, 255, 255)
    PlayerUtil.HPTextMoveTo(10, 0)
    --[[
    PlayerUtil.SetHPControlOverride(true)
    PlayerUtil.SetHPBarLength(30)
    ]]
    PlayerUtil.SetHPControlOverride(true)
    PlayerUtil.SetHP(50, 99)
    PlayerUtil.SetHPText("?? / 99")
end

function Update()
    PlayerUtil.SetNameColor(1, 1, 1)
    PlayerUtil.SetLVColor(1, 0, 0)
    PlayerUtil.SetHPTextColor32(0, 175, 0)
    --PlayerUtil.SetHP(20, 30, true)
end

function EnteringState(newState, oldState)
    if oldState == "DEFENDING" and final_attack then
        BattleDialogue({"[noskip]YOU WON!\nYou earned 7 XP and 36 gold."})
    end
    if oldState == "DIALOGRESULT" and final_attack then
        State("DONE")
    end
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
    nextwaves = { possible_attacks[math.random(#possible_attacks)] }
end

function DefenseEnding()
    encountertext = RandomEncounterText()
end

function HandleSpare()
    State("ENEMYDIALOGUE")
end

function HandleItem(ItemID)
    BattleDialog({"Selected item " .. ItemID .. "."})
end

function BeforeEncounterEnding()
    final_attack = true
    wavetimer = math.huge
    Audio.Stop()
    nextwaves = {"bullettest_bouncy_end"}
    State("DEFENDING")
    return true
end
