using UnityEngine;

public class HealthSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject _cell;

    public void Activate() => _cell.SetActive(true);
    public void Deactivate() => _cell.SetActive(false);
}
