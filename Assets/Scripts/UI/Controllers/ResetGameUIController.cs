using System;
using UnityEngine.UI;
using Zenject;

public class ResetGameUIController: IResetGameUIController, IDisposable
{
    public Action OnResetButtonClick { get; set; }

    private Button _resetButton;

    [Inject]
    public ResetGameUIController(Button resetButton)
    {
        _resetButton = resetButton;

        _resetButton.onClick.AddListener(ResetButtonClick);
    }

    private void ResetButtonClick()
    {
        OnResetButtonClick?.Invoke();
        ChangeResetButtonInteractable(false);
    }

    public void ChangeResetButtonInteractable(bool isInteractable)
    {
        _resetButton.interactable = isInteractable;
    }

    public void Dispose()
    {
        _resetButton.onClick.RemoveListener(ResetButtonClick);
    }
}
