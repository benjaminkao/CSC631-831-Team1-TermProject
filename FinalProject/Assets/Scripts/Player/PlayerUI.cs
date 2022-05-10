using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text readyUpLabel;
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private GameObject readyUpPrompt;

    public HealthBar HealthBar
    {
        get { return healthBar; }
    }


    private void Start()
    {
        SetReadyUp(false);
        ToggleReadyUpPrompt(true);
    }


    public void SetPlayerName(string name)
    {
        this.playerName.text = name;
    }

    public void SetReadyUp(bool ready)
    {
        readyUpLabel.text = ready ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
    }

    public void ToggleReadyUpActive(bool active)
    {
        readyUpLabel.gameObject.SetActive(active);
    }

    public void ToggleReadyUpPrompt(bool show)
    {
        if(readyUpPrompt == null)
        {
            return;
        }

        readyUpPrompt.SetActive(show);
    }



}
