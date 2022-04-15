using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class LuaImageComponent
    {
        private Image _image;
        internal bool _isUserCreated = true;
        private UnityObject _unityObject;

        private string name { get { return _unityObject.name; } }

        public LuaImageComponent(UnityObject unityObject, bool userCrated)
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
            if (!_isUserCreated) throw new CYFException("You can remove the ImageComponent only that is created by you.");
            if (!isactive) throw new CYFException("Attempt to remove a removed ImageComponent object.");
            Object.Destroy(_image);
        }
    }
}
