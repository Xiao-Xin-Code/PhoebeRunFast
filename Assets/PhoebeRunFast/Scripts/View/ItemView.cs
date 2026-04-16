using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 道具视图
/// </summary>
public class ItemView : MonoBehaviour
{
    [SerializeField] Button[] itemButtons;

    #region Register

    /// <summary>
    /// 注册道具使用事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void RegisterItemUsed(Action<int> action)
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i;
            itemButtons[i].onClick.AddListener(() => action(index));
        }
    }

    #endregion

    #region UnRegister

    /// <summary>
    /// 注销道具使用事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void UnRegisterItemUsed(Action<int> action)
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].onClick.RemoveAllListeners();
        }
    }

    #endregion

    /// <summary>
    /// 更新道具冷却
    /// </summary>
    /// <param name="itemIndex">道具索引</param>
    /// <param name="cooldown">冷却时间</param>
    public void UpdateItemCooldown(int itemIndex, float cooldown)
    {
        if (itemIndex >= 0 && itemIndex < itemButtons.Length)
        {
            // 实现道具冷却显示逻辑
        }
    }
}
