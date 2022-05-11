using Sources.Runtime.Input;
using Sources.Runtime.Interfaces;
using Sources.Runtime.UI___HUD;
using Sources.Runtime.Utils;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Player_Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCompositeRoot : MonoInstaller
    {
        [Header("UI")]
        [SerializeField]
        private StopScreen _dieScreen;
        [SerializeField]
        private StopScreen _pauseScreen;
        
        [Space]
        [SerializeField]
        private Player _player;
        [Header("Health")]
        [SerializeField]
        private int _healthValue;
        [SerializeField]
        private Health _health;
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
        

        private void Init()
        {
            _inputBindings = GetComponent<InputBindings>();
            _inputBindings.Init();
            _movement = new PlayerMovement(GetComponent<Rigidbody2D>(), _speed, _rotationTarget);
            _playerShooter = new PlayerShooter(
                new ObjectPool<Projectile>(10,
                    _playerProjectileAbstractFactory.Create<Projectile, Player>),
                _shootOrigin, _projectileSpeed, _shootDelay);
            
            _blink = new Blink(_movement, transform, _blinkDistance, _blinkCooldown, _startBlinkVFX, _endBlinkVFX,
                _wallBlinkDistance, _wallLayer);
            _blinkCoolDownView.BindAbility(_blink);
            
            _health = new Health(_healthValue);
            _playerAnimator = new PlayerAnimator(GetComponentInChildren<Animator>());
        }
        

        public override void InstallBindings()
        {
            Init();
            Container.Bind<IDamageable>().To<Player>().FromInstance(_player).AsSingle();
            Container.Bind<SceneLoader>().FromInstance(FindObjectOfType<SceneLoader>()).AsSingle();
            Container.Bind<IMovement>().To<PlayerMovement>().FromInstance(_movement).AsSingle();
            Container.Bind<IShooter>().To<PlayerShooter>().FromInstance(_playerShooter).AsSingle();
            Container.Bind<PlayerAnimator>().FromInstance(_playerAnimator).AsSingle();
            Container.Bind<Health>().FromInstance(_health).AsSingle();
            Container.Bind<IAbility>().WithId("Blink").To<Blink>().FromInstance(_blink).AsSingle();
            Container.Bind<StopScreen>().WithId("Die Screen").FromInstance(_dieScreen);
            Container.Bind<StopScreen>().WithId("Pause Screen").FromInstance(_pauseScreen);
        }
    }
}