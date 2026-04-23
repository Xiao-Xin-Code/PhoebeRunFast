using QMVC;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class HomeMenuController : BaseController
{
    [SerializeField] HomeMenuView _view;

	GlobalSystem _globalSystem;


    protected override void OnInit()
    {
        base.OnInit();
		_globalSystem = this.GetSystem<GlobalSystem>();

		// 注册视图事件
		_view.RegisterBeginPressed(OnBeginPressed);
		_view.RegisterSetPressed(OnSetPressed);
	}


	/// <summary>
	/// 开始按钮点击事件
	/// </summary>
	private void OnBeginPressed()
	{
		// 发送转场命令，切换到主场景
		this.SendCommand(new OpenTransitionCommand(StageChanged));
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
		Debug.Log("触发");
		_globalSystem.GlobalModel.Stage.Value = Stage.Main;
	}




	protected override void OnDeInit()
	{
		base.OnDeInit();

		_view.UnRegisterBeginPressed(OnBeginPressed);
		_view.UnRegisterSetPressed(OnSetPressed);
	}

}