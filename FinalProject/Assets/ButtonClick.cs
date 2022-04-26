using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public List<Button> buttons;
    public AK.Wwise.Event buttonClickedEvent; 

    void Start()
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(TaskOnClick);
        }
    }

    void TaskOnClick() {
        buttonClickedEvent.Post(gameObject);
    }
}
