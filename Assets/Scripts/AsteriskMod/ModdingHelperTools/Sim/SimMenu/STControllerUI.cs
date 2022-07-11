using AsteriskMod.ModdingHelperTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class STControllerUI
    {
        private Image DisabledCover;
        private bool scriptChange;

        internal void Awake(Transform controllerView)
        {
            DisabledCover = controllerView.parent.Find("Cover").GetComponent<Image>();
            scriptChange = false;

            AwakeText(controllerView.Find("Text"));
            AwakePosition(controllerView.Find("Pos"));
            AwakeLayer(controllerView.Find("Layer"));
            AwakeColor(controllerView.Find("Color"));
            AwakeAlpha(controllerView.Find("Alpha"));
            AwakeScale(controllerView.Find("Scale"));

            DisabledCover.enabled = true;
        }

        internal void Start()
        {
            StartText();
            StartPosition();
            StartLayer();
            StartColor();
            StartAlpha();
            StartScale();
        }

        internal void UpdateParameters()
        {
            if (SimInstance.STTargetUI.TargetIndex < 0 || SimInstance.STTargetUI.TargetIndex >= SimStaticTextSimMenu.Instance.StaticTextLength)
            {
                DisabledCover.enabled = true;
                return;
            }
            DisabledCover.enabled = false;
            scriptChange = true;

            UpdateTextParameter();
            UpdatePositionParameter();
            UpdateLayerParameter();
            UpdateColorParameter();
            UpdateAlphaParameter();
            UpdateScaleParameter();

            scriptChange = false;
        }

        // --------------------------------------------------------------------------------

        private Dictionary<string, bool> checkedFont = new Dictionary<string, bool>();

        private bool FontExists(string value)
        {
            if (checkedFont.ContainsKey(value)) return checkedFont[value];
            bool exist = SimInstance.FakeSpriteFontRegistry.Get(value) != null;
            checkedFont.Add(value, exist);
            return exist;
        }

        private bool CanNullableFloatParse(string value)
        {
            if (value == "nil" || value == "Nil") return true;
            float _;
            return float.TryParse(value, out _);
        }
        private float? NullableFloatParse(string value)
        {
            if (value == "nil" || value == "Nil") return null;
            return float.Parse(value);
        }

        // --------------------------------------------------------------------------------

        private CYFInputField Text_Text;
        private CYFInputField Text_Font;
        private CYFInputField Text_CharSpacing;
        private CYFInputField Text_LineSpacing;
        private Text Text_width;
        private Text Text_height;

        private void AwakeText(Transform parent)
        {
            Text_Text        = parent.Find("SetText")    .GetComponent<CYFInputField>();
            Text_Font        = parent.Find("Font")       .GetComponent<CYFInputField>();
            Text_CharSpacing = parent.Find("CharSpacing").GetComponent<CYFInputField>();
            Text_LineSpacing = parent.Find("LineSpacing").GetComponent<CYFInputField>();
            Text_width =  parent.Find("width") .Find("Text").GetComponent<Text>();
            Text_height = parent.Find("height").Find("Text").GetComponent<Text>();
        }

        private void StartText()
        {
            CYFInputFieldUtil.AddListener_OnEndEdit(Text_Text, (value) =>
            {
                if (!scriptChange) SimStaticTextSimMenu.Instance.Target.SetText(value);

                Text_width.text  = SimStaticTextSimMenu.Instance.Target.GetTextWidth() .ToString();
                Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();

                SimInstance.STTargetUI.UpdateTargetDropDown();
            });
            CYFInputFieldUtil.AddListener_OnValueChanged(Text_Font, (value) =>
            {
                if (!scriptChange)
                {
                    CYFInputFieldUtil.ShowInputError(Text_Font, FontExists(value));
                    return;
                }

                Text_width.text = SimStaticTextSimMenu.Instance.Target.GetTextWidth().ToString();
                Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();

                CYFInputFieldUtil.ShowInputError(Text_Font, true);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Text_Font, (value) =>
            {
                //if (scriptChange) return;

                if (!FontExists(value))
                {
                    scriptChange = true;
                    Text_Font.InputField.text = SimStaticTextSimMenu.Instance.Target.font;
                    scriptChange = false;
                    return;
                }
                SimStaticTextSimMenu.Instance.Target.SetFont(value);

                Text_width.text = SimStaticTextSimMenu.Instance.Target.GetTextWidth().ToString();
                Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();

                CYFInputFieldUtil.ShowInputError(Text_Font, true);
            });
            CYFInputFieldUtil.AddListener_OnValueChanged(Text_CharSpacing, (value) =>
            {
                if (!scriptChange)
                {
                    CYFInputFieldUtil.ShowInputError(Text_CharSpacing, CanNullableFloatParse(value));
                    return;
                }

                Text_width.text = SimStaticTextSimMenu.Instance.Target.GetTextWidth().ToString();
                Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();

                CYFInputFieldUtil.ShowInputError(Text_CharSpacing, true);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Text_CharSpacing, (value) =>
            {
                //if (scriptChange) return;

                if (!CanNullableFloatParse(value))
                {
                    scriptChange = true;
                    Text_CharSpacing.InputField.text = SimStaticTextSimMenu.Instance.Target.hSpacing.ToString();
                    scriptChange = false;
                    return;
                }
                SimStaticTextSimMenu.Instance.Target.SetCharSpacing(NullableFloatParse(value));

                Text_width.text = SimStaticTextSimMenu.Instance.Target.GetTextWidth().ToString();
                Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();

                CYFInputFieldUtil.ShowInputError(Text_CharSpacing, true);
            });
            CYFInputFieldUtil.AddListener_OnValueChanged(Text_LineSpacing, (value) =>
            {
                if (!scriptChange)
                {
                    CYFInputFieldUtil.ShowInputError(Text_LineSpacing, ParseUtil.CanParseFloat(value));
                    return;
                }

                Text_width.text = SimStaticTextSimMenu.Instance.Target.GetTextWidth().ToString();
                Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();

                CYFInputFieldUtil.ShowInputError(Text_LineSpacing, true);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Text_LineSpacing, (value) =>
            {
                //if (scriptChange) return;

                if (!CanNullableFloatParse(value))
                {
                    scriptChange = true;
                    Text_LineSpacing.InputField.text = SimStaticTextSimMenu.Instance.Target.vSpacing.ToString();
                    scriptChange = false;
                    return;
                }
                SimStaticTextSimMenu.Instance.Target.SetLineSpacing(ParseUtil.GetFloat(value));

                Text_width.text = SimStaticTextSimMenu.Instance.Target.GetTextWidth().ToString();
                Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();

                CYFInputFieldUtil.ShowInputError(Text_LineSpacing, true);
            });
        }

        private void UpdateTextParameter()
        {
            Text_Text.InputField.text = SimStaticTextSimMenu.Instance.Target.text;
            Text_Font.InputField.text = SimStaticTextSimMenu.Instance.Target.font;
            Text_CharSpacing.InputField.text = SimStaticTextSimMenu.Instance.Target.hSpacing.ToString();
            Text_LineSpacing.InputField.text = SimStaticTextSimMenu.Instance.Target.vSpacing.ToString();
            Text_width.text = SimStaticTextSimMenu.Instance.Target.GetTextWidth().ToString();
            Text_height.text = SimStaticTextSimMenu.Instance.Target.GetTextHeight().ToString();
        }

        // --------------------------------------------------------------------------------

        private Text Position_xLabel;
        private CYFInputField Position_x;
        private Text Position_yLabel;
        private CYFInputField Position_y;
        private Toggle Position_Abs;

        private void AwakePosition(Transform parent)
        {
            Position_xLabel = parent.Find("xLabel").GetComponent<Text>();
            Position_x      = parent.Find("x")     .GetComponent<CYFInputField>();
            Position_yLabel = parent.Find("yLabel").GetComponent<Text>();
            Position_y      = parent.Find("y")     .GetComponent<CYFInputField>();
            Position_Abs    = parent.Find("Abs")   .GetComponent<Toggle>();
        }

        private void StartPosition()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_x, (value) => CYFInputFieldUtil.ShowInputError(Position_x, ParseUtil.CanParseInt(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_y, (value) => CYFInputFieldUtil.ShowInputError(Position_y, ParseUtil.CanParseInt(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_x, (value) =>
            {
                if (scriptChange) return;
                if (!ParseUtil.CanParseInt(value))
                {
                    scriptChange = true;
                    Position_x.InputField.text = (Position_Abs.isOn ? SimStaticTextSimMenu.Instance.Target.absx : SimStaticTextSimMenu.Instance.Target.x).ToString();
                    scriptChange = false;
                    return;
                }
                if (Position_Abs.isOn) SimStaticTextSimMenu.Instance.Target.absx = ParseUtil.GetInt(value);
                else                   SimStaticTextSimMenu.Instance.Target.   x = ParseUtil.GetInt(value);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_y, (value) =>
            {
                if (scriptChange) return;
                if (!ParseUtil.CanParseInt(value))
                {
                    scriptChange = true;
                    Position_y.InputField.text = (Position_Abs.isOn ? SimStaticTextSimMenu.Instance.Target.absy : SimStaticTextSimMenu.Instance.Target.y).ToString();
                    scriptChange = false;
                    return;
                }
                if (Position_Abs.isOn) SimStaticTextSimMenu.Instance.Target.absy = ParseUtil.GetInt(value);
                else                   SimStaticTextSimMenu.Instance.Target.   y = ParseUtil.GetInt(value);
            });
            UnityToggleUtil.AddListener(Position_Abs, (value) =>
            {
                scriptChange = true;
                Position_xLabel.text = value ? "absx:" : "x:";
                Position_yLabel.text = value ? "absy:" : "y:";
                Position_x.InputField.text = (value ? SimStaticTextSimMenu.Instance.Target.absx : SimStaticTextSimMenu.Instance.Target.x).ToString();
                Position_y.InputField.text = (value ? SimStaticTextSimMenu.Instance.Target.absy : SimStaticTextSimMenu.Instance.Target.y).ToString();
                scriptChange = false;
            });
        }

        private void UpdatePositionParameter()
        {
            Position_x.InputField.text = (Position_Abs.isOn ? SimStaticTextSimMenu.Instance.Target.absx : SimStaticTextSimMenu.Instance.Target.x).ToString();
            Position_y.InputField.text = (Position_Abs.isOn ? SimStaticTextSimMenu.Instance.Target.absy : SimStaticTextSimMenu.Instance.Target.y).ToString();
        }

        // --------------------------------------------------------------------------------

        private Dropdown Layer_layer;

        private readonly string[] LAYERS = new[] { "Bottom", "BelowUI", "BelowArena", "BelowPlayer", "BelowBullet", "Top" };
        private int ConvertLayerName(string layerName)
        {
            if (layerName == "") return 2;
            for (var i = 0; i < 5; i++)
            {
                if (layerName == LAYERS[i]) return i;
            }
            return 2;
        }
        private string ConvertToLayerName(int layerDropDownValue)
        {
            if (layerDropDownValue < 0) return LAYERS[2];
            if (layerDropDownValue <= 5) return LAYERS[layerDropDownValue];
            return LAYERS[2];
        }

        private void AwakeLayer(Transform parent)
        {
            Layer_layer = parent.Find("layer").GetComponent<Dropdown>();
        }

        private void StartLayer()
        {
            Layer_layer.onValueChanged.RemoveAllListeners();
            Layer_layer.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                SimStaticTextSimMenu.Instance.Target.layer = ConvertToLayerName(value);
            });
        }

        private void UpdateLayerParameter()
        {
            Layer_layer.value = ConvertLayerName(SimStaticTextSimMenu.Instance.Target.layer);
        }

        // --------------------------------------------------------------------------------

        private CYFInputField Color_rValue;
        private Slider Color_r;
        private Text Color_rMaxLabel;
        private CYFInputField Color_gValue;
        private Slider Color_g;
        private Text Color_gMaxLabel;
        private CYFInputField Color_bValue;
        private Slider Color_b;
        private Text Color_bMaxLabel;
        private Toggle Color_32Toggle;

        private void AwakeColor(Transform parent)
        {
            Color_rValue = parent.Find("rValue").GetComponent<CYFInputField>();
            Color_r      = parent.Find("r")     .GetComponent<Slider>();
            Color_gValue = parent.Find("gValue").GetComponent<CYFInputField>();
            Color_g      = parent.Find("g")     .GetComponent<Slider>();
            Color_bValue = parent.Find("bValue").GetComponent<CYFInputField>();
            Color_b      = parent.Find("b")     .GetComponent<Slider>();
            Color_rMaxLabel = Color_r.transform.Find("max").GetComponent<Text>();
            Color_gMaxLabel = Color_g.transform.Find("max").GetComponent<Text>();
            Color_bMaxLabel = Color_b.transform.Find("max").GetComponent<Text>();
            Color_32Toggle = parent.Find("color32").GetComponent<Toggle>();
        }

        private void StartColor()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Color_rValue, (value) => CYFInputFieldUtil.ShowInputError(Color_rValue, !Color_32Toggle.isOn ? ParseUtil.CanParseFloat(value) : ParseUtil.CanParseByteAccurately(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Color_gValue, (value) => CYFInputFieldUtil.ShowInputError(Color_gValue, !Color_32Toggle.isOn ? ParseUtil.CanParseFloat(value) : ParseUtil.CanParseByteAccurately(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Color_bValue, (value) => CYFInputFieldUtil.ShowInputError(Color_bValue, !Color_32Toggle.isOn ? ParseUtil.CanParseFloat(value) : ParseUtil.CanParseByteAccurately(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Color_rValue, (value) =>
            {
                if (scriptChange) return;
                if ((!Color_32Toggle.isOn && !ParseUtil.CanParseFloat(value)) || (Color_32Toggle.isOn && !ParseUtil.CanParseByteAccurately(value)))
                {
                    scriptChange = true;
                    if (!Color_32Toggle.isOn)
                    {
                        float r = SimStaticTextSimMenu.Instance.Target.color[0];
                        Color_rValue.InputField.text = r.ToString();
                        Color_r.value = r;
                    }
                    else
                    {
                        byte r32 = (byte)SimStaticTextSimMenu.Instance.Target.color32[0];
                        Color_rValue.InputField.text = r32.ToString();
                        Color_r.value = r32;
                    }
                    scriptChange = false;
                    return;
                }
                if (!Color_32Toggle.isOn)
                {
                    float r = ParseUtil.GetFloat(value);
                    if (r < 0) r = 0;
                    if (r > 1) r = 1;
                    float[] color = SimStaticTextSimMenu.Instance.Target.color;
                    SimStaticTextSimMenu.Instance.Target.color = new float[3] { r, color[1], color[2] };
                    scriptChange = true;
                    Color_rValue.InputField.text = r.ToString(); // adjust out of range
                    Color_r.value = r;
                    scriptChange = false;
                }
                else
                {
                    byte r32 = byte.Parse(value);
                    float[] color32 = SimStaticTextSimMenu.Instance.Target.color32;
                    SimStaticTextSimMenu.Instance.Target.color32 = new float[3] { r32, color32[1], color32[2] };
                    scriptChange = true;
                    Color_r.value = r32;
                    scriptChange = false;
                }
            });
            Color_r.onValueChanged.RemoveAllListeners();
            Color_r.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                if (!Color_32Toggle.isOn)
                {
                    float[] color = SimStaticTextSimMenu.Instance.Target.color;
                    SimStaticTextSimMenu.Instance.Target.color = new float[3] { value, color[1], color[2] };
                    scriptChange = true;
                    Color_rValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    float[] color32 = SimStaticTextSimMenu.Instance.Target.color32;
                    SimStaticTextSimMenu.Instance.Target.color32 = new float[3] { value, color32[1], color32[2] };
                    scriptChange = true;
                    Color_rValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Color_gValue, (value) =>
            {
                if (scriptChange) return;
                if ((!Color_32Toggle.isOn && !ParseUtil.CanParseFloat(value)) || (Color_32Toggle.isOn && !ParseUtil.CanParseByteAccurately(value)))
                {
                    scriptChange = true;
                    if (!Color_32Toggle.isOn)
                    {
                        float g = SimStaticTextSimMenu.Instance.Target.color[1];
                        Color_gValue.InputField.text = g.ToString();
                        Color_g.value = g;
                    }
                    else
                    {
                        byte g32 = (byte)SimStaticTextSimMenu.Instance.Target.color32[1];
                        Color_gValue.InputField.text = g32.ToString();
                        Color_g.value = g32;
                    }
                    scriptChange = false;
                    return;
                }
                if (!Color_32Toggle.isOn)
                {
                    float g = ParseUtil.GetFloat(value);
                    if (g < 0) g = 0;
                    if (g > 1) g = 1;
                    float[] color = SimStaticTextSimMenu.Instance.Target.color;
                    SimStaticTextSimMenu.Instance.Target.color = new float[3] { color[0], g, color[2] };
                    scriptChange = true;
                    Color_gValue.InputField.text = g.ToString();
                    Color_g.value = g;
                    scriptChange = false;
                }
                else
                {
                    byte g32 = byte.Parse(value);
                    float[] color32 = SimStaticTextSimMenu.Instance.Target.color32;
                    SimStaticTextSimMenu.Instance.Target.color32 = new float[3] { color32[0], g32, color32[2] };
                    scriptChange = true;
                    Color_g.value = g32;
                    scriptChange = false;
                }
            });
            Color_g.onValueChanged.RemoveAllListeners();
            Color_g.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                if (!Color_32Toggle.isOn)
                {
                    float[] color = SimStaticTextSimMenu.Instance.Target.color;
                    SimStaticTextSimMenu.Instance.Target.color = new float[3] { color[0], value, color[2] };
                    scriptChange = true;
                    Color_gValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    float[] color32 = SimStaticTextSimMenu.Instance.Target.color32;
                    SimStaticTextSimMenu.Instance.Target.color32 = new float[3] { color32[0], value, color32[2] };
                    scriptChange = true;
                    Color_gValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Color_bValue, (value) =>
            {
                if (scriptChange) return;
                if ((!Color_32Toggle.isOn && !ParseUtil.CanParseFloat(value)) || (Color_32Toggle.isOn && !ParseUtil.CanParseByteAccurately(value)))
                {
                    scriptChange = true;
                    if (!Color_32Toggle.isOn)
                    {
                        float b = SimStaticTextSimMenu.Instance.Target.color[2];
                        Color_bValue.InputField.text = b.ToString();
                        Color_b.value = b;
                    }
                    else
                    {
                        byte b32 = (byte)SimStaticTextSimMenu.Instance.Target.color32[2];
                        Color_bValue.InputField.text = b32.ToString();
                        Color_b.value = b32;
                    }
                    scriptChange = false;
                    return;
                }
                if (!Color_32Toggle.isOn)
                {
                    float b = ParseUtil.GetFloat(value);
                    if (b < 0) b = 0;
                    if (b > 1) b = 1;
                    float[] color = SimStaticTextSimMenu.Instance.Target.color;
                    SimStaticTextSimMenu.Instance.Target.color = new float[3] { color[0], color[1], b };
                    scriptChange = true;
                    Color_bValue.InputField.text = b.ToString();
                    Color_b.value = b;
                    scriptChange = false;
                }
                else
                {
                    byte b32 = byte.Parse(value);
                    float[] color32 = SimStaticTextSimMenu.Instance.Target.color32;
                    SimStaticTextSimMenu.Instance.Target.color32 = new float[3] { color32[0], color32[1], b32 };
                    scriptChange = true;
                    Color_b.value = b32;
                    scriptChange = false;
                }
            });
            Color_b.onValueChanged.RemoveAllListeners();
            Color_b.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                if (!Color_32Toggle.isOn)
                {
                    float[] color = SimStaticTextSimMenu.Instance.Target.color;
                    SimStaticTextSimMenu.Instance.Target.color = new float[3] { color[0], color[1], value };
                    scriptChange = true;
                    Color_bValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    float[] color32 = SimStaticTextSimMenu.Instance.Target.color32;
                    SimStaticTextSimMenu.Instance.Target.color32 = new float[3] { color32[0], color32[1], value };
                    scriptChange = true;
                    Color_bValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
            });
            UnityToggleUtil.AddListener(Color_32Toggle, (value) =>
            {
                // if (scriptChange) return;
                scriptChange = true;
                Color_rMaxLabel.text = value ? "255" : "1";
                Color_gMaxLabel.text = value ? "255" : "1";
                Color_bMaxLabel.text = value ? "255" : "1";
                Color_r.wholeNumbers = value;
                Color_g.wholeNumbers = value;
                Color_b.wholeNumbers = value;
                Color_r.maxValue = value ? 255 : 1;
                Color_g.maxValue = value ? 255 : 1;
                Color_b.maxValue = value ? 255 : 1;
                UpdateColorParameter();
                scriptChange = false;
            });
        }

        private void UpdateColorParameter()
        {
            float[] color = !Color_32Toggle.isOn ? SimStaticTextSimMenu.Instance.Target.color : SimStaticTextSimMenu.Instance.Target.color32;
            Color_rValue.InputField.text = color[0].ToString();
            Color_gValue.InputField.text = color[1].ToString();
            Color_bValue.InputField.text = color[2].ToString();
            Color_r.value = color[0];
            Color_g.value = color[1];
            Color_b.value = color[2];
        }

        // --------------------------------------------------------------------------------

        private CYFInputField Alpha_value;
        private Slider Alpha_slider;
        private Text Alpha_maxLabel;
        private Toggle Alpha_32Toggle;

        private void AwakeAlpha(Transform parent)
        {
            Alpha_value    = parent.Find("aValue") .GetComponent<CYFInputField>();
            Alpha_slider   = parent.Find("a")      .GetComponent<Slider>();
            Alpha_32Toggle = parent.Find("alpha32").GetComponent<Toggle>();
            Alpha_maxLabel = Alpha_slider.transform.Find("max").GetComponent<Text>();
        }

        private void StartAlpha()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Alpha_value, (value) => CYFInputFieldUtil.ShowInputError(Alpha_value, !Alpha_32Toggle.isOn ? ParseUtil.CanParseFloat(value) : ParseUtil.CanParseByteAccurately(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Alpha_value, (value) =>
            {
                if (scriptChange) return;
                if ((!Alpha_32Toggle.isOn && !ParseUtil.CanParseFloat(value)) || (Alpha_32Toggle.isOn && !ParseUtil.CanParseByteAccurately(value)))
                {
                    scriptChange = true;
                    if (!Alpha_32Toggle.isOn)
                    {
                        float a = SimStaticTextSimMenu.Instance.Target.alpha;
                        Alpha_value.InputField.text = a.ToString();
                        Alpha_slider.value = a;
                    }
                    else
                    {
                        byte a32 = (byte)SimStaticTextSimMenu.Instance.Target.alpha32;
                        Alpha_value.InputField.text = a32.ToString();
                        Alpha_slider.value = a32;
                    }
                    scriptChange = false;
                    return;
                }
                if (!Alpha_32Toggle.isOn)
                {
                    float a = ParseUtil.GetFloat(value);
                    if (a < 0) a = 0;
                    if (a > 1) a = 1;
                    SimStaticTextSimMenu.Instance.Target.alpha = a;
                    scriptChange = true;
                    Alpha_value.InputField.text = a.ToString(); // adjust out of range
                    Alpha_slider.value = a;
                    scriptChange = false;
                }
                else
                {
                    byte a32 = byte.Parse(value);
                    scriptChange = true;
                    SimStaticTextSimMenu.Instance.Target.alpha32 = a32;
                    Alpha_slider.value = a32;
                    scriptChange = false;
                }
            });
            Alpha_slider.onValueChanged.RemoveAllListeners();
            Alpha_slider.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                if (!Alpha_32Toggle.isOn)
                {
                    SimStaticTextSimMenu.Instance.Target.alpha = value;
                    scriptChange = true;
                    Alpha_value.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    SimStaticTextSimMenu.Instance.Target.alpha32 = value;
                    scriptChange = true;
                    Alpha_value.InputField.text = value.ToString();
                    scriptChange = false;
                }
            });
            UnityToggleUtil.AddListener(Alpha_32Toggle, (value) =>
            {
                scriptChange = true;
                Alpha_maxLabel.text = value ? "255" : "1";
                Alpha_slider.wholeNumbers = value;
                Alpha_slider.maxValue = value ? 255 : 1;
                UpdateAlphaParameter();
                scriptChange = false;
            });
        }

        private void UpdateAlphaParameter()
        {
            float alpha = !Alpha_32Toggle.isOn ? SimStaticTextSimMenu.Instance.Target.alpha : SimStaticTextSimMenu.Instance.Target.alpha32;
            Alpha_value.InputField.text = alpha.ToString();
            Alpha_slider.value = alpha;
        }

        // --------------------------------------------------------------------------------

        private CYFInputField Scale_xscale;
        private CYFInputField Scale_yscale;

        private void AwakeScale(Transform parent)
        {
            Scale_xscale = parent.Find("xscale").GetComponent<CYFInputField>();
            Scale_yscale = parent.Find("yscale").GetComponent<CYFInputField>();
        }

        private void StartScale()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Scale_xscale, (value) => CYFInputFieldUtil.ShowInputError(Scale_xscale, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Scale_yscale, (value) => CYFInputFieldUtil.ShowInputError(Scale_yscale, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Scale_xscale, (value) =>
            {
                if (scriptChange) return;
                if (!ParseUtil.CanParseFloat(value))
                {
                    scriptChange = true;
                    Scale_xscale.InputField.text = SimStaticTextSimMenu.Instance.Target.xscale.ToString();
                    scriptChange = false;
                    return;
                }
                SimStaticTextSimMenu.Instance.Target.xscale = ParseUtil.GetFloat(value);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Scale_yscale, (value) =>
            {
                if (scriptChange) return;
                if (!ParseUtil.CanParseFloat(value))
                {
                    scriptChange = true;
                    Scale_yscale.InputField.text = SimStaticTextSimMenu.Instance.Target.yscale.ToString();
                    scriptChange = false;
                    return;
                }
                SimStaticTextSimMenu.Instance.Target.yscale = ParseUtil.GetFloat(value);
            });
        }

        private void UpdateScaleParameter()
        {
            Scale_xscale.InputField.text = SimStaticTextSimMenu.Instance.Target.xscale.ToString();
            Scale_yscale.InputField.text = SimStaticTextSimMenu.Instance.Target.yscale.ToString();
        }
    }
}
