using UnityEngine;

[CreateAssetMenu(fileName = nameof(IceAnimationConfig), menuName = "CustomSO/" + nameof(IceAnimationConfig), order = 2)]
public class IceAnimationConfig : ScriptableObject
{
    [SerializeField] private float _force;
    [SerializeField] private int _vibrato;
    [SerializeField] private float _duration;

    public float Force => _force;
    public int Vibrato => _vibrato;
    public float Duration => _duration;
}
