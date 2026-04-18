using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 游戏状态视图
/// </summary>
public class GameStatusView : MonoBehaviour
{
    [SerializeField] Button pauseButton;
    [SerializeField] Text coinText;

    #region Register

    /// <summary>
    /// 注册暂停按钮点击事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void RegisterPausePressed(UnityAction action)
    {
        pauseButton.onClick.AddListener(action);
    }

    #endregion

    #region UnRegister

    /// <summary>
    /// 注销暂停按钮点击事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void UnRegisterPausePressed(UnityAction action)
    {
        pauseButton.onClick.RemoveListener(action);
    }

    #endregion

    /// <summary>
    /// 更新金币显示
    /// </summary>
    /// <param name="coinCount">金币数量</param>
    public void UpdateCoinDisplay(int coinCount)
    {
        coinText.text = coinCount.ToString();
    }
}
