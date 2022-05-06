using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "RifleAudioStorage", menuName = "Scriptables/Audio/RifleAudioStorage")]
public class RifleAudioStorage : ScriptableObject
{
    public AK.Wwise.Event noAmmoEvent;
    public AK.Wwise.Event impactEvent;
    public AK.Wwise.Event shootEvent;
    public AK.Wwise.RTPC ammoRTPC;
}
