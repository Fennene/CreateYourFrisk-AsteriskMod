using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class UnityObject
    {
        internal GameObject _gameObject;
        internal bool _isUserCreated;
        private LuaImageComponent _image;

        public UnityObject(GameObject gameObject, bool found)
        {
            _gameObject = gameObject;
            _isUserCreated = !found;
            _image = new LuaImageComponent(this, _isUserCreated);
            if (_gameObject == null || !_isUserCreated) return;
            _gameObject.transform.localPosition = new Vector3(0, 0, _gameObject.transform.localPosition.z);
        }

        public LuaImageComponent Image {
            get {
                if (!isactive) throw new CYFException("Attempt to get ImageComponent from a removed GameObject object.");
                return _image;
            }
        }

        public bool isactive { get { return _gameObject != null; } }

        public bool isusercreated { get { return _isUserCreated; } }

        public void Remoeve()
        {
            if (!isactive) throw new CYFException("Attempt to remove a removed GameObject object.");
            if (!_isUserCreated) throw new CYFException("The GameObject that is got by calling Find() can not be removed.");
            Object.Destroy(_gameObject);
        }

        public UnityObject Find(string name)
        {
            if (!isactive) throw new CYFException("Attempt to perform action on a removed GameObject object.");
            Transform child = _gameObject.transform.Find(name);
            if (child == null) throw new CYFException("GameObject \"" + name + "\" is not found in GameObject \"" + name + "\"");
            return new UnityObject(child.gameObject, true);
        }

        public UnityObject CreateObject(string name)
        {
            if (!isactive) throw new CYFException("Attempt to create GameObject on a removed GameObject object.");
            GameObject child = new GameObject(name);
            child.transform.parent = _gameObject.transform;
            child.AddComponent<RectTransform>();
            child.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            child.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            //child.transform.localPosition = new Vector3(0, 0, child.transform.localPosition.z);
            return new UnityObject(child, false);
        }

        public string name
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed GameObject object.");
                return _gameObject.name;
            }
            set {
                if (!isactive) throw new CYFException("Attempt to set parameter from a removed GameObject object.");
                if (!_isUserCreated) throw new CYFException("The GameObject that is got by calling Find() can not be changed name.");
                _gameObject.name = value;
            }
        }

        public int x
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed GameObject object.");
                //return Mathf.RoundToInt(_gameObject.transform.localPosition.x);
                return Mathf.RoundToInt(_gameObject.GetComponent<RectTransform>().anchoredPosition.x);
            }
            set {
                if (!isactive) throw new CYFException("Attempt to move a removed GameObject object.");
                MoveTo(value, y);
            }
        }

        public int y
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed GameObject object.");
                //return Mathf.RoundToInt(_gameObject.transform.localPosition.y);
                return Mathf.RoundToInt(_gameObject.GetComponent<RectTransform>().anchoredPosition.x);
            }
            set {
                if (!isactive) throw new CYFException("Attempt to move a removed GameObject object.");
                MoveTo(x, value);
            }
        }

        public int absx
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed GameObject object.");
                return Mathf.RoundToInt(_gameObject.transform.position.x);
            }
            set {
                if (!isactive) throw new CYFException("Attempt to move a removed GameObject object.");
                MoveToAbs(value, absy);
            }
        }

        public int absy
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed GameObject object.");
                return Mathf.RoundToInt(_gameObject.transform.position.y);
            }
            set {
                if (!isactive) throw new CYFException("Attempt to move a removed GameObject object.");
                MoveToAbs(absx, value);
            }
        }

        public void Move(int x, int y)
        {
            //if (!isactive) throw new CYFException("Attempt to move a removed GameObject object.");
            //_gameObject.transform.localPosition = new Vector3(x + this.x, y + this.y, _gameObject.transform.localPosition.z);
            MoveTo(x + this.x, y + this.y);
        }

        public void MoveTo(int newX, int newY)
        {
            if (!isactive) throw new CYFException("Attempt to move a removed GameObject object.");
            //_gameObject.transform.localPosition = new Vector3(newX, newY, _gameObject.transform.localPosition.z);
            _gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(newX, newY);
        }

        public void MoveToAbs(int newX, int newY)
        {
            if (!isactive) throw new CYFException("Attempt to move a removed GameObject object.");
            _gameObject.transform.position = new Vector3(newX, newY, _gameObject.transform.position.z);
        }
    }
}
