using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public Button Join;

    public TransitionController transition;
    public GameObject main;

	private void Start()
	{
        Join.onClick.AddListener(JoinPressed);
	}

	public void JoinPressed()
    {
        Join.interactable = false;
        transition.ToTransition(ref Progress, ref ProgressOver, LoadFunction);
	}

    Coroutine coroutine;

    private void LoadFunction()
    {
        if (coroutine!=null)
        {
            StopCoroutine(coroutine);
        }
		coroutine = StartCoroutine(Load());
    }

    event UnityAction<float> Progress;
    event UnityAction ProgressOver;


	IEnumerator Load()
    {
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            yield return null;
            Progress?.Invoke(progress);

		}
		yield return null;
		ProgressOver?.Invoke();
    }
}
