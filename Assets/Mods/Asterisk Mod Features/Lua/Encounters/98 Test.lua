-- You should check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
"poseur_common"
}

enemypositions = {
{0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

function EncounterStarting()
    Player.name = string.upper("Nil256")

    Audio.Stop()
    State("PAUSE")
end

function NewText(text, x, y, layer)
    local t = CreateText(text, {x, y}, 65536, layer)
    t.HideBubble()
    t.progressmode = "none"
    return t
end

local text1 = nil
local text2 = nil

function Update()
    if textCreated == nil then
        text1 = NewText("[font:uidialog][noskip]This text's volume of sound is 1.", 50, 180, "Top")
        textCreated = true
    end
    if text1.lineComplete and text2 == nil then
        text2 = NewText("[font:uidialog][noskip]This text's volume of sound is 0.5.", 50, 130, "Top")
        text2.SetSoundVolume(0.5)
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