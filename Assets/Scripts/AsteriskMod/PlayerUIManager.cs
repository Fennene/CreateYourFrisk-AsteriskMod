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
            NameLVManager.GetComponent<TextManager>().SetText(new TextMessage(PlayerCharacter.instance.Name.ToUpper() + "  LV " + lvText, false, true));
        }

        public void SetNameLVColor(bool name, Color color)
        {
            StringInfo stringInfo = new StringInfo(PlayerCharacter.instance.Name);
            int length = stringInfo.LengthInTextElements;
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
            Transform HPLabel = HPManager.transform.Find("HPLabel");
            GameObject barManager = HPLabel.transform.Find("HPBar").gameObject;
            GameObject textMan = barManager.transform.Find("HPTextParent").gameObject;
            for (var i = 0; i < textMan.transform.childCount; i++)
            {
                textMan.transform.GetChild(i).GetComponent<MaskImage>().color = color;
                textMan.transform.GetChild(i).GetComponent<Letter>().colorFromText = color;
            }
        }
    }
}
