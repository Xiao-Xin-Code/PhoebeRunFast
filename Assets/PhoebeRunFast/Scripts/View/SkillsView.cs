using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 技能视图
/// </summary>
public class SkillsView : MonoBehaviour
{
    [SerializeField] Button[] skillButtons;

    #region Register

    /// <summary>
    /// 注册技能使用事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void RegisterSkillUsed(Action<int> action)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int index = i;
            skillButtons[i].onClick.AddListener(() => action(index));
        }
    }

    #endregion

    #region UnRegister

    /// <summary>
    /// 注销技能使用事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void UnRegisterSkillUsed(Action<int> action)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].onClick.RemoveAllListeners();
        }
    }

    #endregion

    /// <summary>
    /// 更新技能冷却
    /// </summary>
    /// <param name="skillIndex">技能索引</param>
    /// <param name="cooldown">冷却时间</param>
    public void UpdateSkillCooldown(int skillIndex, float cooldown)
    {
        if (skillIndex >= 0 && skillIndex < skillButtons.Length)
        {
            // 实现技能冷却显示逻辑
        }
    }
}
