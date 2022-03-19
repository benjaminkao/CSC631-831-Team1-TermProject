using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class PlayerInteractNetworking : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _interactAction;
    private bool _canInteract;

    private void Start()
    {
        // temporary change: IR_Scene does not contain a player character as of now.
        // It's just a demo of the Tower Spawner logic as it stands independently. 
        // Once we integrate the tower with Benjamin/Alekya's player character, we can
        // revert the boolean to its original value and rely on the TriggerEnter/Exit
        // methods again. 
        //_canInteract = true;
        _canInteract = false; // ORIGINAL VALUE
    }

    private void Update()
    {
        // input read subject to change depending on how we decide to handle
        // interactions
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interact pressed");
            if (_canInteract)
            {
                Debug.Log("Interaction successful");
                _interactAction.Invoke();
            }
        }
    }


    /** IMPORTANT
     * ====================================================================
     * What is with the AssignClientAuthority() and RemoveClientAuthority() calls?
     * 
     * - Firstly, what does AssignClientAuthority() and RemoveClientAuthority() do?
     * 
     * Basically, it gives a specified NetworkIdentity a specified authority, which in most cases
     * will be a Player's client authority.
     * 
     * - Why do we need to assign a specified NetworkIdentity (or in this case the TowerSpawner)
     * the Player's client authority?
     * 
     * The only thing that can communicate with the server are the clients and the only thing that
     * the server considers clients are players (which is referenced in NetworkManager PlayerPrefab).
     * So, if a GameObject with a NetworkIdentity does not have this client authority when calling a
     * [Command] function, then Mirror will throw an error saying "This object does not have the 
     * authority to call [Command] function". However, if you give the GameObject with a NetworkIdentity
     * a Player's authority, then the GameObject can call the [Command] function.
     * 
     * - How to give a GameObject with a NetworkIdentity a player's authority?
     * You will need a reference to both the Player and the GameObject you would like to give authority to
     * and their NetworkIdentity component. Then, all you need to do is use the AssignClientAuthority()
     * function to give the GameObject the player's connectionToClient or client authority. Also, when I
     * was researching this concept, most of the solutions I read said you should do these in the
     * OnTriggerEnter() and OnTriggerExit() functions which kind of makes sense because you do not want
     * these GameObjects to always have the player's client authority. Also, this works perfectly for
     * us since we are using Izaiah's TowerSpawner where we step on a little platform.
     * 
    */
    private void OnTriggerEnter(Collider other)
    {
        NetworkIdentity towerSpawnerIdentity = GetComponentInParent<NetworkIdentity>();

        Debug.Log(towerSpawnerIdentity.gameObject.name);

        if (other.gameObject.CompareTag("Player"))
        {
            ContainmentPlayer player = other.gameObject.GetComponent<ContainmentPlayer>();

            towerSpawnerIdentity.AssignClientAuthority(player.GetComponent<NetworkIdentity>().connectionToClient);




            Debug.Log("Can interact");
            _canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NetworkIdentity towerSpawnerIdentity = GetComponentInParent<NetworkIdentity>();

        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            towerSpawnerIdentity.RemoveClientAuthority();


            Debug.Log("No longer able to interact");
            _canInteract = false;
        }
    }
}
