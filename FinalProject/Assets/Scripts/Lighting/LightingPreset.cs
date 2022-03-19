using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enables us to create an instance of this class as a file in the Unity Editor in the right-click menu
[System.Serializable]
[CreateAssetMenu(fileName = "LightingPreset", menuName = "Scriptables/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{

    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
