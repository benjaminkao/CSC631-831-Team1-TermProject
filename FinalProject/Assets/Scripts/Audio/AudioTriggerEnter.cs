using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerEnter : MonoBehaviour
{

    [SerializeField] private AK.Wwise.Event ambientEvent;
    [SerializeField] private AK.Wwise.Switch switchEvent;
    [SerializeField] private GameObject objectAffected; 

    public bool forEvent, forSwitch; 


    [SerializeField] private List<string> tags;

    private void OnTriggerEnter(Collider other)
    {

        if (tags.Contains(other.gameObject.tag))
        {   
            if(forEvent) {
                ambientEvent.Post(objectAffected);
            }

            if(forSwitch) {
                switchEvent.SetValue(objectAffected); 
            }
            
        }
    }
}
