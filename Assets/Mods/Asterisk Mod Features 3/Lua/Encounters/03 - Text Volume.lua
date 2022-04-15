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
enemies = {"poseur"}
enemypositions = {{0, 0}}
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
function Update()
    frame_counter = frame_counter + 1
    if frame_counter == 1 then
        text1 = CreateText("[font:uidialog][noskip]The sound's volume of this text is 1.", {10, 290}, 1023, "Top")
        text1.HideBubble()
        text1.progressmode = "none"
        text1.SetSoundVolume(1)
    end
    if frame_counter == 150 then
        text2 = CreateText("[font:uidialog][noskip]The sound's volume of this text is 0.5.", {10, 240}, 1023, "Top")
        text2.HideBubble()
        text2.progressmode = "none"
        text2.SetSoundVolume(0.5)
    end
    if frame_counter == 300 then
        text3 = CreateText("[font:uidialog][noskip]The sound of this text is muted.", {10, 190}, 1023, "Top")
        text3.HideBubble()
        text3.progressmode = "none"
        text3.SetSoundMute(true)
    end
end