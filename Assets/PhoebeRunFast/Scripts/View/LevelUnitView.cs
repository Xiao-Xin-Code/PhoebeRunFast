using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnitView : MonoBehaviour
{
	[SerializeField] Image icon;
   

	public void SetIconState(bool isOn)
	{
		icon.color = isOn ? new Color(1, 1, 0, 1) : new Color(0.5f, 0.5f, 0.5f, 1);
	}
}
