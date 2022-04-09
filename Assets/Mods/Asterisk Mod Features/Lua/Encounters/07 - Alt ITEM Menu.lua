-- You need to check that the player use AsteriskMod.
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
    "poseur"
}

enemypositions = {
    {0, 0}
}

possible_attacks = {"bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou"}
inventory = {"DogTest1", "DogTest2", "DogTest3", "DogTest4", "DogTest5", "DogTest6", "DogTest7", "DogTest8"}

function EncounterStarting()
    Player.name = "Nil256"

    -- For opening ITEM Menu.
    Inventory.AddCustomItems({"Fake"}, {3})
    Inventory.SetInventory({"Fake"})
end

function EnteringState(newState, oldState)
    if newState == "ITEMMENU" then
        customstatename = "ITEMMENU" -- means Lua/States/ITEMMENU.lua
        State("CUSTOMSTATE") -- assign "CUSTOMSTATE"
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
    if #inventory == 0 then -- if inventory is empty,
        Inventory.SetInventory({}) -- disable ITEM Menu by setting empty inventory.
    end
    BattleDialog({"Selected item " .. ItemID .. "."})
end