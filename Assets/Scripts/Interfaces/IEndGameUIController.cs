using System;

public interface IEndGameUIController
{
    public Action OnEndGameButtonClick { get; set; }
    public void ShowEndGameUI(bool isWin);
}
