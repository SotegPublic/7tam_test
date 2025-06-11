using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInProgressState : BaseState, IDisposable
{
    private IPlayerInputHandler _inputHandler;
    private IFiguresOnFieldHolder _figuresOnFieldHolder;
    private IFiguresBarController _figuresBarController;
    private IEndGameChecker _endGameChecker;
    private IEndGameUIController _endGameUIController;
    private IResetGameUIController _resetGameUIController;
    private IIcyFiguresSystem _icyFiguresSystem;
    private GameStatusHolder _gameStatusHolder;
    private GameConfig _gameConfig;
    private LayersConfig _layersConfig;
    private Camera _camera;

    private PlayerInput _playerInput;
    private bool _isTouchVisualAwait;

    [Inject]
    public GameInProgressState(IPlayerInputHandler inputHandler, GameConfig gameConfig, Camera camera, IFiguresBarController figuresBarController,
        IEndGameChecker endGameChecker, IEndGameUIController endGameUIController, IResetGameUIController resetGameUIController,
        GameStatusHolder gameStatusHolder, IFiguresOnFieldHolder figuresOnFieldHolder, IIcyFiguresSystem icyFiguresSystem, LayersConfig layersConfig)
    {
        _inputHandler = inputHandler;
        _gameConfig = gameConfig;
        _camera = camera;
        _figuresBarController = figuresBarController;
        _endGameChecker = endGameChecker;
        _endGameUIController = endGameUIController;
        _resetGameUIController = resetGameUIController;
        _gameStatusHolder = gameStatusHolder;
        _figuresOnFieldHolder = figuresOnFieldHolder;
        _icyFiguresSystem = icyFiguresSystem;
        _layersConfig = layersConfig;

        _endGameUIController.OnEndGameButtonClick += EndState;
        _resetGameUIController.OnResetButtonClick += ResetLevel;
    }

    public override void EnterState()
    {
        _playerInput = _inputHandler.PlayerInput;
        _playerInput.Enable();

        _playerInput.Player.Touch.performed += IsOnTouch;

        _resetGameUIController.ChangeResetButtonInteractable(true);
        _gameStatusHolder.IsGameEnd = false;
    }

    private void IsOnTouch(InputAction.CallbackContext context)
    {
        IsOnTouchAsync().Forget();
    }

    private async UniTask IsOnTouchAsync()
    {
        if (_gameStatusHolder.IsGameEnd)
            return;

        if (_isTouchVisualAwait)
            return;

        _isTouchVisualAwait = true;

        var cameraDistance = Mathf.Abs(_camera.transform.position.z);
        var pointerPosition = _playerInput.Player.Touch.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(pointerPosition);

        var hit = Physics2D.GetRayIntersection(ray, cameraDistance * 2, _layersConfig.BaseMask);

        if (hit.collider != null)
        {
            var view = hit.collider.gameObject.GetComponent<FigureView>();
            if (view.IsInBar)
                return;

            await _figuresBarController.AddFigureIntoBar(view);

            _icyFiguresSystem.TryCrackIce();

            if (_endGameChecker.IsGameEnd(out var isWin))
            {
                _endGameUIController.ShowEndGameUI(isWin);
                _gameStatusHolder.IsGameEnd = true;
                _gameStatusHolder.IsGameReseted = false;
            }
        }

        _isTouchVisualAwait = false;
    }

    private void ResetLevel()
    {
        ResetLevelAsync().Forget();
    }

    private async UniTask ResetLevelAsync()
    {
        await UniTask.WaitUntil(() => !_isTouchVisualAwait); // await visual scenario

        _gameStatusHolder.IsGameEnd = true;
        _gameStatusHolder.IsGameReseted = true;
        _gameStatusHolder.CollectionsCountWhenReset = _figuresOnFieldHolder.GetCollectionsCount();

        _resetGameUIController.ChangeResetButtonInteractable(true);
        EndState();
    }

    public override void ExitState()
    {
        _playerInput.Player.Touch.performed -= IsOnTouch;
        _playerInput.Disable();
        _resetGameUIController.ChangeResetButtonInteractable(false);
    }

    public void Dispose()
    {
        _endGameUIController.OnEndGameButtonClick -= EndState;
        _resetGameUIController.OnResetButtonClick -= ResetLevel;
    }
}
