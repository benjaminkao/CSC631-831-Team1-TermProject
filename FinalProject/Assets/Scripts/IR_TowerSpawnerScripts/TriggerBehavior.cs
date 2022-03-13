using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject _interactText;
    [SerializeField]
    private GameObject _spawnerMenu;

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
            _interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SpawnerMenuSelection menu = _spawnerMenu.GetComponent<SpawnerMenuSelection>();
        _interactText.SetActive(false);
        if(menu.MenuActive)
        {
            menu.MenuActive = false;
        }
    }
}
