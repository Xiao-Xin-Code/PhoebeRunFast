using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家状态视图
/// </summary>
public class PlayerStatusView : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image manaBar;


    /// <summary>
    /// 更新生命值
    /// </summary>
    /// <param name="currentHealth"></param>
    public void UpdateHealthBar(float currentHealth)
    {
        healthBar.fillAmount = currentHealth;
    }

    /// <summary>
    /// 更新精力值
    /// </summary>
    /// <param name="currentMana"></param>
    public void UpdateManaBar(float currentMana)
    {
        manaBar.fillAmount = currentMana;
    }
}
