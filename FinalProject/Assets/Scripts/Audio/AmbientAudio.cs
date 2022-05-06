using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{

    public AmbientAudioStorage audioStorage;

    // Start is called before the first frame update
    void Start()
    {
        if(NetworkManagerContainment.IsHeadless())
        {
            return;
        }
        audioStorage.ambientEvent.Post(gameObject);
    }

}
