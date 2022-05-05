using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBossFootSteps : MonoBehaviour
{
     public ZombieBossAudioStorage audioStorage;

    void PlayZombieBossFootStep() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieBossFootStep.Post(gameObject); 
    }
}
