-- You should check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

encountertext = ""
enemies = { "poseur_common" }
enemypositions = { {0, 0} }

mask = CreateSprite("px", "Top")
mask.color = {0, 0, 0}
mask.Scale(640, 480)

function EncounterStarting()
    Audio.Stop()
    State("PAUSE")
end

local text1 = nil
local text2 = nil
local text3 = nil
local frame_counter = 0

function NewText(text, x, y, volume)
    local text = CreateText(text, {x, y}, 65536, "Top")
    text.HideBubble()
    text.progressmode = "none"
    text.SetSoundVolume(volume)
    if volume == 0 then text.SetSoundMute(true) end
    return text
end

function Update()
    frame_counter = frame_counter + 1
    if frame_counter == 1 then
        text1 = NewText("[font:uidialog][noskip]The sound's volume of this text is 1.", 10, 290, 1)
    end
    if frame_counter == 150 then
        text2 = NewText("[font:uidialog][noskip]The sound's volume of this text is 0.5.", 10, 240, 0.5)
    end
    if frame_counter == 300 then
        text3 = NewText("[font:uidialog][noskip]The sound of this text is muted.", 10, 190, 0)
    end
end
