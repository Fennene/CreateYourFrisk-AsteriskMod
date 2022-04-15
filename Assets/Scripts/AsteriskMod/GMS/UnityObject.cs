using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class UnityObject
    {
        internal GameObject _gameObject;
        private LuaImageComponent _image;

        public UnityObject(GameObject gameObject)
        {
            _gameObject = gameObject;
            _image = new LuaImageComponent(this);
        }

        public LuaImageComponent Image {
            get {
                if (!isactive) throw new CYFException("Attempt to get ImageComponent from a removed GameObject object.");
                return _image;
            }
        }

        public bool isactive { get { return _gameObject != null; } }

        public void Remoeve()
        {
            if (!isactive) throw new CYFException("Attempt to remove a removed GameObject object.");
            Object.Destroy(_gameObject);
        }

        public UnityObject Find(string name)
        {
            if (!isactive) throw new CYFException("Attempt to perform action on a removed GameObject object.");
            Transform child = _gameObject.transform.Find(name);
            if (child == null) throw new CYFException("GameObject \"" + name + "\" is not found in GameObject \"" + name + "\"");
            return new UnityObject(child.gameObject);
        }

        public UnityObject CreateObject(string name)
        {
            if (!isactive) throw new CYFException("Attempt to create GameObject on a removed GameObject object.");
            GameObject child = new GameObject(name);
            child.transform.parent = _gameObject.transform;
            return new UnityObject(child);
        }

        public string name
        {
            get {
                if (!isactive) throw new CYFException("Attempt to get parameter from a removed GameObject object.");
                return _gameObject.name;
            }
            set { _gameObject.name = value; }
        }
    }
}
