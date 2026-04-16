
using QMVC;
using UnityEngine;

/// <summary>
/// 技能控制器
/// </summary>
public class SkillsController : BaseController
{
    [SerializeField] SkillsView _view;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        // 注册视图事件
        _view.RegisterSkillUsed(OnSkillUsed);
    }

    /// <summary>
    /// 技能使用事件
    /// </summary>
    /// <param name="skillIndex">技能索引</param>
    private void OnSkillUsed(int skillIndex)
    {
        // 发送使用技能命令
        this.SendCommand(new UseSkillCommand(skillIndex));
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        // 注销事件
        _view.UnRegisterSkillUsed(OnSkillUsed);
    }
}
