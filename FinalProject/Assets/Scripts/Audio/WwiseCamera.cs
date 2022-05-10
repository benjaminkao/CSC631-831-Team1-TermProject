using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseCamera : MonoBehaviour
{
    public AkGameObj akGameObject;
    public AkAudioListener audiolistener;

    // Start is called before the first frame update
    void Start()
    {
        if (!NetworkManagerContainment.IsHeadless())
        {
            return;
        }
        akGameObject.enabled = false;
        audiolistener.enabled = false;
    }

}
