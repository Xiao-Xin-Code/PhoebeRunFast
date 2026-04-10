using System.Collections;
using QMVC;
using UnityEngine;

public class HomeController : BaseController
{
    [SerializeField] HomeView _view;

	GameModel _gameModel;


	protected override void OnInit()
	{
		base.OnInit();

		_gameModel = this.GetModel<GameModel>();

		_view.RegisterBeginPressed(OnBeginPressed);

		this.RegisterEvent<LoadHomeEvent>(OnLoadHome);
		this.RegisterEvent<UnLoadHomeEvent>(OnUnLoadHome);
	}

    private void OnBeginPressed()
    {
		this.SendCommand(new OpenTransitionCommand(StageChanged));
    }

	private void StageChanged()
	{
		Debug.Log("触发"); 
		_gameModel.Stage.Value = Stage.Main;
	}


	private void OnLoadHome(LoadHomeEvent evt)
	{
		evt.enumerator = HomeAssetLoad();
	}

	private void OnUnLoadHome(UnLoadHomeEvent evt)
	{
		evt.enumerator = HomeAssetUnLoad();
	}


	IEnumerator HomeAssetLoad()
	{
		yield return new WaitForSeconds(5f);
	}

	IEnumerator HomeAssetUnLoad()
	{
		yield return new WaitForSeconds(5f);
	}


	protected override void OnDeInit()
	{
		base.OnDeInit();
		_view.UnRegisterBeginPressed(OnBeginPressed);
		this.UnRegisterEvent<LoadHomeEvent>(OnLoadHome);
		this.UnRegisterEvent<UnLoadHomeEvent>(OnUnLoadHome);
	}

}
