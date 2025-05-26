using Zenject;

public class ClearState : BaseState
{
    private IClearableFiguresOnFieldHolder _figuresOnFieldHolder;
    private IClearableFiguresBarController _figuresBarController;
    private IFiguresPool _pool;

    [Inject]
    public ClearState(IClearableFiguresBarController clearableFiguresBarController, IFiguresPool pool, IClearableFiguresOnFieldHolder clearableFiguresOnFieldHolder)
    {
        _figuresBarController = clearableFiguresBarController;
        _pool = pool;
        _figuresOnFieldHolder = clearableFiguresOnFieldHolder;
    }

    public override void EnterState()
    {
        var collections = _figuresOnFieldHolder.GetCollectionsForClear();

        for (int i = collections.Count - 1; i >= 0; i--)
        {
            var collection = collections[i];

            for (int j = 0; j < collection.FigureViews.Count; j++)
            {
                collection.FigureViews[j].Clear();
                _pool.RemoveViewToPool(collection.FigureViews[j]);
            }

            _figuresOnFieldHolder.RemoveFiguresCollection(collection);
        }

        _figuresBarController.ClearBar();

        EndState();
    }

    public override void ExitState()
    {
        
    }
}