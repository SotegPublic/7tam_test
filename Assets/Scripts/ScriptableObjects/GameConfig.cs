using UnityEngine;


[CreateAssetMenu(fileName = "GameConfig", menuName = "CustomSO/GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _figuresTypesCountOnField;
    [SerializeField] private int _figuresCollectionLenth;
    [SerializeField] private int _figuresBarLenth;

    [SerializeField] private Material _heavyMaterial;
    [SerializeField] private Material _stickyMaterial;

    public int FiguresTypesCountOnField => _figuresTypesCountOnField;
    public int FiguresCollectionLenth => _figuresCollectionLenth;
    public int FiguresBarLenth => _figuresBarLenth;
}
