using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AsteriskMod.UnityUI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(InputField))]
    public class TextBox : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasRenderer _canvasRenderer;
        [SerializeField] private Image _image;
        [SerializeField] private InputField _inputField;

        private GameObject _child_image;
        private GameObject _child_placeholder;
        private GameObject _child_text;
        private Image _child_image_image;

        private InputField.OnChangeEvent _onValueChangedfromScript;

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasRenderer = GetComponent<CanvasRenderer>();
            _image = GetComponent<Image>();
            _inputField = GetComponent<InputField>();

            _rectTransform.sizeDelta = new Vector2(120f, 18f);
            _rectTransform.localScale = new Vector3(1f, 1f, 1f);

            _canvasRenderer.cullTransparentMesh = true;

            _image.sprite = null;
            _image.color = new Color32(64, 64, 64, 255);
            _image.material = null;

            _child_image = new GameObject("Image", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            _child_image.transform.parent = transform;
            _child_image_image = _child_image.GetComponent<Image>();
            _child_image.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            _child_image.GetComponent<RectTransform>().sizeDelta = new Vector2(-4f, -4f);
            _child_image.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            _child_image.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            _child_image.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            _child_image.transform.position = new Vector3(_child_image.transform.position.x, _child_image.transform.position.y, 80f);
            _child_image.GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            _child_image.GetComponent<Image>().sprite = null;
            _child_image.GetComponent<Image>().color = new Color32(242, 242, 242, 255);
            _child_image.GetComponent<Image>().material = null;
            _child_image.GetComponent<Image>().raycastTarget = true;

            _child_placeholder = new GameObject("Placeholder", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            _child_placeholder.transform.parent = transform;
            _child_placeholder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            _child_placeholder.GetComponent<RectTransform>().sizeDelta = new Vector2(-8f, -4f);
            _child_placeholder.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            _child_placeholder.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            _child_placeholder.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            _child_placeholder.transform.position = new Vector3(_child_image.transform.position.x, _child_image.transform.position.y, 80f);
            _child_placeholder.GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            _child_placeholder.GetComponent<Text>().enabled = false;

            _child_text = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            _child_text.transform.parent = transform;
            _child_text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            _child_text.GetComponent<RectTransform>().sizeDelta = new Vector2(-8f, -4f);
            _child_text.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            _child_text.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            _child_text.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            _child_text.transform.position = new Vector3(_child_image.transform.position.x, _child_image.transform.position.y, 80f);
            _child_text.GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            _child_text.GetComponent<Text>().text = "";
            _child_text.GetComponent<Text>().font = Resources.Load<Font>("Fonts/JF-Dot-Shinonome14/JF-Dot-Shinonome14");
            _child_text.GetComponent<Text>().fontStyle = FontStyle.Normal;
            _child_text.GetComponent<Text>().fontSize = 14;
            _child_text.GetComponent<Text>().lineSpacing = 1;
            _child_text.GetComponent<Text>().supportRichText = false;
            _child_text.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            _child_text.GetComponent<Text>().alignByGeometry = false;
            _child_text.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
            _child_text.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Truncate;
            _child_text.GetComponent<Text>().resizeTextForBestFit = false;
            _child_text.GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            _child_text.GetComponent<Text>().material = null;
            _child_text.GetComponent<Text>().raycastTarget = true;

            _inputField.interactable = true;
            _inputField.transition = Selectable.Transition.None;
            _inputField.navigation = new Navigation() { mode = Navigation.Mode.None };
            _inputField.textComponent = _child_text.GetComponent<Text>();
            _inputField.text = "";
            _inputField.characterLimit = 0;
            _inputField.contentType = InputField.ContentType.Standard;
            _inputField.lineType = InputField.LineType.SingleLine;
            _inputField.placeholder = _child_placeholder.GetComponent<Text>();
            _inputField.caretBlinkRate = 0.85f;
            _inputField.caretWidth = 1;
            _inputField.customCaretColor = false;
            _inputField.selectionColor = new Color32(168, 206, 255, 192);
            _inputField.shouldHideMobileInput = false;
            _inputField.readOnly = false;
            _inputField.onValueChanged.RemoveAllListeners();
            _inputField.onEndEdit.RemoveAllListeners();

            _onValueChangedfromScript = new InputField.OnChangeEvent();
        }

        public RectTransform rectTransform { get { return _rectTransform; } }
        public Image outerImage { get { return _image; } }
        public Image innerImage { get { return _child_image_image; } }
        public InputField inputField { get { return _inputField; } }

        public string text
        {
            get { return _inputField.text; }
            set
            {
                _inputField.text = value;
                onValueChangedFromScript.Invoke(value);
            }
        }

        public Color outerColor
        {
            get { return _image.color; }
            set { _image.color = value; }
        }

        public Color innerColor
        {
            get { return _child_image_image.color; }
            set { _child_image_image.color = value; }
        }

        internal void SetErrorOuterColor(bool error) { _image.color = error ? new Color32(255, 64, 64, 255) : new Color32(64, 64, 64, 255); }

        public InputField.OnChangeEvent onValueChanged
        {
            get { return _inputField.onValueChanged; }
            set { _inputField.onValueChanged = value; }
        }

        public InputField.SubmitEvent onEndEdit
        {
            get { return _inputField.onEndEdit; }
            set { _inputField.onEndEdit = value; }
        }

        public InputField.OnChangeEvent onValueChangedFromScript
        {
            get { return _onValueChangedfromScript; }
            set { _onValueChangedfromScript = value; }
        }

        internal void SetOnValueChangedListener(UnityAction<string> call)
        {
            _inputField.onValueChanged.RemoveAllListeners();
            _inputField.onValueChanged.AddListener(call);
        }

        internal void SetOnEndEditListener(UnityAction<string> call)
        {
            _inputField.onEndEdit.RemoveAllListeners();
            _inputField.onEndEdit.AddListener(call);
        }

        /*
        public class OnChangeFromScriptEvent : UnityEvent<string>
        {
            public OnChangeFromScriptEvent() { }
        }
        */
    }
}
