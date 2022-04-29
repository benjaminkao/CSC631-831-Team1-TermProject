using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootSteps : MonoBehaviour
{
    public PlayerAudioStorage audioStorage;

    void PlayFootStep() {
        audioStorage.FootStep.Post(gameObject); 
    }
}
