using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMenuSelection : MonoBehaviour
{
    private float _towerHeight;
    private Vector3 _towerLocation;
    [SerializeField] private GameObject _spawnMenu;

    [SerializeField] private TowerSpawnerPreset towerSpawnerPreset;

    public bool MenuActive
    {
        get { return _spawnMenu.activeInHierarchy; }
        set { _spawnMenu.SetActive(value); }
    }

    void Start()
    {
        // reference to the top of the tower (root object for TowerSpawner prefab)

        Transform spawnLocation = GetComponent<Transform>();
        MeshRenderer towerMesh = GetComponent<MeshRenderer>();

        _towerHeight = towerMesh.bounds.size.y;
        _towerLocation = spawnLocation.position;
    }


    
    public void CmdSpawnTower(int towerPrefabIndex)
    {

        Debug.Log("Should spawn Tower");

        TowerSpawnerInteractable interactable = GetComponent<TowerSpawnerInteractable>();
        if (interactable.CanInteract)
        {
            Debug.Log("Spawning new tower: " + towerPrefabIndex);

            GameObject towerPrefab = towerSpawnerPreset.GetTowerPrefab(towerPrefabIndex);


            // places object at top of tower
            Vector3 position = new Vector3(_towerLocation.x, _towerLocation.y + _towerHeight, _towerLocation.z);
            GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);


            // disable the tower, as it has already been used
            interactable.CanInteract = false;
            // auto exit the menu
            MenuActive = false;
        }
    }
}
