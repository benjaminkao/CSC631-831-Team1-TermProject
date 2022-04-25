using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootSteps : MonoBehaviour
{
    public AK.Wwise.Event FootStep; 

    void PlayFootStep() {
        FootStep.Post(gameObject); 
    }
}
