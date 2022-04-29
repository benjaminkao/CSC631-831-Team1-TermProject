using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAudio : MonoBehaviour
{
    public RifleAudioStorage audioStorage;


    public void SetRifleAmmoRTPC(int ammoCount)
    {
        audioStorage.ammoRTPC.SetValue(gameObject, ammoCount);
    }

    public void PlayRifleShot()
    {
        audioStorage.shootEvent.Post(gameObject);
    }

}
