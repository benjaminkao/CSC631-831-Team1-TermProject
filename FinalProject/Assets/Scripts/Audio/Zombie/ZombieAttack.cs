using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
        public ZombieAudioStorage audioStorage;

    void PlayZombieAttack() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.ZombieAttack.Post(gameObject); 
    }
}
