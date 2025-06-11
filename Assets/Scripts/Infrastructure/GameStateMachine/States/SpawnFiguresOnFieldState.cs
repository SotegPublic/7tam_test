using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class SpawnFiguresOnFieldState : BaseState
{
    private SpawnPointsHolder _spawnPointsHolder;
    private GameConfig _gameConfig;
    private SpawnConfig _spawnConfig;
    private GameStatusHolder _gameStatus;
    private IAddebleModelFiguresHolder _figuresOnFieldHolder;
    private IChangableIcyFiguresSystem _icyFiguresSystem;
    private FiguresTypesArrayHolder _figuresTypesArrayHolder;
    private IFiguresPool _pool;
    private FigureModifiersConfig _modifiersConfig;

    private List<FigureView> _spawnOrder;

    private int _countHeavyFigures;
    private int _countStickyFigures;
    private int _countIcyFigures;

    [Inject]
    public SpawnFiguresOnFieldState(SpawnPointsHolder spawnPointsHolder, IAddebleModelFiguresHolder figuresOnFieldHolder, GameConfig gameConfig, IFiguresPool pool,
        FiguresTypesArrayHolder figuresTypesArrayHolder, SpawnConfig spawnConfig, GameStatusHolder gameStatus,
        FigureModifiersConfig modifiersConfig, IChangableIcyFiguresSystem icyFiguresSystem)
    {
        _spawnPointsHolder = spawnPointsHolder;
        _figuresOnFieldHolder = figuresOnFieldHolder;
        _gameConfig = gameConfig;
        _pool = pool;
        _figuresTypesArrayHolder = figuresTypesArrayHolder;
        _spawnConfig = spawnConfig;
        _modifiersConfig = modifiersConfig;
        _icyFiguresSystem = icyFiguresSystem;

        _spawnOrder = new List<FigureView>(gameConfig.FiguresCollectionLenth * gameConfig.FiguresTypesCountOnField);
        _gameStatus = gameStatus;
    }

    public override void EnterState()
    {
        CheckIcyFiguresWhenReset();
        EnterStateASync().Forget();
    }

    private void CheckIcyFiguresWhenReset()
    {
        if (!_gameStatus.IsGameReseted)
            return;

        _countIcyFigures = _icyFiguresSystem.CurrentDeletedFigures == _modifiersConfig.FiguresCountDeleteForIceBreak ? _modifiersConfig.MaxCountIcyFigures : 0;
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
                _spawnOrder.Add(view);
            }
        }

        Shuffler.ShuffleList(_spawnOrder);

        await SpawnFigures();

        _spawnOrder.Clear();

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
        for(int i = 0; i < _spawnOrder.Count; i++)
        {
            var view = _spawnOrder[i];

            TryModifingView(view);

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

    private void TryModifingView(FigureView view)
    {
        if(_countHeavyFigures >= _modifiersConfig.MaxCountHeavyFigures &&
           _countStickyFigures >= _modifiersConfig.MaxCountHeavyFigures &&
           _countIcyFigures >= _modifiersConfig.MaxCountHeavyFigures)
        {
            return;
        }
        
        var heavyRoll = _countHeavyFigures < _modifiersConfig.MaxCountHeavyFigures ? Random.Range(0, 101) : 101;
        var stickyRoll = _countStickyFigures < _modifiersConfig.MaxCountStickyFigures ? Random.Range(0, 101) : 101;
        var icyRoll = _countIcyFigures < _modifiersConfig.MaxCountIcyFigures ? Random.Range(0, 101) : 101;

        var droppedModifiers = 0;

        droppedModifiers += heavyRoll <= _modifiersConfig.ChanceToCreateHeavyFigure ? 1 : 0;
        droppedModifiers += stickyRoll <= _modifiersConfig.ChanceToCreateStickyFigure ? 1 : 0;
        droppedModifiers += icyRoll <= _modifiersConfig.ChanceToCreateIcyFigure ? 1 : 0;

        var randomPick = Random.Range(1, droppedModifiers + 1);

        if (heavyRoll < _modifiersConfig.ChanceToCreateHeavyFigure && --randomPick == 0)
        {
            view.RigidBody2D.mass = _modifiersConfig.Weight;
            _countHeavyFigures++;
            return;
        }

        if (stickyRoll < _modifiersConfig.ChanceToCreateStickyFigure && --randomPick == 0)
        {
            view.RigidBody2D.sharedMaterial = _modifiersConfig.StickyMaterial;
            _countStickyFigures++;
            return;
        }

        if (icyRoll < _modifiersConfig.ChanceToCreateIcyFigure)
        {
            view.IceIt();
            _icyFiguresSystem.AddIcyFigure(view);
            _countIcyFigures++;
        }
    }

    public override void ExitState()
    {
        _countIcyFigures = 0;
        _countStickyFigures = 0;
        _countHeavyFigures = 0;
    }
}
