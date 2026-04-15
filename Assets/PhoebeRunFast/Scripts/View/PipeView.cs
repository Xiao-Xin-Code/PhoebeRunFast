using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeView : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    public RectTransform RectTransform => rectTransform;


    [SerializeField] Image top;
    [SerializeField] Image bottom;


}
