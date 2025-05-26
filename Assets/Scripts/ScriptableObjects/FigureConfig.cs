using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class FigureConfig
{
    [SerializeField] private FiguresTypes _figureType;
    [SerializeField] private AssetReference _viewReference;
    [SerializeField] private AssetReference _outerFigureReference;
    [SerializeField] private AssetReference _animalrReference;
    [SerializeField] private Color _lineColor;

    public FiguresTypes FigureType => _figureType;
    public AssetReference ViewReference => _viewReference;
    public AssetReference OuterFigureReference => _outerFigureReference;
    public AssetReference AnimalrReference => _animalrReference;
    public Color LineColor => _lineColor;
}
