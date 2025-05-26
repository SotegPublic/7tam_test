using UnityEngine.UI;

public interface IEndGameUIView
{
    public Button CloseEndGameUI { get; }
    public void ShowUI(bool isWin);
    public void HideUI();
}
