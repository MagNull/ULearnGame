using Sources.Runtime;
using Sources.Runtime.Player_Components;
using UnityEngine;

public class BossTable : MonoBehaviour
{
    [SerializeField]
    private BossTableView _bossTableView;
    [SerializeField]
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _bossTableView.Init(_sceneLoader);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Player _)) 
            _bossTableView.gameObject.SetActive(true);
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent(out Player _)) 
            _bossTableView.gameObject.SetActive(false);
    }
}