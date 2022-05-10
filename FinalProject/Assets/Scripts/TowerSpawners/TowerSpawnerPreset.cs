using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TowerSpawnerPreset", menuName = "Scriptables/TowerSpawnerPreset")]
public class TowerSpawnerPreset : ScriptableObject
{
    public List<GameObject> TowerPrefabs { get; }

    [SerializeField] private List<GameObject> _towerPrefabs;


    public GameObject GetTowerPrefab(int index)
    {
        return _towerPrefabs[index];
    }
}
