using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Button playButton, optionsButton, quitButton, backButton, confirmNameButton, hostLobbyButton, joinLobbyButton, joinButton;
    public AK.Wwise.Event buttonClickedEvent; 

    void Start()
    {
        playButton.onClick.AddListener(TaskOnClick);
        optionsButton.onClick.AddListener(TaskOnClick);
        quitButton.onClick.AddListener(TaskOnClick);
        backButton.onClick.AddListener(TaskOnClick); 
    }

    void TaskOnClick() {
        buttonClickedEvent.Post(gameObject);
    }
}
