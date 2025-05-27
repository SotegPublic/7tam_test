using System;
using System.Collections.Generic;
using System.Linq;

public class CalculateFiguresOrderState : BaseState
{
    private GameStatusHolder _gameStatus;
    private FiguresTypesArrayHolder _figuresTypesArrayHolder;
    private IFiguresOnFieldHolder _figuresOnFieldHolder;

    public CalculateFiguresOrderState(FiguresTypesArrayHolder figuresTypesArrayHolder, GameStatusHolder gameStatus, IFiguresOnFieldHolder figuresOnFieldHolder)
    {
        _figuresTypesArrayHolder = figuresTypesArrayHolder;
        _gameStatus = gameStatus;
        _figuresOnFieldHolder = figuresOnFieldHolder;
    }

    public override void EnterState()
    {
        _figuresTypesArrayHolder.FiguresTypesArray = Enum.GetValues(typeof(FiguresTypes)).Cast<FiguresTypes>().Where(type => type != FiguresTypes.None).ToArray();

        Shuffler.ShuffleArray(_figuresTypesArrayHolder.FiguresTypesArray);

        if(_gameStatus.IsGameReseted)
        {
            ChangeOrder();
        }

        EndState();
    }

    private void ChangeOrder()
    {
        var types = _figuresTypesArrayHolder.FiguresTypesArray;
        var newOrder = new FiguresTypes[types.Length];
        var typesInCollections = _figuresOnFieldHolder.GetCollectionsTypes();
        
        Array.Copy(typesInCollections, newOrder, typesInCollections.Length);
        var currentFreeIndex = typesInCollections.Length;

        var addedTypes = new HashSet<FiguresTypes>(typesInCollections);

        for (int i = 0; i < types.Length; i++)
        {
            if (!addedTypes.Contains(types[i]))
            {
                newOrder[currentFreeIndex] = types[i];
                currentFreeIndex++;
            }
        }

        _figuresTypesArrayHolder.FiguresTypesArray = newOrder;
    }

    public override void ExitState()
    {
    }
}
