using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject _spawnerMenu;
    private bool _canInteract;
    public bool CanInteract
    {
        get
        {
            return _canInteract;
        }
        set
        {
            _canInteract = value;
        }
    }

    private void Start()
    {
        _canInteract = true;
        _spawnerMenu.SetActive(false);    
    }

    public void PerformInteractAction()
    {
        if(_canInteract)
        {
            Debug.Log("Tower Spawner interacted with");
            if (_spawnerMenu.activeInHierarchy)
            {
                _spawnerMenu.SetActive(false);
            }
            else
            {
                _spawnerMenu.SetActive(true);
            }
        }
    }
}
