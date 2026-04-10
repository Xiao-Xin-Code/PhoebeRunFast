using System.Collections;
using QMVC;
using UnityEngine;

public class MainController : BaseController
{
	protected override void OnInit()
	{
		base.OnInit();



		this.RegisterEvent<LoadMainEvent>(OnLoadMain);
		this.RegisterEvent<UnLoadMainEvent>(OnUnLoadMain);
	}



	private void OnLoadMain(LoadMainEvent evt)
	{
		evt.enumerator = MainAssetLoad();
	}

	private void OnUnLoadMain(UnLoadMainEvent evt)
	{
		evt.enumerator = MainAssetUnLoad();
	}



	IEnumerator MainAssetLoad()
	{
		yield return new WaitForSeconds(5f);
	}

	IEnumerator MainAssetUnLoad()
	{
		yield return new WaitForSeconds(5f);
	}



}
