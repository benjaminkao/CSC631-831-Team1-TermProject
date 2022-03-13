using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes this script always execute in the editor and play mode
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{

    [SerializeField] private Light sun;
    [SerializeField] private LightingPreset preset;

    [SerializeField, Range(5, 19)] private float timeOfDay;
    
    private void UpdateLighting(float timePercent)
    {

        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if(sun != null)
        {
            sun.color = preset.DirectionalColor.Evaluate(timePercent);

            // Rotate the sun to simulate the sun going up and down.
            sun.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }

    }

    private void OnValidate()
    {
        // If we already have a reference to the sun, don't need to do anything
        if(sun != null)
        {
            return;
        }

        // This is the light used by the procedural skybox
        if(RenderSettings.sun != null)
        {
            sun = RenderSettings.sun;
        } else
        {
            // If there is no sun, then we just find the first directional light and use that as the sun instead.
            Light[] lights = GameObject.FindObjectsOfType<Light>();

            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    sun = light;
                    return;
                }
            }
        }
    }


    // With [ExecuteAlways] the Update() function is only called when something in the Scene changed
    void Update()
    {
        if(preset == null)
        {
            return;
        }


        if(Application.isPlaying)
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        } else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    public void SetTime(float value)
    {
        this.timeOfDay = value;
    }
}
