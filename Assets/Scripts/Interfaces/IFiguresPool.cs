using Cysharp.Threading.Tasks;

public interface IFiguresPool
{
    public UniTask<FigureView> GetFigureFromPool(FiguresTypes type);
    public UniTask WarmUpFigures(FiguresTypes type, int count);
    public void RemoveViewToPool(FigureView view);
}
