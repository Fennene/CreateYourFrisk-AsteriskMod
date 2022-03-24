using MoonSharp.Interpreter;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.AsteriskMod.Lua
{
    public class UnityObjectRaw
    {
        private GameObject _gameObject;
        private bool isUserCreated = false;
        private Vector3 _relativePosition = Vector3.zero;

        public bool isactive
        {
            get
            {
                return _gameObject == null;
            }
        }

        private void CheckExist()
        {
            if (!isactive)
                throw new CYFException("Attempt to perform action on removed unity object.");
        }

        public string name
        {
            get
            {
                CheckExist();
                return _gameObject.name;
            }
            set
            {
                CheckExist();
                _gameObject.name = value;
            }
        }

        public float x
        {
            get
            {
                CheckExist();
                return _relativePosition.x;
            }
            set
            {
                CheckExist();
                MoveTo(value, _relativePosition.y, _relativePosition.z);
            }
        }

        public float y
        {
            get
            {
                CheckExist();
                return _relativePosition.y;
            }
            set
            {
                CheckExist();
                MoveTo(_relativePosition.x, value, _relativePosition.z);
            }
        }

        public float z
        {
            get
            {
                CheckExist();
                return _relativePosition.z;
            }
            set
            {
                CheckExist();
                MoveTo(_relativePosition.x, _relativePosition.y, value);
            }
        }

        public void MoveTo(float x, float y, float? z = null)
        {
            CheckExist();
            if (!z.HasValue)
                z = 0;
            Vector3 oldObjectPosition = _gameObject.transform.position;
            Vector3 oldRelativePosition = _relativePosition;
            _relativePosition = new Vector3(x, y, z.Value);
            _gameObject.transform.position = new Vector3(
                oldObjectPosition.x - oldRelativePosition.x + _relativePosition.x,
                oldObjectPosition.y - oldRelativePosition.y + _relativePosition.y,
                oldObjectPosition.z - oldRelativePosition.z + _relativePosition.z
            );
        }

        public void Move(float x, float y, float? z = null)
        {
            CheckExist();
            if (!z.HasValue)
                z = 0;
            Vector3 _ = new Vector3(x, y, z.Value);
            _relativePosition += _;
            _gameObject.transform.position += _;
        }

        // Image

        public bool hasImage
        {
            get
            {
                return _gameObject != null && _gameObject.GetComponent<Image>() != null;
            }
        }

        private void CheckImageExists()
        {
            CheckExist();
            if (!hasImage)
                throw new CYFException("Unity Object \"" + name + "\" doesn't have Image Component.");
        }

        public void SetSizeDelta(float width, float height)
        {
            CheckImageExists();
            _gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(width, height);
            _gameObject.GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }
}
