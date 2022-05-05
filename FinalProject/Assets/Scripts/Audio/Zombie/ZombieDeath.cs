using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeath : MonoBehaviour
{
    public ZombieAudioStorage audioStorage;

    void PlayZombieDeath() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieDeath.Post(gameObject); 
    }
}
