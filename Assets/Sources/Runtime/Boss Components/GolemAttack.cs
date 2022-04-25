using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Runtime.Boss_Components
{
    public class GolemAttack : MonoBehaviour
    {
        [SerializeField]
        private Transform _rotationTarget;
        [SerializeField]
        private float _phaseSwitchAttackSpeedMulti;
        private BossPhaseSwitching _phaseSwitching;

        [Header("Jump Attack")]
        [SerializeField]
        private float _jumPower = 10;
        [SerializeField]
        private float _jumpDuration = 2;
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
        
        private BossAnimator _bossAnimator;
        private GolemShooter _shooter;
        private Collider2D _collider2D;
        private Transform _player;

        private bool _isStatic = false; // TODO: Change

        public void Init(BossPhaseSwitching phaseSwitching, BossAnimator animator,
            Transform player, GolemShooter shooter)
        {
            _phaseSwitching = phaseSwitching;
            _bossAnimator = animator;
            _phaseSwitching.PhaseSwitched += IncreaseAttackSpeed;
            _player = player;
            _shooter = shooter;
            _collider2D = GetComponent<Collider2D>();
            enabled = true;
        }

        public void StartAttack()
        {
            if (_phaseSwitching.CurrentPhase.AttacksPool.Length > 0)
                _bossAnimator.TriggerAttack(
                    _phaseSwitching.CurrentPhase.AttacksPool[
                        Random.Range(0, _phaseSwitching.CurrentPhase.AttacksPool.Length)]);
        }

        public void JumpAttack(int count)
        {
            _collider2D.enabled = false;
            var jump = transform.DOJump(_player.position, _jumPower, 1, _jumpDuration);
            jump.onComplete += () =>
            {
                _shooter.RingShoot();
                _collider2D.enabled = true;
                if (++count < _jumpCount)
                    JumpAttack(count);
                else
                    _bossAnimator.OnAttackEnded();
            };
        }

        public void EarthHitAttack()
        {
            _shooter.RingShoot(_hitPoint1.position);
            _shooter.RingShoot(_hitPoint2.position);
            if(++_hitCounter < _hitCount)
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

        private void NormalizeBeamRotation()
        {
            var beamStartPosition = Vector3.Cross(_beam.forward, _player.position - _beam.position);
            if (Mathf.Abs(beamStartPosition.x) > Mathf.Abs(beamStartPosition.y))
                beamStartPosition.y = 0;
            else
                beamStartPosition.x = 0;

            _beam.rotation = Quaternion.LookRotation(_beam.forward, beamStartPosition);
        }

        private void IncreaseAttackSpeed()
        {
            _bossAnimator.IncreaseAttackSpeedMulti(_phaseSwitchAttackSpeedMulti);
            
            _jumpDuration /= _phaseSwitchAttackSpeedMulti;

            _beamMovingDelay /= _phaseSwitchAttackSpeedMulti;
        }

        private IEnumerator BeamMoving()
        {
            yield return new WaitForSeconds(_beamMovingDelay);
            while (_beam.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
                var newRotation = Quaternion.LookRotation(_beam.forward,
                    Vector3.Cross(_beam.forward, _player.position - _beam.position));
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
            var playerDirection = _player.transform.position - transform.position;
            _rotationTarget.right = new Vector3(playerDirection.normalized.x, 0);
        }
    }
}