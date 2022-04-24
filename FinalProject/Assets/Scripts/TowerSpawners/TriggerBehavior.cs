using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TriggerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _interactText;
    [SerializeField] private GameObject _spawnerMenu;

    void Start()
    {
        _interactText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // only display interact prompt if the tower is available for use
        TowerSpawnerInteractable interactable = gameObject.GetComponentInParent<TowerSpawnerInteractable>();
        if (interactable.CanInteract)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // set player reference to the most recent player that has stepped into
                // the trigger

                ContainmentPlayer player = other.GetComponent<ContainmentPlayer>();

                NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();


                interactable.InteractPlayer = other.gameObject;

                if (playerIdentity.hasAuthority)
                {
                    // Only show the Interact Prompt on the player's screen that has authority
                    _interactText.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            SpawnerMenuSelection menu = _spawnerMenu.GetComponent<SpawnerMenuSelection>();


            ContainmentPlayer player = other.GetComponent<ContainmentPlayer>();

            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();

            if (playerIdentity.hasAuthority)
            {
                // Only disable the Interact Prompt on the player's screen that has authority
                _interactText.SetActive(false);
            }

            if (menu.MenuActive)
            {
                menu.MenuActive = false;
            }
        }
    }
}
