using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TowerFX", menuName = "Scriptables/TowerFX")]
public class TowerFX : ScriptableObject
{
    public TrailRenderer bulletTrail;
    public ParticleSystem bulletImpact;
}
