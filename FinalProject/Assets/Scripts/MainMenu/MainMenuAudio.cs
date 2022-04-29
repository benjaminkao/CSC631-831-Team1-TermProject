using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAudio : MonoBehaviour
{
    public MainMenuAudioStorage audioStorage;
    


    // Start is called before the first frame update
    void Start()
    {
        audioStorage.mainMenuMusic.Post(gameObject);

        
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
        audioStorage.mainMenuMusic.Stop(gameObject);
    }

    


}
