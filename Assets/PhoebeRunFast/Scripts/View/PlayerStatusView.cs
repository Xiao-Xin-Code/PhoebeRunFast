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
    [SerializeField] TMP_Text healthValue;
    [SerializeField] Image manaBg;
    [SerializeField] Image manaBar;
    [SerializeField] TMP_Text manaValue;


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



    public void UpdateHealthValue(float value)
    {
        healthValue.text = value.ToString();
    }

    public void UpdateManaValue(float value)
    {
        manaValue.text = value.ToString();
    }


}
