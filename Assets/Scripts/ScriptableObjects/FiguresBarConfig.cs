using UnityEngine;

[CreateAssetMenu(fileName = "FiguresBarConfig", menuName = "CustomSO/FiguresBarConfig", order = 3)]
public class FiguresBarConfig : ScriptableObject
{
    [SerializeField] private float _barScaleForFigureModifier;
    [SerializeField] private float _flyTimeToBar;
    [SerializeField] private float _flyArcHeight;
    [SerializeField] private float _scaleSpeed;

    public float BarScaleForFigureModifier => _barScaleForFigureModifier;
    public float FlyTimeToBar => _flyTimeToBar;
    public float ArcHeight => _flyArcHeight;
    public float ScaleSpeed => _scaleSpeed;
}