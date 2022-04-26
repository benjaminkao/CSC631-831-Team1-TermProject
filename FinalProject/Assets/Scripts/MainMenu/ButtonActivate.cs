using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonActivate : MonoBehaviour
{

    [SerializeField] private Button button;

    private void Start()
    {
        button.interactable = true;
    }




}
