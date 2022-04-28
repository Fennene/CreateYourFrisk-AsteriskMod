using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class CheckBox : MonoBehaviour
    {
        private Toggle toggle;
        private Image background;
        private Text text;
        private bool hasDisabledMask;
        private Image bgMask;
        private Image textMask;

        private Toggle.ToggleEvent commonEvent;
        private Toggle.ToggleEvent userEvent;
        private Toggle.ToggleEvent scriptEvent;
        private bool fromScript;

        private Toggle.ToggleEvent onActiveEvent;

        private List<CheckBox> children;
        private List<bool> reverse;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.enabled = true;
            background = transform.GetChild(0).GetComponent<Image>();
            text = transform.GetChild(1).GetComponent<Text>();

            commonEvent = new Toggle.ToggleEvent();
            userEvent = new Toggle.ToggleEvent();
            scriptEvent = new Toggle.ToggleEvent();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((value) => OnValueChanged(value));

            onActiveEvent = new Toggle.ToggleEvent();

            children = new List<CheckBox>();
            reverse = new List<bool>();

            hasDisabledMask = (text.transform.Find("DisabledMask") != null);
            if (!hasDisabledMask) return;
            bgMask = background.transform.Find("DisabledMask").GetComponent<Image>();
            textMask = text.transform.Find("DisabledMask").GetComponent<Image>();
            toggle.enabled = !bgMask.enabled;
        }

        private void OnValueChanged(bool value)
        {
            if (!isEnabled) return;
            commonEvent.Invoke(value);
            for (var i = 0; i < children.Count; i++)
            {
                /*
                if (reverse[i]) children[i].SetActive(!value);
                else            children[i].SetActive(value);
                */
                children[i].SetActive(value ^ reverse[i]);
            }
            if (fromScript) scriptEvent.Invoke(value);
            else              userEvent.Invoke(value);
            fromScript = false;
        }

        public bool isEnabled
        {
            get { return toggle.enabled; }
        }

        public bool Checked
        {
            get { return isEnabled && toggle.isOn; }
            set
            {
                if (toggle.isOn == value) return;
                fromScript = true;
                toggle.isOn = value;
            }
        }

        public string Text
        {
            get { return text.text; }
            set { text.text = value; }
        }

        public void SetActive(bool active)
        {
            toggle.enabled = active;
            onActive.Invoke(active);
            if (!hasDisabledMask) return;
            bgMask.enabled = textMask.enabled = !active;
        }

        [System.Obsolete]
        public void Enable()
        {
            toggle.enabled = true;
            if (!hasDisabledMask) return;
            bgMask.enabled = textMask.enabled = false;
        }

        [System.Obsolete]
        public void Disable()
        {
            toggle.enabled = false;
            if (!hasDisabledMask) return;
            bgMask.enabled = textMask.enabled = true;
        }

        public Toggle.ToggleEvent onValueChanged
        {
            get { return commonEvent; }
        }

        public Toggle.ToggleEvent onValueChangedFromUser
        {
            get { return userEvent; }
        }

        public Toggle.ToggleEvent onValueChangedFromScript
        {
            get { return scriptEvent; }
        }

        public void AddChildCheckBox(CheckBox child, bool linkActivate = true)
        {
            children.Add(child);
            reverse.Add(!linkActivate);
        }

        public Toggle.ToggleEvent onActive
        {
            get { return onActiveEvent; }
        }
    }
}
