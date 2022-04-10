using Sources.Runtime;
using UnityEngine;

public class InputBindings : IUpdatable
{
    private readonly PlayerInput _playerInput;
    private IMovement _movement;

    public InputBindings()
    {
        _playerInput = new PlayerInput();
    }

    public void BindMovement(IMovement movement) => _movement = movement;

    public void Update(float deltaTime)
    {
        var offset = _playerInput.Player.Movement.ReadValue<Vector2>();
        _movement.Move(offset);
    }

    public void OnEnable()
    {
        _playerInput.Enable();;
    }

    public void OnDisable()
    {
        _playerInput.Disable();
    }
}
