using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools.UI
{
    public class ImageAlphaSlider : ExComponent
    {
        public float initValue;
        public Image target;
        public CYFInputField alphaValue;
        public Slider alphaSlider;
        public Text alphaMin;
        public Text alphaMax;
        public Toggle alpha32Toggle;
        private bool scriptChange;

#if UNITY_EDITOR
        private void ThrowDebugError(string message)
        {
            throw new Exception("GameObject:\"" + gameObject.name + "\" - ImageAlphaSliderコンポーネント: " + message);
        }

        private void Awake()
        {
            if (initValue < 0 || initValue > 1) ThrowDebugError("不正な初期値(value:" + initValue.ToString() + ")");
            //if (target == null) ThrowDebugError("ターゲットのImageが指定されていません。");
            if (alphaValue == null) ThrowDebugError("alpha値を表示するためのInputFieldが指定されていません。");
            if (alphaSlider == null) ThrowDebugError("Sliderが指定されていません。");
            if (alphaMin == null) ThrowDebugError("alpha値の最小値を表示するTextが指定されていません。");
            if (alphaMax == null) ThrowDebugError("alpha値の最大値を表示するTextが指定されていません。");
            if (alpha32Toggle == null) ThrowDebugError("alphaとalpha32を切り替えるためのToggleTextが指定されていません。");
        }
#endif

        private void Start()
        {
            alphaValue.InputField.text = initValue.ToString();
            alphaSlider.wholeNumbers = false;
            alphaSlider.minValue = 0;
            alphaSlider.maxValue = 1;
            alphaSlider.value = initValue;
            alphaMin.text = "0";
            alphaMax.text = "1";
            alpha32Toggle.isOn = false;
            AddListeners();
            if (target == null)
            {
                Disable();
            }
            else
            {
                ChangeAlpha(initValue);
            }
        }

        private void ChangeAlpha(float targetAlpha)
        {
            Color _ = target.color;
            _.a = targetAlpha;
            target.color = _;
        }

        private void ChangeAlpha32(byte targetAlpha)
        {
            Color32 _ = (Color32)target.color;
            _.a = targetAlpha;
            target.color = _;
        }

        private void AddListeners()
        {
            alphaSlider.onValueChanged.RemoveAllListeners();
            alphaSlider.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                if (!alphaSlider.wholeNumbers)
                {
                    ChangeAlpha(value);
                }
                else
                {
                    int _ = (int)value;
                    if (_ < 0) _ = 0;
                    if (_ > 255) _ = 255;
                    ChangeAlpha32((byte)_);
                }
                scriptChange = true;
                alphaValue.InputField.text = value.ToString();
                scriptChange = false;
            });

            alphaValue.InputField.onValueChanged.RemoveAllListeners();
            alphaValue.InputField.onValueChanged.AddListener((value) =>
            {
                if (scriptChange)
                {
                    alphaValue.ResetOuterColor();
                    return;
                }
                bool error = false;
                if (!alphaSlider.wholeNumbers)
                {
                    float _1;
                    error = !float.TryParse(value, out _1);
                }
                else
                {
                    byte _2;
                    error = !byte.TryParse(value, out _2);
                }
                if (error) alphaValue.OuterImage.color = new Color32(255, 64, 64, 255);
                else       alphaValue.ResetOuterColor();
            });

            alphaValue.InputField.onEndEdit.RemoveAllListeners();
            alphaValue.InputField.onEndEdit.AddListener((value) =>
            {
                if (scriptChange) return;
                bool success;
                if (!alphaSlider.wholeNumbers)
                {
                    float alpha;
                    success = float.TryParse(value, out alpha);
                    if (!success)
                    {
                        scriptChange = true;
                        alphaValue.InputField.text = alphaSlider.value.ToString();
                        scriptChange = false;
                        return;
                    }
                    scriptChange = true;
                    alphaSlider.value = alpha;
                    ChangeAlpha(alpha);
                    scriptChange = false;
                }
                else
                {
                    byte alpha32;
                    success = byte.TryParse(value, out alpha32);
                    if (!success)
                    {
                        scriptChange = true;
                        alphaValue.InputField.text = alphaSlider.value.ToString();
                        scriptChange = false;
                        return;
                    }
                    scriptChange = true;
                    alphaSlider.value = (float)alpha32;
                    ChangeAlpha32(alpha32);
                    scriptChange = false;
                }
            });

            alpha32Toggle.onValueChanged.RemoveAllListeners();
            alpha32Toggle.onValueChanged.AddListener((value) =>
            {
            });
        }
    }
}
