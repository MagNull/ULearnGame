using System.Collections;
using Sources.Runtime.Interfaces;
using UnityEngine;
using Zenject;

public class HitFlash : MonoBehaviour
{
    [SerializeField]
    private Material _flashMaterial;
    private Material _defaultMaterial;
    [SerializeField]
    private float _flashDuration;
    [SerializeField]
    private int _flashCount;
    private SpriteRenderer _meshRenderer;
    private Coroutine _flashCoroutine;

    [Inject]
    private void Init(IDamageable player)
    {
        player.Damaged += OnDamaged;
        _meshRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _meshRenderer.material;
    }

    private void OnDamaged(int health)
    {
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);
        _flashCoroutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        for (var i = 0; i < _flashCount; i++)
        {
            _meshRenderer.material = _flashMaterial;
            yield return new WaitForSeconds(_flashDuration / _flashCount);
            _meshRenderer.material = _defaultMaterial;
            yield return new WaitForSeconds(_flashDuration / _flashCount);
        }
    }
}