using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FiguresBarController: IFiguresBarController, IClearableFiguresBarController
{
    private GameConfig _gameConfig;
    private IFiguresPool _pool;
    private IFiguresOnFieldHolder _figuresOnFieldHolder;
    private IFiguresBarVisualController _visualController;

    private FigureView[] _viewsInBar;
    private Transform[] _barElementsTransforms;

    public FiguresBarController(GameConfig gameConfig, FiguresBarConfig figuresBarConfig, Transform[] barElementsTransforms, IFiguresPool pool,
        IFiguresOnFieldHolder figuresOnFieldHolder, IFiguresBarVisualController visualController)
    {
        _gameConfig = gameConfig;
        _barElementsTransforms = barElementsTransforms;
        _pool = pool;
        _figuresOnFieldHolder = figuresOnFieldHolder;
        _visualController = visualController;

        _viewsInBar = new FigureView[gameConfig.FiguresBarLenth];
    }

    public async UniTask AddFigureIntoBar(FigureView figure)
    {
        for(int i = 0; i < _viewsInBar.Length; i++)
        {
            if (_viewsInBar[i] == null)
            {
                _viewsInBar[i] = figure;

                figure.RigidBody2D.isKinematic = true;
                figure.RigidBody2D.velocity = Vector2.zero;
                figure.RigidBody2D.angularVelocity = 0;

                figure.Collider2D.enabled = false;

                var targetPosition = _barElementsTransforms[i].position;
                var isArcLeft = i < (_gameConfig.FiguresBarLenth * 0.5f) ? false : true;
                await _visualController.DrowFlyToBar(figure, targetPosition, isArcLeft);

                figure.SetInBar();

                await CheckCollection(figure.Type);

                break;
            }
        }
    }

    public bool IsNoPlacesInBar()
    {
        for(int i = 0; i < _viewsInBar.Length; i++)
        {
            if (_viewsInBar[i] == null)
                return false;
        }

        return true;
    }

    void IClearableFiguresBarController.ClearBar()
    {
        for(int i = 0; i < _viewsInBar.Length; i++)
        {
            _viewsInBar[i] = null;
        }
    }

    private async UniTask CheckCollection(FiguresTypes type)
    {
        var collection = _figuresOnFieldHolder.GetCollectionByType(type);
        var inBarCount = collection.GetInBarCount();
        
        if(inBarCount == _gameConfig.FiguresCollectionLenth)
        {
            await _visualController.ScaleCollectionViews(collection);
            ClearViewsFromBar(collection);
        }
    }


    private void ClearViewsFromBar(FiguresCollectionModel collection)
    {
        for (int i = 0; i < collection.FigureViews.Count; i++)
        {
            var view = collection.FigureViews[i];
            DeleteViewFromBar(view);
            view.Clear();
            _pool.RemoveViewToPool(view);
        }

        _figuresOnFieldHolder.RemoveFiguresCollection(collection);
    }

    private void DeleteViewFromBar(FigureView view)
    {
        for(int i = 0; i < _viewsInBar.Length; i++)
        {
            if (_viewsInBar[i] == view)
            {
                _viewsInBar[i] = null;
                break;
            }
        }
    }
}
