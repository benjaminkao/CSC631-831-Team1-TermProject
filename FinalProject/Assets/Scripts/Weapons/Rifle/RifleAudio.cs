using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAudio : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event shootEvent;
    [SerializeField] AK.Wwise.RTPC ammoRTPC;


    public void SetRifleAmmoRTPC(int ammoCount)
    {
        ammoRTPC.SetValue(gameObject, ammoCount);
    }

    public void PlayRifleShot()
    {
        shootEvent.Post(gameObject);
    }

}
