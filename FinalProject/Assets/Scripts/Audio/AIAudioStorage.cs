using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AIAudioStorage", menuName = "Scriptables/Audio/AIAudioStorage")]
public class AIAudioStorage : ScriptableObject
{

    public AK.Wwise.Event waveStartVoiceLine1;
    public AK.Wwise.Event waveStartVoiceLine2;
    public AK.Wwise.Event waveStartVoiceLine3;
    public AK.Wwise.Event waveEndVoiceLine1;
    public AK.Wwise.Event waveEndVoiceLine2;
    public AK.Wwise.Event bossWaveStartVoiceLine;
    public AK.Wwise.Event shieldBeaconLowVoiceLine;


}
