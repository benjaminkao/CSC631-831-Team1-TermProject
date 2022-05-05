using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ZombieAudioStorage", menuName = "Scriptables/Audio/ZombieAudioStorage")]
public class ZombieAudioStorage : ScriptableObject
{
    public AK.Wwise.Event ZombieFootStep;
    public AK.Wwise.Event ZombieAttack;
    public AK.Wwise.Event ZombieGroan;
    public AK.Wwise.Event ZombieScreech;
    public AK.Wwise.Event ZombieDamageTaken;
    public AK.Wwise.Event ZombieDeath;


}