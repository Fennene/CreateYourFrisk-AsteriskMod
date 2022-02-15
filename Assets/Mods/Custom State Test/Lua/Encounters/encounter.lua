-- A basic encounter script skeleton you can copy and modify for your own creations.

-- music = "shine_on_you_crazy_diamond" --Either OGG or WAV. Extension is added automatically. Uncomment for custom music.
encountertext = "Poseur strikes a pose!" --Modify as necessary. It will only be read out in the action select screen.
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
"poseur"
}

enemypositions = {
{0, 0}
}

-- A custom list with attacks to choose from. Actual selection happens in EnemyDialogueEnding(). Put here in case you want to use it.
possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}

function EncounterStarting()
    if isModifiedCYF == nil then
        error("This game can not be launched on official CYF.\nUse to lanuch CYF-AsteriskMod.")
    end
    if Asterisk == nil then
        error("This game can be launched on only CYF-AsteriskMod.")
    end
    if not AsteriskExperiment then
        error("This game requires to enable Experimental Feature.\nYou should enable Experimental Feature in AsteriskMod's option.")
    end

    Player.name = "Fennene"

    Inventory.AddCustomItems({"Fake"}, {3})
    Inventory.SetInventory({"Fake"})
end

function EnteringState(newState, oldState)
    if newState == "ITEMMENU" then
        customstatename = "JPITEMMENU"
        State("CUSTOMSTATE")
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