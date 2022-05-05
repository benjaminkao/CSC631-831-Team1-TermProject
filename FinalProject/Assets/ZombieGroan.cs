using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGroan : MonoBehaviour
{
     public ZombieAudioStorage audioStorage;

    void PlayZombieGroan() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieGroan.Post(gameObject); 
   
    }

}
