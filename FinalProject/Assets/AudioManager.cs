using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 



public class AudioManager : NetworkBehaviour
{
    public static event Action<AudioManager> OnAudioManagerEnabled;


    public GameplayMusicAudioStorage audioStorage;
    public AIAudioStorage aiAudioStorage;

    //uint currentState = 0; 
    public const int PREPARATION = 0, BOSS = 1, LOWINTENSITY = 2, HIGHINTENSITY = 3, NONE = 4;

    public const int AIWAVESTART = 5, AIWAVEEND = 6, AIBOSSWAVESTART = 10, AISHIELDBEACONLOW = 11;


    private void OnEnable()
    {
        OnAudioManagerEnabled?.Invoke(this);
    }

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

    [ClientRpc]
    public void RpcPlayAIVoiceLine(int voiceLine)
    {
        int rand = 0;
        switch (voiceLine)
        {
            case AIWAVESTART:

                rand = UnityEngine.Random.Range(0, 2);

                if (rand == 0)
                {
                    aiAudioStorage.waveStartVoiceLine1.Post(gameObject);
                }
                else if (rand == 1)
                {
                    aiAudioStorage.waveStartVoiceLine2.Post(gameObject);
                }
                else if (rand == 2)
                {
                    aiAudioStorage.waveStartVoiceLine3.Post(gameObject);
                }
                break;
            case AIWAVEEND:

                rand = UnityEngine.Random.Range(0, 1);

                if (rand == 0)
                {

                    aiAudioStorage.waveEndVoiceLine1.Post(gameObject);
                }
                else if (rand == 1)
                {
                    aiAudioStorage.waveEndVoiceLine2.Post(gameObject);
                }
                break;
            case AIBOSSWAVESTART:
                aiAudioStorage.bossWaveStartVoiceLine.Post(gameObject);
                break;
            case AISHIELDBEACONLOW:
                aiAudioStorage.shieldBeaconLowVoiceLine.Post(gameObject);
                break;

        }
    }
}
