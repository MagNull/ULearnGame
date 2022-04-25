using Sources.Runtime.Input;
using Sources.Runtime.Utils;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCompositeRoot : MonoBehaviour
    {
        [SerializeField]
        private Player _player;
        [Header("Health")]
        [SerializeField]
        private int _healthValue;
        [SerializeField]
        private CellHealthView _cellHealthView;
        [SerializeField]
        private HitFlash _hitFlash;
        [Header("Movement")]
        [SerializeField]
        private float _speed = 1;
        [SerializeField]
        private Transform _rotationTarget;
        private PlayerMovement _movement;

        [Header("Shooting")]
        [SerializeField]
        private ProjectileFactory _playerProjectileAbstractFactory;
        [SerializeField]
        private float _projectileSpeed = 1;
        [SerializeField]
        private float _shootDelay = 1;
        [SerializeField]
        private Transform _shootOrigin;
        private PlayerShooter _playerShooter;

        [Header("Blink")]
        [SerializeField]
        private float _blinkDistance;
        [SerializeField]
        private float _blinkCooldown;
        [SerializeField]
        private LayerMask _wallLayer;
        [SerializeField]
        private float _wallBlinkDistance;
        [SerializeField]
        private ParticleSystem _startBlinkVFX;
        [SerializeField]
        private ParticleSystem _endBlinkVFX;
        [SerializeField]
        private CoolDownView _blinkCoolDownView;
        private Blink _blink;

        private PlayerAnimator _playerAnimator;
        private InputBindings _inputBindings;

        private void Awake()
        {
            Compose();
        }

        private void Compose()
        {
            _movement = new PlayerMovement(GetComponent<Rigidbody2D>(), _speed, _rotationTarget);
            _playerShooter = new PlayerShooter(
                new ObjectPool<Projectile>(10,
                    _playerProjectileAbstractFactory.Create<Projectile, Player>),
                _shootOrigin, _projectileSpeed, _shootDelay);
            _blink = new Blink(_movement, transform, _blinkDistance, _blinkCooldown, _startBlinkVFX, _endBlinkVFX,
                _wallBlinkDistance, _wallLayer);
            _blinkCoolDownView.BindAbility(_blink);
            _inputBindings = GetComponent<InputBindings>();
            var health = new Health(_healthValue);

            _playerAnimator = new PlayerAnimator(GetComponentInChildren<Animator>());

            _cellHealthView.Init(health.Value, _player);
            
            _hitFlash.Init(_player);
            
            _player.Init(_movement, _playerShooter, _playerAnimator, health);
        }

        private void Start()
        {
            BindInput();
        }

        private void BindInput()
        {
            _inputBindings.BindMovement(_movement);
            _inputBindings.BindShooting(_playerShooter);
            _inputBindings.BindBlink(_blink);
        }
    }
}