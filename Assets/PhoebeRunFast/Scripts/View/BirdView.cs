using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdView : MonoBehaviour
{
	[SerializeField] Rigidbody2D rb;

	public Rigidbody2D RB => rb;



	[SerializeField] RectTransform rectTransform;

	public RectTransform RectTransform => rectTransform;

}
