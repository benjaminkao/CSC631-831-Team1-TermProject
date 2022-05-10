using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LightingAudioStorage", menuName = "Scriptables/Audio/LightingAudioStorage")]
public class LightingAudioStorage : ScriptableObject
{
    public AK.Wwise.Event lightingFlicker;
}
