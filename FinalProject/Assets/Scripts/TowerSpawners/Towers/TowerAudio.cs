using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAudio : MonoBehaviour
{
    public TowerAudioStorage audioStorage;

    public void PlayShootAudio()
    {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.shootEvent.Post(gameObject);
    }

    public void PlaySpawnAudio()
    {
        if(NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        audioStorage.spawningEvent.Post(gameObject);
    }

}
