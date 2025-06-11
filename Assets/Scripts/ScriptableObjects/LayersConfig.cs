using UnityEngine;

[CreateAssetMenu(fileName = nameof(LayersConfig), menuName = "CustomSO/" + nameof(LayersConfig), order = 2)]
public class LayersConfig : ScriptableObject
{
    [SerializeField] private LayerMask _iceMask;
    [SerializeField] private LayerMask _baseMask;
    public LayerMask IceMask => _iceMask;
    public LayerMask BaseMask => _baseMask;
}
