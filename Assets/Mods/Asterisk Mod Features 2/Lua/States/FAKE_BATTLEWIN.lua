local wait_16_frame = false
local frame_counter = 0
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
	StateEditor.SetPlayerVisible(false)
	StateEditor.SetButtonActive()
	local earned = Encounter.Call("_CheckEarned")
	local exp = earned[1]
	local gold = earned[2]
	local new_lv = CalculateNextLove(exp)
	local text = "[font:uidialog]YOU WON!\nYou earned " .. exp .. " EXP and " .. gold .. " gold."
	if Player.lv < new_lv then
		text = text .. "\n[func:LoveUp," .. new_lv .. "]Your LOVE increased."
	end
	StateEditor.SetDialogText(text)
end

function LoveUp(lv)
	Audio.PlaySound("levelup")
	Player.lv = lv
    PlayerUtil.SetHPBarLength(Player.maxhp)
    PlayerUtil.SetHP(Encounter["_player_hp"], Player.maxhp, true)
end

function HandleAction()
	if not StateEditor.GetLineCompleteDialogText() then
		return
	end
	Encounter.Call("SetMaskFadeIn", 16)
	wait_16_frame = true
end

function Update()
	if not wait_16_frame then
		return
	end
	frame_counter = frame_counter + 1
	if frame_counter < 16 then
		return
	end
	Encounter.Call("PrepareRoom")
end

function StateEnding()
end