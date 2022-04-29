using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class ShieldBeaconAudio : NetworkBehaviour 
{
    public ShieldBeaconAudioStorage audioStorage;
    

    void Start()
    {
        audioStorage.shieldNoise.Post(gameObject); 
    }


    [ClientRpc]
    public void RpcShieldBeaconHealthAudio(float health) {
        audioStorage.healthRTPC.SetValue(gameObject, health);
    }
}
