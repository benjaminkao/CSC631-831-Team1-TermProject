using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGroan : MonoBehaviour
{
     public ZombieAudioStorage audioStorage;

    [Range(0, 1)]
    public float eventLikelihood;

    void PlayZombieGroan() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        float rand = Random.Range(0, 1);

        if(rand > eventLikelihood)
        {
            return;
        }


        audioStorage.ZombieGroan.Post(gameObject); 
   
    }

}
