local soul = nil
local soul_shards = {}
local frame_counter = 0
local gameover_spr = nil
local gameover_dialog = nil
local GAMEOVER_DIALOG_TEXTS = {
	"You cannot give\nup just yet...",
	"Our fate rests\nupon you...",
	"You cannot give\nup just yet...",
	"Our fate rests\nupon you...",
	"You cannot give\nup just yet...",
	"It cannot end\nnow!",
	"Don't lose hope!"
}
local frame_counter_end = -1
function StateStarting(oldState)
	Audio.Stop()
	soul = CreateSprite("ut-heart", "RoomDialog")
	soul.color = {1, 0, 0}
	soul.MoveToAbs(Player.absx, Player.absy)
end

function UpdateSoul()
	if frame_counter == 40 then
		Audio.PlaySound("heartbeatbreaker")
		soul.Set("ut-heart-broken")
	elseif frame_counter == 110 then
		Audio.PlaySound("heartsplosion")
		soul.alpha = 0
		for i = 1, 6 do
			local shard = CreateSprite("UI/Battle/heartshard_0", "RoomDialog")
			shard.MoveToAbs(soul.absx, soul.absy)
			shard.color = soul.color
			local rad = math.rad(math.random(360))
			local r = 2 + (math.random(-10, 10) / 80)
			shard["velocity"] = {r * math.cos(rad), r * math.sin(rad)}
			soul_shards[i] = shard
		end
	end
	if #soul_shards ~= 0 then
		for k = 1, #soul_shards do
			soul_shards[k].Move(soul_shards[k]["velocity"][1], soul_shards[k]["velocity"][2])
			soul_shards[k]["velocity"][2] = soul_shards[k]["velocity"][2] - 0.04
			if frame_counter % 6 == 1 then
				soul_shards[k].Set("UI/Battle/heartshard_"..(math.floor((frame_counter - 110) / 6) % 4))
			end
		end
	end
end

function UpdateGameOver()
	if frame_counter == 220 then
		local music = Encounter["deathmusic"]
		if music == nil then
			music = "mus_gameover"
		end
		Audio.LoadFile(music)
		gameover_spr = CreateSprite("UI/spr_gameoverbg_0", "Top")
		gameover_spr.MoveToAbs(320, 354)
		gameover_spr.alpha = 0
	elseif frame_counter > 220 and frame_counter <= 340 then
		gameover_spr.alpha = (frame_counter - 220) / 120
	end
end

function UpdateGameOverText()
	if frame_counter == 390 then
		local dialog_text = Encounter["deathtext"]
		if dialog_text == nil then
			dialog_text = {}
			dialog_text[1] = "[font:uidialog][voice:v_fluffybuns][waitall:2]" .. GAMEOVER_DIALOG_TEXTS[math.random(#GAMEOVER_DIALOG_TEXTS)]
			dialog_text[2] = "[font:uidialog][voice:v_fluffybuns][waitall:2]" .. Player.name .. "!\nStay determined..."
		elseif type(dialog_text) == "string" then
			dialog_text = {dialog_text}
		end
		for i = 1, #dialog_text do
			dialog_text[i] = "[noskip]" .. dialog_text[i]
		end
		gameover_dialog = CreateText(dialog_text, {140, 140}, 65536, "Top")
		gameover_dialog.HideBubble()
		gameover_dialog.progressmode = "manual"
	end
end

function UpdateFadeout()
	if frame_counter_end < 0 then
		return
	end
	frame_counter_end = frame_counter_end + 1
	if frame_counter_end < 120 then
		gameover_spr.alpha = 1 - (frame_counter_end / 120)
	elseif frame_counter_end == 150 then
		Audio.Stop()
		soul.Remove()
		for i = #soul_shards, 1, -1 do
			soul_shards[i].Remove()
			soul_shards[i] = nil
		end
		gameover_spr.Remove()
		Player.lv = 1
		Encounter["_player_hp"] = 20
		Encounter["_player_exp"] = 0
		Encounter.Call("PrepareRoom")
	end
end

function HandleAction()
	if frame_counter <= 390 or frame_counter_end >= 0 then
		return
	end
	if gameover_dialog == nil then
		frame_counter_end = 0
	elseif gameover_dialog.allLinesComplete then
		gameover_dialog = nil
	end
end

function Update()
	frame_counter = frame_counter + 1
	UpdateSoul()
	UpdateGameOver()
	UpdateGameOverText()
	UpdateFadeout()
end

function StateEnding(newState)
end
