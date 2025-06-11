using System;
using Zenject;

public class ClearState : BaseState
{
    private IClearableFiguresOnFieldHolder _figuresOnFieldHolder;
    private IClearableFiguresBarController _figuresBarController;
    private IChangableIcyFiguresSystem _icyFiguresSystem;
    private IFiguresPool _pool;
    private GameStatusHolder _statusHolder;

    [Inject]
    public ClearState(IClearableFiguresBarController clearableFiguresBarController, IFiguresPool pool, IClearableFiguresOnFieldHolder clearableFiguresOnFieldHolder,
        GameStatusHolder gameStatusHolder, IChangableIcyFiguresSystem icyFiguresSystem)
    {
        _figuresBarController = clearableFiguresBarController;
        _pool = pool;
        _figuresOnFieldHolder = clearableFiguresOnFieldHolder;
        _statusHolder = gameStatusHolder;
        _icyFiguresSystem = icyFiguresSystem;
    }

    public override void EnterState()
    {
        if(_statusHolder.IsGameReseted)
        {
            FieldClear();
        }
        else
        {
            FullClear();
        }

        EndState();
    }

    private void FieldClear()
    {
        var collections = _figuresOnFieldHolder.GetCollectionsForClear();

        for (int i = collections.Count - 1; i >= 0; i--)
        {
            var collection = collections[i];

            for (int j = 0; j < collection.FigureViews.Count; j++)
            {
                if (!collection.FigureViews[j].IsInBar)
                {
                    _pool.RemoveViewToPool(collection.FigureViews[j]);
                }
            }

            if(collection.GetInBarCount() == 0)
            {
                _figuresOnFieldHolder.RemoveFiguresCollection(collection);
            }
            else
            {
                collection.RemoveNotInBarViews();
            }
        }

        _icyFiguresSystem.ClearCurrentCollection();
    }

    private void FullClear()
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
        _icyFiguresSystem.FullClear();
    }

    public override void ExitState()
    {
        
    }
}