using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class ShieldBeaconAudio : NetworkBehaviour 
{
    public ShieldBeaconAudioStorage audioStorage;
    

    void Start()
    {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }
        audioStorage.shieldNoise.Post(gameObject); 
    }


    [ClientRpc]
    public void RpcShieldBeaconHealthAudio(float health) {
        audioStorage.healthRTPC.SetValue(gameObject, health);
    }

    [ClientRpc]
    public void RpcShieldBeaconDamaged()
    {
        audioStorage.shieldBeaconDamaged.Post(gameObject);
    }
}
