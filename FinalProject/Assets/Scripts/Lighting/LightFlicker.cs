using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{

    public LightingAudioStorage audioStorage;
    
    public Light lighting;

    public bool isFlickering = false;

    public float timeDelay;

    public float flickerTimeCooldown = 1;

    public float timeBetweenFlicker = 2;


    private void Update()
    {
        if(!isFlickering)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        lighting.enabled = false;
        timeDelay = Random.Range(0.01f, flickerTimeCooldown);

        yield return new WaitForSeconds(timeDelay);

        lighting.enabled = true;

        timeDelay = Random.Range(0.5f, timeBetweenFlicker);

        yield return new WaitForSeconds(timeDelay);

        isFlickering = false;
    }
}
