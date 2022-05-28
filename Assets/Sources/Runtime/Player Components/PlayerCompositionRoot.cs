using Sources.Runtime.Input;
using Sources.Runtime.Interfaces;
using Sources.Runtime.UI___HUD;
using Sources.Runtime.Utils;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Player_Components
{
    [RequireComponent(
        typeof(Rigidbody2D),
        typeof(InputBindings),
        typeof(Player))]
    public class PlayerCompositionRoot : MonoInstaller
    {
        [SerializeField]
        private Player _player;
        [SerializeField]
        private UnitySerializedDictionary<Currency, int> _startWallet;

        [Space]
        [Header("UI")]
        [SerializeField]
        private StopScreen _dieScreen;
        [SerializeField]
        private StopScreen _pauseScreen;

        [Space]
        [Header("Health")]
        [SerializeField]
        private int _healthValue;
        private Health _health;

        [Space]
        [Header("Movement")]
        [SerializeField]
        private float _speed = 1;
        [SerializeField]
        private Transform _rotationTarget;
        private PlayerMovement _movement;

        [Space]
        [Header("Shooting")]
        [SerializeField]
        private ProjectileFactory _playerProjectileFactory;
        [SerializeField]
        private float _projectileSpeed = 1;
        [SerializeField]
        private int _projectileDamage;
        [SerializeField]
        private float _shootDelay = 1;
        [SerializeField]
        private Transform _shootOrigin;
        private PlayerShooter _playerShooter;

        [Space]
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
        private PlayerWallet _playerWallet;
        private InputBindings _inputBindings;

        private void Init()
        {
            _inputBindings = GetComponent<InputBindings>();
            _inputBindings.Init();

            FirstOpenCheck();

            _player = GetComponent<Player>();
            _movement = new PlayerMovement(GetComponent<Rigidbody2D>(), _speed, _rotationTarget);
            _playerShooter = new PlayerShooter(
                new ObjectPool<Projectile>(10,
                    _playerProjectileFactory.Create<Projectile, Player>),
                _shootOrigin, _projectileSpeed, _shootDelay, _projectileDamage);

            _blink = new Blink(_movement, transform, _blinkDistance, _blinkCooldown, _startBlinkVFX, _endBlinkVFX,
                _wallBlinkDistance, _wallLayer);
            _blinkCoolDownView.BindAbility(_blink);

            _playerAnimator = new PlayerAnimator(GetComponentInChildren<Animator>());
        }

        private void FirstOpenCheck()
        {
            if (PlayerPrefs.GetInt(PlayerPrefsConstants.FirstTimeOpening, 0) == 0)
            {
                PlayerPrefs.DeleteAll();
                _health = new Health(_healthValue);
                _playerWallet = new PlayerWallet(_startWallet);

                PlayerPrefs.SetInt(PlayerPrefsConstants.FirstTimeOpening, -1);
                PlayerPrefs.SetInt(PlayerPrefsConstants.PlayerHeatlh, _health.Value);
                PlayerPrefs.SetFloat(PlayerPrefsConstants.PlayerMoveSpeed, _speed);
                PlayerPrefs.SetFloat(PlayerPrefsConstants.PlayerAttackSpeed, 1 / _shootDelay);
                PlayerPrefs.SetInt(PlayerPrefsConstants.PlayerDamage, _projectileDamage);
            }
            else
            {
                _playerWallet = new PlayerWallet(new UnitySerializedDictionary<Currency, int>());
                _health = new Health(PlayerPrefs.GetInt(PlayerPrefsConstants.PlayerHeatlh));
                _speed = PlayerPrefs.GetFloat(PlayerPrefsConstants.PlayerMoveSpeed);
                _shootDelay = 1 / PlayerPrefs.GetFloat(PlayerPrefsConstants.PlayerAttackSpeed);
                _projectileDamage = PlayerPrefs.GetInt(PlayerPrefsConstants.PlayerDamage);
            }
        }


        public override void InstallBindings()
        {
            Init();
            Container.Bind<SceneLoader>().FromInstance(FindObjectOfType<SceneLoader>()).AsSingle();

            Container.Bind<IDamageable>().To<Player>().FromInstance(_player).AsSingle();
            Container.Bind<IMovement>().To<PlayerMovement>().FromInstance(_movement).AsSingle();
            Container.Bind<IShooter>().To<PlayerShooter>().FromInstance(_playerShooter).AsSingle();
            Container.Bind<PlayerAnimator>().FromInstance(_playerAnimator).AsSingle();

            Container.Bind<PlayerWallet>().FromInstance(_playerWallet).AsSingle();

            Container.Bind<IHealth>().To<Health>().FromInstance(_health);
            Container.Bind<Health>().WithId("Player").FromInstance(_health);

            Container.Bind<IAbility>().WithId("Blink").To<Blink>().FromInstance(_blink).AsSingle();

            Container.Bind<StopScreen>().WithId("Die Screen").FromInstance(_dieScreen);
            Container.Bind<StopScreen>().WithId("Pause Screen").FromInstance(_pauseScreen);
        }
    }
}