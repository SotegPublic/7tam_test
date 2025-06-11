using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class MainGameInstaller : MonoInstaller
{
    [SerializeField] private GameBootstrapper _bootstrapper;
    [SerializeField] private Transform _poolTransform;
    [SerializeField] private FiguresConfigsHolder _figuresConfigsHolder;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private FigureModifiersConfig _modifiersConfig;
    [SerializeField] private FiguresBarConfig _barConfig;
    [SerializeField] private SpawnConfig _spawnConfig;
    [SerializeField] private LayersConfig _layersConfig;
    [SerializeField] private IceAnimationConfig _iceAnimationConfig;
    [SerializeField] private Transform[] _barElementsTransforms;

    public override void InstallBindings()
    {
        Container.Bind<GameBootstrapper>().FromInstance(_bootstrapper).AsSingle();
        InstallConfigs();

        Container.Bind<IPlayerInputHandler>().To<InputHandler>().AsSingle();
        Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();
        Container.Bind<IEndGameChecker>().To<EndGameChecker>().AsSingle();
        Container.Bind<GameStatusHolder>().AsSingle();

        InstallFiguresSystemsBindings();
        InstallPoolBindings();
    }

    private void InstallConfigs()
    {
        Container.BindInstance(_gameConfig).AsSingle();
        Container.BindInstance(_modifiersConfig).AsSingle();
        Container.BindInstance(_barConfig).AsSingle();
        Container.BindInstance(_spawnConfig).AsSingle();
        Container.BindInstance(_layersConfig).AsSingle();
        Container.BindInstance(_iceAnimationConfig).AsSingle();
    }

    private void InstallFiguresSystemsBindings()
    {
        Container.BindInterfacesTo<FiguresBarController>().AsSingle().WithArguments(_barElementsTransforms);
        Container.Bind<FiguresTypesArrayHolder>().AsSingle();
        Container.BindInterfacesTo<FiguresOnFieldHolder>().AsSingle();
        Container.Bind<IFiguresBarVisualController>().To<FiguresBarVisualController>().AsSingle();
        Container.BindInterfacesTo<IcyFiguresSystem>().AsSingle();

    }

    private void InstallPoolBindings()
    {
        Container.BindInterfacesTo<FigureCreator>().AsSingle().WithArguments(_figuresConfigsHolder);
        Container.Bind<IFiguresPool>().To<FiguresPool>().AsSingle().WithArguments(_poolTransform);
    }
}
