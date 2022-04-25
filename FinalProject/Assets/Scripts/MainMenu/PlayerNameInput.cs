using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This code is from Dapper Dino's Main Menu Lobby Tutorial: https://www.youtube.com/watch?v=Fx8efi2MNz0
/// </summary>


public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;


    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        SetupInputField();
    }

    private void SetupInputField()
    {
        if(!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            return;
        }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
