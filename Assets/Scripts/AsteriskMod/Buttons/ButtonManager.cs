using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>
    /// Provides the method of modifying each buttons.<br/>
    /// Be attached to <c>Canvas/UIRect</c> in <c>Battle</c> Scene
    /// </summary>
    public class ButtonManager : MonoBehaviour
    {
        private Image fightButton, actButton, itemButton, mercyButton;
        private Sprite fightButtonSprite, actButtonSprite, itemButtonSprite, mercyButtonSprite;
        private CYFExtender.ImageObject fightButtonMan, actButtonMan, itemButtonMan, mercyButtonMan;

        private void Awake()
        {
            Transform fight = transform.Find("FightBt");
            Transform act   = transform.Find("FightBt");
            Transform item  = transform.Find("FightBt");
            Transform mercy = transform.Find("FightBt");
            fightButton = fight.GetComponent<Image>();
            actButton   = act.GetComponent<Image>();
            itemButton  = item.GetComponent<Image>();
            mercyButton = mercy.GetComponent<Image>();
            if (GlobalControls.crate)
            {
                fightButtonSprite = SpriteRegistry.Get("UI/Buttons/gifhtbt_1");
                actButtonSprite   = SpriteRegistry.Get("UI/Buttons/catbt_1");
                itemButtonSprite  = SpriteRegistry.Get("UI/Buttons/tembt_1");
                mercyButtonSprite = SpriteRegistry.Get("UI/Buttons/mecrybt_1");
            }
            else
            {
                fightButtonSprite = SpriteRegistry.Get("UI/Buttons/fightbt_1");
                actButtonSprite   = SpriteRegistry.Get("UI/Buttons/actbt_1");
                itemButtonSprite  = SpriteRegistry.Get("UI/Buttons/itembt_1");
                mercyButtonSprite = SpriteRegistry.Get("UI/Buttons/mercybt_1");
            }
            fightButtonMan = fight.GetComponent<CYFExtender.ImageObject>();
            actButtonMan   = act.GetComponent<CYFExtender.ImageObject>();
            itemButtonMan  = item.GetComponent<CYFExtender.ImageObject>();
            mercyButtonMan = mercy.GetComponent<CYFExtender.ImageObject>();
        }
    }
}
