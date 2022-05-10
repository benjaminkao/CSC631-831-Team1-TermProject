using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerEnter : MonoBehaviour
{

    [SerializeField] private AK.Wwise.State wwiseState;


    public enum WwiseTrigger {
        forEvent, forState, forSwitch
    }

    public WwiseTrigger triggerType;



    [SerializeField] private List<string> tags;

    private void OnTriggerEnter(Collider other)
    {

        if (tags.Contains(other.gameObject.tag))
        {
            switch(triggerType)
            {
                case WwiseTrigger.forEvent:

                    break;
                case WwiseTrigger.forState:
                    wwiseState.SetValue();
                    break;
                case WwiseTrigger.forSwitch:
                    break;
            }
        }
    }
}
