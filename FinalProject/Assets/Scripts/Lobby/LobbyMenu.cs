using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerContainment networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;


    public void HostLobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }
}
