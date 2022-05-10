using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For anything Scene Management
using UnityEngine.SceneManagement;


public class MainMenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }



}
