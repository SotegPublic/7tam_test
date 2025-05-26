using System;
using Zenject;

public class InputHandler : IDisposable, IPlayerInputHandler
{
    private PlayerInput _playerInput;

    public PlayerInput PlayerInput => _playerInput;

    [Inject]
    public void Construct()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
    }

    public void Dispose()
    {
        _playerInput.Disable();
    }
}
