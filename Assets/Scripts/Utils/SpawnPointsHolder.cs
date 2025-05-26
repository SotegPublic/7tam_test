using System;
using UnityEngine;

[Serializable]
public class SpawnPointsHolder
{
    [SerializeField] private Transform[] _spawnPoints;

    public Transform[] SpawnPoints => _spawnPoints;
}