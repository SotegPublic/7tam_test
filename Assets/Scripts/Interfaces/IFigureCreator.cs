using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IFigureCreator
{
    public UniTask<FigureView> CreateFigure(Vector2 position, FiguresTypes type);
}
