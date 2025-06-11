using UnityEngine;

[CreateAssetMenu(fileName = nameof(FigureModifiersConfig), menuName = "CustomSO/" + nameof(FigureModifiersConfig), order = 0)]
public class FigureModifiersConfig : ScriptableObject
{
    [SerializeField] private float _weight;
    [SerializeField][Range(0, 100)] private int _chanceToCreateHeavyFigure;
    [SerializeField] private int _maxCountHeavyFigures;
    [SerializeField] private PhysicsMaterial2D _stickyMaterial;
    [SerializeField][Range(0, 100)] private int _chanceToCreateStickyFigure;
    [SerializeField] private int _maxCountStickyFigures;
    [SerializeField][Range(0, 100)] private int _chanceToCreateIcyFigure;
    [SerializeField] private int _maxCountIcyFigures;
    [SerializeField] private int _figuresCountDeleteForIceBracke;

    public float Weight => _weight;
    public int ChanceToCreateHeavyFigure => _chanceToCreateHeavyFigure;
    public int MaxCountHeavyFigures => _maxCountHeavyFigures;
    public PhysicsMaterial2D StickyMaterial => _stickyMaterial;
    public int ChanceToCreateStickyFigure => _chanceToCreateStickyFigure;
    public int MaxCountStickyFigures => _maxCountStickyFigures;
    public int ChanceToCreateIcyFigure => _chanceToCreateIcyFigure;
    public int MaxCountIcyFigures => _maxCountIcyFigures;
    public int FiguresCountDeleteForIceBreak => _figuresCountDeleteForIceBracke;
}
