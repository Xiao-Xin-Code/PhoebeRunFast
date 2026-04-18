using System.Collections;
using DG.Tweening;
using Frame;
using QMVC;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 阶段系统
/// </summary>
public class StageSystem : AbstractSystem
{
	BootModel _bootModel;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		_bootModel = this.GetModel<BootModel>();
		// 注册阶段变化事件
		_bootModel.Stage.RegisterWithOldValue(OnStageChanged);
	}

	/// <summary>
	/// 阶段变化前事件
	/// </summary>
	/// <param name="stage">旧阶段</param>
	/// <param name="newStage">新阶段</param>
	private void OnStageChanged(Stage stage, Stage newStage)
	{
		MonoService.Instance.StartCoroutine(TotalProgress(stage, newStage));
	}

	/// <summary>
	/// 总进度协程
	/// </summary>
	/// <param name="oldStage">旧阶段</param>
	/// <param name="newStage">新阶段</param>
	/// <returns>协程</returns>
	IEnumerator TotalProgress(Stage oldStage, Stage newStage)
	{
		float cur = 0;
		yield return UnLoad(oldStage);
		bool half1Complete = false;
		Tween half1 = DOTween.To(() => cur, x => { cur = x; this.SendEvent(new UpdateProgressEvent(cur)); }, 0.5f, 1f);
		half1.OnComplete(() => half1Complete = true);
		half1.Play();
		yield return Load(newStage);
		while (!half1Complete)
		{
			yield return null;
		}
		Tween half2 = DOTween.To(() => cur, x => { cur = x; this.SendEvent(new UpdateProgressEvent(cur)); }, 1f, 1f);
		yield return half2.WaitForCompletion();
		this.SendEvent(new CloseTransitionEvent(() => OnInitByLoadOver(newStage)));
	}

	/// <summary>
	/// 卸载场景
	/// </summary>
	/// <param name="stage">阶段</param>
	/// <returns>协程</returns>
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

	/// <summary>
	/// 加载场景
	/// </summary>
	/// <param name="stage">阶段</param>
	/// <returns>协程</returns>
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
	/// <param name="stage">阶段</param>
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
