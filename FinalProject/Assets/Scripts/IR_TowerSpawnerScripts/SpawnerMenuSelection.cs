using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMenuSelection : MonoBehaviour
{
    private float _towerHeight;
    private Vector3 _towerLocation;
    private GameObject _spawnPad;

    public bool MenuActive
    {
        get { return gameObject.activeInHierarchy; }
        set { gameObject.SetActive(value); }
    }

    private void Start()
    {
        // reference to the top of the tower (root object for TowerSpawner prefab)
        _spawnPad = transform.parent.gameObject;

        Transform spawnLocation = _spawnPad.GetComponent<Transform>();
        MeshRenderer towerMesh = _spawnPad.GetComponent<MeshRenderer>();

        _towerHeight = towerMesh.bounds.size.y;
        _towerLocation = spawnLocation.position;
    }

    public void SpawnTower(GameObject towerPrefab)
    {
        TowerSpawnerInteractable interactable = _spawnPad.GetComponent<TowerSpawnerInteractable>();
        if (interactable.CanInteract)
        {
            Debug.Log("Spawning new tower");
            // places object at top of tower
            Vector3 position = new Vector3(_towerLocation.x, _towerLocation.y + _towerHeight, _towerLocation.z);
            Instantiate(towerPrefab, position, Quaternion.identity);

            // disable the tower, as it has already been used
            interactable.CanInteract = false;
            // auto exit the menu
            MenuActive = false;
        }
    }
}
