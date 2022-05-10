using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "RadioAudioStorage", menuName = "Scriptables/Audio/RadioAudioStorage")]
public class RadioStorage : ScriptableObject
{
    public AK.Wwise.Event radioMusic;

    public AK.Wwise.State radioState;
    public AK.Wwise.State normalState;
    public AK.Wwise.State noneState;


    public List<AK.Wwise.Switch> radioSwitches;
}
