Arena.ResizeImmediate(250, 130)
BlueSoul = require "FN!BlueSoulLib"
BlueSoul.SetBlue(true, true, true)
BlueSoul.SetControlOverride(true)
PrupleSoul = require "FN!PurpleSoulLib"
PrupleSoul.CreateNet(5, false)
PrupleSoul.SetPurple(true, false, true)
PrupleSoul.MoveToNet(3)
PrupleSoul.SetControlOverride(true)

soul_half = CreateSprite("ut-heart-half", "BelowBullet")
soul_half.MoveToAbs(Player.absx, Player.absy)
soul_half.color32 = {213, 53, 217}

function Update()
	BlueSoul.Update()
	PrupleSoul.Update()
	soul_half.MoveToAbs(Player.absx, Player.absy)
	soul_half.alpha = PlayerUtil.GetSoulAlpha()
end

function EndingWave()
	soul_half.Remove()
	PrupleSoul.RemoveNet()
	PrupleSoul.SetPurple(false, false, true)
	BlueSoul.SetBlue(false, true, true)
end
