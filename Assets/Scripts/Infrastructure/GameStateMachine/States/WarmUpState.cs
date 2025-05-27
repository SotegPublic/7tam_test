using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class WarmUpState : BaseState
{
    private GameConfig _gameConfig;
    private GameStatusHolder _gameStatus;
    private FiguresTypesArrayHolder _figuresTypesArrayHolder;
    private IFiguresOnFieldHolder _figuresOnFieldHolder;
    private IFiguresPool _figuresPool;

    private List<UniTask> tasks;

    public WarmUpState(GameConfig gameConfig, IFiguresPool figuresPool, FiguresTypesArrayHolder figuresTypesArrayHolder, GameStatusHolder gameStatus,
     IFiguresOnFieldHolder figuresOnFieldHolder)
    {
        _gameConfig = gameConfig;
        _figuresPool = figuresPool;
        _figuresTypesArrayHolder = figuresTypesArrayHolder;
        _gameStatus = gameStatus;
        _figuresOnFieldHolder = figuresOnFieldHolder;

        tasks = new List<UniTask>(_gameConfig.FiguresTypesCountOnField);
    }

    public override void EnterState()
    {
        EnterStateAsync().Forget();
    }

    public async UniTask EnterStateAsync()
    {
        var types = _figuresTypesArrayHolder.FiguresTypesArray;

        if (types.Length < _gameConfig.FiguresTypesCountOnField)
            throw new Exception("gameConfig.FiguresTypesCountOnField value more than we have types count");

        var warmUpCollectionsCount = _gameStatus.IsGameReseted ? _gameStatus.CollectionsCountWhenReset : _gameConfig.FiguresTypesCountOnField;

        for (int i = 0; i < warmUpCollectionsCount; i++)
        {
            int warmUpCount = GetWarmUpCount(types[i]);

            tasks.Add(_figuresPool.WarmUpFigures(types[i], warmUpCount));
        }

        await UniTask.WhenAll(tasks);
        EndState();
    }

    private int GetWarmUpCount(FiguresTypes type)
    {
        var warmUpCount = _gameConfig.FiguresCollectionLenth;

        if (_gameStatus.IsGameReseted)
        {
            var collection = _figuresOnFieldHolder.GetCollectionModelByType(type);
            warmUpCount = collection == null ? warmUpCount : _gameConfig.FiguresCollectionLenth - collection.GetInBarCount();
        }

        return warmUpCount;
    }

    public override void ExitState()
    {
        tasks.Clear();
    }
}
