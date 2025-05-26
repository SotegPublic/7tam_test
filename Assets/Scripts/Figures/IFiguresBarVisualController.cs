using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IFiguresBarVisualController
{
    public UniTask DrowFlyToBar(FigureView figure, Vector3 targetPosition, bool isArcLeft);
    public UniTask ScaleCollectionViews(FiguresCollectionModel collection);
}