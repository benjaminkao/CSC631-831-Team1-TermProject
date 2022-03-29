using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _spawnerMenu;
    private GameObject _interactPlayer;
    public GameObject InteractPlayer 
    { 
        get { return _interactPlayer; }
        set { _interactPlayer = value; } 
    }

    private bool _canInteract;
    public bool CanInteract
    {
        get { return _canInteract; }
        set { _canInteract = value; }
    }

    void OnEnable()
    {
        _canInteract = true;
        _spawnerMenu.SetActive(false);    
    }

    public void PerformInteractAction()
    {
        //Debug.Log("Can open Spawn Menu: " + _canInteract);
        if (_canInteract)
        {
            Debug.Log("Tower Spawner interacted with");
            if (_spawnerMenu.activeInHierarchy)
            {
                _spawnerMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }   
            else
            {
                _spawnerMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
