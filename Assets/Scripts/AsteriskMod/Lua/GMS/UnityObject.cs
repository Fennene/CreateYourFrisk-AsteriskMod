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

        public bool isReferenced
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
            isReferenced = !cyf;
            Image = new LuaImage(_gameObject.GetComponent<Image>(), this);
        }

        public bool isactive
        {
            get { return _gameObject != null; }
        }

        public void AddComponent(string name)
        {
            if (Image.hasComponent) return;
            Image = new LuaImage(_gameObject.AddComponent<Image>(), this);
        }

        public string name
        {
            get { return _gameObject.name; }
            set { _gameObject.name = value; }
        }

        public void Remove()
        {
            if (!isReferenced)
                throw new CYFException("GameObject \"" + name + "\" can't be removed.");
            if (!isactive)
                throw new CYFException("You cannot remove GameObject has already removed.");
            Image.Remove();
            GameObject.Destroy(_gameObject);
            _gameObject = null;
        }

        public void MoveTo(float x, float y, float z = 0)
        {
            if (!isactive)
                throw new CYFException("You cannot move removed GameObject.");
            _gameObject.transform.position = new Vector3(x, y, z);
        }

        public float x
        {
            get {
                if (!isactive)
                    throw new CYFException("You cannot aceess removed GameObject.");
                return _gameObject.transform.position.x;
            }
            set { MoveTo(value, y, z); }
        }

        public float y
        {
            get
            {
                if (!isactive)
                    throw new CYFException("You cannot aceess removed GameObject.");
                return _gameObject.transform.position.y;
            }
            set { MoveTo(x, value, z); }
        }

        public float z
        {
            get
            {
                if (!isactive)
                    throw new CYFException("You cannot aceess removed GameObject.");
                return _gameObject.transform.position.z;
            }
            set { MoveTo(x, y, value); }
        }

        public void Move(float x, float y, float z = 0)
        {
            if (!isactive)
                throw new CYFException("You cannot move removed GameObject.");
            MoveTo(_gameObject.transform.position.x + x, _gameObject.transform.position.y + y, _gameObject.transform.position.z + z);
        }
    }
}
