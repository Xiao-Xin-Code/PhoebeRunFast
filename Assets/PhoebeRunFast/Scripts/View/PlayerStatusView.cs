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
    /// 更新血条
    /// </summary>
    /// <param name="currentHealth">当前生命值</param>
    /// <param name="maxHealth">最大生命值</param>
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        healthBar.fillAmount = Mathf.Clamp01(fillAmount);
    }

    /// <summary>
    /// 更新蓝量条
    /// </summary>
    /// <param name="currentMana">当前魔法值</param>
    /// <param name="maxMana">最大魔法值</param>
    public void UpdateManaBar(float currentMana, float maxMana)
    {
        float fillAmount = currentMana / maxMana;
        manaBar.fillAmount = Mathf.Clamp01(fillAmount);
    }
}
