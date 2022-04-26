using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAudio : MonoBehaviour
{

    [SerializeField] private AK.Wwise.Event mainMenuMusic;


    // Start is called before the first frame update
    void Start()
    {
        mainMenuMusic.Post(gameObject);

        
    }

    //private void OnEnable()
    //{
    //    SceneManager.activeSceneChanged += StopMainMenuMusic;
    //}

    private void OnDestroy()
    {
        StopMainMenuMusic();
    }



    void StopMainMenuMusic()
    {
        mainMenuMusic.Stop(gameObject);
    }

    


}
