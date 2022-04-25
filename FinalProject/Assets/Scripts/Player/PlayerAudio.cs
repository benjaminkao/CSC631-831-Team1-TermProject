using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class PlayerAudio : NetworkBehaviour
{
   [SerializeField]
   private AK.Wwise.Event bossVoiceLine; 
   [SerializeField]
   private AK.Wwise.Event damagedVoiceLine; 
   [SerializeField]
   private AK.Wwise.Event reloadingVoiceLine; 
   [SerializeField]
   private AK.Wwise.Event roundendingVoiceLine; 
   [SerializeField]
   private AK.Wwise.Event shieldbeacondamagedVoiceLine; 
   [SerializeField]
   private AK.Wwise.Event shieldbeaconlowVoiceLine;
    [SerializeField]
    private AK.Wwise.Event noammoVoiceLine;

   [SerializeField]
   private AK.Wwise.RTPC playerHealth; 
   [SerializeField]
   private AK.Wwise.Event playerHealthAudio; 

   public const int BOSS = 0, DAMAGED = 1, RELOADING = 2, ROUNDENDING = 3, SHIELDBEACONDAMAGED = 4, SHIELDBEACONLOW = 5, NOAMMO = 6;  
   
    public override void OnStartClient() {
        if(isServerOnly) return; 

        Debug.Log("PlayerHealthAudio"); 
        playerHealthAudio.Post(gameObject); 

    }

    public void SetPlayerHealthRTPC(float health) {
        playerHealth.SetValue(gameObject, health);
    }

    [ClientRpc]
    public void TriggerVoiceLine(int voiceLine)
    {
        switch(voiceLine)
        {
           case PlayerAudio.BOSS:
               bossVoiceLine.Post(gameObject); 
               break;
           case PlayerAudio.DAMAGED:
                damagedVoiceLine.Post(gameObject);
               break;
            case PlayerAudio.NOAMMO:
                noammoVoiceLine.Post(gameObject);
                break;
           case PlayerAudio.RELOADING:
                reloadingVoiceLine.Post(gameObject);
               break;
           case PlayerAudio.ROUNDENDING:
                roundendingVoiceLine.Post(gameObject);
               break;
           case PlayerAudio.SHIELDBEACONDAMAGED:
                shieldbeacondamagedVoiceLine.Post(gameObject);
               break;
           case PlayerAudio.SHIELDBEACONLOW:
                shieldbeaconlowVoiceLine.Post(gameObject);
               break;

        }

        Debug.Log($"Triggered Voice Line: {voiceLine}");
    }
}
