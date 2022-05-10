using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootSteps : MonoBehaviour
{
    public PlayerAudioStorage audioStorage;

    public ContainmentPlayer player;

    void PlayFootStep() {
        if (NetworkManagerContainment.IsHeadless())
        {
            return;
        }

        


        if (player.IsGrounded)
        {
            string floorTag;
            RaycastHit hit;

            if(Physics.Raycast(player.transform.position, Vector3.down, out hit))
            {
                floorTag = hit.collider.gameObject.tag;
                Debug.Log(floorTag);
                if (floorTag == "Grass")
                {
                    audioStorage.FootStepDirt.SetValue(gameObject);
                } else
                {
                    audioStorage.FootStepMetal.SetValue(gameObject);
                }
            }

            




            audioStorage.FootStep.Post(gameObject);
        }
    }
}
