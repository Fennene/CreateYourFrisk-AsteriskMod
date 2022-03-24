using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.Lua.GMS
{
    public class UnityObject
    {
        private GameObject _gameObject;

        public bool canModify
        {
            get;
            private set;
        }

        public LuaImage Image
        { 
            get;
            private set;
        }

        public UnityObject(GameObject gameObject, bool cyf)
        {
            _gameObject = gameObject;
            canModify = !cyf;
            Image = new LuaImage(_gameObject.GetComponent<Image>(), this);
        }

        public bool exists
        {
            get { return _gameObject != null; }
        }

        public string name
        {
            get { return _gameObject.name; }
            set { _gameObject.name = value; }
        }

        public void Remove()
        {
            if (!canModify)
                throw new CYFException("GameObject \"" + name + "\" can't be removed.");
            Image.Remove();
            GameObject.Destroy(_gameObject);
        }

        public void MoveTo(float x, float y, float z = 0)
        {
            _gameObject.transform.position = new Vector3(x, y, z);
        }

        public float x
        {
            get { return _gameObject.transform.position.x; }
            set { MoveTo(value, y, z); }
        }

        public float y
        {
            get { return _gameObject.transform.position.y; }
            set { MoveTo(x, value, z); }
        }

        public float z
        {
            get { return _gameObject.transform.position.z; }
            set { MoveTo(x, y, value); }
        }

        public void Move(float x, float y, float z = 0)
        {
            MoveTo(_gameObject.transform.position.x + x, _gameObject.transform.position.y + y, _gameObject.transform.position.z + z);
        }
    }
}
