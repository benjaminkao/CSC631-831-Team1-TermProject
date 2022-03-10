using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnerPreset", menuName = "Scriptables/SpawnerPreset")]
public class SpawnerPreset : ScriptableObject
{
    public List<Poolable> spawnablePrefabs;
}
