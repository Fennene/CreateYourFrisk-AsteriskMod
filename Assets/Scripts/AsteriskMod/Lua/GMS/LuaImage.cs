using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.Lua.GMS
{
    public class LuaImage
    {
        private UnityObject unityObject;

        private Image _image;

        public bool hasComponent
        {
            get { return _image != null; }
        }

        public LuaImage(Image image, UnityObject self)
        {
            _image = image;
            unityObject = self;
        }

        public void Remove()
        {
            if (unityObject.isReferenced)
                throw new CYFException("GameObject \"" + unityObject.name + "\"'s any component can't be removed.");
            GameObject.Destroy(_image);
            _image = null;
        }

        public void MoveTo(float x, float y, float z = 0)
        {
            if (!hasComponent)
                throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
            _image.rectTransform.position = new Vector3(x, y, z);
        }

        public float x
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.position.x;
            }
            set
            {
                MoveTo(value, y, z);
            }
        }

        public float y
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.position.y;
            }
            set
            {
                MoveTo(x, value, z);
            }
        }

        public float z
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.position.z;
            }
            set
            {
                MoveTo(x, y, value);
            }
        }

        public void Move(float x, float y, float z = 0)
        {
            if (!hasComponent)
                throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
            MoveTo(_image.rectTransform.position.x + x, _image.rectTransform.position.y + y, _image.rectTransform.position.z + z);
        }

        public void Resize(float width, float height)
        {
            if (!hasComponent)
                throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
            _image.rectTransform.sizeDelta = new Vector2(width, height);
        }

        public float width
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.sizeDelta.x;
            }
            set
            {
                Resize(value, height);
            }
        }

        public float height
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.sizeDelta.y;
            }
            set
            {
                Resize(width, value);
            }
        }

        public void Scale(float xscale, float yscale, float zscale = 1f)
        {
            if (!hasComponent)
                throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
            _image.rectTransform.localScale = new Vector3(xscale, yscale, zscale);
        }

        public float xscale
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.localScale.x;
            }
            set
            {
                Scale(value, yscale, zscale);
            }
        }

        public float yscale
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.localScale.y;
            }
            set
            {
                Scale(xscale, value, zscale);
            }
        }

        public float zscale
        {
            get
            {
                if (!hasComponent)
                    throw new CYFException("GameObject \"" + unityObject.name + "\" doesn't have image component.");
                return _image.rectTransform.localScale.z;
            }
            set
            {
                Scale(xscale, yscale, value);
            }
        }
    }
}
