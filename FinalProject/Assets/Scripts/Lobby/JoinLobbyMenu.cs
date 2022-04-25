using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerContainment networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;


    private void OnEnable()
    {
        NetworkManagerContainment.OnClientConnected += HandleClientConnected;
        NetworkManagerContainment.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerContainment.OnClientConnected -= HandleClientConnected;
        NetworkManagerContainment.OnClientDisconnected -= HandleClientDisconnected;
    }


    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;
        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }

}
