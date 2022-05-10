using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Craftable : MonoBehaviour
{

    #region Public Variables




    #endregion Public Variables

    #region Private Variables
    [Tooltip("The distance between the center of the Craftable GameObject and the bottom of the Craftable GameObject")]
    [SerializeField] private Vector3 spawnPositionDistance;


    [Tooltip("The amount of time it takes to craft.")]
    [SerializeField] private float craftTime;


    [SerializeField] private new Renderer renderer;

    #endregion Private Variables



    #region Public Methods

    public void Spawn(Vector3 spawnPosition)
    {

        this.transform.position = spawnPosition + spawnPositionDistance;

        gameObject.SetActive(true);

        StartCoroutine(SpawnRoutine());
    }





    #endregion Public Methods


    #region Coroutine Methods

    public IEnumerator SpawnRoutine()
    {
        
        for(float progress = 0; progress < craftTime; progress += 0.1f)
        {
            Debug.Log($"Building... {progress / craftTime}%");
            this.renderer.material.SetFloat("Progress", progress / craftTime);
            yield return new WaitForSeconds(0.1f);
        }

        this.renderer.material.SetFloat("Progress", 1.1f);

    }



    #endregion Coroutine Methods



    #region UnityEvent Methods

    private void Awake()
    {
        if (this.renderer != null) return;

        this.renderer = GetComponent<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion UnityEvent Methods
}
