using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 技能视图
/// </summary>
public class SkillsView : MonoBehaviour
{
    [SerializeField] Button ultimateButton; // 大招按钮
    [SerializeField] Button skillButton; // 小技能按钮

    #region Register

    public void RegisterUltimatePressed(UnityAction action)
    {
        ultimateButton.onClick.AddListener(action);
    }

    public void RegisterSkillPressed(UnityAction action)
    {
        skillButton.onClick.AddListener(action);
    }

    #endregion

    #region UnRegister

    public void UnRegisterUltimatePressed(UnityAction action)
    {
        ultimateButton.onClick.RemoveListener(action);
    }

    public void UnRegisterSkillPressed(UnityAction action)
    {
        skillButton.onClick.RemoveListener(action);
    }

    #endregion

    #region Cooldown

    /// <summary>
    /// 更新技能冷却
    /// </summary>
    /// <param name="skillIndex">技能索引</param>
    /// <param name="cooldown">冷却时间</param>
    public void UpdateUltimateCooldown(float cooldown)
    {
        // 实现大招冷却显示逻辑
    }

    /// <summary>
    /// 更新小技能冷却
    /// </summary>
    /// <param name="cooldown">冷却时间</param>
    public void UpdateSkillCooldown(float cooldown)
    {
        // 实现小技能冷却显示逻辑
    }

    #endregion
}
