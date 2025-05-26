using Cysharp.Threading.Tasks;

public interface IFiguresBarController
{
    public UniTask AddFigureIntoBar(FigureView figure);
    public bool IsNoPlacesInBar();
}