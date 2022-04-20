using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    /// <summary>PlayerのHP用とLua用の<see cref="LifeBarController"/></summary>
    public class PlayerLifeBar : MonoBehaviour
    {
        private Color fillColor;
        private Color backgroundColor;
        private Image fill;
        private Image background;

        private float currentFill = 1.0f;
        private float oldFill = 1.0f;
        private float desiredFill = 1.0f;
        private const float fillLinearTime = 1.0f; // 現在のバーの位置(長さ)が目的の位置(長さ)に移動するまでの時間
        private float fillTimer;
        private float totalwidth;
        private const bool player = false;
        private const bool whenDamage = false;
        private float whenDamageValue = 0.0f;

        private void Start()
        {
            totalwidth = fill.rectTransform.rect.width;
            background.color = backgroundColor;
            fill.color = fillColor;
            // プレハブ(Prefab)用 順番を正しく入れ替える。
            background.transform.SetAsLastSibling();
            fill.transform.SetAsLastSibling();
        }

        /// <summary>Fillバー(黄色と赤色なら黄色の方) の長さを代入した値に応じて 即座に 変える。</summary>
        /// <param name="fillvalue">0.0 ~ 1.0 の範囲。</param>
        public void SetInstant(float fillvalue)
        {
            currentFill = fillvalue;
            desiredFill = fillvalue;
            //fill.fillAmount = fillvalue;
            //fill.rectTransform.sizeDelta = new Vector2(1, fillvalue);
            //* if (player) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * 90, fill.rectTransform.offsetMin.y);
            //* else if (whenDamage) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * whenDamageValue, fill.rectTransform.offsetMin.y);
            //* else
            //* {
                if (fillvalue > 1)
                    fillvalue = 1;
                fill.rectTransform.offsetMax = new Vector2(-(Mathf.Min(PlayerCharacter.instance.MaxHP, 100) * (1 - fillvalue)) * 1.2f, fill.rectTransform.offsetMin.y);
            //* }
            //fill.rectTransform.offsetMax = new Vector2(-(1 - fillvalue) * PlayerCharacter.instance.MaxHP * 1.2f, fill.rectTransform.offsetMin.y);
        }

        /// <summary>現在のFillバーの位置(長さ)から 代入した位置(長さ)への linear-time(線形時間遷移(等速直線運動かと思われ)) を開始する。</summary>
        /// <param name="fillvalue">0.0 ~ 1.0 の範囲。</param>
        public void SetLerp(float fillvalue)
        {
            if (fillvalue > 1)
                fillvalue = 1;
            oldFill = currentFill;
            desiredFill = fillvalue;
            fillTimer = 0.0f;
        }

        /// <summary>第一引数の位置(長さ)から 第二引数の位置(長さ)への linear-time(線形時間遷移(等速直線運動かと思われ)) を開始する。</summary>
        /// <param name="originalValue">開始値, 0.0 ~ 1.0 の範囲。</param>
        /// <param name="fillValue">終了値, 0.0 ~ 1.0 の範囲。</param>
        public void SetLerp(float originalValue, float fillValue)
        {
            SetInstant(originalValue);
            SetLerp(fillValue);
        }

        /// <summary>Fillバー(デフォルトでは黄色)の色を変える。</summary>
        public void SetFillColor(Color c)
        {
            fillColor = c;
            fill.color = c;
        }

        /// <summary>Backgroundバー(デフォルトでは赤色)の色を変える。</summary>
        public void SetBackgroundColor(Color c)
        {
            backgroundColor = c;
            background.color = c;
        }

        /// <summary>表示/非表示の切り替え</summary>
        /// <param name="visible">true で表示, falseで非表示</param>
        public void SetVisible(bool visible)
        {
            foreach (Image img in GetComponentsInChildren<Image>())
                img.enabled = visible;
        }

        /// <summary>SetLerp用。</summary>
        private void Update()
        {
            if (currentFill == desiredFill || UIController.instance.frozenState != UIController.UIState.PAUSE) return;

            currentFill = Mathf.Lerp(oldFill, desiredFill, fillTimer / fillLinearTime);
            //fill.fillAmount = currentFill;
            //fill.rectTransform.sizeDelta = new Vector2(0, -(1-currentFill) * totalwidth);
            //* if (player) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * PlayerCharacter.instance.HP * 1.2f, fill.rectTransform.offsetMin.y);
            //* else if (whenDamage) fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * whenDamageValue, fill.rectTransform.offsetMin.y);
            /** else*/ fill.rectTransform.offsetMax = new Vector2(-(1 - currentFill) * totalwidth, fill.rectTransform.offsetMin.y);
            fillTimer += Time.deltaTime;
        }
    }
}
