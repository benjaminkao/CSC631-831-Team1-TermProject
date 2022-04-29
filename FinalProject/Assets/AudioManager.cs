using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 



public class AudioManager : NetworkBehaviour
{
    public GameplayMusicAudioStorage audioStorage;

    //uint currentState = 0; 
    public const int PREPARATION = 0, BOSS = 1, LOWINTENSITY = 2, HIGHINTENSITY = 3, NONE = 4;

    // Update is called once per frame
    void Update()
    {
        if(!isServer) return; 
        

        //AkSoundEngine.GetState(preparationGameplayState.GroupId, out currentState);

        // if(Input.GetKeyDown(KeyCode.M)) {
        //     Debug.Log("Hello 'M' key is being pressed!");
        //     if(currentState == preparationGameplayState.Id) {
        //         noneState.SetValue(); 
        //         bossGameplayState.SetValue(); 
        //         Debug.Log("Set Boss Music");
        //     } else if(currentState == bossGameplayState.Id) {
        //         noneState.SetValue(); 
        //         preparationGameplayState.SetValue(); 
        //         Debug.Log("Set preparation music");
        //     }
        // }


        if(Input.GetKeyDown(KeyCode.M)) {
            Debug.Log("Hello 'M' key is being pressed!");
            RpcChangeGameplayAudioState(AudioManager.PREPARATION);
        }

        if(Input.GetKeyDown(KeyCode.K)) {
            Debug.Log("Hello 'K' key is being pressed!");
            RpcChangeGameplayAudioState(AudioManager.BOSS);
        }
    }

    [ClientRpc]
    public void RpcChangeGameplayAudioState(int state) 
    {
        switch(state)
        {
            case AudioManager.NONE:
                audioStorage.noneState.SetValue(); 
                break; 
            case AudioManager.PREPARATION:  
                Debug.Log("AudioManager-Preparation Music");
                audioStorage.preparationGameplayState.SetValue(); 
                break; 
            case AudioManager.BOSS : 
                Debug.Log("AudioManager-Boss Music");
                audioStorage.bossGameplayState.SetValue(); 
                break;
            case AudioManager.LOWINTENSITY : 
                Debug.Log("AudioManager-LowIntensity Music");
                audioStorage.lowIntensityGameplayState.SetValue();
                break;
            case AudioManager.HIGHINTENSITY :
                Debug.Log("AudioManager-highIntensity Music");
                audioStorage.highIntensityGameplayState.SetValue();
                break;
        }
    }
}
