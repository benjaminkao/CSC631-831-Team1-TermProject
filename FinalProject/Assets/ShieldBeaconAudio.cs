using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class ShieldBeaconAudio : NetworkBehaviour 
{
    [SerializeField]
    private AK.Wwise.RTPC healthRTPC;
    [SerializeField]
    private AK.Wwise.Event shieldNoise;  
    

    void Start()
    {
        shieldNoise.Post(gameObject); 
    }


    [ClientRpc]
    public void RpcShieldBeaconHealthAudio(float health) {
        healthRTPC.SetValue(gameObject, health);
    }
}
