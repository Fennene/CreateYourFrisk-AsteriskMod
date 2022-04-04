local frame_counter = 0
local run_soul = nil
local run_soul_index = 0
local frame_counter_total = 60
local totalEXP = {
	0, 10, 30, 70, 120,
	200, 300, 500, 800, 1200,
	1700, 2500, 3500, 5000, 7000,
	10000, 15000, 25000, 50000, 99999
}
function CalculateNextLove(earnedEXP)
	local exp = Encounter["_player_exp"] + earnedEXP
	Encounter["_player_exp"] = exp
	local new_love = 0
	for lv = 1, 20 do
		if exp >= totalEXP[lv] then
			new_love = new_love + 1
		end
	end
	new_love = math.min(new_love, 20)
	return new_love
end
function StateStarting()
	Audio.Stop()
	Audio.PlaySound("runaway")
	StateEditor.SetPlayerVisible(false)
	StateEditor.SetButtonActive()
	local earned = Encounter.Call("_CheckEarned")
	local exp = earned[1]
	local gold = earned[2]
	local new_lv = CalculateNextLove(exp)
	local text = "[font:uidialog]Escaped..."
	if exp > 0 or gold > 0 then
		text = text .. "\n"
		if Player.lv < new_lv then
			text = text .. "You earned " .. exp .. " EXP and " .. gold .. " gold.\n[func:LoveUp," .. new_lv .. "]Your LOVE increased."
			frame_counter_total = 60 + ((string.len(text) - 42) * 3)
		else
			text = text .. "You earned " .. exp .. " EXP and " .. gold .. " gold."
			frame_counter_total = 60 + ((string.len(text) - 28) * 3)
		end
	end
	StateEditor.SetDialogText(text)
	run_soul = CreateSprite("spr_heartgtfo_0", "BelowBullet")
	run_soul.color = {1, 0, 0}
	run_soul.MoveToAbs(65, 160)
end

function Update()
	frame_counter = frame_counter + 1
	if frame_counter % 2 == 0 then
		run_soul.Move(-2, 0)
	end
	if frame_counter % 4 == 0 then
		run_soul_index = (run_soul_index + 1) % 2
		run_soul.Set("spr_heartgtfo_" .. run_soul_index)
	end
	if frame_counter == frame_counter_total then
		Encounter.Call("SetMaskFadeIn", 16)
	end
	if frame_counter == frame_counter_total + 16 then
		run_soul.Remove()
		Encounter.Call("PrepareRoom")
	end
end

function StateEnding()
end