using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBossDeath : MonoBehaviour
{
     public ZombieBossAudioStorage audioStorage;

    void PlayZombieBossDeath() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieBossDeath.Post(gameObject); 
    }
}
