using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class LuaImageComponent
    {
        private Image _image;
        private UnityObject _unityObject;

        public LuaImageComponent(UnityObject unityObject)
        {
            _unityObject = unityObject;
            if (_unityObject._gameObject == null) return;
            _image = _unityObject._gameObject.GetComponent<Image>();
        }

        private string name { get { return _unityObject.name; } }

        public bool isactive { get { return _image != null; } }

        public void AddComponent()
        {
            if (isactive)
                throw new CYFException("GameObject \"" + name + "\" have ImageComponent already.");
            _image = _unityObject._gameObject.AddComponent<Image>();
        }

        public void Remove()
        {
            if (!isactive)
                throw new CYFException("Attempt to remove a removed ImageComponent object.");
            Object.Destroy(_image);
        }
    }
}
