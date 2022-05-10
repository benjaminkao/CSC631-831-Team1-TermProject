using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Radio : NetworkBehaviour
{

    public RadioStorage audioStorage;

    public Collider interactableRangeCollider;

    public GameObject interactPrompt;

    public int currentChannel;
    public int numChannels;

    private bool _canInteract;

    NetworkIdentity radioIdentity;


    // Start is called before the first frame update
    void Start()
    {
        audioStorage.radioMusic.Post(gameObject);

        currentChannel = 0;
        numChannels = audioStorage.radioSwitches.Count;
        _canInteract = false;
        radioIdentity = GetComponent<NetworkIdentity>();
    }


    private void Update()
    {
        // input read subject to change depending on how we decide to handle
        // interactions
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_canInteract)
            {
                CmdSwitchRadioMusic();
            }
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ContainmentPlayer player = other.GetComponent<ContainmentPlayer>();
            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();

            if (isServer)
            {
                radioIdentity.RemoveClientAuthority();
                radioIdentity.AssignClientAuthority(playerIdentity.connectionToClient);
            }

            if (player.hasAuthority)
            {
                interactPrompt.SetActive(true);
                this._canInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ContainmentPlayer player = other.GetComponent<ContainmentPlayer>();
            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();

            if(isServer)
            {
                radioIdentity.RemoveClientAuthority();
            }

            if (player.hasAuthority)
            {
                interactPrompt.SetActive(false);
                this._canInteract = false;
            }
        }
    }

    [Command]
    public void CmdSwitchRadioMusic()
    {
        RpcSwitchMusic();
    }


    [ClientRpc]
    public void RpcSwitchMusic()
    {
        if(numChannels == 0)
        {
            return;
        }

        currentChannel++;

        if(currentChannel >= numChannels)
        {
            currentChannel = 0;
        }

        audioStorage.radioSwitches[currentChannel].SetValue(gameObject);
    }


}
