using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleLockView : MonoBehaviour
{
    Image lockIcon;

    Button unLockBtn;



    public void StateInit()
    {
        gameObject.SetActive(false);
    }
}
