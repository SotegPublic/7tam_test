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
    [SerializeField] private FiguresBarConfig _barConfig;
    [SerializeField] private Transform[] _barElementsTransforms;

    public override void InstallBindings()
    {
        Container.Bind<GameBootstrapper>().FromInstance(_bootstrapper).AsSingle();

        Container.BindInterfacesTo<InputHandler>().AsSingle();
        Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();
        Container.Bind<IEndGameChecker>().To<EndGameChecker>().AsSingle();
        Container.Bind<GameStatusHolder>().AsSingle();

        InstallFiguresSystemsBindings();
        InstallPoolBindings();
    }

    private void InstallFiguresSystemsBindings()
    {
        Container.BindInterfacesTo<FiguresBarController>().AsSingle().WithArguments(_gameConfig, _barElementsTransforms, _barConfig);
        Container.Bind<FiguresTypesArrayHolder>().AsSingle();
        Container.BindInterfacesTo<FiguresOnFieldHolder>().AsSingle().WithArguments(_gameConfig);
        Container.Bind<IFiguresBarVisualController>().To<FiguresBarVisualController>().AsSingle().WithArguments(_barConfig);

    }

    private void InstallPoolBindings()
    {
        Container.BindInterfacesTo<FigureCreator>().AsSingle().WithArguments(_figuresConfigsHolder);
        Container.Bind<IFiguresPool>().To<FiguresPool>().AsSingle().WithArguments(_poolTransform);
    }
}
