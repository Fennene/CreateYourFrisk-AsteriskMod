using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    // Be attached in Assets/Battle.Unity/Canvas/Stats
    public class PlayerUIManager : MonoBehaviour
    {
        private GameObject NameLVManager;
        private GameObject HPManager;
        private Vector2 position;
        private Vector2 namelvPosition;
        private Vector2 hpPosition;
        private Vector2 hpTextPositon;

        public bool hpbarControlOverride;
        internal int forceChangeHP;
        internal int forceChangeMaxHP;
        internal string forceChangeHPString;

        private byte requests = 0;
        private const byte REQUEST_SET_HPTEXT_COLOR = 16;
        private Color request_set_hptext_color_value;
        private const byte REQUEST_SET_LV = 8;
        private string request_set_lv_value;
        private const byte REQUEST_SET_NAME_COLOR = 4;
        private Color request_set_name_color_value;
        private const byte REQUEST_SET_LV_COLOR = 2;
        private Color request_set_lv_color_value;
        private const byte REQUEST_SET_NAMELV_COLOR = 1;
        private Color request_set_namelv_color_value;
        private int[] request_set_namelv_color_value2 = new int[2] { 0, 0 };

        public static PlayerUIManager Instance;

        private void Start()
        {
            NameLVManager = transform.Find("NameLv").gameObject;
            HPManager = transform.Find("HPRect").gameObject;
            position = Vector2.zero;
            namelvPosition = Vector2.zero;
            hpPosition = Vector2.zero;
            hpTextPositon = Vector2.zero;
            hpbarControlOverride = false;
            forceChangeHP = -1;
            forceChangeMaxHP = -1;
            forceChangeHPString = "";
            Instance = this;
        }

        public void SetPosition(float x, float y)
        {
            Vector2 oldPosition = transform.GetComponent<RectTransform>().anchoredPosition;
            Vector2 originPosition = new Vector2(oldPosition.x - position.x, oldPosition.y - position.y);
            position = new Vector2(x, y);
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(originPosition.x + x, originPosition.y + y);
        }

        public void MovePosition(float x, float y)
        {
            SetPosition(position.x + x, position.y + y);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetNameLVPosition(float x, float y)
        {
            Vector2 oldPosition = NameLVManager.GetComponent<RectTransform>().anchoredPosition;
            Vector2 originPosition = new Vector2(oldPosition.x - namelvPosition.x, oldPosition.y - namelvPosition.y);
            namelvPosition = new Vector2(x, y);
            NameLVManager.GetComponent<RectTransform>().anchoredPosition = new Vector2(originPosition.x + x, originPosition.y + y);
        }

        public void MoveNameLVPosition(float x, float y)
        {
            SetNameLVPosition(namelvPosition.x + x, namelvPosition.y + y);
        }

        public Vector2 GetNameLVPosition()
        {
            return namelvPosition;
        }

        public void SetLVText(string lvText)
        {
            if (NameLVManager.GetComponent<TextManager>() == null)
            {
                requests += REQUEST_SET_LV;
                request_set_lv_value = lvText;
                return;
            }
            NameLVManager.GetComponent<TextManager>().SetText(new TextMessage(PlayerCharacter.instance.Name.ToUpper() + "  LV " + lvText, false, true));
        }

        public void SetNameLVColor(bool name, Color color)
        {
            if (NameLVManager.transform.childCount == 0)
            {
                if (name)
                {
                    requests += REQUEST_SET_NAME_COLOR;
                    request_set_name_color_value = color;
                }
                else
                {
                    requests += REQUEST_SET_LV_COLOR;
                    request_set_lv_color_value = color;
                }
                return;
            }
            StringInfo stringInfo = new StringInfo(PlayerCharacter.instance.Name);
            int length = stringInfo.LengthInTextElements;
            //length = PlayerCharacter.instance.Name.Length;
            //Debug.Log("isNameColor: " + name);
            //Debug.Log("NameLength: " + length);
            //Debug.Log("Children: " + NameLVManager.transform.childCount);
            for (var i = 0; i < NameLVManager.transform.childCount; i++)
            {
                //Debug.Log("Loop: " + i);
                if ((name && i > length) || (!name && i <= length))
                {
                    continue;
                }
                //Debug.Log("LoopOK: " + i);
                NameLVManager.transform.GetChild(i).GetComponent<MaskImage>().color = color;
                NameLVManager.transform.GetChild(i).GetComponent<Letter>().colorFromText = color;
            }
        }

        public void SetNameLVColorManually(int start, int end, Color color)
        {
            if (NameLVManager.transform.childCount == 0)
            {
                requests += REQUEST_SET_NAMELV_COLOR;
                request_set_namelv_color_value = color;
                request_set_namelv_color_value2 = new int[2] { start, end };
                return;
            }
            for (var i = start - 1; i <= end - 1; i++)
            {
                NameLVManager.transform.GetChild(i).GetComponent<MaskImage>().color = color;
            }
        }

        public void SetHPPosition(float x, float y)
        {
            Vector2 oldPosition = HPManager.GetComponent<RectTransform>().anchoredPosition;
            Vector2 originPosition = new Vector2(oldPosition.x - hpPosition.x, oldPosition.y - hpPosition.y);
            hpPosition = new Vector2(x, y);
            HPManager.GetComponent<RectTransform>().anchoredPosition = new Vector2(originPosition.x + x, originPosition.y + y);
        }

        public void MoveHPPosition(float x, float y)
        {
            SetHPPosition(hpPosition.x + x, hpPosition.y + y);
        }

        public Vector2 GetHPPosition()
        {
            return hpPosition;
        }

        public void SetHPLabelColor(Color color)
        {
            GameObject HPLabel = HPManager.transform.Find("HPLabel").gameObject;
            HPLabel.GetComponent<Image>().color = color;
        }

        public void SetHPBarColor(bool background, Color color)
        {
            Transform HPLabel = HPManager.transform.Find("HPLabel");
            GameObject barManager = HPLabel.transform.Find("HPBar").gameObject;
            if (background)
            {
                barManager.GetComponent<LifeBarController>().setBackgroundColor(color);
            }
            else
            {
                barManager.GetComponent<LifeBarController>().setFillColor(color);
            }
            GameObject bar = barManager.transform.Find(background ? "HPBarBG" : "HPBarFill").gameObject;
            bar.GetComponent<Image>().color = color;
        }

        public void SetHPTextPosition(float x, float y)
        {
            Transform HPLabel = HPManager.transform.Find("HPLabel");
            GameObject barManager = HPLabel.transform.Find("HPBar").gameObject;
            GameObject textMan = barManager.transform.Find("HPTextParent").gameObject;
            Vector2 oldPosition = textMan.GetComponent<RectTransform>().anchoredPosition;
            Vector2 originPosition = new Vector2(oldPosition.x - hpTextPositon.x, oldPosition.y - hpTextPositon.y);
            hpTextPositon = new Vector2(x, y);
            textMan.GetComponent<RectTransform>().anchoredPosition = new Vector2(originPosition.x + x, originPosition.y + y);
        }

        public void MoveHPTextPosition(float x, float y)
        {
            SetHPTextPosition(hpTextPositon.x + x, hpTextPositon.y + y);
        }

        public Vector2 GetHPTextPosition()
        {
            return hpTextPositon;
        }

        public void SetHPText(string hp)
        {
            Transform HPLabel = HPManager.transform.Find("HPLabel");
            GameObject barManager = HPLabel.transform.Find("HPBar").gameObject;
            GameObject textMan = barManager.transform.Find("HPTextParent").gameObject;
            textMan.GetComponent<TextManager>().SetText(new TextMessage(hp, false, true));
        }

        public void SetHPTextColor(Color color)
        {
            if (!UIStats.instance.canModify)
            {
                requests += REQUEST_SET_HPTEXT_COLOR;
                request_set_hptext_color_value = color;
                return;
            }
            Transform HPLabel = HPManager.transform.Find("HPLabel");
            GameObject barManager = HPLabel.transform.Find("HPBar").gameObject;
            GameObject textMan = barManager.transform.Find("HPTextParent").gameObject;
            for (var i = 0; i < textMan.transform.childCount; i++)
            {
                textMan.transform.GetChild(i).GetComponent<MaskImage>().color = color;
                textMan.transform.GetChild(i).GetComponent<Letter>().colorFromText = color;
            }
        }

        /// <summary>
        /// Call methods called before initialize objects again.<br />
        /// 初期化前に呼ばれた関数を再度呼び出します。
        /// </summary>
        public void Request()
        {
            if (requests == 0) return;
            if (((double)requests / 16.0) >= 1.0)
            {
                SetHPTextColor(request_set_hptext_color_value);
                requests -= REQUEST_SET_HPTEXT_COLOR;
            }
            if (((double)requests / 8.0) >= 1.0)
            {
                NameLVManager.GetComponent<TextManager>().SetText(new TextMessage(PlayerCharacter.instance.Name.ToUpper() + "  LV " + request_set_lv_value, false, true));
                requests -= REQUEST_SET_LV;
            }
            if (((double)requests / 4.0) >= 1.0)
            {
                SetNameLVColor(true, request_set_name_color_value);
                requests -= REQUEST_SET_NAME_COLOR;
            }
            if (((double)requests / 2.0) >= 1.0)
            {
                SetNameLVColor(false, request_set_lv_color_value);
                requests -= REQUEST_SET_LV_COLOR;
            }
            if (requests % 2 == 1)
            {
                SetNameLVColorManually(request_set_namelv_color_value2[0], request_set_namelv_color_value2[1], request_set_namelv_color_value);
                requests -= REQUEST_SET_NAMELV_COLOR;
            }
        }
    }
}
