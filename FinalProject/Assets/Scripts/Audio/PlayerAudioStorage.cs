using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerAudioStorage", menuName = "Scriptables/Audio/PlayerAudioStorage")]
public class PlayerAudioStorage : ScriptableObject
{
    public AK.Wwise.Event bossVoiceLine;
    public AK.Wwise.Event damagedVoiceLine;
    public AK.Wwise.Event reloadingVoiceLine;
    public AK.Wwise.Event roundendingVoiceLine;
    public AK.Wwise.Event shieldbeacondamagedVoiceLine;
    public AK.Wwise.Event shieldbeaconlowVoiceLine;
    public AK.Wwise.Event noammoVoiceLine;

    public AK.Wwise.RTPC playerHealth;
    public AK.Wwise.Event playerHealthAudio;

    public AK.Wwise.Event FootStep;
}
