using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusic : MonoBehaviour
{

    public GameplayMusicAudioStorage audioStorage;


    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.gameplayMusic.Post(gameObject);
    }

}
