using System.Collections;
using Sources.Runtime;
using Sources.Runtime.Boss_Components.Reaper;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ReaperSummon : Projectile
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private Vector2 _rotationBounds = new(150, 225);
    private Transform _playerTransform;
    private ReaperShooter _reaperShooter;

    [Inject]
    protected void Init([Inject(Id = "Player")] Transform playerTransform, ProjectileFactory factory,
        ReaperShooter reaperShooter)
    {
        _playerTransform = playerTransform;
        _reaperShooter = reaperShooter;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(_attackDelay);
            _reaperShooter.Shoot(transform.position, (_playerTransform.position - transform.position).normalized);
        }
    }

    private void OnEnable()
    {
        Collided += OnCollided;
    }

    private void OnDisable()
    {
        Collided -= OnCollided;
    }

    private void OnCollided(Collider2D collider2D)
    {
        var newVelocity =
            Quaternion.AngleAxis(Random.Range(_rotationBounds.x, _rotationBounds.y), transform.forward) *
            _rigidbody2D.velocity.normalized;
        SetVelocity(newVelocity * _speed);
    }
}