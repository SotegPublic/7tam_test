using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFiguresOnFieldState : BaseState
{
    private SpawnPointsHolder _spawnPointsHolder;
    private GameConfig _gameConfig;
    private SpawnConfig _spawnConfig;
    private GameStatusHolder _gameStatus;
    private IAddebleModelFiguresHolder _figuresOnFieldHolder;
    private FiguresTypesArrayHolder _figuresTypesArrayHolder;
    private IFiguresPool _pool;

    private List<FigureView> spawnOrder;

    public SpawnFiguresOnFieldState(SpawnPointsHolder spawnPointsHolder, IAddebleModelFiguresHolder figuresOnFieldHolder, GameConfig gameConfig, IFiguresPool pool,
        FiguresTypesArrayHolder figuresTypesArrayHolder, SpawnConfig spawnConfig, GameStatusHolder gameStatus)
    {
        _spawnPointsHolder = spawnPointsHolder;
        _figuresOnFieldHolder = figuresOnFieldHolder;
        _gameConfig = gameConfig;
        _pool = pool;
        _figuresTypesArrayHolder = figuresTypesArrayHolder;
        _spawnConfig = spawnConfig;

        spawnOrder = new List<FigureView>(gameConfig.FiguresCollectionLenth * gameConfig.FiguresTypesCountOnField);
        _gameStatus = gameStatus;
    }

    public override void EnterState()
    {
        EnterStateASync().Forget();
    }

    private async UniTask EnterStateASync()
    {
        var countTypes = _gameStatus.IsGameReseted ? _gameStatus.CollectionsCountWhenReset : _gameConfig.FiguresTypesCountOnField;

        for (int i = 0; i < countTypes; i++)
        {
            int spawnFiguresCount = GetSpawnFiguresCount(_figuresTypesArrayHolder.FiguresTypesArray[i]);

            for (int j = 0; j < spawnFiguresCount; j++)
            {
                var view = await _pool.GetFigureFromPool(_figuresTypesArrayHolder.FiguresTypesArray[i]);
                spawnOrder.Add(view);
            }
        }

        Shuffler.ShuffleList(spawnOrder);

        await SpawnFigures();

        spawnOrder.Clear();

        EndState();
    }

    private int GetSpawnFiguresCount(FiguresTypes figureType)
    {
        var spawnFiguresCount = _gameConfig.FiguresCollectionLenth;

        if (_gameStatus.IsGameReseted)
        {
            var exsitCollection = _figuresOnFieldHolder.GetCollectionModelByType(figureType);
            spawnFiguresCount = exsitCollection == null ? spawnFiguresCount : spawnFiguresCount - exsitCollection.GetInBarCount();
        }

        return spawnFiguresCount;
    }

    private async UniTask SpawnFigures()
    {
        for(int i = 0; i < spawnOrder.Count; i++)
        {
            var view = spawnOrder[i];
            var spawnPointIndex = i % _spawnPointsHolder.SpawnPoints.Length;
            view.transform.position = _spawnPointsHolder.SpawnPoints[spawnPointIndex].position;
            var rb = view.RigidBody2D;
            rb.isKinematic = false;
            view.Collider2D.enabled = true;

            rb.AddForce(Vector2.down * _spawnConfig.DownwardForce, ForceMode2D.Force);

            _figuresOnFieldHolder.AddModel(view.Type, view);

            await UniTask.Delay(_spawnConfig.SpawnDelay);
        }
    }

    public override void ExitState()
    {
    }
}
