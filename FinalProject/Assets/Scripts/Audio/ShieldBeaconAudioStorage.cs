using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ShieldBeaconAudioStorage", menuName = "Scriptables/Audio/ShieldBeaconAudioStorage")]
public class ShieldBeaconAudioStorage : ScriptableObject
{
    public AK.Wwise.RTPC healthRTPC;
    public AK.Wwise.Event shieldNoise;
    public AK.Wwise.Event shieldBeaconDamaged;
}
