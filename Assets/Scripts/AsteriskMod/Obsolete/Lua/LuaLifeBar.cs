using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.Lua
{
    public class LuaLifeBar : MonoBehaviour
    {
        private LifeBarController _barController;
        private RectTransform _lifebarRt;
        private float _value;
        private float _length;

        private void Awake()
        {
            _barController = GetComponent<LifeBarController>();
            _lifebarRt = _barController.GetComponent<RectTransform>();
            _value = 20;
            _length = 20;
        }

        public float value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        public float length
        {
            get { return _length; }
            set { SetLength(value); }
        }

        public void SetValue(float value)
        {
            float hpMax = _length,
                  hpFrac = value / hpMax;
            _barController.setInstantOverride(hpFrac, (int)_length);
        }

        public void SetLength(float length, bool ignoreLimit = false)
        {
            float len = length * 1.2f;
            if (!ignoreLimit) len = Mathf.Min(120, len);
            _lifebarRt.sizeDelta = new Vector2(len, _lifebarRt.sizeDelta.y);
            _length = length;
        }

        public void SetBackgroundColor(float r, float g, float b, float a = 1f)
        {
            _barController.setBackgroundColor(new Color(r, g, b, a));
        }

        public void SetBackgroundColor32(byte r, byte g, byte b, byte a = 255)
        {
            _barController.setBackgroundColor(new Color32(r, g, b, a));
        }

        public void SetFillColor(float r, float g, float b, float a = 1f)
        {
            _barController.setFillColor(new Color(r, g, b, a));
        }

        public void SetFillColor32(byte r, byte g, byte b, byte a = 255)
        {
            _barController.setFillColor(new Color32(r, g, b, a));
        }

        public void SendToTop()
        {
            transform.SetAsLastSibling();
        }

        public void SendToBottom()
        {
            transform.SetAsFirstSibling();
        }
    }
}
