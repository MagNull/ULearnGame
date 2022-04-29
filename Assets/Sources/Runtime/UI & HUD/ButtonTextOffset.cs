using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Runtime.UI___HUD
{
    [RequireComponent(typeof(Button))]
    public class ButtonTextOffset : MonoBehaviour, IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField]
        private float _downOffset;
        [SerializeField]
        private TextMeshProUGUI _text;

        public void OnPointerDown(PointerEventData eventData) =>
            _text.transform.position += Vector3.down * _downOffset;

        public void OnPointerUp(PointerEventData eventData) =>
            _text.transform.position -= Vector3.down * _downOffset;
    }
}