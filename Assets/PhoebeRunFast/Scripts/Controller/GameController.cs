using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : BaseController
{
    GameModel _gameModel;

	//TODO: 先初始化数据
	//TODO: 同步转场动态
	//TODO: 触发初始加载，加载到初始画面


	protected override void OnInit()
	{
		base.OnInit();

		_gameModel = this.GetModel<GameModel>();

		Debug.Log(_gameModel);

		SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
	}


}
