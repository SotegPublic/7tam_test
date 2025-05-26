using UnityEngine;


[CreateAssetMenu(fileName = "GameConfig", menuName = "CustomSO/GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _figuresTypesCountOnField;
    [SerializeField] private int _figuresCollectionLenth;
    [SerializeField] private int _figuresBarLenth;
    [SerializeField] private LayerMask _figuresLayerMask;

    public int FiguresTypesCountOnField => _figuresTypesCountOnField;
    public int FiguresCollectionLenth => _figuresCollectionLenth;
    public int FiguresBarLenth => _figuresBarLenth;
    public LayerMask FiguresLayerMask => _figuresLayerMask;
}
