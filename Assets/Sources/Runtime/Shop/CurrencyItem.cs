using Sources.Runtime.Player_Components;
using UnityEngine;

public class CurrencyItem : MonoBehaviour
{
    [SerializeField]
    private Currency _type;
    [SerializeField]
    private int _amount = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent(out Player player))
            return;
        player.AddCurrency(_type, _amount);
        Destroy(gameObject);
    }
}
