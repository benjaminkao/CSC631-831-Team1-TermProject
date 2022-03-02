using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptables/EnemyType")]
public class EnemyType : ScriptableObject
{
    public GameObject bloodSprayPrefab;
    public GameObject bloodPrefab;
}
