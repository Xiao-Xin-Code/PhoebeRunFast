using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 暂停视图
/// </summary>
public class PauseView : MonoBehaviour
{
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button resumeButton;

    #region Register

    /// <summary>
    /// 注册暂停按钮点击事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void RegisterPausePressed(UnityAction action)
    {
        pauseButton.onClick.AddListener(action);
    }

    /// <summary>
    /// 注册恢复按钮点击事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void RegisterResumePressed(UnityAction action)
    {
        resumeButton.onClick.AddListener(action);
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

    /// <summary>
    /// 注销恢复按钮点击事件
    /// </summary>
    /// <param name="action">回调函数</param>
    public void UnRegisterResumePressed(UnityAction action)
    {
        resumeButton.onClick.RemoveListener(action);
    }

    #endregion

    /// <summary>
    /// 更新暂停UI
    /// </summary>
    /// <param name="gameState">游戏状态</param>
    public void UpdatePauseUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Running:
                pauseButton.gameObject.SetActive(true);
                pauseMenu.SetActive(false);
                break;
            case GameState.Paused:
                pauseButton.gameObject.SetActive(false);
                pauseMenu.SetActive(true);
                break;
            default:
                pauseButton.gameObject.SetActive(true);
                pauseMenu.SetActive(false);
                break;
        }
    }
}
