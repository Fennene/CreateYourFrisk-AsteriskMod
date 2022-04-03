-- You should check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

-- We can't mute sounds when you spare enemies or you are gameover, unfortunately.
-- However, if you use CustomState, you can do that.

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
"poseur_10"
}

enemypositions = {
{0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}
registed_sounds = {"BeginBattle1", "BeginBattle2", "BeginBattle3", "dogsecret", "enemydust", "healsound", "heartbeatbreaker", "heartsplosion", "hitsound", "HotCat", "HotDog", "hurtsound", "LegHero", "levelup", "menuconfirm", "menumove", "runaway", "saved", "SeaTea", "ShopFail", "ShopSuccess", "slice", "success"}
--registed_voices = {"monsterfont", "tem1", "tem2", "tem3", "tem4", "tem5", "tem6", "uifont", "uifontold", "v_asriel", "v_flowey", "v_floweymad", "v_fluffybuns", "v_papyrus", "v_sans"}

function EncounterStarting()
    Player.name = string.upper("Nil256")

    Audio.Volume(0)
    for i = 1, #registed_sounds do
        Audio[registed_sounds[i]] = "silence"
    end

    ArenaUtil.SetDialogTextMute(true)
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