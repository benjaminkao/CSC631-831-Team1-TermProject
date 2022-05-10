using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class PlayerAudio : NetworkBehaviour
{
    public PlayerAudioStorage audioStorage;

   public const int BOSS = 0, DAMAGED = 1, RELOADING = 2, ROUNDENDING = 3, SHIELDBEACONDAMAGED = 4, SHIELDBEACONLOW = 5, NOAMMO = 6, TOWERSPAWNING = 7;  
   
    public override void OnStartAuthority() {
        if(isServerOnly) return;

        audioStorage.playerHealthAudio.Post(gameObject); 

    }

    public void SetPlayerHealthRTPC(float health) {
        audioStorage.playerHealth.SetValue(gameObject, health);
    }

    public void TriggerVoiceLine(int voiceLine)
    {
        if(!isServer)
        {
            return;
        }

        RpcTriggerVoiceLine(voiceLine);
    }

    [ClientRpc]
    public void RpcTriggerVoiceLine(int voiceLine)
    {
        switch(voiceLine)
        {
           case PlayerAudio.BOSS:
                audioStorage.bossVoiceLine.Post(gameObject); 
               break;
           case PlayerAudio.DAMAGED:
                audioStorage.damagedVoiceLine.Post(gameObject);
               break;
            case PlayerAudio.NOAMMO:
                audioStorage.noammoVoiceLine.Post(gameObject);
                break;
           case PlayerAudio.RELOADING:
                audioStorage.reloadingVoiceLine.Post(gameObject);
               break;
           case PlayerAudio.ROUNDENDING:
                audioStorage.roundendingVoiceLine.Post(gameObject);
               break;
           case PlayerAudio.SHIELDBEACONDAMAGED:
                audioStorage.shieldbeacondamagedVoiceLine.Post(gameObject);
               break;
           case PlayerAudio.SHIELDBEACONLOW:
                audioStorage.shieldbeaconlowVoiceLine.Post(gameObject);
               break;
            case PlayerAudio.TOWERSPAWNING:
                audioStorage.towerSpawnerVoiceLine.Post(gameObject);
                break;

        }

        //Debug.Log($"Triggered Voice Line: {voiceLine}");
    }
}
