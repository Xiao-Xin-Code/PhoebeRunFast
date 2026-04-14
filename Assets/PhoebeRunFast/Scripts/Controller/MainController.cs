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
		this.RegisterEvent<MainInitByTransitionOverEvent>(OnMainInitByTransitionOver);
	}



	private void OnMainInitByTransitionOver(MainInitByTransitionOverEvent evt)
	{

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
		yield return new WaitForSeconds(1f);
	}

	IEnumerator MainAssetUnLoad()
	{
		yield return new WaitForSeconds(1f);
	}


	protected override void OnDeInit()
	{
		base.OnDeInit();

		this.UnRegisterEvent<LoadMainEvent>(OnLoadMain);
		this.UnRegisterEvent<UnLoadMainEvent>(OnUnLoadMain);
		this.UnRegisterEvent<MainInitByTransitionOverEvent>(OnMainInitByTransitionOver);
	}

}
