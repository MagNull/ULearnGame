using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sources.Runtime.Boss_Components
{
    public class GolemAttack : BossAttack<GolemShooter>
    {
        [SerializeField]
        private Transform _rotationTarget;

        [Header("Jump Attack")]
        [SerializeField]
        private float _jumpPower = 10;
        [SerializeField]
        private float _jumpDuration = 2;
        [SerializeField]
        private Transform _shadow;
        [SerializeField]
        private int _jumpCount;

        [Header("Beam Attack")]
        [SerializeField]
        private Transform _beam;
        [SerializeField]
        private float _beamSpeed;
        [SerializeField]
        private float _beamMovingDelay = 1;

        [Header("Earth hit attack")]
        [SerializeField]
        private Transform _hitPoint1;
        [SerializeField]
        private Transform _hitPoint2;
        [SerializeField]
        private int _hitCount = 1;
        [SerializeField]
        private int _hitCounter;
        
        private Collider2D _collider2D;
        private Transform _playerTransform;

        private bool _isStatic = false;

        [Inject]
        protected void Init(BossPhaseSwitching phaseSwitching, BossAnimator animator,
            [Inject(Id = "Player")]Transform playerTransform, GolemShooter shooter)
        {
            base.Init(phaseSwitching, animator, shooter);
            _playerTransform = playerTransform;
            _collider2D = GetComponent<Collider2D>();
        }

        public void JumpAttack(int count)
        {
            _collider2D.enabled = false;
            _shadow.parent = null;
            TweenShadow();

            var jump = transform.DOJump(_playerTransform.position, _jumpPower, 1, _jumpDuration);
            jump.onComplete += () =>
            {
                _shooter.RingShoot();
                _shadow.parent = transform;
                _collider2D.enabled = true;
                if (++count < _jumpCount)
                    JumpAttack(count);
                else
                    _bossAnimator.OnAttackEnded();
            };
        }

        private void TweenShadow()
        {
            var playerPos = _playerTransform.position;
            _shadow.DOMove(playerPos, _jumpDuration);
            _shadow.DOScale(Vector3.zero, _jumpDuration / 2)
                .onComplete += () =>
            {
                _shadow.position = playerPos;
                _shadow.DOScale(Vector3.one, _jumpDuration / 2);
            };
        }

        public void EarthHitAttack()
        {
            _shooter.RingShoot(_hitPoint1.position);
            _shooter.RingShoot(_hitPoint2.position);
            if (++_hitCounter < _hitCount)
                return;
            _hitCounter = 0;
            _bossAnimator.OnAttackEnded();
        }

        public void StartBeamMoving()
        {
            _isStatic = true;
            _rotationTarget.right = Vector3.right;
            NormalizeBeamRotation();
            StartCoroutine(BeamMoving());
        }

        protected override void IncreaseAttackSpeed()
        {
            _bossAnimator.IncreaseAttackSpeedMulti(PhaseSwitchAttackSpeedMulti);

            _jumpDuration /= PhaseSwitchAttackSpeedMulti;

            _beamMovingDelay /= PhaseSwitchAttackSpeedMulti;
        }

        private void NormalizeBeamRotation()
        {
            var beamStartPosition = Vector3.Cross(_beam.forward, _playerTransform.position - _beam.position);
            if (Mathf.Abs(beamStartPosition.x) > Mathf.Abs(beamStartPosition.y))
                beamStartPosition.y = 0;
            else
                beamStartPosition.x = 0;

            _beam.rotation = Quaternion.LookRotation(_beam.forward, beamStartPosition);
        }

        private IEnumerator BeamMoving()
        {
            yield return new WaitForSeconds(_beamMovingDelay);
            while (_beam.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
                var newRotation = Quaternion.LookRotation(_beam.forward,
                    Vector3.Cross(_beam.forward, _playerTransform.position - _beam.position));
                _beam.rotation = Quaternion.RotateTowards(_beam.rotation, newRotation, Time.deltaTime * _beamSpeed);
            }

            _isStatic = false;
            _bossAnimator.OnAttackEnded();
        }

        private void Update()
        {
            if (!_isStatic)
                LookAtPlayerSide();
        }

        private void LookAtPlayerSide()
        {
            var playerDirection = _playerTransform.transform.position - transform.position;
            _rotationTarget.right = new Vector3(playerDirection.normalized.x, 0);
        }
    }
}