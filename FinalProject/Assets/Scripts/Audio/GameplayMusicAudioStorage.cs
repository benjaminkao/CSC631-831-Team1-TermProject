using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameplayMusicAudioStorage", menuName = "Scriptables/Audio/GameplayMusicAudioStorage")]
public class GameplayMusicAudioStorage : ScriptableObject
{
    public AK.Wwise.State preparationGameplayState;
    public AK.Wwise.State bossGameplayState;
    public AK.Wwise.State lowIntensityGameplayState;
    public AK.Wwise.State highIntensityGameplayState;
    public AK.Wwise.State noneState;

    public AK.Wwise.Event gameplayMusic;
}
