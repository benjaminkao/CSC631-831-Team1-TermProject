using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MainMenuAudioStorage", menuName = "Scriptables/Audio/MainMenuAudioStorage")]
public class MainMenuAudioStorage : ScriptableObject
{

    public AK.Wwise.Event buttonClickedEvent;
    public AK.Wwise.Event buttonHoverEvent;
    public AK.Wwise.Event mainMenuMusic;

}
