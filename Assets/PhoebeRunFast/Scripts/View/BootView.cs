using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootView : MonoBehaviour
{
    [SerializeField] Canvas canvas_static;
    [SerializeField] Canvas canvas_dynamic;
    [SerializeField] Canvas canvas_pop;


    [SerializeField] GameObject mask;

    public void SetMaskVisible(bool visible)
    {
        mask.SetActive(visible);
    }
}
