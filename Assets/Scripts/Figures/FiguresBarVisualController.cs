using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FiguresBarVisualController: IFiguresBarVisualController
{
    private FiguresBarConfig _figuresBarConfig;

    public FiguresBarVisualController(FiguresBarConfig figuresBarConfig)
    {
        _figuresBarConfig = figuresBarConfig;
    }

    public async UniTask DrowFlyToBar(FigureView figure, Vector3 targetPosition, bool isArcLeft)
    {
        var duration = _figuresBarConfig.FlyTimeToBar;
        var arcHeight = isArcLeft ? _figuresBarConfig.ArcHeight : _figuresBarConfig.ArcHeight * -1;

        var startPos = figure.transform.position;
        var endPos = targetPosition;
        var dir = (endPos - startPos).normalized;
        var arcDir = new Vector3(-dir.y, dir.x, dir.z);

        var controlPoint = (startPos + endPos) * 0.5f + (arcDir * arcHeight);
        var path = new Vector3[] { startPos, controlPoint, endPos };

        var sequence = DOTween.Sequence().Append(figure.transform.DOPath(path, duration, PathType.CatmullRom)).SetEase(Ease.InOutQuad).
            Join(figure.transform.DOScale(figure.transform.localScale * _figuresBarConfig.BarScaleForFigureModifier, duration)).
            Join(figure.transform.DORotate(Vector3.zero, duration));

        await sequence.ToUniTask();
    }

    public async UniTask ScaleCollectionViews(FiguresCollectionModel collection)
    {
        var tasks = System.Buffers.ArrayPool<UniTask>.Shared.Rent(collection.FigureViews.Count);

        for (int i = 0; i < collection.FigureViews.Count; i++)
        {
            var view = collection.FigureViews[i];

            tasks[i] = view.transform.DOScale(Vector3.zero, _figuresBarConfig.ScaleSpeed).ToUniTask();
        }
        await UniTask.WhenAll(tasks);

        System.Buffers.ArrayPool<UniTask>.Shared.Return(tasks);
    }
}
