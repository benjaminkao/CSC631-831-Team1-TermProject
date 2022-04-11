using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

 public enum GameplayMusicState {PREPARATION, BOSS, LOWINTENSITY, HIGHINTENSITY};

public class AudioManager : NetworkBehaviour
{
    [SerializeField]
    private AK.Wwise.State preparationGameplayState;
    [SerializeField]
    private AK.Wwise.State bossGameplayState;
    [SerializeField]
    private AK.Wwise.State lowIntensityGameplayState;
    [SerializeField]
    private AK.Wwise.State highIntensityGameplayState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isServer) return; 
        
        if(Input.GetKeyDown(KeyCode.M)) {
            RpcChangeGameplayAudioState(GameplayMusicState.BOSS);
        }

        if(Input.GetKeyDown(KeyCode.K)) {
            RpcChangeGameplayAudioState(GameplayMusicState.PREPARATION);
        }
    }

    [ClientRpc]
    public void RpcChangeGameplayAudioState(GameplayMusicState gameplayMusicState) 
    {
        switch(gameplayMusicState)
        {
            case GameplayMusicState.PREPARATION : 
                preparationGameplayState.SetValue(); 
                break; 
            case GameplayMusicState.BOSS : 
                bossGameplayState.SetValue(); 
                break;
            case GameplayMusicState.LOWINTENSITY : 
                lowIntensityGameplayState.SetValue();
                break;
            case GameplayMusicState.HIGHINTENSITY : 
                highIntensityGameplayState.SetValue();
                break;
        }
    }
}
