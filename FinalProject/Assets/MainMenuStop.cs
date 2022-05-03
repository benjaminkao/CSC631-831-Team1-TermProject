using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStop : MonoBehaviour
{
    public AK.Wwise.Event mainMenuMusicStop; 
    void Start()
    {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        mainMenuMusicStop.Post(gameObject); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
