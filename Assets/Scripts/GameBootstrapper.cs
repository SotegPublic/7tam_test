using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    private IGameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(IGameStateMachine stateMachine)
    {
        _gameStateMachine = stateMachine;
    }

    private void Awake()
    {
        if (Screen.orientation != ScreenOrientation.Portrait &&
            Screen.orientation != ScreenOrientation.PortraitUpsideDown)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    private void Start()
    {
        _gameStateMachine.StartGame();
    }
}
