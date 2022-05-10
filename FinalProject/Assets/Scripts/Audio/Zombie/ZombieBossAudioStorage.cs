using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ZombieBossAudioStorage", menuName = "Scriptables/Audio/ZombieBossAudioStorage")]
public class ZombieBossAudioStorage : ScriptableObject
{
    public AK.Wwise.Event ZombieBossFootStep;
    public AK.Wwise.Event ZombieBossAttack;
    public AK.Wwise.Event ZombieBossRoar;
    public AK.Wwise.Event ZombieBossDamageTaken;
    public AK.Wwise.Event ZombieBossDeath;
}