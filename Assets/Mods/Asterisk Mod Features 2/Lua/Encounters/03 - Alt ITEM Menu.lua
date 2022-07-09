-- You need to check that the player use AsteriskMod.
if Asterisk == nil then
    error("This mod can be launched on only CYF-AsteriskMod."
       .. "\n[このModはCYF-AsteriskModでのみ起動できます。]"
       .. "\n\nAsteriskMod -> https://github.com/Fennene/CreateYourFrisk-AsteriskMod"
    )
end

--[[

-- CustomState --

OK. Finally, I'll show you to the power of CustomState.

We'll make Japanese-styled ITEM menu.

In Japan, ITEM menu is different of English version.

< English >                          < Japanese >     △
＊ Pie         ＊ Steak              ＊ Pie            □
＊ SnowPiece   ＊ L. Hero            ＊ Steak          □
                   PAGE 1            ＊ SnowPiece     ▽

Don't worry if you don't know Japanese-styled ITEM menu
'cause we have the codes.

At the First, let's check out defined variables, EncounterStarting(), and EnteringState().

]]

encountertext = "Poseur strikes a pose!"
nextwaves = {"bullettest_chaserorb"}
wavetimer = 4.0
arenasize = {155, 130}
enemies = {"poseur"}
enemypositions = {{0, 0}}

possible_attacks = {
    "bullettest_bouncy", "bullettest_chaserorb", "bullettest_touhou",
    "bullettest_randomcircle", "bullettest_cyanorange",
    "bluesoultest_smalljump", "bluesoultest_hittoplatform", "bluesoultest_movetoplatform"
}

-- This is fake inventory used for ITEMMENU.
inventory = {"DogTest1", "DogTest2", "DogTest3", "DogTest4", "DogTest5", "DogTest6", "DogTest7", "DogTest8"}

function EncounterStarting()
    Player.name = "Nil256"

    -- For opening ITEM Menu.
    Inventory.AddCustomItems({"Fake"}, {3})
    Inventory.SetInventory({"Fake"})
end

function EnteringState(newState, oldState)
    if newState == "ITEMMENU" then
        -- If the argument is "ITEMMENU", the state go normal ITEMMENU,
        --    but this means that the state go ITEMMENU of Lua/States/ITEMMENU.lua.
        -- CustomState is depended on file names of Lua/States, so you can name the files using existed states of CYF.
        State("*ITEMMENU")
        -- customstatename = "ITEMMENU"
        -- State("CUSTOMSTATE")
        -- 
        -- There is nothing special here.
        -- We should check out Lua/States/ITEMMENU.lua.
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

function BeforeDeath()
    Player.sprite.color = {1, 0, 0}
end