using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Runtime.Boss_Components
{
    public class BossAttack : MonoBehaviour
    {
        [Header("Jump Attack")]
        [SerializeField]
        private float _jumPower = 10;
        [SerializeField]
        private float _jumpDuration = 2;

        [Header("Beam Attack")]
        [SerializeField]
        private Transform _beam;
        [SerializeField]
        private float _beamSpeed;

        private BossPhase _currentPhase;
        private BossAnimator _bossAnimator;
        private BossShooter _shooter;
        private Collider2D _collider2D;
        private Transform _player;

        private bool _isStatic = false; // TODO: Change

        public void Init(BossPhase currentPhase, BossAnimator animator, Boss boss, Transform player,
            BossShooter shooter)
        {
            boss.Damaged += OnDamaged;
            _currentPhase = currentPhase;
            _bossAnimator = animator;
            _player = player;
            _shooter = shooter;
            _collider2D = GetComponent<Collider2D>();
            enabled = true;
        }

        public void StartAttack() =>
            _bossAnimator.TriggerAttack(_currentPhase.AttacksPool[Random.Range(0, _currentPhase.AttacksPool.Length)]);

        public void JumpAttack()
        {
            _collider2D.enabled = false;
            var jump = transform.DOJump(_player.position, _jumPower, 1, _jumpDuration);
            jump.onComplete += () =>
            {
                _bossAnimator.OnAttackEnded();
                _shooter.RingShoot();
                _collider2D.enabled = true;
            };
        }

        public void StartBeamMoving()
        {
            StartCoroutine(BeamMoving());
        }

        private IEnumerator BeamMoving()
        {
            _isStatic = true;
            while (_beam.gameObject.activeSelf)
            {
                var newRotation = Quaternion.LookRotation(_beam.forward,
                    Vector3.Cross(_beam.forward, _player.position - _beam.position));
                _beam.rotation = Quaternion.RotateTowards(_beam.rotation, newRotation, Time.deltaTime * _beamSpeed);
                yield return new WaitForEndOfFrame();
            }

            _beam.rotation = Quaternion.LookRotation(_beam.forward,
                Vector3.Cross(_beam.forward, _player.position - _beam.position));
            _isStatic = false;
            _bossAnimator.OnAttackEnded();
        }
        
        private void Update()
        {
            if(!_isStatic)
                LookAtPlayerSide();
        }

        private void LookAtPlayerSide()
        {
            var playerDirection = _player.transform.position - transform.position;
            transform.right = new Vector3(playerDirection.normalized.x, 0);
        }

        private void OnDamaged(int health)
        {
            if (health <= _currentPhase.NextPhase.HealthThreshold)
                _currentPhase = _currentPhase.NextPhase;
        }
    }
}