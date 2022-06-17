using AsteriskMod.ModdingHelperTools.UI;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal static class SPControllerUI
    {
        private static Image DisabledCover;
        private static bool scriptChange;

        internal static void Awake(Transform controllerView)
        {
            DisabledCover = controllerView.parent.Find("Cover").GetComponent<Image>();
            scriptChange = false;

            AwakeSprite(controllerView.Find("Sprite"));
            AwakePosition(controllerView.Find("Pos"));
            AwakeScale(controllerView.Find("Scale"));
            AwakeColor(controllerView.Find("Color"));
            AwakeAlpha(controllerView.Find("Alpha"));
            AwakeRotation(controllerView.Find("Rotation"));
            AwakeLayer(controllerView.Find("Layer"));
            AwakeCode(controllerView.Find("Code"));

            DisabledCover.enabled = true;
        }

        internal static void Start()
        {
            StartSprite();
            StartPosition();
            StartScale();
            StartColor();
            StartAlpha();
            StartRotation();
            StartLayer();
            StartCode();
        }

        internal static void UpdateParameters()
        {
            if (SPTargetDelUI.TargetIndex < 0
            || (!SPTargetDelUI.IsTargetBullet && SPTargetDelUI.TargetIndex >= SimSprProjSimMenu.SpriteLength)
            || (SPTargetDelUI.IsTargetBullet && SPTargetDelUI.TargetIndex >= SimSprProjSimMenu.BulletLength))
            {
                DisabledCover.enabled = true;
                return;
            }
            DisabledCover.enabled = false;
            scriptChange = true;
            UpdateSpriteParameter();
            UpdatePositionParameter();
            UpdateScaleParameter();
            UpdateColorParameter();
            UpdateAlphaParameter();
            UpdateRotationParameter();
            UpdateLayerParameter();
            UpdateCodeParameter();
            scriptChange = false;
        }

        // --------------------------------------------------------------------------------

        private static bool SpriteExists(string value)
        {
            FileInfo fi = new FileInfo(FakeFileLoader.pathToModFile("Sprites/" + value + ".png"));
            if (!fi.Exists) fi = new FileInfo(FakeFileLoader.pathToDefaultFile("Sprites/" + value + ".png"));
            return fi.Exists;
        }

        private static bool CanFloatParse(string value) { float _; return float.TryParse(value, out _); }

        private static bool CanByteParse(string value) { byte _; return byte.TryParse(value, out _); }

        // --------------------------------------------------------------------------------

        private static CYFInputField Sprite_Set;
        private static Text Sprite_width;
        private static Text Sprite_height;

        private static void AwakeSprite(Transform parent)
        {
            Sprite_Set = parent.Find("Set").GetComponent<CYFInputField>();
            Sprite_width  = parent.Find("width") .Find("Text").GetComponent<Text>();
            Sprite_height = parent.Find("height").Find("Text").GetComponent<Text>();
        }

        private static void StartSprite()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Sprite_Set, (value) =>
            {
                if (!scriptChange)
                {
                    CYFInputFieldUtil.ShowInputError(Sprite_Set, SpriteExists(value));
                    return;
                }

                Sprite_width.text  = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.width,(bullet)  => bullet.sprite.width)) .ToString();
                Sprite_height.text = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.height,(bullet) => bullet.sprite.height)).ToString();

                CYFInputFieldUtil.ShowInputError(Sprite_Set, true);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Sprite_Set, (value) =>
            {
                // if (scriptChange) return;

                if (!SpriteExists(value))
                {
                    scriptChange = true;
                    Sprite_Set.InputField.text = ((string)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.spritename, (bullet) => bullet.sprite.spritename)).ToString();
                    scriptChange = false;
                    return;
                }

                SimSprProjSimMenu.ActionToTarget((sprite) => sprite.Set(value), (bullet) => bullet.sprite.Set(value));

                Sprite_width.text  = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.width,(bullet)  => bullet.sprite.width)) .ToString();
                Sprite_height.text = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.height,(bullet) => bullet.sprite.height)).ToString();

                SPTargetDelUI.UpdateTargetDropDown();
            });
        }

        private static void UpdateSpriteParameter()
        {
            Sprite_Set.InputField.text = ((string)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.spritename, (bullet) => bullet.sprite.spritename)).ToString();
        }

        // --------------------------------------------------------------------------------

        private static Text Position_xLabel;
        private static CYFInputField Position_x;
        private static Text Position_yLabel;
        private static CYFInputField Position_y;
        private static Toggle Position_Abs;

        private static void AwakePosition(Transform parent)
        {
            Position_xLabel = parent.Find("xLabel").GetComponent<Text>();
            Position_x      = parent.Find("x")     .GetComponent<CYFInputField>();
            Position_yLabel = parent.Find("yLabel").GetComponent<Text>();
            Position_y      = parent.Find("y")     .GetComponent<CYFInputField>();
            Position_Abs    = parent.Find("Abs")   .GetComponent<Toggle>();
        }

        private static void StartPosition()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_x, (value) => CYFInputFieldUtil.ShowInputError(Position_x, CanFloatParse(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_y, (value) => CYFInputFieldUtil.ShowInputError(Position_y, CanFloatParse(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_x, (value) =>
            {
                if (scriptChange) return;
                if (!CanFloatParse(value))
                {
                    scriptChange = true;
                    Position_x.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                        (sprite) => { return Position_Abs.isOn ? sprite.absx : sprite.x; },
                        (bullet) => { return Position_Abs.isOn ? bullet.absx : bullet.x; }
                    )).ToString();
                    scriptChange = false;
                    return;
                }
                SimSprProjSimMenu.ActionToTarget(
                    (sprite) =>
                    {
                        if (Position_Abs.isOn) sprite.absx = ParseUtil.GetFloat(value);
                        else                   sprite.x =    ParseUtil.GetFloat(value);
                    },
                    (bullet) =>
                    {
                        if (Position_Abs.isOn) bullet.absx = ParseUtil.GetFloat(value);
                        else                   bullet.x =    ParseUtil.GetFloat(value);
                    }
                );
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_y, (value) =>
            {
                if (scriptChange) return;
                if (!CanFloatParse(value))
                {
                    scriptChange = true;
                    Position_y.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                        (sprite) => { return Position_Abs.isOn ? sprite.absy : sprite.y; },
                        (bullet) => { return Position_Abs.isOn ? bullet.absy : bullet.y; }
                    )).ToString();
                    scriptChange = false;
                    return;
                }
                SimSprProjSimMenu.ActionToTarget(
                    (sprite) =>
                    {
                        if (Position_Abs.isOn) sprite.absy = ParseUtil.GetFloat(value);
                        else                   sprite.y =    ParseUtil.GetFloat(value);
                    },
                    (bullet) =>
                    {
                        if (Position_Abs.isOn) bullet.absy = ParseUtil.GetFloat(value);
                        else                   bullet.y =    ParseUtil.GetFloat(value);
                    }
                );
            });
            UnityToggleUtil.AddListener(Position_Abs, (value) =>
            {
                scriptChange = true;
                Position_xLabel.text = value ? "absx:" : "x:";
                Position_yLabel.text = value ? "absy:" : "y:";
                Position_x.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return value ? sprite.absx : sprite.x; },
                    (bullet) => { return value ? bullet.absx : bullet.x; }
                )).ToString();
                Position_y.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return value ? sprite.absy : sprite.y; },
                    (bullet) => { return value ? bullet.absy : bullet.y; }
                )).ToString();
                scriptChange = false;
            });
        }

        private static void UpdatePositionParameter()
        {
            Position_x.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                (sprite) => { return Position_Abs.isOn ? sprite.absx : sprite.x; },
                (bullet) => { return Position_Abs.isOn ? bullet.absx : bullet.x; }
                )).ToString();
            Position_y.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                (sprite) => { return Position_Abs.isOn ? sprite.absy : sprite.y; },
                (bullet) => { return Position_Abs.isOn ? bullet.absy : bullet.y; }
                )).ToString();
        }

        // --------------------------------------------------------------------------------

        private static CYFInputField Scale_xscale;
        private static CYFInputField Scale_yscale;

        private static void AwakeScale(Transform parent)
        {
            Scale_xscale  = parent.Find("xscale").GetComponent<CYFInputField>();
            Scale_yscale  = parent.Find("yscale").GetComponent<CYFInputField>();
        }

        private static void StartScale()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Scale_xscale, (value) => CYFInputFieldUtil.ShowInputError(Scale_xscale, CanFloatParse(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Scale_yscale, (value) => CYFInputFieldUtil.ShowInputError(Scale_yscale, CanFloatParse(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Scale_xscale, (value) =>
            {
                if (scriptChange) return;
                if (!CanFloatParse(value))
                {
                    scriptChange = true;
                    Scale_xscale.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.xscale, (bullet) => bullet.sprite.xscale)).ToString();
                    scriptChange = false;
                    return;
                }
                SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.xscale = ParseUtil.GetFloat(value); }, (bullet) => { bullet.sprite.xscale = ParseUtil.GetFloat(value); });
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Scale_yscale, (value) =>
            {
                if (scriptChange) return;
                if (!CanFloatParse(value))
                {
                    scriptChange = true;
                    Scale_yscale.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.yscale, (bullet) => bullet.sprite.yscale)).ToString();
                    scriptChange = false;
                    return;
                }
                SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.yscale = ParseUtil.GetFloat(value); }, (bullet) => { bullet.sprite.yscale = ParseUtil.GetFloat(value); });
            });
        }

        private static void UpdateScaleParameter()
        {
            Scale_xscale.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.xscale, (bullet) => bullet.sprite.xscale)).ToString();
            Scale_yscale.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.yscale, (bullet) => bullet.sprite.yscale)).ToString();
        }

        // --------------------------------------------------------------------------------

        private static CYFInputField Color_rValue;
        private static Slider Color_r;
        private static Text Color_rMaxLabel;
        private static CYFInputField Color_gValue;
        private static Slider Color_g;
        private static Text Color_gMaxLabel;
        private static CYFInputField Color_bValue;
        private static Slider Color_b;
        private static Text Color_bMaxLabel;
        private static Toggle Color_32Toggle;

        private static void AwakeColor(Transform parent)
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

        private static void StartColor()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Color_rValue, (value) => CYFInputFieldUtil.ShowInputError(Color_rValue, !Color_32Toggle.isOn ? CanFloatParse(value) : CanByteParse(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Color_gValue, (value) => CYFInputFieldUtil.ShowInputError(Color_gValue, !Color_32Toggle.isOn ? CanFloatParse(value) : CanByteParse(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Color_bValue, (value) => CYFInputFieldUtil.ShowInputError(Color_bValue, !Color_32Toggle.isOn ? CanFloatParse(value) : CanByteParse(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Color_rValue, (value) =>
            {
                if (scriptChange) return;
                if ((!Color_32Toggle.isOn && !CanFloatParse(value)) || (Color_32Toggle.isOn && !CanByteParse(value)))
                {
                    scriptChange = true;
                    if (!Color_32Toggle.isOn)
                    {
                        float r = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color[0], (bullet) => bullet.sprite.color[0]);
                        Color_rValue.InputField.text = r.ToString();
                        Color_r.value = r;
                    }
                    else
                    {
                        byte r32 = (byte)((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color32[0], (bullet) => bullet.sprite.color32[0]));
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
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color = sprite.color;
                            sprite.color = new float[3] { r, color[1], color[2] };
                        },
                        (bullet) =>
                        {
                            float[] color = bullet.sprite.color;
                            bullet.sprite.color = new float[3] { r, color[1], color[2] };
                        }
                    );
                    scriptChange = true;
                    Color_rValue.InputField.text = r.ToString(); // adjust out of range
                    Color_r.value = r;
                    scriptChange = false;
                }
                else
                {
                    byte r32 = byte.Parse(value);
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color32 = sprite.color32;
                            sprite.color32 = new float[3] { r32, color32[1], color32[2] };
                        },
                        (bullet) =>
                        {
                            float[] color32 = bullet.sprite.color32;
                            bullet.sprite.color32 = new float[3] { r32, color32[1], color32[2] };
                        }
                    );
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
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color = sprite.color;
                            sprite.color = new float[3] { value, color[1], color[2] };
                        },
                        (bullet) =>
                        {
                            float[] color = bullet.sprite.color;
                            bullet.sprite.color = new float[3] { value, color[1], color[2] };
                        }
                    );
                    scriptChange = true;
                    Color_rValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color32 = sprite.color32;
                            sprite.color32 = new float[3] { value, color32[1], color32[2] };
                        },
                        (bullet) =>
                        {
                            float[] color32 = bullet.sprite.color32;
                            bullet.sprite.color32 = new float[3] { value, color32[1], color32[2] };
                        }
                    );
                    scriptChange = true;
                    Color_rValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Color_gValue, (value) =>
            {
                if (scriptChange) return;
                if ((!Color_32Toggle.isOn && !CanFloatParse(value)) || (Color_32Toggle.isOn && !CanByteParse(value)))
                {
                    scriptChange = true;
                    if (!Color_32Toggle.isOn)
                    {
                        float g = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color[1], (bullet) => bullet.sprite.color[1]);
                        Color_gValue.InputField.text = g.ToString();
                        Color_g.value = g;
                    }
                    else
                    {
                        byte g32 = (byte)((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color32[1], (bullet) => bullet.sprite.color32[1]));
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
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color = sprite.color;
                            sprite.color = new float[3] { color[0], g, color[2] };
                        },
                        (bullet) =>
                        {
                            float[] color = bullet.sprite.color;
                            bullet.sprite.color = new float[3] { color[0], g, color[2] };
                        }
                    );
                    scriptChange = true;
                    Color_gValue.InputField.text = g.ToString();
                    Color_g.value = g;
                    scriptChange = false;
                }
                else
                {
                    byte g32 = byte.Parse(value);
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color32 = sprite.color32;
                            sprite.color32 = new float[3] { color32[0], g32, color32[2] };
                        },
                        (bullet) =>
                        {
                            float[] color32 = bullet.sprite.color32;
                            bullet.sprite.color32 = new float[3] { color32[0], g32, color32[2] };
                        }
                    );
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
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color = sprite.color;
                            sprite.color = new float[3] { color[0], value, color[2] };
                        },
                        (bullet) =>
                        {
                            float[] color = bullet.sprite.color;
                            bullet.sprite.color = new float[3] { color[0], value, color[2] };
                        }
                    );
                    scriptChange = true;
                    Color_gValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color32 = sprite.color32;
                            sprite.color32 = new float[3] { color32[0], value, color32[2] };
                        },
                        (bullet) =>
                        {
                            float[] color32 = bullet.sprite.color32;
                            bullet.sprite.color32 = new float[3] { color32[0], value, color32[2] };
                        }
                    );
                    scriptChange = true;
                    Color_gValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Color_bValue, (value) =>
            {
                if (scriptChange) return;
                if ((!Color_32Toggle.isOn && !CanFloatParse(value)) || (Color_32Toggle.isOn && !CanByteParse(value)))
                {
                    scriptChange = true;
                    if (!Color_32Toggle.isOn)
                    {
                        float b = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color[2], (bullet) => bullet.sprite.color[2]);
                        Color_bValue.InputField.text = b.ToString();
                        Color_b.value = b;
                    }
                    else
                    {
                        byte b32 = (byte)((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color32[2], (bullet) => bullet.sprite.color32[2]));
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
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color = sprite.color;
                            sprite.color = new float[3] { color[0], color[1], b };
                        },
                        (bullet) =>
                        {
                            float[] color = bullet.sprite.color;
                            bullet.sprite.color = new float[3] { color[0], color[1], b };
                        }
                    );
                    scriptChange = true;
                    Color_bValue.InputField.text = b.ToString();
                    Color_b.value = b;
                    scriptChange = false;
                }
                else
                {
                    byte b32 = byte.Parse(value);
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color32 = sprite.color32;
                            sprite.color32 = new float[3] { color32[0], color32[1], b32 };
                        },
                        (bullet) =>
                        {
                            float[] color32 = bullet.sprite.color32;
                            bullet.sprite.color32 = new float[3] { color32[0], color32[1], b32 };
                        }
                    );
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
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color = sprite.color;
                            sprite.color = new float[3] { color[0], color[1], value };
                        },
                        (bullet) =>
                        {
                            float[] color = bullet.sprite.color;
                            bullet.sprite.color = new float[3] { color[0], color[1], value };
                        }
                    );
                    scriptChange = true;
                    Color_bValue.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    SimSprProjSimMenu.ActionToTarget(
                        (sprite) =>
                        {
                            float[] color32 = sprite.color32;
                            sprite.color32 = new float[3] { color32[0], color32[1], value };
                        },
                        (bullet) =>
                        {
                            float[] color32 = bullet.sprite.color32;
                            bullet.sprite.color32 = new float[3] { color32[0], color32[1], value };
                        }
                    );
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

        private static void UpdateColorParameter()
        {
            float[] color;
            if (!Color_32Toggle.isOn)
            {
                color = (float[])SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color, (bullet) => bullet.sprite.color);
            }
            else
            {
                color = (float[])SimSprProjSimMenu.GetFromTarget((sprite) => sprite.color32, (bullet) => bullet.sprite.color32);
            }
            Color_rValue.InputField.text = color[0].ToString();
            Color_gValue.InputField.text = color[1].ToString();
            Color_bValue.InputField.text = color[2].ToString();
            Color_r.value = color[0];
            Color_g.value = color[1];
            Color_b.value = color[2];
        }

        // --------------------------------------------------------------------------------

        private static CYFInputField Alpha_value;
        private static Slider Alpha_slider;
        private static Text Alpha_maxLabel;
        private static Toggle Alpha_32Toggle;

        private static void AwakeAlpha(Transform parent)
        {
            Alpha_value    = parent.Find("aValue") .GetComponent<CYFInputField>();
            Alpha_slider   = parent.Find("a")      .GetComponent<Slider>();
            Alpha_32Toggle = parent.Find("alpha32").GetComponent<Toggle>();
            Alpha_maxLabel = Alpha_slider.transform.Find("max").GetComponent<Text>();
        }

        private static void StartAlpha()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Alpha_value, (value) => CYFInputFieldUtil.ShowInputError(Alpha_value, !Alpha_32Toggle.isOn ? CanFloatParse(value) : CanByteParse(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Alpha_value, (value) =>
            {
                if (scriptChange) return;
                if ((!Alpha_32Toggle.isOn && !CanFloatParse(value)) || (Alpha_32Toggle.isOn && !CanByteParse(value)))
                {
                    scriptChange = true;
                    if (!Alpha_32Toggle.isOn)
                    {
                        float a = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.alpha, (bullet) => bullet.sprite.alpha);
                        Alpha_value.InputField.text = a.ToString();
                        Alpha_slider.value = a;
                    }
                    else
                    {
                        byte a32 = (byte)((float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.alpha, (bullet) => bullet.sprite.alpha));
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
                    SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.alpha = a; }, (bullet) => { bullet.sprite.alpha = a; });
                    scriptChange = true;
                    Alpha_value.InputField.text = a.ToString(); // adjust out of range
                    Alpha_slider.value = a;
                    scriptChange = false;
                }
                else
                {
                    byte a32 = byte.Parse(value);
                    scriptChange = true;
                    SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.alpha32 = a32; }, (bullet) => { bullet.sprite.alpha32 = a32; });
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
                    SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.alpha = value; }, (bullet) => { bullet.sprite.alpha = value; });
                    scriptChange = true;
                    Alpha_value.InputField.text = value.ToString();
                    scriptChange = false;
                }
                else
                {
                    SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.alpha32 = value; }, (bullet) => { bullet.sprite.alpha32 = value; });
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

        private static void UpdateAlphaParameter()
        {
            float alpha;
            if (!Alpha_32Toggle.isOn)
            {
                alpha = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.alpha, (bullet) => bullet.sprite.alpha);
            }
            else
            {
                alpha = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.alpha32, (bullet) => bullet.sprite.alpha32);
            }
            Alpha_value.InputField.text = alpha.ToString();
            Alpha_slider.value = alpha;
        }

        // --------------------------------------------------------------------------------

        private static CYFInputField Rotation_value;
        private static Slider Rotation_slider;

        private static void AwakeRotation(Transform parent)
        {
            Rotation_value  = parent.Find("rotationValue").GetComponent<CYFInputField>();
            Rotation_slider = parent.Find("rotation")     .GetComponent<Slider>();
        }

        private static void StartRotation()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Rotation_value, (value) => CYFInputFieldUtil.ShowInputError(Rotation_value, CanFloatParse(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Rotation_value, (value) =>
            {
                if (scriptChange) return;
                if (!CanFloatParse(value))
                {
                    float rotation = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.rotation, (bullet) => bullet.sprite.rotation);
                    scriptChange = true;
                    Rotation_value.InputField.text = rotation.ToString();
                    Rotation_slider.value = (int)rotation;
                    scriptChange = false;
                    return;
                }
                float rotate = ParseUtil.GetFloat(value);
                if (rotate < 0) rotate = 0;
                if (rotate > 360) rotate = 360;
                SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.rotation = rotate; }, (bullet) => { bullet.sprite.rotation = rotate; });
                scriptChange = true;
                Rotation_value.InputField.text = rotate.ToString();
                Rotation_slider.value = (int)rotate;
                scriptChange = false;
            });
            Rotation_slider.onValueChanged.RemoveAllListeners();
            Rotation_slider.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.rotation = value; }, (bullet) => { bullet.sprite.rotation = value; });
                scriptChange = true;
                Rotation_value.InputField.text = value.ToString();
                scriptChange = false;
            });
        }

        private static void UpdateRotationParameter()
        {
            float rotation = (float)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.rotation, (bullet) => bullet.sprite.rotation);
            Rotation_value.InputField.text = rotation.ToString();
            Rotation_slider.value = (int)rotation;
        }

        // --------------------------------------------------------------------------------

        private static Dropdown Layer_layer;

        private static readonly string[] LAYERS = new[] { "Bottom", "BelowUI", "BelowArena", "BelowPlayer", "BelowBullet", "Top" };
        private static int ConvertLayerName(string layerName)
        {
            if (layerName == "") return !SPTargetDelUI.IsTargetBullet ? 2 : 5;
            if (layerName == "Top") return !SPTargetDelUI.IsTargetBullet ? 5 : 6;
            for (var i = 0; i < 5; i++)
            {
                if (layerName == LAYERS[i]) return i;
            }
            return !SPTargetDelUI.IsTargetBullet ? 2 : 5;
        }
        private static string ConvertToLayerName(int layerDropDownValue)
        {
            if (layerDropDownValue < 0) return !SPTargetDelUI.IsTargetBullet ? LAYERS[2] : "";
            if (layerDropDownValue > (!SPTargetDelUI.IsTargetBullet ? 5 : 6)) return !SPTargetDelUI.IsTargetBullet ? LAYERS[2] : "";
            if (layerDropDownValue < 5) return LAYERS[layerDropDownValue];
            if (layerDropDownValue == 5) return !SPTargetDelUI.IsTargetBullet ? LAYERS[5] : "";
            return !SPTargetDelUI.IsTargetBullet ? LAYERS[2] : "";
        }

        private static void AwakeLayer(Transform parent)
        {
            Layer_layer = parent.Find("layer").GetComponent<Dropdown>();
        }

        private static void StartLayer()
        {
            Layer_layer.onValueChanged.RemoveAllListeners();
            Layer_layer.onValueChanged.AddListener((value) =>
            {
                if (scriptChange) return;
                SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.layer = ConvertToLayerName(value); }, (bullet) => { bullet.layer = ConvertToLayerName(value); });
            });
        }

        private static void UpdateLayerParameter()
        {
            Layer_layer.options = new System.Collections.Generic.List<Dropdown.OptionData>();
            for (var i = 0; i < LAYERS.Length; i++)
            {
                Layer_layer.options.Add(new Dropdown.OptionData { text = LAYERS[i] });
                if (i == 4 && SPTargetDelUI.IsTargetBullet) Layer_layer.options.Add(new Dropdown.OptionData { text = "BulletPool" });
            }
            Layer_layer.RefreshShownValue();
            Layer_layer.value = ConvertLayerName((string)SimSprProjSimMenu.GetFromTarget((sprite) => sprite.layer, (bullet) => bullet.layer));
        }

        // --------------------------------------------------------------------------------

        private static CYFInputField Code_Script;
        private static Toggle Code_Update;
        private static Toggle Code_Really;
        private static Button Code_Run;
        private static Image Code_RunImage;

        private static void AwakeCode(Transform parent)
        {
            Code_Script   = parent.Find("Script").GetComponent<CYFInputField>();
            Code_Update   = parent.Find("Update").GetComponent<Toggle>();
            Code_Really   = parent.Find("Really").GetComponent<Toggle>();
            Code_Run      = parent.Find("Run")   .GetComponent<Button>();
            Code_RunImage = parent.Find("Run")   .GetComponent<Image>();
        }

        private static void StartCode()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Code_Script, (_) => Code_Script.ResetOuterColor());
            UnityToggleUtil.AddListener(Code_Really, (value) => UnityButtonUtil.SetActive(Code_Run, Code_RunImage, value));
            UnityButtonUtil.AddListener(Code_Run, () =>
            {
                DevelopHint.ToDo();
            });
        }

        private static void UpdateCodeParameter()
        {
            Code_Update.isOn = false;
            Code_Script.InputField.text = !SPTargetDelUI.IsTargetBullet ? "sprite.SendToTop()" : "bullet.sprite.SendToTop()";
            Code_Script.ResetOuterColor();
            Code_Really.isOn = false;
        }
    }
}
