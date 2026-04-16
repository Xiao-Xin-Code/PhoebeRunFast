using QMVC;
using UnityEngine;

/// <summary>
/// 暂停控制器
/// </summary>
public class PauseController : BaseController
{
    [SerializeField] PauseView _view;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        // 注册视图事件
        _view.RegisterPausePressed(OnPausePressed);
        _view.RegisterResumePressed(OnResumePressed);

        // 注册系统事件
        this.RegisterEvent<GameStateChangeEvent>(OnGameStateChange);
    }

    /// <summary>
    /// 暂停按钮点击事件
    /// </summary>
    private void OnPausePressed()
    {
        // 发送暂停命令
        this.SendCommand(new PauseGameCommand());
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
    /// 游戏状态变化事件
    /// </summary>
    /// <param name="evt">事件参数</param>
    private void OnGameStateChange(GameStateChangeEvent evt)
    {
        _view.UpdatePauseUI(evt.newState);
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        // 注销事件
        _view.UnRegisterPausePressed(OnPausePressed);
        _view.UnRegisterResumePressed(OnResumePressed);
        this.UnRegisterEvent<GameStateChangeEvent>(OnGameStateChange);
    }
}
