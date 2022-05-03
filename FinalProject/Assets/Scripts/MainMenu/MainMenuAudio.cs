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
        if(NetworkManagerContainment.IsHeadless())
        {
            return;
        }

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
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.mainMenuMusic.Stop(gameObject);
    }

    


}
