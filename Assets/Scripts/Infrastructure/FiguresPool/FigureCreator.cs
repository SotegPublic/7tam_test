using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class FigureCreator : IFigureCreator, IDisposable
{
    private IGameObjectFactory _gameObjectFactory;
    private FiguresConfigsHolder _figuresConfigs;

    private Dictionary<FiguresTypes, FigureHandlers> figureHandlers = new Dictionary<FiguresTypes, FigureHandlers>(16);

    [Inject]
    public FigureCreator(IGameObjectFactory gameObjectFactory, FiguresConfigsHolder figuresConfigs)
    {
        _gameObjectFactory = gameObjectFactory;
        _figuresConfigs = figuresConfigs;
    }

    public async UniTask<FigureView> CreateFigure(Vector2 position, FiguresTypes type)
    {
        var config = _figuresConfigs.GetConfig(type);

        if (config == null)
            throw new Exception("wrong Figure type");

        if(!figureHandlers.ContainsKey(type))
        {
            var viewHandle = Addressables.LoadAssetAsync<GameObject>(config.ViewReference);
            var innerFigureHandle = Addressables.LoadAssetAsync<GameObject>(config.OuterFigureReference);
            var animal = Addressables.LoadAssetAsync<GameObject>(config.AnimalrReference);

            figureHandlers.Add(type, new FigureHandlers
            {
                ViewHandle = viewHandle,
                InnerFigureHandle = innerFigureHandle,
                AnimalHandle = animal
            });
        }

        var viewTask = GetGameObject(figureHandlers[type].ViewHandle, position);
        var innerFigureTask = GetGameObject(figureHandlers[type].InnerFigureHandle, position);
        var animalTask = GetGameObject(figureHandlers[type].AnimalHandle, position);

        (GameObject viewObject, GameObject innerObject, GameObject animalObject) = await UniTask.WhenAll(viewTask, innerFigureTask, animalTask);

        var figureView = viewObject.GetComponent<FigureView>();
        figureView.SpriteRenderer.color = config.LineColor;

        animalObject.transform.SetParent(innerObject.transform);

        innerObject.transform.SetParent(figureView.ParentTransform);
        innerObject.transform.localPosition = Vector2.zero;

        figureView.Collider2D.enabled = false;
        figureView.RigidBody2D.isKinematic = true;
        figureView.Type = type;

        return figureView;
    }

    public void Dispose()
    {
        foreach(var handlers in figureHandlers.Values)
        {
            handlers.ViewHandle.Release();
            handlers.InnerFigureHandle.Release();
            handlers.AnimalHandle.Release();
        }
    }

    private async UniTask<GameObject> GetGameObject(AsyncOperationHandle<GameObject> handler, Vector2 position)
    {
        if (handler.Status == AsyncOperationStatus.None)
            await handler.Task;

        if (handler.Status != AsyncOperationStatus.Succeeded)
            throw new Exception("Error on tile prefab load");

        return _gameObjectFactory.Create(handler.Result, position, Quaternion.identity, null);
    }
}
