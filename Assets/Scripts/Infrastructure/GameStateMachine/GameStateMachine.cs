using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class GameStateMachine : IGameStateMachine, IDisposable
{
    private IChangableGameStateHolder _gameStateHolder;
    private IStateFactory _statesFactory;

    private Dictionary<Type, IGameState> _gameStates = new Dictionary<Type, IGameState>(8);

    [Inject]
    public GameStateMachine(IChangableGameStateHolder stateHolder, IStateFactory gamseStatesFactory)
    {
        _gameStateHolder = stateHolder;
        _statesFactory = gamseStatesFactory;

        _gameStates.Add(typeof(CalculateFiguresOrderState), _statesFactory.CreateState<CalculateFiguresOrderState>());
        _gameStates.Add(typeof(WarmUpState), _statesFactory.CreateState<WarmUpState>());
        _gameStates.Add(typeof(SpawnFiguresOnFieldState), _statesFactory.CreateState<SpawnFiguresOnFieldState>());
        _gameStates.Add(typeof(GameInProgressState), _statesFactory.CreateState<GameInProgressState>());
        _gameStates.Add(typeof(ClearState), _statesFactory.CreateState<ClearState>());
    }

    [Inject]
    public void Init()
    {
        foreach(var state in _gameStates.Values)
        {
            state.OnStateEnd += EndCurrentState;
        }
    }

    public void EndCurrentState(Type currentStateType)
    {
        if (_gameStateHolder.GetCurrentGameState() != currentStateType)
            return;

        _gameStates[currentStateType].ExitState();

        switch (currentStateType)
        {
            case var _ when currentStateType == typeof(CalculateFiguresOrderState):
                _gameStateHolder.ChangeCurrentGameState(typeof(WarmUpState));
                _gameStates[typeof(WarmUpState)].EnterState();
                break;
            case var _ when currentStateType == typeof(WarmUpState):
                _gameStateHolder.ChangeCurrentGameState(typeof(SpawnFiguresOnFieldState));
                _gameStates[typeof(SpawnFiguresOnFieldState)].EnterState();
                break;
            case var _ when currentStateType == typeof(SpawnFiguresOnFieldState):
                _gameStateHolder.ChangeCurrentGameState(typeof(GameInProgressState));
                _gameStates[typeof(GameInProgressState)].EnterState();
                break;
            case var _ when currentStateType == typeof(GameInProgressState):
                _gameStateHolder.ChangeCurrentGameState(typeof(ClearState));
                _gameStates[typeof(ClearState)].EnterState();
                break;
            case var _ when currentStateType == typeof(ClearState):
                _gameStateHolder.ChangeCurrentGameState(typeof(CalculateFiguresOrderState));
                _gameStates[typeof(CalculateFiguresOrderState)].EnterState();
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        _gameStateHolder.ChangeCurrentGameState(typeof(CalculateFiguresOrderState));
        _gameStates[typeof(CalculateFiguresOrderState)].EnterState();
    }

    public void Dispose()
    {
        foreach (var state in _gameStates.Values)
        {
            state.OnStateEnd -= EndCurrentState;
        }
    }
}
