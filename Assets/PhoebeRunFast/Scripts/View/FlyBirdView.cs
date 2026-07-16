using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyBirdView : MonoBehaviour
{

	[SerializeField] RectTransform pipePoint;

	[SerializeField] RectTransform pipeParent;

	[SerializeField] RectTransform pipeRecylePoint;

	[SerializeField] Text scoreText;
	[SerializeField] Text maxScoreText;



	public void SetScore(int score)
	{
		scoreText.text = score.ToString();
	}


	public void SetMaxScore(int maxScore)
	{
		maxScoreText.text = maxScore.ToString();
	}


	public RectTransform PipPoint=>pipePoint;

	public RectTransform PipeRecyclePoint => pipeRecylePoint;

	public RectTransform PipeParent => pipeParent;
	


}
