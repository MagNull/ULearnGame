using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Runtime.UI___HUD
{
    [RequireComponent(typeof(Button))]
    public class ButtonChildOffset : MonoBehaviour, IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField]
        private float _downOffset;
        [SerializeField]
        private Transform _childObject;

        public void OnPointerDown(PointerEventData eventData) =>
            _childObject.transform.position += Vector3.down * _downOffset;

        public void OnPointerUp(PointerEventData eventData) =>
            _childObject.transform.position -= Vector3.down * _downOffset;
    }
}