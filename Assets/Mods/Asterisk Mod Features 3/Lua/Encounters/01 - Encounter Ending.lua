-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end
-- checks that the player enable Exprimental Features option.
if not AsteriskExperiment then
    error("You should enable Experimental Features in option.")
end

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_bouncy"}
wavetimer = 4.0
arenasize = {155, 130}
flee = false
enemies = {"poseur01"}
enemypositions = {{0, 0}}

final_attack = false

function EncounterStarting()
    Player.name = "Nil256"
    Player.lv = 10
    Player.hp = Player.lv * 4 + 16
end

function EnteringState(newState, oldState)
    if not final_attack then return end
    if oldState == "DEFENDING" then
        BattleDialogue({"[noskip]YOU WON!\nYou earned 7 XP and 36 gold."})
    end
    if oldState == "DIALOGRESULT" then
        State("DONE")
    end
end

function EnemyDialogueStarting()
end

function EnemyDialogueEnding()
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