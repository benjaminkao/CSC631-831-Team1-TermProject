using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public List<Button> buttons;

    public MainMenuAudioStorage audioStorage;

    void Start()
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(TaskOnClick);
        }
    }

    void TaskOnClick() {
        audioStorage.buttonClickedEvent.Post(gameObject);
    }
}
