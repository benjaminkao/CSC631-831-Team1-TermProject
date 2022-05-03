using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUIManager : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject craftingHUD;

    [SerializeField] PlayerUI[] playerUis = new PlayerUI[3];
    [SerializeField] PlayerUI localPlayer;



    [SerializeField] TowerSpawner selectedTowerSpawner;

    private int _otherPlayerCount;

    // Start is called before the first frame update
    void Start()
    {
        if(pauseMenu == null)
        {
            Debug.LogError("Pause Menu is undefined.");
            //UnityEditor.EditorApplication.isPlaying = false;
        }

        this._otherPlayerCount = 0;

        foreach(PlayerUI playerUI in playerUis)
        {
            playerUI.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }



    }


    public void ReturnToMainMenu()
    {
        Debug.Log("Return to main menu");
        //SceneManager.LoadScene(0);
    }


    public void ToggleCraftingMenu(TowerSpawner towerSpawner)
    {
        this.selectedTowerSpawner = craftingHUD.activeSelf ? null : towerSpawner;

        craftingHUD.SetActive(!craftingHUD.activeSelf);

        Cursor.lockState = CursorLockMode.None;
    }


    public void SpawnSelectedObject(Craftable prefab)
    {
        if(selectedTowerSpawner == null)
        {
            return;
        }


        GameObject tower = Instantiate(prefab.gameObject);

        selectedTowerSpawner.SpawnTower(tower.GetComponent<Craftable>());

        craftingHUD.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public PlayerUI GetLocalPlayerUI()
    {
        return localPlayer;
    }

    public PlayerUI GetOtherPlayerUI()
    {
        PlayerUI result = playerUis[this._otherPlayerCount++];
        return result;
    }
}
