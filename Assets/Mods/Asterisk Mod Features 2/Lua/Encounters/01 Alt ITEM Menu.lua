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

enemies = {
"poseur_common"
}

enemypositions = {
{0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}
item_inventory = {"DogTest1", "DogTest2", "DogTest3", "DogTest4", "DogTest5", "DogTest6", "DogTest7", "DogTest8"}

function EncounterStarting()
    Player.name = string.upper("Nil256")

    Inventory.AddCustomItems({"Fake"}, {3})
    Inventory.SetInventory({"Fake"})
end

function EnteringState(newState, oldState)
    if newState == "ITEMMENU" then
        customstatename = "ITEMMENU"
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