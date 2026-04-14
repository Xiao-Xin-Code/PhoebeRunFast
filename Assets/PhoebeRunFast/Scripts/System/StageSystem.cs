using System.Collections;
using DG.Tweening;
using Frame;
using QMVC;
using UnityEngine.SceneManagement;

public class StageSystem : AbstractSystem
{
	GameModel _gameModel;

	protected override void OnInit()
	{
		_gameModel = this.GetModel<GameModel>();
		_gameModel.Stage.RegisterWithOldValue(BeforeStageChanged);
	}

	private void BeforeStageChanged(Stage stage, Stage newStage)
	{
		MonoService.Instance.StartCoroutine(TotalProgress(stage, newStage));
	}

	IEnumerator TotalProgress(Stage oldStage, Stage newStage)
	{
		float cur = 0;
		yield return UnLoad(oldStage);
		bool half1Complete = false;
		Tween half1 = DOTween.To(() => cur, x => { cur = x; this.SendEvent(new UpdateProgressEvent(cur)); }, 0.5f, 10f);
		half1.OnComplete(() => half1Complete = false);
		half1.Play();
		yield return Load(newStage);
		while (half1Complete)
		{
			yield return null;
		}
		Tween half2 = DOTween.To(() => cur, x => { cur = x; this.SendEvent(new UpdateProgressEvent(cur)); }, 1f, 1f);
		yield return half2.WaitForCompletion();
		this.SendEvent(new CloseTransitionEvent(() => OnInitByLoadOver(newStage)));
	}

	IEnumerator UnLoad(Stage stage)
	{
		switch (stage)
		{
			case Stage.Home:
				UnLoadHomeEvent unLoadHomeEvent = new UnLoadHomeEvent();
				this.SendEvent(unLoadHomeEvent);
				yield return unLoadHomeEvent.enumerator;
				break;
			case Stage.Main:
				UnLoadMainEvent unLoadMainEvent = new UnLoadMainEvent();
				this.SendEvent(unLoadMainEvent);
				yield return unLoadMainEvent.enumerator;
				break;
			case Stage.Game:
				UnLoadGameEvent unLoadGameEvent = new UnLoadGameEvent();
				this.SendEvent(unLoadGameEvent);
				yield return unLoadGameEvent.enumerator;
				break;
			default:
				break;
		}
		yield return SceneManager.UnloadSceneAsync((int)stage);
	}

	IEnumerator Load(Stage stage)
	{
		yield return SceneManager.LoadSceneAsync((int)stage, LoadSceneMode.Additive);
		switch (stage)
		{
			case Stage.Home:
				LoadHomeEvent loadHomeEvent = new LoadHomeEvent();
				this.SendEvent(loadHomeEvent);
				yield return loadHomeEvent.enumerator;
				break;
			case Stage.Main:
				LoadMainEvent loadMainEvent = new LoadMainEvent();
				this.SendEvent(loadMainEvent);
				yield return loadMainEvent.enumerator;
				break;
			case Stage.Game:
				LoadGameEvent loadGameEvent = new LoadGameEvent();
				this.SendEvent(loadGameEvent);
				yield return loadGameEvent.enumerator;
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 用于触发一些，需要加载完毕并在结束转场后才触发的
	/// </summary>
	/// <param name="stage"></param>
	private void OnInitByLoadOver(Stage stage)
	{
		switch (stage)
		{
			case Stage.Home:
				this.SendEvent<HomeInitByTransitionOverEvent>();
				break;
			case Stage.Main:
				this.SendEvent<MainInitByTransitionOverEvent>();
				break;
			case Stage.Game:
				this.SendEvent<GameInitByTransitionOverEvent>();
				break;
			default:
				break;
		}
	}
}
