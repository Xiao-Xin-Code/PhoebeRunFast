using QMVC;
using UnityEngine;

/// <summary>
/// 道具控制器
/// </summary>
public class ItemController : BaseController
{
    [SerializeField] ItemView _view;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        // 注册视图事件
        _view.RegisterItemUsed(OnItemUsed);
    }

    /// <summary>
    /// 道具使用事件
    /// </summary>
    /// <param name="itemIndex">道具索引</param>
    private void OnItemUsed(int itemIndex)
    {
        // 发送使用道具命令
        this.SendCommand(new UseItemCommand(itemIndex));
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        // 注销事件
        _view.UnRegisterItemUsed(OnItemUsed);
    }
}
