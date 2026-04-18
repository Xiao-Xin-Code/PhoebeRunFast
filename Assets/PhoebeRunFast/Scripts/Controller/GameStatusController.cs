using QMVC;
using UnityEngine;

/// <summary>
/// 游戏状态控制器
/// </summary>
public class GameStatusController : BaseController
{
    [SerializeField] GameStatusView _view;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        // 注册视图事件
        _view.RegisterPausePressed(OnPausePressed);
    }

    /// <summary>
    /// 暂停按钮点击事件
    /// </summary>
    private void OnPausePressed()
    {
        this.SendCommand<GamePauseCommand>();
    }

    /// <summary>
    /// 恢复按钮点击事件
    /// </summary>
    private void OnResumePressed()
    {
        // 发送恢复命令
        this.SendCommand(new ResumeGameCommand());
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        // 注销事件
        _view.UnRegisterPausePressed(OnPausePressed);
    }
}
