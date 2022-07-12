using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class LuaImageComponent_
    {
        private Image _image;
        internal bool _isUserCreated = true;
        private UnityObject_ _unityObject;

        private string name { get { return _unityObject.name; } }

        public LuaImageComponent_(UnityObject_ unityObject, bool userCrated)
        {
            _unityObject = unityObject;
            if (_unityObject._gameObject == null) return;
            _isUserCreated = userCrated;
            _image = _unityObject._gameObject.GetComponent<Image>();
        }

        public bool isactive { get { return _image != null; } }

        public bool isusercreated { get { return _isUserCreated; } }

        public void AddComponent()
        {
            if (isactive) throw new CYFException("GameObject \"" + name + "\" have ImageComponent already.");
            _image = _unityObject._gameObject.AddComponent<Image>();
        }

        public void Remove()
        {
            if (!isactive) throw new CYFException("Attempt to remove a removed ImageComponent object.");
            if (!_isUserCreated) throw new CYFException("You can remove the ImageComponent only that is created by you.");
            Object.Destroy(_image);
        }

        public int x
        {
            get
            {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return Mathf.RoundToInt(_image.rectTransform.localPosition.x);
            }
            set
            {
                if (!isactive) throw new CYFException("Attempt to move a removed ImageComponent object.");
                MoveTo(value, y);
            }
        }

        public int y
        {
            get
            {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return Mathf.RoundToInt(_image.rectTransform.localPosition.y);
            }
            set
            {
                if (!isactive) throw new CYFException("Attempt to move a removed ImageComponent object.");
                MoveTo(x, value);
            }
        }

        public int absx
        {
            get
            {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return Mathf.RoundToInt(_image.rectTransform.position.x);
            }
            set
            {
                if (!isactive) throw new CYFException("Attempt to move a removed ImageComponent object.");
                MoveToAbs(value, absy);
            }
        }

        public int absy
        {
            get
            {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return Mathf.RoundToInt(_image.rectTransform.position.y);
            }
            set
            {
                if (!isactive) throw new CYFException("Attempt to move a removed ImageComponent object.");
                MoveToAbs(absx, value);
            }
        }

        public void Move(int x, int y)
        {
            if (!isactive) throw new CYFException("Attempt to move a removed ImageComponent object.");
            _image.rectTransform.localPosition = new Vector3(x + this.x, y + this.y, _image.rectTransform.localPosition.z);
        }

        public void MoveTo(int newX, int newY)
        {
            if (!isactive) throw new CYFException("Attempt to move a removed ImageComponent object.");
            _image.rectTransform.localPosition = new Vector3(newX, newY, _image.rectTransform.localPosition.z);
        }

        public void MoveToAbs(int newX, int newY)
        {
            if (!isactive) throw new CYFException("Attempt to move a removed ImageComponent object.");
            _image.rectTransform.position = new Vector3(newX, newY, _image.rectTransform.position.z);
        }

        public float[] color
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return new[] { _image.color.r, _image.color.g, _image.color.b };
            }
            set {
                if (!isactive) throw new CYFException("Attempt to color a removed ImageComponent object.");
                if (value == null) throw new CYFException("ImageComponent.color can't be nil.");
                switch (value.Length)
                {
                    case 3: _image.color = new Color(value[0], value[1], value[2], alpha); break;
                    case 4: _image.color = new Color(value[0], value[1], value[2], value[3]); break;
                    default: throw new CYFException("You need 3 or 4 numeric values when setting a ImageComponent's color.");
                }
            }
        }

        public float[] color32
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return new float[] { ((Color32)_image.color).r, ((Color32)_image.color).g, ((Color32)_image.color).b };
            }
            set {
                if (!isactive) throw new CYFException("Attempt to color a removed ImageComponent object.");
                if (value == null) throw new CYFException("ImageComponent.color32 can't be nil.");
                for (int i = 0; i < value.Length; i++)
                    if (value[i] < 0) value[i] = 0;
                    else if (value[i] > 255) value[i] = 255;
                if (value.Length == 3) _image.color = new Color32((byte)value[0], (byte)value[1], (byte)value[2], (byte)alpha32);
                else if (value.Length == 4) _image.color = new Color32((byte)value[0], (byte)value[1], (byte)value[2], (byte)value[3]);
                else throw new CYFException("You need 3 or 4 numeric values when setting a ImageComponent's color.");
            }
        }

        public float alpha
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return _image.color.a;
            }
            set {
                if (!isactive) throw new CYFException("Attempt to set alpha to a removed ImageComponent object.");
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Mathf.Clamp01(value));
            }
        }

        public float alpha32
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed ImageComponent object.");
                return ((Color32)_image.color).a;
            }
            set {
                if (!isactive) throw new CYFException("Attempt to set alpha32 to a removed ImageComponent object.");
                _image.color = new Color32(((Color32)_image.color).r, ((Color32)_image.color).g, ((Color32)_image.color).b, (byte)value);
            }
        }
    }
}
