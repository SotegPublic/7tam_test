using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FiguresPool : IFiguresPool
{
    private IFigureCreator _figureCreator;
    private Transform _poolTransform;

    private Dictionary<FiguresTypes, List<FigureView>> _viewsPool = new Dictionary<FiguresTypes, List<FigureView>>(24);

    [Inject]
    public FiguresPool (IFigureCreator figureCreator, Transform poolTransform)
    {
        _figureCreator = figureCreator;
        _poolTransform = poolTransform;
    }

    private async UniTask<FigureView> CreateFigure(FiguresTypes type)
    {
        return await _figureCreator.CreateFigure(_poolTransform.position, type);
    }

    public async UniTask<FigureView> GetFigureFromPool(FiguresTypes type)
    {
        if(!_viewsPool.ContainsKey(type))
            _viewsPool.Add(type, new List<FigureView>());
        
        if(_viewsPool[type].Count == 0)
        {
            return await CreateFigure(type);
        }
        else
        {
            var figure = _viewsPool[type][0];
            _viewsPool[type].RemoveAt(0);

            return figure;
        }
    }

    public async UniTask WarmUpFigures(FiguresTypes type, int count)
    {
        if (!_viewsPool.ContainsKey(type))
        {
            _viewsPool.Add(type, new List<FigureView>(count));
        }
        else
        {
            var inPoolCount = _viewsPool[type].Count;
            count = count - inPoolCount < 0 ? 0 : count - inPoolCount;

            if (count == 0)
                return;
        }

        var tasks = System.Buffers.ArrayPool<UniTask<FigureView>>.Shared.Rent(count);

        for (int i = 0; i < count; i++)
        {
            tasks[i] = CreateFigure(type);
        }

        var views = await UniTask.WhenAll(tasks);

        for (int i = 0; i < count; i++)
        {
            _viewsPool[type].Add(views[i]);
        }

        System.Buffers.ArrayPool<UniTask<FigureView>>.Shared.Return(tasks);
    }

    public void RemoveViewToPool(FigureView view)
    {
        view.RigidBody2D.isKinematic = true;
        view.RigidBody2D.velocity = Vector2.zero;
        view.RigidBody2D.angularVelocity = 0;

        view.Collider2D.enabled = false;
        view.transform.rotation = Quaternion.identity;
        view.transform.localScale = Vector3.one;
        view.transform.position = _poolTransform.position;

        _viewsPool[view.Type].Add(view);
    }
}
