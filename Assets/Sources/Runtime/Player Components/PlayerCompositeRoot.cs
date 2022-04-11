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
        [SerializeField] private float _speed = 1;
        private PlayerMovement _movement;

        private PlayerAnimator _playerAnimator;
        private InputBindings _inputBindings;

        private void Awake()
        {
            Compose();
        }

        private void Compose()
        {
            _movement = new PlayerMovement(GetComponent<Rigidbody2D>(), _speed);
            _inputBindings = new InputBindings();
        
            _playerAnimator = new PlayerAnimator(GetComponent<Animator>());
            _movement.Moved += _playerAnimator.OnMoved;
        
            GetComponent<Player>().Init(_movement);
        }

        private void Start()
        {
            _inputBindings.BindMovement(_movement);
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