using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家状态视图
/// </summary>
public class PlayerStatusView : MonoBehaviour
{
    [SerializeField] Image healthBg;
    [SerializeField] Image healthBar;
    [SerializeField] Image lossBar;
    [SerializeField] TMP_Text healthValue;
    [SerializeField] Image energyBg;
    [SerializeField] Image energyBar;
    [SerializeField] TMP_Text energyValue;


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
    /// <param name="currentEnergy"></param>
    public void UpdateEnergyBar(float currentEnergy)
    {
        energyBar.fillAmount = currentEnergy;
    }



    public void UpdateHealthValue(float value)
    {
        healthValue.text = value.ToString();
    }

    public void UpdateEnergyValue(float value)
    {
        energyValue.text = value.ToString();
    }


}
