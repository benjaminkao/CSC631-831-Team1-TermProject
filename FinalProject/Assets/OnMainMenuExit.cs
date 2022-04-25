using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class OnMainMenuExit : MonoBehaviour
{
    public AK.Wwise.Event mainMenuStopEvent; 
    int nextSceneIndex; 

    void Start() {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == nextSceneIndex) {
            mainMenuStopEvent.Post(gameObject); 
        }
    }
}
