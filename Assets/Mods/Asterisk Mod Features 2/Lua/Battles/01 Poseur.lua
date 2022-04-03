local BattleData = {}

BattleData.Audio = "mus_battle1"
BattleData.TargetEnemy = 2

function BattleData.EncounterStarting()
	State("ACTIONSELECT")
end

function BattleData.EnemyDialogueStarting()
end

function BattleData.EnemyDialogueEnding()
    nextwaves = { possible_attacks[math.random(#possible_attacks)] }
end

function BattleData.DefenseEnding()
    encountertext = RandomEncounterText()
end

return BattleData