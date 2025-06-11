using UnityEngine;
using Zenject;

public class GameStatesInstaller : MonoInstaller
{
    [SerializeField] private SpawnPointsHolder _spawnPointsHolder;
    [SerializeField] private Camera _mainCamera;

    public override void InstallBindings()
    {
        Container.Bind<IResolver>().To<DIResolver>().AsSingle().NonLazy();
        Container.Bind<IStateFactory>().To<StateFactory>().AsSingle().NonLazy();

        Container.BindInterfacesTo<CurrentGameStateHolder>().AsSingle().NonLazy();
        Container.BindInterfacesTo<GameStateMachine>().AsSingle().NonLazy();

        //bind states
        Container.BindInterfacesAndSelfTo<CalculateFiguresOrderState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WarmUpState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SpawnFiguresOnFieldState>().AsSingle().WithArguments(_spawnPointsHolder).NonLazy();
        Container.BindInterfacesAndSelfTo<GameInProgressState>().AsSingle().WithArguments(_mainCamera).NonLazy();
        Container.BindInterfacesAndSelfTo<ClearState>().AsSingle();
    }
}
