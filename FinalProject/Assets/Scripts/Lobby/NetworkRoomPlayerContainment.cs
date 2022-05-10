using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class NetworkRoomPlayerContainment : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private List<TMP_Text> playerNameTexts = new List<TMP_Text>(4);
    [SerializeField] private List<TMP_Text> playerReadyTexts = new List<TMP_Text>(4);
    [SerializeField] private Button startGameButton = null;


    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";

    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    [SyncVar(hook = nameof(HandleLeaderStatusChanged))]
    [SerializeField] private bool isLeader;

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerContainment room;

    private NetworkManagerContainment Room
    {
        get
        {
            if(room != null)
            {
                return room;
            }
            return room = NetworkManager.singleton as NetworkManagerContainment;
        }
    }



    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);

        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateDisplay();

    }


    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    private void OnEnable()
    {
        startGameButton.interactable = false;
    }


    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    public void HandleLeaderStatusChanged(bool oldValue, bool newValue) => UpdateStartGameDisplay();

    private void UpdateStartGameDisplay()
    {
        startGameButton.gameObject.SetActive(isLeader);
    }


    private void UpdateDisplay()
    {
        if(!hasAuthority)
        {
            foreach(var player in Room.RoomPlayers)
            {
                if(player.hasAuthority)
                {
                    player.UpdateDisplay();
                }
            }

            return;
        }


        for (int i = 0; i < playerNameTexts.Count; i++)
        {
            playerNameTexts[i].text = "Waitig For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        bool isReadyToStart = true;
        for(int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            if(!Room.RoomPlayers[i].IsReady)
            {
                isReadyToStart = false;
            }

            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }

        if(isLeader)
        {
            HandleReadyToStart(isReadyToStart);
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if(!isLeader)
        {
            return;
        }

        if(startGameButton == null)
        {
            return;
        }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if(Room.RoomPlayers[0].connectionToClient != connectionToClient)
        {
            return;
        }

        Room.StartGame();
    }


    [ClientRpc]
    public void RpcSetLeader(bool isLeader)
    {
        IsLeader = isLeader;
    }

    [ClientRpc]
    public void RpcHandleReadyToStart(bool isReadyToStart)
    {
        HandleReadyToStart(isReadyToStart);
    }

}
