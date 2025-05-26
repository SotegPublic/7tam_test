using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInstaller: MonoInstaller
{
    [SerializeField] private EndGameUIView _endGameView;
    [SerializeField] private Button _resetButton;

    public override void InstallBindings()
    {
        Container.Bind<IEndGameUIView>().To<EndGameUIView>().FromInstance(_endGameView);

        Container.BindInterfacesTo<EndGameUIController>().AsSingle();
        Container.BindInterfacesTo<ResetGameUIController>().AsSingle().WithArguments(_resetButton);
    }
}