using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMenuSelection : MonoBehaviour
{
    [SerializeField] private GameObject _spawnMenu;
    [SerializeField] private TowerSpawnerPreset towerSpawnerPreset;
    [SerializeField] private int _spawnCost;
    
    private float _towerHeight;
    private Vector3 _towerLocation;
    private GameObject _towerOwner;

    public bool MenuActive
    {
        get { return _spawnMenu.activeInHierarchy; }
        set { 
            _spawnMenu.SetActive(value);
            Cursor.lockState = _spawnMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }
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
        // Save the reference to the player to access their points in the future
        _towerOwner = interactable.InteractPlayer;
        PointBank playerBank = _towerOwner.GetComponent<PointBank>();


        if (interactable.CanInteract && playerBank.HasSufficientPoints(_spawnCost))
        {
            GameObject towerPrefab = towerSpawnerPreset.GetTowerPrefab(towerPrefabIndex);
            playerBank.SpendPoints(_spawnCost);

            Debug.Log(string.Format("Spawning new tower: {0}. Bank before / after: {1} / {2}",
                towerPrefabIndex, playerBank.TotalPoints + _spawnCost, playerBank.TotalPoints));

            StartCoroutine(TowerSpawnProgress(towerPrefab));

            // disable the tower, as it has already been used
            interactable.CanInteract = false;
            // auto exit the menu
            MenuActive = false;
        } else
        {
            Debug.Log("Cannot spawn tower. Insufficient number of points.");
        }
    }

    public IEnumerator TowerSpawnProgress(GameObject towerPrefab)
    {
        // can add multiple stages of waiting for gradually building the geometry
        yield return new WaitForSecondsRealtime(1.0f);

        // places object at top of tower
        Vector3 position = new Vector3(_towerLocation.x, _towerLocation.y + _towerHeight, _towerLocation.z);
        GameObject towerGO = Instantiate(towerPrefab, position, Quaternion.identity);

        TowerTargeting tower = towerGO.GetComponent<TowerTargeting>();

        tower.Owner = this._towerOwner.GetComponent<ContainmentPlayer>();
    }
}
