using System.Collections;
using QMVC;
using UnityEngine;

public class GameController : BaseController
{

	protected override void OnInit()
	{
		base.OnInit();


		this.RegisterEvent<LoadGameEvent>(OnLoadGame);
		this.RegisterEvent<UnLoadGameEvent>(OnUnLoadGame);
		this.RegisterEvent<GameInitByTransitionOverEvent>(OnGameInitByTransitionOver);
	}



	private void OnGameInitByTransitionOver(GameInitByTransitionOverEvent evt)
	{
		//TODO: 初始可以是一个开场动画（实时）
		//播放
		Debug.Log("开场动画");
	}


	private void OnLoadGame(LoadGameEvent evt)
	{
		evt.enumerator = GameAssetLoad();
	}

	private void OnUnLoadGame(UnLoadGameEvent evt)
	{
		evt.enumerator = GameAssetUnLoad();
	}


	IEnumerator GameAssetLoad()
	{
		//TODO: 加载资源
		//TODO: 初始一段环境
		//TODO: 初始Player

		yield return new WaitForSeconds(5f);		
	}

	IEnumerator GameAssetUnLoad()
	{
		yield return new WaitForSeconds(5f);
	}




	protected override void OnDeInit()
	{
		base.OnDeInit();

		this.UnRegisterEvent<LoadGameEvent>(OnLoadGame);
		this.UnRegisterEvent<UnLoadGameEvent>(OnUnLoadGame);
		this.UnRegisterEvent<GameInitByTransitionOverEvent>(OnGameInitByTransitionOver);

	}

}
