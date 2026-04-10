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
		yield return new WaitForSeconds(5f);
	}

	IEnumerator GameAssetUnLoad()
	{
		yield return new WaitForSeconds(5f);
	}



}
