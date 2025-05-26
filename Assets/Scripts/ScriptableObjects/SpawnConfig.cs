using UnityEngine;

[CreateAssetMenu(fileName = "SpawnConfig", menuName = "CustomSO/SpawnConfig", order = 2)]
public class SpawnConfig : ScriptableObject
{
    [SerializeField] private int _downwardForce;
    [SerializeField] private int _spawnDelay;

    public int DownwardForce => _downwardForce;
    public int SpawnDelay => _spawnDelay;
}
