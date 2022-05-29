using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonShrink : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private float _shrinkPower;
    [SerializeField]
    private float _shrinkDuration;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOPunchScale(Vector3.one * _shrinkPower, _shrinkDuration);
    }
}
