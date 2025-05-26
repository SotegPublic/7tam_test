public class EndGameChecker: IEndGameChecker
{
    private IFiguresBarController _figuresBarController;
    private IFiguresOnFieldHolder _figuresOnFieldHolder;

    public EndGameChecker(IFiguresBarController figuresBarController, IFiguresOnFieldHolder figuresOnFieldHolder)
    {
        _figuresBarController = figuresBarController;
        _figuresOnFieldHolder = figuresOnFieldHolder;
    }

    public bool IsGameEnd(out bool isWin)
    {
        if(_figuresBarController.IsNoPlacesInBar())
        {
            isWin = false;
            return true;
        }

        if(_figuresOnFieldHolder.IsNoFiguresOnField())
        {
            isWin = true;
            return true;
        }

        isWin = false;
        return false;
    }
}
