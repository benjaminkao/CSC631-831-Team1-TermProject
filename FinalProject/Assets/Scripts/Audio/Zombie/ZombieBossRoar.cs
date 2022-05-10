using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBossRoar : MonoBehaviour
{
     public ZombieBossAudioStorage audioStorage;

    void PlayZombieBossRoar() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieBossRoar.Post(gameObject); 
    }
}
