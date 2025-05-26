using System;

public interface IResetGameUIController
{
    public Action OnResetButtonClick { get; set; }
    public void ChangeResetButtonInteractable(bool isInteractable);
}
