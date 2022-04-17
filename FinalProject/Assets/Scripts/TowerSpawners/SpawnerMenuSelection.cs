using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnerMenuSelection : NetworkBehaviour
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

    public override void OnStartServer()
    {
        // reference to the top of the tower (root object for TowerSpawner prefab)

        Transform spawnLocation = GetComponent<Transform>();
        MeshRenderer towerMesh = GetComponent<MeshRenderer>();

        _towerHeight = towerMesh.bounds.size.y;
        _towerLocation = spawnLocation.position;
    }


    /** IMPORTANT
 * ===================================================
 * Why did I (Benjamin) change SpawnTower(GameObject towerPrefab); to [Command] CmdSpawnTower(int towerPrefabIndex);?
 * 
 * - Firstly, what does [Command] do? 
 * 
 * It signals that this command should be run on the server, so if 
 * the client calls this function, the command will be sent to the server and then the server
 * will run this function.
 * 
 * - Next, why did you change the parameter from GameObject?
 * 
 * In the Mirror documentation, [Command] and [ClientRpc] (which I haven't used yet) only accept these
 * types of arguments:
 *      * basic types (byte, int, float, string, UInt64, etc)
 *      * arrays of basic types
 *      * structs containing allowable types
 *      * built-in unity math types (Vector3, Quaternion, etc.)
 *      * NetworkIdentity
 *      * NetworkInstanceId
 *      * NetworkHash128
 *      * GameObject with a NetworkIdentity component attached
 * 
 * Now although the towerPrefabs have a NetworkIdentity component attached to them, during testing
 * I noticed that the parameter will always be set to NULL.
 * 
 * 
 * - Ok, then why can't you use NetworkIdentity as the parameter instead if the towerPrefabs
 * already have the NetworkIdentity component?
 * 
 * I also had a similar thought and when I tried this, I received a similar error where the 
 * Network Identity parameter was NULL.
 * 
 * - Why is the NetworkIdentity parameter NULL when using a Prefab?
 * 
 * The reason this happened is because a NetworkIdentity is only given to a GameObject that exists
 * within the scene. Since I am giving the function prefabs, these don't exist in the scene, thus
 * they don't have a NetworkIdentity and are therefore NULL>
 * 
 * - So, what about putting the prefab in the scene and just keeping it hidden?
 * 
 * I also had a similar thought and when I tried this, I received an error in NetworkServer.Spawn()
 * where it said the GameObject already exists in the scene so it is confusing to the "NetworkClients"
 * if the game spawns another one.
 * 
 * 
 * - So, now we get to why I changed the parameter from GameObject to an int?
 * 
 *      1. int is a basic type so it can be sent over the network with the [Command] directive.
 *      2. Not dealing with NetworkIdentity's on prefabs, so NetworkIdentity won't be NULL
 *      3. Since all clients/server are running a copy of the game, they will of course know which
 *         towers are possible to spawn so how about just having a list of towerPrefabs that the
 *         spawner can spawn.
 *      4. Then, when the [Command] comes with the specified towerPrefabIndex, we can spawn the tower.
 * 
 * 
 * 
 * 
 * - After this entire explanation, what is the main takeaway?
 * 
 * REMEMBER that the client and server/host are running the exact same game. This means both sides have
 * access to all of the same GameObjects/Assets. So, there is no point (and is not allowed) in sending
 * GameObjects or really any large data type/custom data type. Instead, we can send a unique identifier
 * that corresponds to the GameObject we want and then the client or server can handle it on their side.
 * 
 * 
 * 
*/
    [Command]
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

        #region Spawn Tower Networking

        NetworkServer.Spawn(towerGO);

        #endregion Spawn Tower Networking

        TowerTargeting tower = towerGO.GetComponent<TowerTargeting>();

        tower.Owner = this._towerOwner.GetComponent<ContainmentPlayer>();
        tower.TowerSpawner = this.GetComponent<TowerSpawnerInteractable>();
    }
}
