using DefaultNamespace;
using Sources.Runtime.Input;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    [RequireComponent(typeof(Rigidbody2D),
        typeof(Animator),
        typeof(Player))]
    public class PlayerCompositeRoot : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        private float _speed = 1;
        private PlayerMovement _movement;

        [Header("Shooting")]
        [SerializeField]
        private ProjectileFactory _projectileFactory;
        [SerializeField]
        private float _projectileSpeed = 1;
        [SerializeField]
        private float _shootDelay = 1;
        [SerializeField]
        private Transform _shootOrigin;
        private PlayerShooter _playerShooter;

        private PlayerAnimator _playerAnimator;
        private InputBindings _inputBindings;

        private void Awake()
        {
            Compose();
        }

        private void Compose()
        {
            _movement = new PlayerMovement(GetComponent<Rigidbody2D>(), _speed);
            _playerShooter = new PlayerShooter(new ObjectPool<Projectile>(100, _projectileFactory), 
                _shootOrigin, _projectileSpeed, _shootDelay, this);
            _inputBindings = new InputBindings();

            _playerAnimator = new PlayerAnimator(GetComponent<Animator>());
            _movement.Moved += _playerAnimator.OnMoved;

            GetComponent<Player>().Init(_movement, _playerShooter);
        }

        private void Start()
        {
            BindInput();
        }

        private void BindInput()
        {
            _inputBindings.BindMovement(_movement);
            _inputBindings.BindShooting(_playerShooter);
        }

        private void OnEnable()
        {
            _inputBindings.OnEnable();
        }

        private void OnDisable()
        {
            _inputBindings.OnDisable();
        }

        private void FixedUpdate()
        {
            _inputBindings.Update(Time.deltaTime);
        }
    }
}