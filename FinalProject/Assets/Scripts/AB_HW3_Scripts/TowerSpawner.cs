using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{

    #region Public Variables

    #endregion Public Variables


    #region Private Variables

    // The GameObject that the spawned tower will be put on top of.
    [Tooltip("The GameObject that the spawned tower will be put on top of.")]
    [SerializeField] private GameObject spawnPad;


    [Tooltip("The GameObject that is connected to the TowerSpawner")]
    [SerializeField] private Craftable tower;

    private MeshRenderer renderer;

    #endregion Private Variables

    void OnEnable()
    {
        renderer = GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnTower(Craftable tower)
    {
        if(this.tower != null)
        {
            // Tower already exists on this TowerSpawner, can't spawn anything else
            return;
        }


        this.tower = tower;
        this.tower.Spawn(spawnPad.transform.position);

        this.renderer.material.color = Color.red;
    }

}
