using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

public class CalculateFiguresOrderState : BaseState
{
    private GameConfig _gameConfig;
    private GameStatusHolder _gameStatus;
    private FiguresTypesArrayHolder _figuresTypesArrayHolder;
    private IFiguresPool _figuresPool;

    private List<UniTask> tasks;

    public CalculateFiguresOrderState(GameConfig gameConfig, IFiguresPool figuresPool, FiguresTypesArrayHolder figuresTypesArrayHolder, GameStatusHolder gameStatus)
    {
        _gameConfig = gameConfig;
        _figuresPool = figuresPool;
        _figuresTypesArrayHolder = figuresTypesArrayHolder;
        _gameStatus = gameStatus;

        tasks = new List<UniTask>(_gameConfig.FiguresTypesCountOnField);
    }

    public async override void EnterState()
    {
        tasks.Clear();

        _figuresTypesArrayHolder.FiguresTypesArray = Enum.GetValues(typeof(FiguresTypes)).Cast<FiguresTypes>().Where(type => type != FiguresTypes.None).ToArray();
        var types = _figuresTypesArrayHolder.FiguresTypesArray;

        Shuffler.ShuffleArray(types);

        if (types.Length < _gameConfig.FiguresTypesCountOnField)
            throw new Exception("gameConfig.FiguresTypesCountOnField value more than we have types count");

        var warmUpCollectionsCount = _gameStatus.IsGameReseted ? _gameStatus.CollectionsCountWhenReset : _gameConfig.FiguresTypesCountOnField;

        for (int i = 0; i < warmUpCollectionsCount; i++)
        {
            tasks.Add(_figuresPool.WarmUpFigures(types[i], _gameConfig.FiguresCollectionLenth));
        }

        await UniTask.WhenAll(tasks);

        EndState();
    }

    public override void ExitState()
    {
        tasks.Clear();
    }
}
