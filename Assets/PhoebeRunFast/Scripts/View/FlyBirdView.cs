using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBirdView : MonoBehaviour
{

	[SerializeField] RectTransform pipePoint;

	[SerializeField] RectTransform pipeParent;

	[SerializeField] RectTransform pipeRecylePoint;

	public RectTransform PipPoint=>pipePoint;

	public RectTransform PipeRecyclePoint => pipeRecylePoint;

	public RectTransform PipeParent => pipeParent;
	


}
