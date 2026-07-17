using QMVC;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class HomeMenuController : BaseController
{
    [SerializeField] HomeMenuView _view;

	GlobalSystem _globalSystem;
	AccountSystem _accountSystem;


    protected override void OnInit()
    {
        base.OnInit();
		_globalSystem = this.GetSystem<GlobalSystem>();
		_accountSystem = this.GetSystem<AccountSystem>();
		
		// 注册视图事件
		_view.RegisterBeginPressed(OnBeginPressed);
		_view.RegisterSetPressed(OnSetPressed);
	}


	/// <summary>
	/// 开始按钮点击事件
	/// </summary>
	private void OnBeginPressed()
	{
		if(_globalSystem.GlobalModel.AccountJsons == null || _globalSystem.GlobalModel.AccountJsons.Length == 0)
		{
			Debug.LogError("AccountJsons is null or empty");//去注册
			this.SendCommand(new SignLoginActiveCommand(true, true));
			return;
		}


		if(_globalSystem.GlobalModel.UserJson != null)
		{
			if(!string.IsNullOrEmpty(_globalSystem.GlobalModel.UserJson.userId))
			{
				if(_accountSystem.CheckHasAccount(_globalSystem.GlobalModel.UserJson.userId))
				{
					// 发送转场命令，切换到登录场景
					this.SendCommand(new OpenTransitionCommand(StageChanged));
					return;
				}
			}
		}

		//去登陆
		this.SendCommand(new SignLoginActiveCommand(true,false));
	}

	/// <summary>
	/// 设置按钮点击事件
	/// </summary>
	private void OnSetPressed()
	{
		// 发送设置激活命令
		this.SendCommand(new SettingActiveCommand(true));
	}

	/// <summary>
	/// 阶段切换方法
	/// </summary>
	private void StageChanged()
	{
		_globalSystem.GlobalModel.Stage.Value = Stage.Main;
	}




	protected override void OnDeInit()
	{
		base.OnDeInit();

		_view.UnRegisterBeginPressed(OnBeginPressed);
		_view.UnRegisterSetPressed(OnSetPressed);
	}

}