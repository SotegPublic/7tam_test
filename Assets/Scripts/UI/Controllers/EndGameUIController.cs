using System;
using Zenject;

public class EndGameUIController: IEndGameUIController, IDisposable
{
    public Action OnEndGameButtonClick { get; set; }

    private IEndGameUIView _view;

    [Inject]
    public EndGameUIController(IEndGameUIView view)
    {
        _view = view;

        _view.CloseEndGameUI.onClick.AddListener(EndButtonClick);
    }

    private void EndButtonClick()
    {
        OnEndGameButtonClick?.Invoke();
        _view.HideUI();
    }

    public void ShowEndGameUI(bool isWin)
    {
        _view.ShowUI(isWin);
    }

    public void Dispose()
    {
        _view.CloseEndGameUI.onClick.RemoveListener(EndButtonClick);
    }
}
