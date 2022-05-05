using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBossAttack : MonoBehaviour
{
     public ZombieBossAudioStorage audioStorage;

    void PlayZombieBossAttack() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieBossAttack.Post(gameObject); 
    }
}
