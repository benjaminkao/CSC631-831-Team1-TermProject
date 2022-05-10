using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TowerAudioStorage", menuName = "Scriptables/Audio/TowerAudioStorage")]
public class TowerAudioStorage : ScriptableObject
{
    public AK.Wwise.Event spawningEvent;
    public AK.Wwise.Event shootEvent;
}
