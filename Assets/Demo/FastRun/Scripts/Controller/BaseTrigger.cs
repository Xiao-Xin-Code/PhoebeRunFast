using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTrigger : BaseController
{
	protected override void Init()
	{
		
	}

	[SerializeField] protected LayerMask targetLayer;


	protected virtual void OnTriggerEnter(Collider other)
	{
		Debug.Log("Enter" + other.gameObject.layer + "|" + targetLayer);
		if (!isActiveAndEnabled) return;
		if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
		{
			OnValidTriggerEnter(other);
		}
	}

	protected virtual void OnTriggerStay(Collider other)
	{
		if (!isActiveAndEnabled) return;
		if (other.gameObject.layer == targetLayer) 
		{
			OnValidTriggerStay(other);
		}
	}




	protected abstract void OnValidTriggerEnter(Collider other);
	protected abstract void OnValidTriggerStay(Collider other);


}
