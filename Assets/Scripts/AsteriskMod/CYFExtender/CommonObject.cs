using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.CYFExtender
{
    public class CommonObject : MonoBehaviour
    {
        private Vector2 relativePosition = Vector2.zero;

        public float x
        {
            get { return relativePosition.x; }
        }

        public float y
        {
            get { return relativePosition.y; }
        }

        public void MoveTo(float newX, float newY)
        {
            Vector2 initPos = GetComponent<RectTransform>().anchoredPosition - relativePosition;
            relativePosition = new Vector2(newX, newY);
            GetComponent<RectTransform>().anchoredPosition = initPos + relativePosition;
        }

        public void MoveToAbs(float newX , float newY)
        {
            Vector2 initPos = GetComponent<RectTransform>().anchoredPosition - relativePosition;
            gameObject.transform.position = new Vector3(newX, newY, gameObject.transform.position.z);
            relativePosition = GetComponent<RectTransform>().anchoredPosition - initPos;
        }
    }
}
