using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerEnter : MonoBehaviour
{

    [SerializeField] private AK.Wwise.Event ambientEvent;


    [SerializeField] private List<string> tags;

    private void OnTriggerEnter(Collider other)
    {

        if (tags.Contains(other.gameObject.tag))
        {
            ambientEvent.Post(gameObject);
        }
    }
}
