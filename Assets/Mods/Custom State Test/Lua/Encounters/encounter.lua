encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}

enemies = {
"poseur"
}

enemypositions = {
{0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}
item_inventory = {"DogTest1", "DogTest2", "DogTest3", "DogTest4", "DogTest5", "DogTest6", "DogTest7", "DogTest8"}

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

function FirstFrameUpdate()
    PlayerUtil.HPUIMoveTo(-40, 0)
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
    if #item_inventory == 0 then
        Inventory.SetInventory({})
    end
    BattleDialog({"Selected item " .. ItemID .. "."})
end