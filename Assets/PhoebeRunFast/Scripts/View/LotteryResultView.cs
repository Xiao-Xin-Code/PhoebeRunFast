

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LotteryResultView : MonoBehaviour
{
    [SerializeField] RectTransform resultParent;
    [SerializeField] RectTransform startRect;

    [SerializeField] Button skipBtn;

    [SerializeField] RectTransform[] points;

    public RectTransform ResultParent => resultParent;
    public RectTransform StartRect => startRect;

    public RectTransform[] Points => points;


    #region Register Event
    
    public void RegisterSkipPressed(UnityAction action)
    {
        skipBtn.onClick.AddListener(action);
    }

    #endregion

    #region UnRegister

    public void UnRegisterSkipPressed(UnityAction action)
    {
        skipBtn.onClick.RemoveListener(action);
    }

    #endregion
}