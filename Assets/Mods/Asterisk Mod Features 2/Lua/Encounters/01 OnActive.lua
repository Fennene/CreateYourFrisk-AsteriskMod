-- You should check that the player use AsteriskMod.
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
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}
playerskipdocommand = true

enemies = {
    "Null", "poseur_01"
}

enemypositions = {
    {0, 0}, {0, 0}
}

CreateLayer("ScreenMask", "Top", true)
_mask = CreateSprite("px", "ScreenMask")
_mask.color = {0, 0, 0}
_mask.Scale(640, 480)

_player_position = {320, 240}
_player_direction = 0

function EncounterStarting()
    Audio.Stop()
    Player.name = "You"
    SetFrameBasedMovement(true)
    CreateLayer("RoomBottom", "ScreenMask", false)
    CreateLayer("RoomBackground", "RoomBottom", false)
    CreateLayer("Room", "RoomBackground", false)
    CreateLayer("RoomForeground", "Room", false)
    CreateLayer("RoomDialog", "RoomForeground", false)
    CreateLayer("RoomDialogText", "RoomDialog", false)
    CreateLayer("RoomEnc", "RoomDialogText", false)
    CreateLayer("RoomEncSoul", "RoomEnc", false)
    CreateLayer("RoomTop", "RoomEncSoul", false)
    customstatename = "TITLE"
    State("CUSTOMSTATE")
end

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

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