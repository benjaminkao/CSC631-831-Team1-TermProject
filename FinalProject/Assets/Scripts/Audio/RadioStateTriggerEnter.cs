using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class RadioStateTriggerEnter : MonoBehaviour
{
    public RadioStorage audioStorage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ContainmentPlayer player = other.GetComponent<ContainmentPlayer>();
            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();

            if (player.hasAuthority)
            {
                audioStorage.radioState.SetValue();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ContainmentPlayer player = other.GetComponent<ContainmentPlayer>();
            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();

            if (player.hasAuthority)
            {
                audioStorage.normalState.SetValue();
            }
        }
    }
}
