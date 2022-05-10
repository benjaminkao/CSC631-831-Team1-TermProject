using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AmbientAudioStorage", menuName ="Scriptables/Audio/AmbientAudioStorage")]
public class AmbientAudioStorage : ScriptableObject
{
    public AK.Wwise.Event ambientEvent;
}
