using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFootSteps : MonoBehaviour
{
    public ZombieAudioStorage audioStorage;

    void PlayZombieFootStep() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieFootStep.Post(gameObject); 
    }
}
