using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
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
        _canInteract = true;
        //_canInteract = false; // ORIGINAL VALUE
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Can interact");
            _canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("No longer able to interact");
            _canInteract = false;
        }
    }
}
